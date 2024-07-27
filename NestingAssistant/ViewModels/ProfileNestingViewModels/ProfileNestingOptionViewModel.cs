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
        [ObservableProperty]
        [Range(0, 9999)]
        private double _spacing = 5d;

        [ObservableProperty]
        [Range(0, 9999)]
        private int _maxSegments = 10;

        [ObservableProperty]
        [Range(0, 9999)]
        private int _populationSize = 50;

        [ObservableProperty]
        [Range(0, 9999)]
        private int _generations = 100;

        [ObservableProperty]
        [Range(0, 1)]
        private double _mutationRate = 0.1;
    }
}