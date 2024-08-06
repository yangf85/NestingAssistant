using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public struct ProfileMaterial
    {
        public string Type { get; set; }

        public double Length { get; set; }

        public int Piece { get; set; }

        public ProfileMaterial()
        {
            Type = "Profile";
            Length = 6000;
            Piece = 10;
        }
    }
}