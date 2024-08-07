using GeneticSharp;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public class ProfileNester
    {
        private readonly List<ProfileMaterial> _materials;

        private readonly List<ProfilePart> _parts;

        private readonly ProfileNestingOption _option;

        public ProfileNester(List<ProfileMaterial> materials, List<ProfilePart> parts, ProfileNestingOption option)
        {
            _materials = materials;
            _parts = parts;
            _option = option;
        }

        public void Nest()
        {
            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation(true);
            var fitness = new ProfileNestingFitness(_materials, _parts, _option);
            var chromosome = new ProfileNestingChromosome(_materials, _parts, _option);
            var population = new Population(50, 500, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new OrTermination(
                    new FitnessThresholdTermination(0),
                    new GenerationNumberTermination(5000)) // 设定最大世代数，防止无限运行
            };

            ga.GenerationRan += (s, e) =>
            {
                var bestChromosome = ga.BestChromosome as ProfileNestingChromosome;
                var genes = bestChromosome.GetGenes().Select(g => (ProfileNestingPlan)g.Value).ToList();

                Console.WriteLine($"Generation {ga.GenerationsNumber} best solution:");
                foreach (var plan in genes)
                {
                    Console.WriteLine($"Material Length: {plan.Length}, Segments: {string.Join(", ", plan.Segments)}");
                }
            };

            ga.Start();

            var best = ga.BestChromosome as ProfileNestingChromosome;
            var bestGenes = best.GetGenes().Select(g => (ProfileNestingPlan)g.Value).ToList();

            Console.WriteLine("Best solution found:");
            foreach (var plan in bestGenes)
            {
                Console.WriteLine(plan.ToString());
            }

            var group1 = bestGenes.GroupBy(p => p.Length).ToDictionary(g => g.Key, g => g.Count());

            foreach (var item in group1)
            {
                Console.WriteLine($"Material Length: {item.Key}, Count: {item.Value}");
            }

            var group2 = bestGenes.SelectMany(i => i.Segments).GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());

            foreach (var item in group2)
            {
                Console.WriteLine($"Part Length: {item.Key}, Count: {item.Value}");
            }
        }
    }
}