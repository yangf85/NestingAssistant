using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNestingFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            //if (chromosome is not ProfileNestingChromosome nestingChromosome)
            //{
            //    return -double.MaxValue;
            //}

            //if (nestingChromosome.NestingResult is null)
            //{
            //    return -double.MaxValue;
            //}

            //var waste = nestingChromosome.NestingResult.Material.Length - nestingChromosome.NestingResult.Parts.Sum(i => i.Length);
            //if (waste < 0)
            //{
            //    return -double.MaxValue;
            //}

            //return -waste;
            return 0;
        }
    }
}