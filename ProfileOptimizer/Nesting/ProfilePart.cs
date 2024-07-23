using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting;

public class NestingOption
{
    // 切割缝隙
    public double Spacing { get; set; }

    // 每根型材最多切割多少段
    public int MaxSegments { get; set; }

    // 遗传算法的参数
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
        Parts = new List<PlacedProfilePart>();
    }

    public override string ToString()
    {
        if (Parts.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        builder.AppendLine($"Category: {Category}, Piece: {Piece}, Length: {Length}");
        foreach (var item in Parts)
        {
            builder.AppendLine($"  Part Label: {item.Label}, Piece: {item.Piece}, Length: {item.Length}");
        }
        builder.AppendLine($"Utilization: {Utilization:P}");
        return builder.ToString();
    }

    public double Utilization => CalculateUtilization();

    private double CalculateUtilization()
    {
        if (Parts.Count == 0)
        {
            return 0;
        }

        return Parts.Sum(i => i.Length * i.Piece) / Length;
    }
}

public class ProfileNester
{
    private readonly NestingOption _option;
    private readonly Random _random;
    private int currentGeneration;

    public ProfileNester(NestingOption option)
    {
        _option = option;
        _random = new Random();
        currentGeneration = 0;
    }

    public List<ProfileNestingResult> Nest(List<ProfileMaterial> materials, List<ProfilePart> parts)
    {
        Validate(materials, parts, _option);

        var population = InitializePopulation(_option.PopulationSize, parts);

        for (currentGeneration = 0; currentGeneration < _option.Generations; currentGeneration++)
        {
            population = EvolvePopulation(population, materials, _option.MutationRate);
        }

        var bestIndividual = population.MaxBy(p => CalculateFitness(p.Value, materials));
        return ConvertToNestingResults(bestIndividual.Value, materials);
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

    private Dictionary<int, List<PlacedProfilePart>> InitializePopulation(int populationSize, List<ProfilePart> parts)
    {
        var population = new Dictionary<int, List<PlacedProfilePart>>();
        for (int i = 0; i < populationSize; i++)
        {
            var individual = new List<PlacedProfilePart>();
            foreach (var part in parts)
            {
                for (int j = 0; j < part.Piece; j++)
                {
                    individual.Add(new PlacedProfilePart
                    {
                        Category = part.Category,
                        Label = part.Label,
                        Piece = 1,
                        Length = part.Length
                    });
                }
            }
            // Randomize order of parts
            individual = individual.OrderBy(p => _random.Next()).ToList();
            population.Add(i, individual);
        }
        return population;
    }

    private Dictionary<int, List<PlacedProfilePart>> EvolvePopulation(Dictionary<int, List<PlacedProfilePart>> population, List<ProfileMaterial> materials, double mutationRate)
    {
        var newPopulation = new Dictionary<int, List<PlacedProfilePart>>();
        int newIndividualId = 0;

        var bestIndividual = population.MaxBy(p => CalculateFitness(p.Value, materials));
        newPopulation.Add(newIndividualId++, bestIndividual.Value); // Elite individual

        var populationList = population.Values.ToList();
        for (int i = 0; i < (populationList.Count - 1) / 2; i++)
        {
            var parent1 = SelectIndividual(populationList, materials);
            var parent2 = SelectIndividual(populationList, materials);

            var offspring1 = Crossover(parent1, parent2);
            var offspring2 = Crossover(parent2, parent1);

            Mutate(offspring1, mutationRate);
            Mutate(offspring2, mutationRate);

            newPopulation.Add(newIndividualId++, offspring1);
            newPopulation.Add(newIndividualId++, offspring2);
        }

        return newPopulation;
    }

    private List<PlacedProfilePart> SelectIndividual(List<List<PlacedProfilePart>> population, List<ProfileMaterial> materials)
    {
        var tournamentSize = 5;
        var tournament = new List<List<PlacedProfilePart>>();

        for (int i = 0; i < tournamentSize; i++)
        {
            var randomIndex = _random.Next(population.Count);
            tournament.Add(population[randomIndex]);
        }

        return tournament.MaxBy(individual => CalculateFitness(individual, materials));
    }

    private List<PlacedProfilePart> Crossover(List<PlacedProfilePart> parent1, List<PlacedProfilePart> parent2)
    {
        var crossoverPoint1 = _random.Next(parent1.Count);
        var crossoverPoint2 = _random.Next(crossoverPoint1, parent1.Count);
        var offspring = parent1.Take(crossoverPoint1)
                              .Concat(parent2.Skip(crossoverPoint1).Take(crossoverPoint2 - crossoverPoint1))
                              .Concat(parent1.Skip(crossoverPoint2)).ToList();
        return offspring;
    }

    private void Mutate(List<PlacedProfilePart> individual, double mutationRate)
    {
        double adaptiveMutationRate = mutationRate * (1 - (double)currentGeneration / _option.Generations);
        for (int i = 0; i < individual.Count; i++)
        {
            if (_random.NextDouble() < adaptiveMutationRate)
            {
                var randomIndex = _random.Next(individual.Count);
                var temp = individual[i];
                individual[i] = individual[randomIndex];
                individual[randomIndex] = temp;
            }
        }
    }

    private double CalculateFitness(List<PlacedProfilePart> individual, List<ProfileMaterial> materials)
    {
        double fitness = 0.0;

        var materialGroups = materials.GroupBy(m => m.Category).ToDictionary(g => g.Key, g => g.ToList());
        var partGroups = individual.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var category in partGroups.Keys)
        {
            if (!materialGroups.ContainsKey(category))
            {
                continue;
            }

            double totalUsedLength = 0.0;
            double totalMaterialLength = materialGroups[category].Sum(m => m.Length * m.Piece);

            foreach (var part in partGroups[category])
            {
                totalUsedLength += (part.Length + _option.Spacing);
            }

            // Ensure fitness value is normalized and reflects utilization rate
            fitness += totalUsedLength / totalMaterialLength;
        }

        return fitness;
    }

    private List<ProfileNestingResult> ConvertToNestingResults(List<PlacedProfilePart> individual, List<ProfileMaterial> materials)
    {
        var results = new List<ProfileNestingResult>();

        var individualGroups = individual.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var material in materials)
        {
            var remainingLength = material.Length;
            var segmentCount = 0;
            var placedParts = new List<PlacedProfilePart>();

            if (individualGroups.ContainsKey(material.Category))
            {
                foreach (var part in individualGroups[material.Category])
                {
                    var partLengthWithSpacing = part.Length + _option.Spacing;

                    if (remainingLength >= partLengthWithSpacing && segmentCount < _option.MaxSegments)
                    {
                        placedParts.Add(part);
                        remainingLength -= partLengthWithSpacing;
                        segmentCount++;
                    }
                }
            }

            var result = new ProfileNestingResult
            {
                Category = material.Category,
                Piece = material.Piece,
                Length = material.Length,
                Parts = placedParts
            };

            results.Add(result);
        }

        return results;
    }
}