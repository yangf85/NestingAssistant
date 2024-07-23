// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using ProfileOptimizer.Nesting;


var materials = new List<ProfileMaterial>
{
    new ProfileMaterial("Material1", 5, 100),   // Category, Piece, Length
    new ProfileMaterial("Material2", 3, 150),
    new ProfileMaterial("Material3", 4, 120)
};

var parts = new List<ProfilePart>
{
    new ProfilePart("Material1", "Label1", 2, 30),   // Category, Label, Piece, Length
    new ProfilePart("Material1", "Label2", 3, 50),
    new ProfilePart("Material2", "Label3", 1, 80),
    new ProfilePart("Material2", "Label4", 4, 40),
    new ProfilePart("Material3", "Label5", 2, 60)
};

var option = new NestingOption
{
    Spacing = 2,   // 切割缝隙
    MaxSegments = 10   // 每根型材最多切割多少段
};



