using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    /// <summary>
    /// 适应度
    /// </summary>
    public class ProfileNestingFitness : IFitness
    {
        private List<ProfileMaterial> _materials;

        private List<ProfilePart> _parts;

        private ProfileNestingOption _option;

        public ProfileNestingFitness(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        // 适应度函数需要满足以下条件：
        // 1. 所使用的每种长度的原材料数量不能超过给定的对应长度的原材料库存。
        // 2. 每种长度的零件数量不能超过给定的对应长度的零件需求，且不能少于给定的零件需求。
        //    如果在原材料的数量不够放置所有的零件，可以少于给定的数量。
        // 3. 在每根原材料上放置的零件数量应尽可能多，但不能超过设定的最大数量。
        // 4. 每根原材料上放置的零件长度总和不能超过该原材料的长度。
        // 5. 尽可能减少使用的原材料种类。
        // 6. 尽可能减少每根原材料的剩余长度。
        // 优先级排序：
        // 1. 确保零件数量不超过需求，且在材料不足时可少于需求。
        // 2. 确保原材料数量不超过库存。
        // 3. 确保每根原材料上放置的零件长度总和不超过该原材料的长度。
        // 4. 在此基础上，按放置的零件数量、减少原材料种类和减少剩余长度进行优化。

        public double Evaluate(IChromosome chromosome)
        {
            var plans = chromosome.GetGenes().Select(i => (ProfileNestingPlan)i.Value).ToList();

            // 记录每种长度的原材料使用数量
            var materialUsage = new Dictionary<double, int>();

            // 记录每种长度的零件使用数量
            var partUsage = new Dictionary<double, int>();

            // 初始化原材料和零件使用情况
            foreach (var material in _materials)
            {
                materialUsage[material.Length] = 0;
            }
            foreach (var part in _parts)
            {
                partUsage[part.Length] = 0;
            }

            // 计算原材料和零件的使用情况
            foreach (var plan in plans)
            {
                if (materialUsage.ContainsKey(plan.Length))
                {
                    materialUsage[plan.Length]++;
                }

                double totalPartsLength = 0;
                foreach (var segment in plan.Segments)
                {
                    if (partUsage.ContainsKey(segment))
                    {
                        partUsage[segment]++;
                    }
                    totalPartsLength += segment;
                }

                // 确保每根原材料上放置的零件长度总和不超过该原材料的长度
                if (totalPartsLength > plan.Length)
                {
                    return -10000; // 违反约束条件，给予最低适应度
                }
            }

            // 确保零件使用数量不超过给定的需求
            foreach (var part in _parts)
            {
                if (partUsage[part.Length] > part.Piece)
                {
                    return -10000; // 违反约束条件，给予最低适应度
                }
            }

            // 确保原材料使用数量不超过给定的库存
            foreach (var material in _materials)
            {
                if (materialUsage[material.Length] > material.Piece)
                {
                    return -10000; // 违反约束条件，给予最低适应度
                }
            }

            // 计算适应度值
            double fitness = 0;

            // 优先级次要的条件
            // 条件 3：在每根原材料上放置的零件数量应尽可能多，但不能超过设定的最大数量
            foreach (var plan in plans)
            {
                if (plan.Segments.Count <= _option.MaxSegments)
                {
                    fitness += plan.Segments.Count * 10;
                }
            }

            // 条件 5：尽可能减少使用的原材料种类
            fitness -= materialUsage.Count(m => m.Value > 0) * 5;

            // 条件 6：尽可能减少每根原材料的剩余长度
            foreach (var plan in plans)
            {
                double totalPartsLength = plan.Segments.Sum();
                fitness -= (plan.Length - totalPartsLength);
            }

            return fitness;
        }
    }
}