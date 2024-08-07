using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingPlan
    {
        public UsageProfileMaterial Material { get; set; } = new UsageProfileMaterial();

        public List<UsageProfilePart> Parts { get; set; } = [];
    }
}