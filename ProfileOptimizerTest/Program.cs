// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using GeneticSharp;
using ProfileOptimizer.Nesting;

var materials = new List<Material>
        {
            new Material { Length = 100, Piece = 3 },
            new Material { Length = 200, Piece = 2 }
        };

var parts = new List<Part>
        {
            new Part { Length = 30, Piece = 4 },
            new Part { Length = 50, Piece = 5 },
            new Part { Length = 70, Piece = 2 }
        };

var plans = Nester.Nest(materials, parts);

foreach (var plan in plans)
{
    Console.WriteLine($"Material Length: {plan.Material.Length}, Piece: {plan.Material.Piece}");
    var cuts = plan.Parts
        .GroupBy(p => p.Length)
        .Select(g => new Cut { Length = g.Key, Count = g.Sum(p => p.Piece) })
        .ToList();

    Console.WriteLine($"  Cutting Plan: {string.Join(", ", cuts.Select(c => $"{c.Length}x{c.Count}"))}");
}

internal class Nester
{
    public static List<NestingPlan> Nest(List<Material> materials, List<Part> parts)
    {
        var chromosome = new NestingChromosome(materials, parts);
        var population = new Population(50, 100, chromosome);
        var fitness = new NestingFitness(materials, parts);
        var selection = new EliteSelection();
        var crossover = new UniformCrossover();
        var mutation = new TworsMutation();
        var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
        ga.Termination = new GenerationNumberTermination(100);

        ga.Start();

        var bestChromosome = ga.BestChromosome as NestingChromosome;
        return bestChromosome?.ToNestingPlans() ?? new List<NestingPlan>();
    }
}

internal class Material
{
    public double Length { get; set; }

    public int Piece { get; set; }
}

internal class Part
{
    public double Length { get; set; }

    public int Piece { get; set; }
}

internal class Cut
{
    public double Length { get; set; }

    public int Count { get; set; }
}

internal class NestingPlan
{
    public Material Material { get; set; } = new();

    public List<Part> Parts { get; set; } = [];
}

internal class NestingChromosome : ChromosomeBase
{
    private readonly List<Material> _materials;

    private readonly List<Part> _parts;

    public NestingChromosome(List<Material> materials, List<Part> parts) : base(materials.Sum(m => m.Piece))
    {
        _materials = materials;
        _parts = parts;

        // Initialize genes
        for (int i = 0; i < Length; i++)
        {
            ReplaceGene(i, GenerateGene(i));
        }
    }

    public override IChromosome CreateNew()
    {
        return new NestingChromosome(_materials, _parts);
    }

    public override Gene GenerateGene(int geneIndex)
    {
        var materialIndex = geneIndex % _materials.Count;
        var material = _materials[materialIndex];
        var partCombinations = GetCombinations(_parts, material.Length);
        var random = RandomizationProvider.Current;
        var combinationIndex = random.GetInt(0, partCombinations.Count);
        return new Gene(new NestingPlan { Material = material, Parts = partCombinations[combinationIndex] });
    }

    public List<NestingPlan> ToNestingPlans()
    {
        var plans = new List<NestingPlan>();

        for (int i = 0; i < Length; i++)
        {
            var nestingPlan = (NestingPlan)GetGene(i).Value;
            plans.Add(nestingPlan);
        }

        return plans;
    }

    private List<List<Part>> GetCombinations(List<Part> parts, double maxLength)
    {
        var result = new List<List<Part>>();
        GetCombinationsRecursive(parts, new List<Part>(), 0, maxLength, result);
        return result;
    }

    private void GetCombinationsRecursive(List<Part> parts, List<Part> current, int index, double maxLength, List<List<Part>> result)
    {
        double totalLength = current.Sum(p => p.Length * p.Piece);

        if (totalLength <= maxLength)
        {
            result.Add(new List<Part>(current));
        }

        if (index >= parts.Count || totalLength >= maxLength)
        {
            return;
        }

        for (int i = 0; i <= parts[index].Piece; i++)
        {
            var newPart = new Part { Length = parts[index].Length, Piece = i };
            current.Add(newPart);
            GetCombinationsRecursive(parts, current, index + 1, maxLength, result);
            current.RemoveAt(current.Count - 1);
        }
    }
}

internal class NestingFitness : IFitness
{
    private readonly List<Material> _materials;

    private readonly List<Part> _parts;

    public NestingFitness(List<Material> materials, List<Part> parts)
    {
        _materials = materials;
        _parts = parts;
    }

    public double Evaluate(IChromosome chromosome)
    {
        var nestingChromosome = chromosome as NestingChromosome;
        var plans = nestingChromosome?.ToNestingPlans();

        if (plans == null) return 0;

        double totalWaste = 0;
        var partCounts = new Dictionary<double, int>();

        foreach (var part in _parts)
        {
            partCounts[part.Length] = part.Piece;
        }

        foreach (var plan in plans)
        {
            double usedLength = plan.Parts.Sum(p => p.Length * p.Piece);
            double waste = plan.Material.Length - usedLength;
            totalWaste += Math.Max(waste, 0);

            foreach (var part in plan.Parts)
            {
                if (partCounts.ContainsKey(part.Length))
                {
                    partCounts[part.Length] -= part.Piece;
                    if (partCounts[part.Length] < 0)
                    {
                        totalWaste += Math.Abs(partCounts[part.Length]) * part.Length; // Penalty for overuse
                    }
                }
            }
        }

        // Penalty for underuse of parts
        foreach (var count in partCounts.Values)
        {
            if (count > 0)
            {
                totalWaste += count;
            }
        }

        return 1.0 / (1 + totalWaste); // Minimize waste
    }
}