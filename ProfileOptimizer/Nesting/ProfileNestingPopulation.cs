using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingPopulation : TplPopulation
    {
        private List<ProfileMaterial> _materials;

        private List<ProfilePart> _parts;

        private ProfileNestingOption _option;

        //最小的个体数量应该和原材料长度的种类个数一致，而最大的个体数量应该和原材料的总件数一致
        public ProfileNestingPopulation(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option, int minSize, int maxSize, IChromosome adamChromosome) : base(minSize, maxSize, adamChromosome)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        public override void CreateInitialGeneration()
        {
        }
    }
}