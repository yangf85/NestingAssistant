// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using GeneticSharp;
using ProfileOptimizer.Nesting;

var materials = new List<ProfileMaterial>
        {
            new ProfileMaterial { Type="A", Length = 6000, Piece = 10 },
            new ProfileMaterial {Type="B", Length = 5000, Piece = 5 },
            new ProfileMaterial {Type="A", Length = 4000, Piece = 5 }
        };

var parts = new List<ProfilePart>
        {
            new ProfilePart {Type="A", Length = 1000, Piece = 15 },
            new ProfilePart {Type="A", Length = 1200, Piece = 10 },
            new ProfilePart {Type="B", Length = 1300, Piece = 8},
            new ProfilePart {Type="B", Length = 600, Piece = 8},
            new ProfilePart {Type="A", Length = 400, Piece = 20},
            new ProfilePart {Type="B", Length = 300, Piece = 18},
            new ProfilePart {Type="A", Length = 900, Piece = 32},
        };
var option = new ProfileNestingOption()
{
    MaxSegments = 10,
    Spacing = 10,
    IsShowPartIndex = true,
};

var nester = new GreedyProfileNester(parts, materials, option);

var progress = new Progress<double>(percent =>
{
    Console.WriteLine($"进度: {percent}%");
});

var summary = await nester.NestAsync();

var t = 1;