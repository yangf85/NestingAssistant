using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting;

public class GreedyNestingSummary : ProfileNestingSummary
{
    private List<GreedyProfileNestingResult> _results;

    public GreedyNestingSummary(List<GreedyProfileNestingResult> results, ProfileNestingOption option)
    {
        _results = new List<GreedyProfileNestingResult>(results);

        Summary(_results, option);
    }

    private void Summary(List<GreedyProfileNestingResult> results, ProfileNestingOption option)
    {
        _results.RemoveAll(i => i.Parts.Count == 0);

        var list = new List<GreedyProfileNestingPlan>();
        foreach (var item in results)
        {
            var plan = new GreedyProfileNestingPlan();
            plan.Update(item, option);
            list.Add(plan);
        }

        Plans = list.GroupBy(i => i)
                    .Select(g => new ProfileNestingPlan()
                    {
                        Length = g.Key.Length,
                        Type = g.Key.Type,
                        Utilization = g.Key.Utilization,
                        RemainLength = g.Key.RemainLength,
                        NestingPlan = g.Key.NestingPlan,
                        PartPiece = g.Key.PartPiece,
                        MaterialPiece = g.Count(),
                    })
                    .OrderBy(i => i.Type)
                    .ThenBy(i => i.Length)
                    .ToList();

        Materials = list.GroupBy(i => new { i.Type, i.Length })
                        .Select(g => new ProfileMaterial()
                        {
                            Type = g.Key.Type,
                            Length = g.Key.Length,
                            Piece = g.Count(),
                        })
                        .OrderBy(i => i.Type)
                        .ThenBy(i => i.Length)
                        .ToList();

        TotalLength = Plans.Sum(i => i.Length * i.MaterialPiece);
        TotalRemainLength = Plans.Sum(i => i.RemainLength * i.MaterialPiece);
        MaterialPiece = Plans.Sum(i => i.MaterialPiece);
        PartPiece = Plans.Sum(i => i.PartPiece * i.MaterialPiece);
        AverageUtilization = TotalLength != 0 ? (TotalLength - TotalRemainLength) / TotalLength : 0;
    }
}