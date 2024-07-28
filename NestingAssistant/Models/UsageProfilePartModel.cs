using CommunityToolkit.Mvvm.ComponentModel;

namespace NestingAssistant.Models
{
    public class UsageProfilePartModel
    {
        public string Type { get; set; }

        public string Label { get; set; }

        public int Piece { get; set; }

        public double Length { get; set; }
    }
}