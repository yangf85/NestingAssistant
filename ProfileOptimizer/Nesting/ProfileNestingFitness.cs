using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public class ProfileNestingFitness : IFitness
{
    private readonly List<ProfilePart> _parts;

    private readonly List<ProfileMaterial> _materials;

    private readonly ProfileNestingOption _options;

    public ProfileNestingFitness(List<ProfilePart> parts, List<ProfileMaterial> materials, ProfileNestingOption options)
    {
        _parts = parts;
        _materials = materials;
        _options = options;
    }

    public double Evaluate(IChromosome chromosome)
    {
        var profileChromosome = chromosome as ProfileNestingChromosome;
        var partSequence = profileChromosome.GetPartSequence();

        // 初始化使用材料列表
        var usageMaterials = new List<UsageProfileMaterial>();
        foreach (var material in _materials)
        {
            for (int i = 0; i < material.Piece; i++)
            {
                usageMaterials.Add(new UsageProfileMaterial
                {
                    Type = material.Type,
                    Piece = 1,
                    Length = material.Length,
                    Parts = new List<UsageProfilePart>(),
                    Utilization = 0
                });
            }
        }

        // 套裁逻辑
        foreach (var partIndex in partSequence)
        {
            var part = _parts[partIndex];
            var material = usageMaterials.FirstOrDefault(m => m.Type == part.Type && m.Piece > 0);
            if (material == null)
            {
                // 如果没有找到匹配的材料，则该解无效，适应度为0
                return 0;
            }

            var totalLength = material.Parts.Sum(p => p.Length) + (material.Parts.Count > 0 ? (material.Parts.Count - 1) * _options.Spacing : 0);
            if (totalLength + part.Length <= material.Length && material.Parts.Count < _options.MaxSegments)
            {
                material.Parts.Add(new UsageProfilePart
                {
                    Type = part.Type,
                    Label = part.Label,
                    Piece = part.Piece,
                    Length = part.Length
                });
                material.Utilization = (totalLength + part.Length) / material.Length;
            }
            else
            {
                // 如果当前材料不足以切割此零件，则换下一根材料
                material = usageMaterials.FirstOrDefault(m => m.Type == part.Type && m.Piece > 0 &&
                    (m.Parts.Sum(p => p.Length) + part.Length + (m.Parts.Count * _options.Spacing - _options.Spacing) <= m.Length));
                if (material == null)
                {
                    return 0;
                }
                material.Parts.Add(new UsageProfilePart
                {
                    Type = part.Type,
                    Label = part.Label,
                    Piece = 1,
                    Length = part.Length
                });
                var newTotalLength = material.Parts.Sum(p => p.Length) + (material.Parts.Count - 1) * _options.Spacing;
                material.Utilization = newTotalLength / material.Length;
            }
        }

        // 计算总剩余长度
        double totalRemainingLength = usageMaterials.Sum(m => m.Length - (m.Parts.Sum(p => p.Length) + (m.Parts.Count > 0 ? (m.Parts.Count - 1) * _options.Spacing : 0)));

        // 剩余长度越少，适应度越高
        return 1.0 / (totalRemainingLength + 1); // +1 避免除零
    }
}