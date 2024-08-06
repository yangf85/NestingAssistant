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

        public List<ProfileNestingResult> Results { get; private set; }

        public ProfileNestingChromosome(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option) : base(materials.Sum(i => i.Piece))
        {
            _materials = materials;
            _parts = parts;
            _option = option;

            CreateGenes();
        }

        public override IChromosome CreateNew()
        {
            throw new NotImplementedException();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            throw new NotImplementedException();
        }

        public List<UsageProfileMaterial> Init()
        {
            var materials = new List<UsageProfileMaterial>(_materials.Sum(i => i.Piece));
            var parts = new List<UsageProfilePart>(_parts.Sum(i => i.Piece));

            // 按长度对零件进行排序（从大到小）
            parts = parts.OrderByDescending(p => p.Length).ToList();

            foreach (var material in materials)
            {
                if (parts.Count == 0)
                {
                    break;
                }

                while (material.RemainLength > 0 && parts.Count > 0)
                {
                    if (material.Parts.Count >= _option.MaxSegments)
                    {
                        break;
                    }

                    // 寻找适合当前原材料剩余长度的零件
                    UsageProfilePart? selectedPart = null;
                    for (int i = 0; i < parts.Count; i++)
                    {
                        if (parts[i].Length <= material.RemainLength)
                        {
                            selectedPart = parts[i];
                            parts.RemoveAt(i);
                            break;
                        }
                    }

                    if (selectedPart.HasValue)
                    {
                        material.Parts.Add(selectedPart.Value);
                    }
                    else
                    {
                        // 没有适合的零件可以放置，跳出循环
                        break;
                    }
                }
            }

            return materials;
        }
    }
}