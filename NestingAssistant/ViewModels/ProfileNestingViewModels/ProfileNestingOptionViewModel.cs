using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingOptionViewModel : ObservableValidator
    {
        private double _spacing = 5d;

        [Range(0, 9999)]
        [ObservableProperty]
        private int _maxSegments = 10;

        [Range(0, 9999)]
        [ObservableProperty]
        private int _populationSize = 50;

        [Range(0, 9999)]
        [ObservableProperty]
        private int _generations = 100;

        [Range(0, 1)]
        [ObservableProperty]
        private double _mutationRate = 0.1;

        [Required]
        [MinLength(10)]
        [ObservableProperty]
        private string _name = "";

        [Range(0, 9999)]
        public double Spacing
        {
            get => _spacing;
            set => SetProperty(ref _spacing, value, true);
        }
    }
}