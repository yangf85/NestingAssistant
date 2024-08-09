using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class GeneticProfileNestingPlan
    {
        public double Length { get; set; }

        public double RemainLength { get; set; }

        public List<double> Segments { get; set; } = [];

        public override string ToString()
        {
            if (Segments.Count == 0)
            {
                return "Empty";
            }
            return $"{Length}:{Segments.Sum()}={string.Join("+", Segments.Select(i => i))}";
        }
    }
}