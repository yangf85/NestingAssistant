// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using GeneticSharp;
using ProfileOptimizer.Nesting;

var materials = new List<ProfileMaterial>
        {
            new ProfileMaterial { Length = 6000, Piece = 10 },
            new ProfileMaterial { Length = 5000, Piece = 5 },
            new ProfileMaterial { Length = 4000, Piece = 5 }
        };

var parts = new List<ProfilePart>
        {
            new ProfilePart { Length = 1000, Piece = 15 },
            new ProfilePart { Length = 1200, Piece = 10 },
            new ProfilePart { Length = 1300, Piece = 8},
            new ProfilePart { Length = 600, Piece = 8},
            new ProfilePart { Length = 400, Piece = 20},
            new ProfilePart { Length = 300, Piece = 18},
            new ProfilePart { Length = 900, Piece = 32},
        };
var option = new ProfileNestingOption()
{
    MaxSegments = 10,
};

var nester = new ProfileNester(materials, parts, option);
nester.Nest();