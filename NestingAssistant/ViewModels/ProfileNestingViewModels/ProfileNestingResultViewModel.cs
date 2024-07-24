using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingResultViewModel : ObservableValidator
    {
        [ObservableProperty]
        private double _remainLength;

        [ObservableProperty]
        private double _utilization;

        [ObservableProperty]
        private string _category;

        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _length;

        [ObservableProperty]
        private double _spacing;

        public ObservableCollection<PlacedProfilePartViewModel> Parts { get; set; } = new();
    }
}