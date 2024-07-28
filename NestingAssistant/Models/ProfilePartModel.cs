using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfilePartModel
    {
        [ExcelColumn(Name = "类型", Index = 0)]
        public string Type { get; set; }

        [ExcelColumn(Name = "长度", Index = 1)]
        public double Length { get; set; }

        [ExcelColumn(Name = "件数", Index = 2)]
        public int Piece { get; set; }

        [ExcelColumn(Name = "标签", Index = 3)]
        public string Label { get; set; }
    }
}