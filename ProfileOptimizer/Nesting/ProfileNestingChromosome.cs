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

        public List<ProfileNestingPlan> Plans { get; private set; } = [];

        public ProfileNestingChromosome(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option) : base(materials.Sum(i => i.Piece))
        {
            _materials = materials;
            _parts = parts;
            _option = option;

            CreateGenes();
        }

        public override IChromosome CreateNew()
        {
            return new ProfileNestingChromosome(_materials, _parts, _option);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            var materials = new List<UsageProfileMaterial>(_materials.Sum(i => i.Piece));
            var parts = new List<UsageProfilePart>(_parts.Sum(i => i.Piece));

            for (int i = 0; i < materials.Count; i++)
            {
                var result = new ProfileNestingPlan();
                result.Material = new UsageProfileMaterial()
                {
                    Id = i,
                    Length = _materials[i].Length,
                };

                for (int j = 0; j < _option.MaxSegments; j++)
                {
                    var index = RandomizationProvider.Current.GetInt(0, parts.Count);

                    result.Parts.Add(parts[index]);
                }

                Plans.Add(result);
            }
        }
    }
}