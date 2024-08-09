using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    // 适应度函数需要满足以下条件：
    // 1. 所使用的每种长度的原材料数量不能超过给定的对应长度的原材料库存。
    // 2. 每种长度的零件数量不能超过给定的对应长度的零件需求，且不能少于给定的零件需求(如果在原材料的数量不够放置所有的零件，可以少于给定的数量)。
    // 3. 在每根原材料上放置的零件数量应尽可能多，但不能超过设定的最大数量。
    // 4. 每根原材料上放置的零件长度总和不能超过该原材料的长度。
    // 5. 同时尽可能减少使用原材料的总数量，同时尽可能减少每根原材料的剩余长度。
    // 6. 尽可能减少使用的原材料种类,。
    // 7. 尽可能的把相同的零件放在同一根原材料上。
    public class ProfileNestingFitness : IFitness
    {
        private const double PenaltyExceededLength = 10000;        // 超过原材料长度的惩罚权重

        private const double PenaltyExceededPartUsage = 10000;     // 超过零件需求数量的惩罚权重

        private const double PenaltyUnderPartUsage = 10000;         // 少于零件需求数量的惩罚权重

        private const double PenaltyExceededMaterialUsage = 10000; // 超过原材料库存的惩罚权重

        private const double PenaltyExceededMaxSegments = 10000;    // 超过单根原材料最大零件数量的惩罚权重

        private const double WeightRemainingLength = 1;            // 原材料剩余长度的权重

        private const double WeightSamePartsInMaterial = 10;       // 相同零件放在同一根原材料上的奖励权重

        private const double WeightMaterialVariety = 50;           // 使用原材料种类数量的权重

        private readonly List<ProfileMaterial> _materials;

        private readonly List<ProfilePart> _parts;

        private readonly ProfileNestingOption _option;

        public ProfileNestingFitness(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var plans = chromosome.GetGenes().Select(i => (GeneticProfileNestingPlan)i.Value).ToList();

            // 初始化惩罚值
            double penalty = 0;

            // 条件：在每根原材料上放置的零件数量应尽可能多，但不能超过设定的最大数量
            foreach (var plan in plans)
            {
                if (plan.Segments.Count > _option.MaxSegments)
                {
                    penalty += PenaltyExceededMaxSegments; // 超过最大数量，增加惩罚
                }
            }

            // 记录每种长度的原材料使用数量
            var materialUsage = new Dictionary<double, int>();
            foreach (var material in _materials)
            {
                materialUsage[material.Length] = 0;
            }

            // 记录每种长度的零件使用数量
            var partUsage = new Dictionary<double, int>();
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
                    penalty += PenaltyExceededLength; // 违反约束条件，增加惩罚
                }
            }

            // 确保零件使用数量不超过给定的需求，且不能少于需求（考虑到库存限制）
            foreach (var part in _parts)
            {
                if (partUsage[part.Length] > part.Piece)
                {
                    penalty += PenaltyExceededPartUsage; // 违反约束条件，增加惩罚
                }
                else if (partUsage[part.Length] < part.Piece)
                {
                    penalty += (part.Piece - partUsage[part.Length]) * PenaltyUnderPartUsage;
                }
            }

            // 确保原材料使用数量不超过给定的库存
            foreach (var material in _materials)
            {
                if (materialUsage[material.Length] > material.Piece)
                {
                    penalty += PenaltyExceededMaterialUsage; // 违反约束条件，增加惩罚
                }
            }

            // 计算适应度值
            double fitness = 0;

            // 条件4：尽可能减少每根原材料的剩余长度
            foreach (var plan in plans)
            {
                double totalPartsLength = plan.Segments.Sum();
                fitness -= (plan.Length - totalPartsLength) * WeightRemainingLength;
            }

            // 条件6：尽可能的把相同的零件放在同一根原材料上
            foreach (var plan in plans)
            {
                var segmentGroups = plan.Segments.GroupBy(s => s);
                foreach (var group in segmentGroups)
                {
                    if (group.Count() > 1)
                    {
                        fitness += group.Count() * WeightSamePartsInMaterial; // 对每组相同零件奖励相应分数
                    }
                }
            }

            // 条件7：尽可能减少使用的原材料种类
            fitness -= materialUsage.Count(m => m.Value > 0) * WeightMaterialVariety;

            // 如果原材料中包含长度为0的段，则适应度减100
            foreach (var item in plans)
            {
                if (item.Segments.Contains(0))
                {
                    fitness -= 100;
                }
            }

            // 返回适应度值，减去惩罚值
            return fitness - penalty;
        }
    }
}