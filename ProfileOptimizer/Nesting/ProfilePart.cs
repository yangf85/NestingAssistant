using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting;

public class NestingOption
{
    public double Spacing { get; set; }
    public int MaxSegments { get; set; } = 10;
    public int PopulationSize { get; set; } = 50;
    public int Generations { get; set; } = 100;
    public double MutationRate { get; set; } = 0.1;
}

public class ProfilePart
{
    public ProfilePart(string category, string label, int piece, double length)
    {
        Category = category;
        Label = label;
        Piece = piece;
        Length = length;
    }

    public string Category { get; set; }
    public string Label { get; set; }
    public int Piece { get; set; }
    public double Length { get; set; }
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

public class PlacedProfilePart : IEquatable<PlacedProfilePart>, IComparable<PlacedProfilePart>
{
    public string Category { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Piece { get; set; }
    public double Length { get; set; }

    public int CompareTo(PlacedProfilePart? other)
    {
        if (other is null)
        {
            return 1;
        }

        int categoryComparison = string.Compare(Category, other.Category, StringComparison.Ordinal);
        if (categoryComparison != 0)
        {
            return categoryComparison;
        }

        int labelComparison = string.Compare(Label, other.Label, StringComparison.Ordinal);
        if (labelComparison != 0)
        {
            return labelComparison;
        }

        int lengthComparison = Length.CompareTo(other.Length);
        if (lengthComparison != 0)
        {
            return lengthComparison;
        }

        return Piece.CompareTo(other.Piece);
    }

    public bool Equals(PlacedProfilePart? other)
    {
        if (other is null) return false;
        return Category == other.Category &&
               Label == other.Label &&
               Piece == other.Piece &&
               Math.Abs(Length - other.Length) < 1e-10;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PlacedProfilePart);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Category.GetHashCode();
            hash = hash * 23 + Label.GetHashCode();
            hash = hash * 23 + Piece.GetHashCode();
            long lengthBits = BitConverter.DoubleToInt64Bits(Length);
            hash = hash * 23 + lengthBits.GetHashCode();
            return hash;
        }
    }
}

public class ProfileNestingResult : IEquatable<ProfileNestingResult>
{
    public string Category { get; set; }
    public int Piece { get; set; }
    public double Length { get; set; }
    public double Spacing { get; set; }
    public List<PlacedProfilePart> Parts { get; set; } = new List<PlacedProfilePart>();

 
    public double RemainLength => CalculateRemainLength();
    public double Utilization => CalculateUtilization();

    public ProfileNestingResult()
    {
        Parts = new List<PlacedProfilePart>();
    }

    public void Merge()
    {
        var parts = Parts.GroupBy(i => new { i.Category, i.Label, i.Length })
                         .Select(g =>
                         {
                             var first = g.First();
                             first.Piece = g.Sum(i => i.Piece);
                             return first;
                         }).ToList();

        Parts = parts;
    }

    public override string ToString()
    {
        if (Parts.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        builder.AppendLine($"Category: {Category}, Piece: {Piece}, Length: {Length}, RemainLength: {RemainLength}");
        foreach (var item in Parts)
        {
            builder.AppendLine($"  Part Label: {item.Label}, Piece: {item.Piece}, Length: {item.Length}");
        }
        builder.AppendLine($"Utilization: {Utilization:P}");
        return builder.ToString();
    }

    private double CalculateRemainLength()
    {
        if (Parts.Count == 0)
        {
            return Length;
        }

        var partLength = Parts.Sum(i => i.Length * i.Piece);
        var spacingLength = (Parts.Count - 1) * Spacing;
        return Length - partLength - spacingLength;
    }

    private double CalculateUtilization()
    {
        if (Parts.Count == 0)
        {
            return 0;
        }

        return Parts.Sum(i => i.Length * i.Piece) / Length;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ProfileNestingResult);
    }

    public bool Equals(ProfileNestingResult other)
    {
        if (other == null)
        {
            return false;
        }

        if (Category != other.Category || Length != other.Length)
        {
            return false;
        }

        Parts.Sort();
        other.Parts.Sort();
        return Parts.SequenceEqual(other.Parts);
    }

    public override int GetHashCode()
    {
        int hashCode = Category.GetHashCode();
        hashCode = (hashCode * 397) ^ Length.GetHashCode();

        foreach (var part in Parts)
        {
            hashCode = (hashCode * 397) ^ part.GetHashCode();
        }

        return hashCode;
    }
}

public class ProfileNester
{
    private readonly Random _random = new Random();

    public List<ProfileNestingResult> Nest(List<ProfileMaterial> materials, List<ProfilePart> parts, NestingOption option)
    {
        Validate(materials, parts, option);

        var materialGroups = materials.GroupBy(m => m.Category).ToDictionary(g => g.Key, g => g.ToList());
        var partGroups = parts.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.ToList());

        var results = new List<ProfileNestingResult>();

        foreach (var category in partGroups.Keys)
        {
            var materialList = materialGroups[category];
            var partList = partGroups[category];

            var bestSolution = RunGeneticAlgorithm(materialList, partList, option);

            results.AddRange(bestSolution);
        }

        return MergeResults(results);
    }

    private List<ProfileNestingResult> RunGeneticAlgorithm(List<ProfileMaterial> materials, List<ProfilePart> parts, NestingOption option)
    {
        var population = InitializePopulation(materials, parts, option);

        for (var generation = 0; generation < option.Generations; generation++)
        {
            var newPopulation = new List<List<ProfileNestingResult>>();

            for (var i = 0; i < option.PopulationSize / 2; i++)
            {
                var parent1 = Select(population);
                var parent2 = Select(population);

                var children = Crossover(parent1, parent2, option);
                newPopulation.Add(Mutate(children[0], option));
                newPopulation.Add(Mutate(children[1], option));
            }

            population = newPopulation;
        }

        return population.OrderByDescending(solution => CalculateFitness(solution)).First();
    }

    private List<List<ProfileNestingResult>> InitializePopulation(List<ProfileMaterial> materials, List<ProfilePart> parts, NestingOption option)
    {
        var population = new List<List<ProfileNestingResult>>();

        for (var i = 0; i < option.PopulationSize; i++)
        {
            var solution = new List<ProfileNestingResult>();
            var materialCounts = materials.ToDictionary(m => m, m => m.Piece);

            foreach (var material in materials)
            {
                while (materialCounts[material] > 0)
                {
                    var nestingResult = new ProfileNestingResult
                    {
                        Category = material.Category,
                        Piece = 1,
                        Length = material.Length,
                        Spacing = option.Spacing
                    };

                    var assignedParts = parts.Where(p => p.Category == material.Category).OrderBy(p => _random.Next()).ToList();
                    var remainingLength = material.Length;
                    var segments = 0;
                    var partCounts = assignedParts.ToDictionary(p => p, p => p.Piece);

                    foreach (var part in assignedParts)
                    {
                        while (segments < option.MaxSegments && remainingLength >= (part.Length + option.Spacing) && partCounts[part] > 0)
                        {
                            nestingResult.Parts.Add(new PlacedProfilePart
                            {
                                Category = part.Category,
                                Label = part.Label,
                                Piece = 1,
                                Length = part.Length
                            });

                            remainingLength -= (part.Length + option.Spacing);
                            partCounts[part] -= 1;
                            segments++;
                        }
                    }

                    if (nestingResult.Parts.Any())
                    {
                        solution.Add(nestingResult);
                        materialCounts[material] -= 1;
                    }
                }
            }

            population.Add(solution);
        }

        return population;
    }

    private List<ProfileNestingResult> Select(List<List<ProfileNestingResult>> population)
    {
        var totalFitness = population.Sum(solution => CalculateFitness(solution));
        var randomValue = _random.NextDouble() * totalFitness;

        var runningSum = 0.0;
        foreach (var solution in population)
        {
            runningSum += CalculateFitness(solution);
            if (runningSum >= randomValue)
            {
                return solution;
            }
        }

        return population.Last();
    }

    private List<List<ProfileNestingResult>> Crossover(List<ProfileNestingResult> parent1, List<ProfileNestingResult> parent2, NestingOption option)
    {
        var children = new List<List<ProfileNestingResult>>();

        var count = parent1.Count;
        if (count < 2)
        {
            children.Add(parent1);
            children.Add(parent2);
            return children;
        }

        var crossoverPoint1 = _random.Next(1, count);
        var crossoverPoint2 = _random.Next(crossoverPoint1, count);

        var child1 = parent1.Take(crossoverPoint1)
            .Concat(parent2.Skip(crossoverPoint1).Take(crossoverPoint2 - crossoverPoint1))
            .Concat(parent1.Skip(crossoverPoint2)).ToList();

        var child2 = parent2.Take(crossoverPoint1)
            .Concat(parent1.Skip(crossoverPoint1).Take(crossoverPoint2 - crossoverPoint1))
            .Concat(parent2.Skip(crossoverPoint2)).ToList();

        children.Add(child1);
        children.Add(child2);

        return children;
    }

    private List<ProfileNestingResult> Mutate(List<ProfileNestingResult> solution, NestingOption option)
    {
        if (_random.NextDouble() < option.MutationRate)
        {
            var materialIndex = _random.Next(solution.Count);
            var material = solution[materialIndex];

            if (material.Parts.Count > 1)
            {
                var partIndex1 = _random.Next(material.Parts.Count);
                var partIndex2 = _random.Next(material.Parts.Count);

                var temp = material.Parts[partIndex1];
                material.Parts[partIndex1] = material.Parts[partIndex2];
                material.Parts[partIndex2] = temp;
            }
        }

        return solution;
    }

    private double CalculateFitness(List<ProfileNestingResult> solution)
    {
        return solution.Average(result => result.Utilization);
    }

    private void Validate(List<ProfileMaterial> materials, List<ProfilePart> parts, NestingOption option)
    {
        var materialGroups = materials.GroupBy(m => m.Category).ToDictionary(g => g.Key, g => g.ToList());
        var partGroups = parts.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var category in partGroups.Keys)
        {
            if (!materialGroups.ContainsKey(category))
            {
                throw new Exception($"缺少类别为{category}的原材料");
            }

            var maxMaterial = materialGroups[category].MaxBy(i => i.Length);
            var maxPart = partGroups[category].MaxBy(i => i.Length);

            if (maxMaterial.Length < maxPart.Length)
            {
                throw new Exception($"类别{category}的最大原材料长度{maxMaterial.Length}小于最大型材长度{maxPart.Length}");
            }

            var materialLength = materialGroups[category].Sum(i => i.Length * i.Piece);
            var partLength = partGroups[category].Sum(i => (i.Length + option.Spacing) * i.Piece);
            if (materialLength < partLength)
            {
                throw new Exception($"类别{category}的原材料总长度{materialLength}小于型材总长度{partLength}");
            }
        }
    }

    private List<ProfileNestingResult> MergeResults(List<ProfileNestingResult> results)
    {
        return results.GroupBy(r => r)
                      .Select(g =>
                      {
                          var mergedResult = g.First();
                          mergedResult.Piece = g.Count();
                          mergedResult.Merge();
                          return mergedResult;
                      })
                      .ToList();
    }
}

