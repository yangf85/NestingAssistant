using GeneticSharp;
using ProfileOptimizer.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class GreedyProfileNester : INestHandler
    {
        private readonly List<ProfileMaterial> _materials;

        private readonly List<ProfilePart> _parts;

        private readonly ProfileNestingOption _option;

        public IProgress<double> Progress => throw new NotImplementedException();

        public GreedyProfileNester(List<ProfilePart> parts, List<ProfileMaterial> materials, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;

            QuantizationProfileMaterialLength();
            QuantizationProfilePartLength();
        }

        public async Task<ProfileNestingSummary> NestAsync()
        {
            var groups = GroupByType(_parts, _materials);
            var tasks = new List<Task<List<GreedyProfileNestingResult>>>();
            var totalGroups = groups.Count;

            foreach (var group in groups)
            {
                tasks.Add(Task.Run(() =>
                {
                    var nestedPlans = Nest(group.Value.Materials, group.Value.Parts, _option);
                    return nestedPlans;
                }));
            }

            var results = await Task.WhenAll(tasks);

            var summary = new GreedyNestingSummary(results.SelectMany(x => x).ToList(), _option);

            return summary;
        }

        private static int FindClosestIndexNotGreaterThan(double num, List<double> numbers)
        {
            int index = numbers.BinarySearch(num);

            if (index >= 0)
            {
                // 如果找到目标值，直接返回索引
                return index;
            }
            else
            {
                // 如果没有找到目标值，BinarySearch 返回负数，
                // 该负数是一个位移操作后的值，表示第一个大于目标元素的索引。
                int insertionPoint = ~index;

                // 如果插入点为0，说明目标值比所有元素都小
                if (insertionPoint == 0)
                {
                    return -1; // 没有符合条件的元素
                }
                else
                {
                    // 返回插入点前一个位置的索引
                    return insertionPoint - 1;
                }
            }
        }

        private static Dictionary<string, (List<ProfileMaterial> Materials, List<ProfilePart> Parts)> GroupByType(List<ProfilePart> parts, List<ProfileMaterial> materials)
        {
            var partGroups = parts.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.ToList());
            var materialGroups = materials.GroupBy(m => m.Type).ToDictionary(g => g.Key, g => g.ToList());

            // 获取所有的类型
            var materialTypes = new HashSet<string>(materialGroups.Keys);
            var partTypes = new HashSet<string>(partGroups.Keys);

            // 验证类型是否匹配
            if (!materialTypes.SetEquals(partTypes))
            {
                var missingMaterialTypes = string.Join(", ", partTypes.Except(materialTypes));
                var missingPartTypes = string.Join(", ", materialTypes.Except(partTypes));
                throw new ProfileTypeMismatchException($"Material types not matching with part types.\nMissing Material Types: {missingMaterialTypes}\nMissing Part Types: {missingPartTypes}");
            }

            var result = new Dictionary<string, (List<ProfileMaterial> Materials, List<ProfilePart> Parts)>();

            foreach (var key in materialGroups.Keys)
            {
                result.Add(key, (materialGroups[key], partGroups[key]));
            }

            return result;
        }

        private List<GreedyProfileNestingResult> Nest(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            var result = new List<GreedyProfileNestingResult>();

            var usageMaterials = materials.SelectMany(i => Enumerable.Repeat(
                                            new UsageProfileMaterial()
                                            {
                                                Type = i.Type,
                                                Length = i.Length,
                                            }, i.Piece))
                                            .OrderBy(x => x.Length)
                                            .ToList();
            var usageParts = parts.SelectMany(i => Enumerable.Repeat(
                                   new UsageProfilePart()
                                   {
                                       Index = i.Index,
                                       Type = i.Type,
                                       Length = i.Length,
                                       Label = i.Label,
                                   }, i.Piece))
                                   .OrderBy(x => x.Length)
                                   .ToList();

            var usagePartLengths = usageParts.Select(x => x.Length).ToList();

            while (usageMaterials.Count > 0)
            {
                var materialIndex = Random.Shared.Next(0, usageMaterials.Count);

                var plan = new GreedyProfileNestingResult
                {
                    Material = usageMaterials[materialIndex],
                };

                usageMaterials.RemoveAt(materialIndex);

                while (true)
                {
                    if (usageParts.Count == 0)
                    {
                        break;
                    }

                    var remainLength = plan.Parts.Count switch
                    {
                        0 => plan.Material.Length,
                        _ => plan.Material.Length - (plan.Parts.Sum(i => i.Length) + (plan.Parts.Count - 1) * option.Spacing)
                    };

                    var index = FindClosestIndexNotGreaterThan(remainLength, usagePartLengths);

                    if (index == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (plan.Parts.Count < _option.MaxSegments)
                        {
                            plan.Parts.Add(usageParts[index]);
                            usageParts.RemoveAt(index);
                            usagePartLengths.RemoveAt(index);
                        }
                    }

                    if (plan.Parts.Count == _option.MaxSegments)
                    {
                        break;
                    }
                }

                result.Add(plan);
            }
            return result;
        }

        private void QuantizationProfileMaterialLength()
        {
            foreach (var material in _materials)
            {
                material.Length = Math.Round(material.Length, 6);
            }
        }

        private void QuantizationProfilePartLength()
        {
            foreach (var material in _parts)
            {
                material.Length = Math.Round(material.Length, 6);
            }
        }
    }
}