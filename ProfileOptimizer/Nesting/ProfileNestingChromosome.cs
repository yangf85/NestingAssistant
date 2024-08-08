using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingChromosome : ChromosomeBase
    {
        private List<ProfileMaterial> _materials;

        private List<ProfilePart> _parts;

        private ProfileNestingOption _option;

        public List<ProfileNestingPlan> Plans { get; private set; }

        public ProfileNestingChromosome(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option) : base(materials.Sum(i => i.Piece))
        {
            _materials = materials;
            _parts = parts;
            _option = option;

            Plans = new List<ProfileNestingPlan>(Length);

            Init(materials, parts, option);

            CreateGenes();
        }

        public override IChromosome CreateNew()
        {
            return new ProfileNestingChromosome(_materials, _parts, _option);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            var index = RandomizationProvider.Current.GetInt(0, Plans.Count);
            return new Gene(Plans[index]);
        }

        private void Init(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            var materialLengths = materials.SelectMany(i => Enumerable.Repeat(i.Length, i.Piece)).ToList();
            var partLengths = parts.SelectMany(i => Enumerable.Repeat(i.Length, i.Piece)).ToList();

            // 预先对零件长度进行排序
            partLengths.Sort();

            while (materialLengths.Count > 0)
            {
                // 随机选择一个原材料
                var materialIndex = RandomizationProvider.Current.GetInt(0, materialLengths.Count);
                var materialLength = materialLengths[materialIndex];

                var plan = new ProfileNestingPlan()
                {
                    Length = materialLength,
                };

                materialLengths.RemoveAt(materialIndex);

                while (true)
                {
                    if (partLengths.Count == 0)
                    {
                        break;
                    }

                    // 当前剩余长度
                    double remainingLength = plan.Length - plan.Segments.Sum();

                    // 使用二分查找找到最接近剩余长度的零件
                    int index = partLengths.BinarySearch(remainingLength);

                    // 如果找不到完全匹配的零件，BinarySearch 返回一个负数，
                    // 这个负数的绝对值表示插入点的索引。
                    if (index < 0)
                    {
                        index = ~index;

                        // 选择最接近的零件
                        if (index == partLengths.Count || (index > 0 && remainingLength - partLengths[index - 1] < partLengths[index] - remainingLength))
                        {
                            index--;
                        }
                    }

                    // 选择零件并进行尝试添加
                    double selectedPart = partLengths[index];
                    plan.Segments.Add(selectedPart);
                    if (plan.Length < plan.Segments.Sum())
                    {
                        // 如果超过了原材料长度，撤销上一步操作
                        plan.Segments.RemoveAt(plan.Segments.Count - 1);
                        break;
                    }
                    else
                    {
                        // 从可用零件列表中移除已使用的零件
                        partLengths.RemoveAt(index);
                    }

                    if (plan.Segments.Count == _option.MaxSegments)
                    {
                        break;
                    }
                }

                Plans.Add(plan);
            }
        }
    }
}