using CommunityToolkit.Mvvm.ComponentModel;
using MiniExcelLibs.Attributes;

namespace NestingAssistant.Models
{
    public class ProfileNestingSummaryModel
    {
        [ExcelColumn(Name = "类型", Index = 0)]
        public string Type { get; set; }

        [ExcelColumn(Name = "长度", Index = 1)]
        public double Length { get; set; }

        [ExcelColumn(Name = "件数", Index = 2)]
        public int Piece { get; set; }

        [ExcelColumn(Name = "利用率", Index = 3, Format = "P2")]
        public double Utilization { get; set; }

        [ExcelColumn(Name = "总长", Index = 4)]
        public double TotalLength { get; set; }

        [ExcelColumn(Name = "余料总长", Index = 5)]
        public double RemainTotalLength { get; set; }
    }
}