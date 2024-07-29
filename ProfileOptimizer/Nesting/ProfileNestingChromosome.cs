using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public class ProfileNestingChromosome : ChromosomeBase
{
    public ProfileNestingChromosome(int numberOfParts) : base(numberOfParts)
    {
        // Create genes with random sequence of parts
        for (int i = 0; i < numberOfParts; i++)
        {
            ReplaceGene(i, new Gene(i));
        }
    }

    public override IChromosome CreateNew()
    {
        return new ProfileNestingChromosome(Length);
    }

    public override Gene GenerateGene(int geneIndex)
    {
        return new Gene(geneIndex);
    }

    public override IChromosome Clone()
    {
        var clone = base.Clone() as ProfileNestingChromosome;
        return clone;
    }

    public int[] GetPartSequence()
    {
        return GetGenes().Select(g => (int)g.Value).ToArray();
    }
}