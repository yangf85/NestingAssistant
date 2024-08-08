using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class UsageProfileMaterial : IComparable<UsageProfileMaterial>
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public double Length { get; set; }

        public bool IsUsed { get; set; }

        public int CompareTo(UsageProfileMaterial? other)
        {
            if (other is null)
            {
                return 1;
            }

            //先按Type比较
            int typeComparison = string.Compare(Type, other.Type, StringComparison.Ordinal);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            //最后按Length比较
            return Length.CompareTo(other.Length);
        }
    }
}