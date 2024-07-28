using CommunityToolkit.Mvvm.ComponentModel;
using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfileMaterialModel
    {
        [ExcelColumn(Name = "类型", Index = 0)]
        public string Type { get; set; }

        [ExcelColumn(Name = "件数", Index = 2)]
        public int Piece { get; set; }

        [ExcelColumn(Name = "长度", Index = 1)]
        public double Length { get; set; }
    }
}