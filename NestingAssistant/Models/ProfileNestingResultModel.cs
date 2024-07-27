using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfileNestingResultModel
    {
        public string Category { get; set; }

        public int Piece { get; set; }

        public double Length { get; set; }

        public double Spacing { get; set; }

        public List<PlacedProfilePartModel> Parts { get; set; } = new List<PlacedProfilePartModel>();

        public double RemainLength { get; set; }

        public double Utilization { get; set; }
    }
}