using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class PlacedProfilePartModel
    {
        public string Category { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public int Piece { get; set; }

        public double Length { get; set; }
    }
}