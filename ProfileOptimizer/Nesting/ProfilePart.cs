using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public class ProfileNestingOption
{
    //切割间隙
    public double Spacing { get; set; } = 5d;

    //最大切割数
    public int MaxSegments { get; set; } = 10;

    //种群大小
    public int PopulationSize { get; set; } = 50;

    //迭代次数
    public int Generations { get; set; } = 100;

    //变异率
    public float MutationRate { get; set; } = 0.1f;
}

public class ProfilePart
{
    public string Type { get; set; }

    public string Label { get; set; }

    public int Piece { get; set; }

    public double Length { get; set; }
}

public class ProfileMaterial
{
    public string Type { get; set; }

    public int Piece { get; set; }

    public double Length { get; set; }
}

public class UsageProfilePart
{
    public bool IsUsage { get; set; }
    public string Type { get; set; }

    public string Label { get; set; }

    public int Piece { get; set; }

    public double Length { get; set; }
}

public class UsageProfileMaterial
{
    public bool IsUsage { get; set; }
    public string Type { get; set; }

    public int Piece { get; set; }

    public double Length { get; set; }

    public string NestingPlan => BuildNestingPlan();

    public List<UsageProfilePart> Parts { get; set; } = [];

    public double Utilization { get; set; }

    private string BuildNestingPlan()
    {
        if (Parts.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        builder.AppendLine($"{Type}:{Length}={Piece}");
        foreach (var item in Parts)
        {
            builder.Append($"{item.Label}:{item.Length}={item.Piece} ");
        }

        return builder.ToString();
    }
}

public class ProfileNestingSummary
{
    public string Type { get; set; }

    public double Length { get; set; }

    public int Piece { get; set; }

    public double ToltalLength { get; set; }

    public double Utilization { get; set; }

    public double RemainLength { get; set; }
}

public class ProfileNestingResult
{
    public List<UsageProfileMaterial> Materials { get; set; } = [];

    public List<ProfileNestingSummary> Summaries { get; set; } = [];
}

public class ProfileNester
{
}