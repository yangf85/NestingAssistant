using CommunityToolkit.Mvvm.ComponentModel;
using ProfileOptimizer.Nesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NestingAssistant.ViewModels
{
    public partial class UsageProfileMaterialViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private double _length;

        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _utilization;

        [ObservableProperty]
        private string _nestingPlan;

        public ObservableCollection<UsageProfilePartViewModel> Parts { get; set; } = [];
    }
}