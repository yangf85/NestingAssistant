using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfileNestingPlanModel
    {
        public double Length { get; set; }

        public int MaterialPiece { get; set; }

        public string NestingPlan { get; set; }

        public int PartPiece { get; set; }

        public double RemainLength { get; set; }

        public string Type { get; set; }

        public double Utilization { get; set; }
    }
}