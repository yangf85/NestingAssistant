using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp;

namespace ProfileOptimizer.Nesting;

public struct ProfilePart
{
    public string Label { get; set; }
    public double Length { get; set; }
    public int Piece { get; set; }
    public string Type { get; set; }
}

public struct ProfileMaterial
{
    public ProfileMaterial()
    {
        Parts = [];
    }

    public double Length { get; set; }
    public List<ProfilePart> Parts { get; set; }
    public int Piece { get; set; }
    public string Type { get; set; }
    public double Utilization { get; set; }
}

public class ProfileNestingOption
{
    //迭代次数
    public int Generations { get; set; } = 100;

    //最大切割数
    public int MaxSegments { get; set; } = 10;

    //变异率
    public float MutationRate { get; set; } = 0.1f;

    //种群大小
    public int PopulationSize { get; set; } = 50;

    //切割间隙
    public double Spacing { get; set; } = 5d;
}

public class ProfileNestingSummary
{
    public double Length { get; set; }
    public int Piece { get; set; }
    public double RemainLength { get; set; }
    public double ToltalLength { get; set; }
    public string Type { get; set; }
    public double Utilization { get; set; }
}

public class ProfileNester
{
}