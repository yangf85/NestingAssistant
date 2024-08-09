using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting;

public class ProfilePart
{
    public string Type { get; set; }

    public string Label { get; set; }

    public double Length { get; set; }

    public int Index { get; set; }

    public int Piece { get; set; }

    public ProfilePart()
    {
        Type = "Profile";
        Label = string.Empty;
        Length = 2000;
        Piece = 5;
        Index = 0;
    }
}