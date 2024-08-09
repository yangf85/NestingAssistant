using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class GreedyProfileNestingPlan : ProfileNestingPlan, IEquatable<GreedyProfileNestingPlan>
    {
        public bool Equals(GreedyProfileNestingPlan? other)
        {
            if (other is null)
            {
                return false;
            }

            return Length == other.Length && NestingPlan == other.NestingPlan;
        }

        public override bool Equals(object obj)
        {
            if (obj is GreedyProfileNestingPlan other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Length.GetHashCode();
            hash = hash * 31 + (NestingPlan?.GetHashCode() ?? 0);
            return hash;
        }

        public override string ToString()
        {
            return NestingPlan;
        }

        public void Update(GreedyProfileNestingResult result, ProfileNestingOption option)
        {
            Type = result.Material.Type;
            Length = result.Material.Length;
            RemainLength = result.Material.Length - result.Parts.Sum(p => p.Length) - (result.Parts.Count - 1) * option.Spacing;
            PartPiece = result.Parts.Count;

            NestingPlan = UpdateNestingPlan(result, option);
            Utilization = UpdateUtilization();
        }

        private static string UpdateNestingPlan(GreedyProfileNestingResult result, ProfileNestingOption option)
        {
            var groups = result.Parts
                               .GroupBy(i => i)
                               .Select(g => new
                               {
                                   g.Key.Type,
                                   g.Key.Label,
                                   g.Key.Length,
                                   g.Key.Index,
                                   PieceCount = g.Count()
                               });

            var builder = new StringBuilder(groups.Count() * 30); // 假设每个组平均需要20字符，实际可根据数据调整

            // 抽取拼接逻辑
            foreach (var group in groups)
            {
                if (option.IsShowPartIndex)
                {
                    builder.Append($"({group.Index})");
                }

                builder.Append($"{group.Length}x{group.PieceCount} ");
            }

            return builder.ToString().TrimEnd();
        }

        private double UpdateUtilization()
        {
            if (Length == 0)
            {
                return 0;
            }

            return (Length - RemainLength) / Length;
        }
    }
}