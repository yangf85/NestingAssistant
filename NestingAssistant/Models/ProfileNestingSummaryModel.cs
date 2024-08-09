using CommunityToolkit.Mvvm.ComponentModel;
using MiniExcelLibs.Attributes;
using ProfileOptimizer.Nesting;
using System.Collections.Generic;

namespace NestingAssistant.Models
{
    public class ProfileNestingSummaryModel
    {
        public double AverageUtilization { get; set; }

        public int MaterialPiece { get; set; }

        public int PartPiece { get; set; }

        public List<ProfileNestingPlanModel> Plans { get; set; } = [];

        public double TotalLength { get; set; }

        public double TotalRemainLength { get; set; }
    }
}