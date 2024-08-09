using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingOption
    {
        public int MaxSegments { get; set; } = 5;

        public double Spacing { get; set; } = 5;

        public bool IsShowPartIndex { get; set; } = false;//在切割方案中显示零件序号
    }
}