using CommunityToolkit.Mvvm.ComponentModel;
using MiniExcelLibs.Attributes;
using NestingAssistant.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NestingAssistant.Models
{
    public class UsageProfileMaterialModel
    {
        [ExcelColumn(Name = "类型", Index = 0)]
        public string Type { get; set; }

        [ExcelColumn(Name = "长度", Index = 1)]
        public double Length { get; set; }

        [ExcelColumn(Name = "件数", Index = 2)]
        public int Piece { get; set; }

        [ExcelColumn(Name = "利用率", Index = 3, Format = "P2")]
        public double Utilization { get; set; }

        [ExcelColumn(Name = "优化方案", Index = 4)]
        public string NestingPlan { get; set; }

        [ExcelColumn(Ignore = true)]
        public List<UsageProfilePartModel> Parts { get; set; } = [];
    }
}