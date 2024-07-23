// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using ProfileOptimizer.Nesting;

var option = new NestingOption
{
    Spacing = 0.5,
    MaxSegments = 10,
    PopulationSize = 100,
    Generations = 200,
    MutationRate = 0.05
};

var materials = new List<ProfileMaterial>
        {
            new ProfileMaterial("Category1", 3, 12.0),
            new ProfileMaterial("Category2", 2, 15.0),
            new ProfileMaterial("Category3", 1, 20.0)
        };

var parts = new List<ProfilePart>
        {
            new ProfilePart("Category1", "PartA", 2, 4.0),
            new ProfilePart("Category1", "PartB", 1, 6.0),
            new ProfilePart("Category2", "PartC", 3, 5.0),
            new ProfilePart("Category3", "PartD", 1, 10.0),
            new ProfilePart("Category3", "PartE", 1, 8.0)
        };

var nester = new ProfileNester(option);
var results = nester.Nest(materials, parts);

foreach (var result in results)
{
    Console.WriteLine(result);
}