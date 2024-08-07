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

            for (int i = 0; i < materialLengths.Count; i++)
            {
                var plan = new ProfileNestingPlan()
                {
                    Length = materialLengths[i],
                };

                while (true)
                {
                    if (partLengths.Count == 0)
                    {
                        plan.Segments.Add(0);
                        break;
                    }
                    var index = RandomizationProvider.Current.GetInt(0, partLengths.Count);

                    plan.Segments.Add(partLengths[index]);

                    if (plan.Length < plan.Segments.Sum())
                    {
                        plan.Segments.RemoveAt(plan.Segments.Count - 1);
                        partLengths.Add(partLengths[index]);
                        break;
                    }
                    else
                    {
                        partLengths.RemoveAt(index);
                    }
                }

                Plans.Add(plan);
            }
        }
    }
}