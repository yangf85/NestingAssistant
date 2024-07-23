using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting;


public class NestingOption
{
    //切割缝隙
    public double Spacing { get; set; }

    //每根型材最多切割多少段
    public int MaxSegments { get; set; }
}

public class ProfilePart : IEquatable<ProfilePart>
{
    public ProfilePart(string category, string label, int piece, double length)
    {
        Category = category;
        Label = label;
        Piece = piece;
        Length = length;
    }

    public string Category { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Piece { get; set; }
    public double Length { get; set; }

    public bool Equals(ProfilePart? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return (Category, Length) == (other.Category, other.Length);
    }

    public override bool Equals(object obj)
    {
        return obj is ProfilePart part && Equals(part);
        
    }

    public override int GetHashCode()
    {
        var hashCode = 17;
        hashCode = hashCode ^ Category.GetHashCode() ^ Length.GetHashCode();
        return hashCode;
    }
}


public class ProfileMaterial
{
    public ProfileMaterial(string category, int piece, double length)
    {
        Category = category;
        Piece = piece;
        Length = length;
    }

    public string Category { get; set; }
    public int Piece { get; set; }
    public double Length { get; set; }
    
}

public class PlacedProfilePart
{
    public string Category { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Piece { get; set; }
    public double Length { get; set; }
}

public class ProfileNestingResult
{
    public string Category { get; set; }
    public int Piece { get; set; }
    public double Length { get; set; }

    public List<PlacedProfilePart> Parts { get; set; }

    public ProfileNestingResult()
    {
        Parts = [];
    }

    public double Utilization => CalculateUtilization();

    public override string ToString()
    {
        if (Parts.Count == 0) return string.Empty;

        var builder = new StringBuilder();

        foreach (var item in Parts)
        {
            builder.AppendLine($"({item.Length}={item.Piece}) ");
        }

        return builder.ToString();
    }

    double CalculateUtilization()
    {
        if(Parts.Count == 0)
        {
            return 0;
        }

        return Parts.Sum(i => i.Length) / Length;

    }
}


public class GeneticProfileNester
{
 
    private void Validate(List<ProfileMaterial> materials, List<ProfilePart> parts, NestingOption option)
    {
        var maxMaterial = materials.MaxBy(i => i.Length);
        var maxPart = parts.MaxBy(i => i.Length);
        if (maxMaterial.Length < maxPart.Length)
        {
            throw new Exception($"最大原材料长度{maxMaterial.Length}小于最大型材长度{maxPart.Length}");
        }

        var materialLength = materials.Sum(i => i.Length * i.Piece);
        var partLength = parts.Sum(i => (i.Length + option.Spacing) * i.Piece);
        if (materialLength < partLength)
        {
            throw new Exception($"原材料总长度{materialLength}小于型材总长度{partLength}");
        }
    }

  
}

