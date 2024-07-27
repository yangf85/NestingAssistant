// See https://aka.ms/new-console-template for more information
// 初始化材料列表
using ProfileOptimizer.Nesting;

// 原材料
List<ProfileMaterial> materials = new List<ProfileMaterial>
        {
            new ProfileMaterial("A", 5, 1050),
            new ProfileMaterial("A", 10, 800),
            new ProfileMaterial("B", 8, 1200)
        };

// 型材零件
List<ProfilePart> parts = new List<ProfilePart>
        {
            new ProfilePart("A", "Part1", 10, 500),
            new ProfilePart("A", "Part2", 8, 300),
            new ProfilePart("B", "Part3", 6, 400),
            new ProfilePart("B", "Part4", 5, 600),
            new ProfilePart("A", "Part5", 3, 600),
            new ProfilePart("B", "Part6", 5, 600),
        };

// NestingOption 设置
ProfileNestingOption option = new ProfileNestingOption
{
    Spacing = 20,
    MaxSegments = 3,
    PopulationSize = 50,
    Generations = 100,
    MutationRate = 0.1
};

// 创建 ProfileNester 实例并执行 Nest 方法
ProfileNester nester = new ProfileNester();
List<PlacedProfileMaterial> results = nester.Nest(materials, parts, option);

// 输出结果
foreach (var result in results)
{
    Console.WriteLine(result.ToString());
}