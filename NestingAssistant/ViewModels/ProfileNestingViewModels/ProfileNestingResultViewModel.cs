using CommunityToolkit.Mvvm.ComponentModel;
using ProfileOptimizer.Nesting;
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
        public ObservableCollection<UsageProfileMaterialViewModel> Materials { get; set; } = [];

        public ObservableCollection<ProfileNestingSummaryViewModel> Summaries { get; set; } = [];
    }
}