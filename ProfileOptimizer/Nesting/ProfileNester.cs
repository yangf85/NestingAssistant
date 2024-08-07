using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNester
    {
        private readonly List<ProfileMaterial> _materials;

        private readonly List<ProfilePart> _parts;

        private readonly ProfileNestingOption _option;

        public ProfileNester(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        public List<ProfileNestingPlan> Nest()
        {
            return null;
        }
    }
}