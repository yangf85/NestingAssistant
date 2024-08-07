using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingFitness : IFitness
    {
        private List<ProfileMaterial> _materials;

        private List<ProfilePart> _parts;

        private ProfileNestingOption _option;

        public ProfileNestingFitness(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        public double Evaluate(IChromosome chromosome)
        {
            if (chromosome is not ProfileNestingChromosome nestingChromosome)
            {
                return double.MinValue;
            }

            if (nestingChromosome.Plans is { Count: 0 })
            {
                return double.MinValue;
            }

            return 0;
        }
    }
}