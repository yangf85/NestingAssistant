using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer
{
    public static class NumberExtension
    {
        public static bool AreApproximatelyEqual(this double self,double other, double tolerance = 1e-6)
        {
            return Math.Abs(self - other) < tolerance;
        }
    }
}
