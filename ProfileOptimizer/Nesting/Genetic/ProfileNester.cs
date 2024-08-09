using GeneticSharp;
using Microsoft.VisualBasic.FileIO;
using ProfileOptimizer.Exceptions;
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
            QuantizationProfileMaterialLength();
            QuantizationProfilePartLength();
        }

        public void Nest()
        {
            var selection = new EliteSelection();
            var crossover = new UniformCrossover(0.1f);
            var mutation = new UniformMutation(true);
            var fitness = new ProfileNestingFitness(_materials, _parts, _option);
            var chromosome = new ProfileNestingChromosome(_materials, _parts, _option);
            var population = new Population(50, 100, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new OrTermination(new FitnessThresholdTermination(0),
                new GenerationNumberTermination(2000)),
            };

            ga.GenerationRan += (s, e) =>
            {
                var bestChromosome = ga.BestChromosome as ProfileNestingChromosome;
                var genes = bestChromosome.GetGenes().Select(g => (GeneticProfileNestingPlan)g.Value).ToList();

                Console.WriteLine($"Generation {ga.GenerationsNumber} best solution:");
                foreach (var plan in genes)
                {
                    Console.WriteLine($"Material Length: {plan.Length}, Segments: {string.Join(", ", plan.Segments)}");
                }
            };

            ga.Start();

            var best = ga.BestChromosome as ProfileNestingChromosome;
            var bestGenes = best.GetGenes().Select(g => (GeneticProfileNestingPlan)g.Value).ToList();

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

        private Dictionary<string, (List<ProfileMaterial> Materials, List<ProfilePart> Parts)> Group(List<ProfileMaterial> materials, List<ProfilePart> parts)
        {
            var materialGroups = materials.GroupBy(m => m.Type).ToDictionary(g => g.Key, g => g.ToList());
            var partGroups = parts.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.ToList());

            // 获取所有的类型
            var materialTypes = new HashSet<string>(materialGroups.Keys);
            var partTypes = new HashSet<string>(partGroups.Keys);

            // 验证类型是否匹配
            if (!materialTypes.SetEquals(partTypes))
            {
                var missingMaterialTypes = string.Join(", ", partTypes.Except(materialTypes));
                var missingPartTypes = string.Join(", ", materialTypes.Except(partTypes));
                throw new ProfileTypeMismatchException($"Material types not matching with part types.\nMissing Material Types: {missingMaterialTypes}\nMissing Part Types: {missingPartTypes}");
            }

            var result = new Dictionary<string, (List<ProfileMaterial> Materials, List<ProfilePart> Parts)>();

            foreach (var key in materialGroups.Keys)
            {
                result.Add(key, (materialGroups[key], partGroups[key]));
            }

            return result;
        }

        private void QuantizationProfileMaterialLength()
        {
            foreach (var material in _materials)
            {
                material.Length = Math.Round(material.Length, 6);
            }
        }

        private void QuantizationProfilePartLength()
        {
            foreach (var material in _parts)
            {
                material.Length = Math.Round(material.Length, 6);
            }
        }
    }
}