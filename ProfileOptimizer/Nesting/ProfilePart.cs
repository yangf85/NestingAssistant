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
    public string Type { get; set; }

    public string Label { get; set; }

    public int Piece { get; set; }

    public double Length { get; set; }
}

public class UsageProfileMaterial
{
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
    public ProfileNestingResult Optimize(ProfileNestingOption options, List<ProfilePart> parts, List<ProfileMaterial> materials)
    {
        var chromosome = new ProfileChromosome(parts, materials);
        var population = new Population(options.PopulationSize, options.PopulationSize * 2, chromosome);

        var fitness = new ProfileFitness(parts, materials, options);
        var selection = new EliteSelection();
        var crossover = new UniformCrossover(0.5f);
        var mutation = new UniformMutation();
        var termination = new GenerationNumberTermination(options.Generations);

        var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
        {
            Termination = termination,
            MutationProbability = options.MutationRate
        };

        ga.GenerationRan += GenerationRan;

        ga.Start();

        var bestChromosome = ga.BestChromosome as ProfileChromosome;
        return TranslateChromosomeToResult(bestChromosome);
    }

    private void GenerationRan(object sender, EventArgs e)
    {
        var ga = sender as GeneticAlgorithm;
        var bestFitness = ga.BestChromosome.Fitness;
        Console.WriteLine($"Generation: {ga.GenerationsNumber}, Best Fitness: {bestFitness}");
    }

    private ProfileNestingResult TranslateChromosomeToResult(ProfileChromosome chromosome)
    {
        var result = new ProfileNestingResult();
        var usedMaterials = chromosome.GetMaterials();
        var summaries = chromosome.GetSummaries();

        result.Materials.AddRange(usedMaterials);
        result.Summaries.AddRange(summaries);

        return result;
    }

    private class ProfileChromosome : ChromosomeBase
    {
        private List<ProfilePart> _parts;

        private List<ProfileMaterial> _materials;

        private Random _random;

        public ProfileChromosome(List<ProfilePart> parts, List<ProfileMaterial> materials) : base(parts.Sum(p => p.Piece))
        {
            _parts = parts;
            _materials = materials;
            _random = new Random();

            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            var validMaterials = _materials.Where(m => m.Type == _parts[geneIndex % _parts.Count].Type).ToList();
            var materialIndex = _random.Next(0, validMaterials.Count);
            var partIndex = geneIndex % _parts.Count;
            return new Gene(new { MaterialIndex = materialIndex, PartIndex = partIndex });
        }

        public override IChromosome CreateNew()
        {
            return new ProfileChromosome(_parts, _materials);
        }

        public List<UsageProfileMaterial> GetMaterials()
        {
            var materialUsages = _materials.Select(material => new UsageProfileMaterial
            {
                Type = material.Type,
                Length = material.Length,
                Piece = material.Piece,
                Parts = new List<UsageProfilePart>()
            }).ToList();

            foreach (var gene in GetGenes())
            {
                var geneValue = (dynamic)gene.Value;
                var material = materialUsages[geneValue.MaterialIndex];
                var part = _parts[geneValue.PartIndex];

                if (material.Type == part.Type)
                {
                    material.Parts.Add(new UsageProfilePart
                    {
                        Type = part.Type,
                        Label = part.Label,
                        Length = part.Length,
                        Piece = 1
                    });

                    material.Piece--;
                }
            }

            materialUsages = materialUsages.Where(m => m.Parts.Any()).ToList();

            foreach (var material in materialUsages)
            {
                material.Utilization = CalculateUtilization(material);
            }

            return materialUsages;
        }

        public List<ProfileNestingSummary> GetSummaries()
        {
            return GetMaterials().Select(material => new ProfileNestingSummary
            {
                Type = material.Type,
                Length = material.Length,
                Piece = material.Piece,
                ToltalLength = material.Length,
                RemainLength = material.Length - material.Parts.Sum(part => part.Length + 5 * (part.Piece - 1)),
                Utilization = material.Utilization
            }).ToList();
        }

        private double CalculateUtilization(UsageProfileMaterial material)
        {
            var totalLengthUsed = material.Parts.Sum(part => part.Length) + 5 * (material.Parts.Count - 1);
            return Math.Min(totalLengthUsed / material.Length, 1.0);
        }
    }

    private class ProfileFitness : IFitness
    {
        private List<ProfilePart> _parts;

        private List<ProfileMaterial> _materials;

        private ProfileNestingOption _options;

        public ProfileFitness(List<ProfilePart> parts, List<ProfileMaterial> materials, ProfileNestingOption options)
        {
            _parts = parts;
            _materials = materials;
            _options = options;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var profileChromosome = chromosome as ProfileChromosome;
            var materials = profileChromosome.GetMaterials();

            var totalUtilization = materials.Sum(material => material.Utilization);

            foreach (var material in materials)
            {
                if (material.Parts.Count > _options.MaxSegments)
                {
                    totalUtilization -= (material.Parts.Count - _options.MaxSegments) * 0.1;
                }
            }

            return totalUtilization;
        }
    }
}