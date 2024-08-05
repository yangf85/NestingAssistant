using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingResult
    {
        public ProfileMaterial Material { get; set; } = new ProfileMaterial();

        public List<ProfilePart> Parts { get; set; } = [];
    }
}