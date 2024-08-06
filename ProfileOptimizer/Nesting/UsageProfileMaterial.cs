using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public struct UsageProfileMaterial
    {
        public int Id { get; set; }

        public double Length { get; set; }

        public List<UsageProfilePart> Parts { get; set; }

        public double RemainLength => Length - Parts.Sum(p => p.Length);

        public UsageProfileMaterial()
        {
            Parts = [];
        }
    }
}