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
        public ProfileNestingResult NestingResult { get; private set; }

        public ProfileNestingChromosome(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option) : base(option.MaxSegments)
        {
        }

        public override IChromosome CreateNew()
        {
            throw new NotImplementedException();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            throw new NotImplementedException();
        }
    }
}