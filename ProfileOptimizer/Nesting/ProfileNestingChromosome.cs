using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public class ProfileNestingChromosome : ChromosomeBase
{
    private readonly List<ProfilePart> _parts;
    private readonly List<ProfileMaterial> _materials;
    private readonly ProfileNestingOption _option;

    public ProfileMaterial Material { get; private set; }

    public ProfileNestingChromosome(List<ProfilePart> parts, List<ProfileMaterial> materials, ProfileNestingOption option) : base(option.MaxSegments)
    {
        _parts = parts;
        _materials = materials;
        _option = option;

        if (materials.Count > 0)
        {
            Material = materials[GeneticSharp.RandomizationProvider.Current.GetInt(0, materials.Count)];

            for (int i = 0; i < option.MaxSegments; i++)
            {
                var index = GeneticSharp.RandomizationProvider.Current.GetInt(0, parts.Count);
                var part = new ProfilePart()
                {
                    Label = parts[index].Label,
                    Length = parts[index].Length,
                    Type = parts[index].Type,
                    Piece = 1,
                };
                Material.Parts.Add(part);
                ReplaceGene(i, new Gene(part));
            }
        }
    }

    public override IChromosome CreateNew()
    {
        return new ProfileNestingChromosome(_parts, _materials, _option);
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
}