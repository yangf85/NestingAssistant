using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class GreedyProfileNestingResult
    {
        public UsageProfileMaterial Material { get; set; } = new();

        public List<UsageProfilePart> Parts { get; set; } = new();

        public override string ToString()
        {
            if (Parts.Count != 0)
            {
                return $"Type: {Material.Type} {Material.Length} Parts:{Parts.Sum(i => i.Length)} {string.Join("+", Parts.Select(p => p.Length))}";
            }
            return string.Empty;
        }
    }
}