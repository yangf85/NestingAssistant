// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using ProfileOptimizer.Nesting;

var options = new ProfileNestingOption
{
    Spacing = 5d,
    MaxSegments = 10,
    PopulationSize = 50,
    Generations = 100,
    MutationRate = 0.1f
};

var parts = new List<ProfilePart>
    {
        new ProfilePart { Type = "Type1", Label = "Part1", Piece = 5, Length = 1200 },
        new ProfilePart { Type = "Type1", Label = "Part2", Piece = 3, Length = 1500 },
        new ProfilePart { Type = "Type2", Label = "Part3", Piece = 4, Length = 2000 },
        new ProfilePart { Type = "Type2", Label = "Part4", Piece = 2, Length = 1000 },
        new ProfilePart { Type = "Type3", Label = "Part5", Piece = 6, Length = 2500 },
        new ProfilePart { Type = "Type3", Label = "Part6", Piece = 2, Length = 3000 },
        new ProfilePart { Type = "Type4", Label = "Part7", Piece = 1, Length = 1200 },
        new ProfilePart { Type = "Type4", Label = "Part8", Piece = 5, Length = 1800 },
        new ProfilePart { Type = "Type5", Label = "Part9", Piece = 2, Length = 2200 },
        new ProfilePart { Type = "Type5", Label = "Part10", Piece = 3, Length = 2400 }
    };

var materials = new List<ProfileMaterial>
    {
        new ProfileMaterial { Type = "Type1", Piece = 3, Length = 6000 },
        new ProfileMaterial { Type = "Type1", Piece = 2, Length = 4500 },
        new ProfileMaterial { Type = "Type2", Piece = 2, Length = 5000 },
        new ProfileMaterial { Type = "Type2", Piece = 1, Length = 5500 },
        new ProfileMaterial { Type = "Type3", Piece = 2, Length = 6000 },
        new ProfileMaterial { Type = "Type3", Piece = 1, Length = 7000 },
        new ProfileMaterial { Type = "Type4", Piece = 1, Length = 6000 },
        new ProfileMaterial { Type = "Type4", Piece = 2, Length = 5500 },
        new ProfileMaterial { Type = "Type5", Piece = 1, Length = 6000 }
    };

//var nester = new ProfileNester();
//var result = nester.Optimize(options, parts, materials);

//Console.WriteLine("Optimization Result:");
//foreach (var material in result.Materials)
//{
//    Console.WriteLine($"Material: {material.Type}, Utilization: {material.Utilization:P}");
//    foreach (var part in material.Parts)
//    {
//        Console.WriteLine($"  Part: {part.Label}, Length: {part.Length}, Quantity: {part.Piece}");
//    }
//}

//Console.WriteLine("\nSummary:");
//foreach (var summary in result.Summaries)
//{
//    Console.WriteLine($"Type: {summary.Type}, Total Length: {summary.ToltalLength}, Remain Length: {summary.RemainLength}, Utilization: {summary.Utilization:P}");
//}