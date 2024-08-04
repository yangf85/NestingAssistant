using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public class ProfileNestingFitness : IFitness
{
    public double Evaluate(IChromosome chromosome)
    {
        if (chromosome is not ProfileNestingChromosome nesting || nesting.Material is null)
        {
            return 0;
        }

        var utilization = nesting.Material.Utilization;
        if (utilization < 0 || utilization > 1)
        {
            return 0;
        }

        //var wasteLength = nesting.Material.Length - nesting.Material.Parts.Sum(i => i.Length);

        //if (wasteLength < 0)
        //{
        //    return 0; // 确保浪费长度不为负
        //}

        //var fitness = utilization * nesting.Material.Parts.Count - wasteLength * 0.1;

        return utilization;
    }
}