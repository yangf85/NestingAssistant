using GeneticSharp;
using ProfileOptimizer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class SimpleProfileNester
    {
        private readonly List<ProfileMaterial> _materials;

        private readonly List<ProfilePart> _parts;

        private readonly ProfileNestingOption _option;

        public List<ProfileNestingPlan> Plans { get; private set; } = [];

        public SimpleProfileNester(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;

            QuantizationProfileMaterialLength();
            QuantizationProfilePartLength();
        }

        private Dictionary<string, (List<ProfileMaterial> Materials, List<ProfilePart> Parts)> GroupByType(List<ProfileMaterial> materials, List<ProfilePart> parts)
        {
            var materialGroups = materials.GroupBy(m => m.Type).ToDictionary(g => g.Key, g => g.ToList());
            var partGroups = parts.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.ToList());

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

        public void Nest()
        {
        }

        private List<ProfileNestingPlan> Nest(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            var result = new List<ProfileNestingPlan>();

            var materialLengths = materials.SelectMany(i => Enumerable.Repeat(i.Length, i.Piece)).OrderBy(x => x).ToList();
            var partLengths = parts.SelectMany(i => Enumerable.Repeat(i.Length, i.Piece)).OrderBy(x => x).ToList();

            while (materialLengths.Count > 0)
            {
                var materialIndex = RandomizationProvider.Current.GetInt(0, materialLengths.Count);

                var plan = new ProfileNestingPlan
                {
                    Length = materialLengths[materialIndex],
                };

                materialLengths.RemoveAt(materialIndex);

                while (true)
                {
                    if (partLengths.Count == 0)
                    {
                        break;
                    }

                    var remainLength = plan.Segments.Count switch
                    {
                        0 => plan.Length,
                        _ => plan.Length - (plan.Segments.Sum() + (plan.Segments.Count - 1) * option.Spacing)
                    };

                    var index = FindClosestIndexNotGreaterThan(remainLength, partLengths);

                    if (index == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (plan.Segments.Count < _option.MaxSegments)
                        {
                            plan.Segments.Add(partLengths[index]);
                            partLengths.RemoveAt(index);
                        }
                    }

                    if (plan.Segments.Count == _option.MaxSegments)
                    {
                        break;
                    }
                }

                result.Add(plan);
            }
            return result;
        }

        private int FindClosestIndexNotGreaterThan(double num, List<double> numbers)
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