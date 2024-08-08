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
            //Plans.RemoveAt(index);
            return new Gene(Plans[index]);
        }

        private void Init(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
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

                Plans.Add(plan);
            }
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
    }
}