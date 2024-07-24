using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class NestingOption : ObservableValidator
    {
        [ObservableProperty]
        private double _spacing = 5d;

        //最大切割数
        [ObservableProperty]
        private int _maxSegments = 10;

        [ObservableProperty]
        private int _populationSize = 50;

        [ObservableProperty]
        private int _generations = 100;

        [ObservableProperty]
        private double _mutationRate = 0.1;
    }
}