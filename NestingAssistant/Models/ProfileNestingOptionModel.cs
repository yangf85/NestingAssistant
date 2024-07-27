using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfileNestingOptionModel
    {
        public double Spacing { get; set; }

        public int MaxSegments { get; set; }

        public int PopulationSize { get; set; }

        public int Generations { get; set; }

        public double MutationRate { get; set; }
    }
}