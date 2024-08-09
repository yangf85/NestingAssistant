using CommunityToolkit.Mvvm.ComponentModel;
using NestingAssistant.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingSummaryViewModel : ObservableObject
    {
        [ObservableProperty]
        private double _totalLength;

        [ObservableProperty]
        private double _totalRemainLength;

        [ObservableProperty]
        private double _averageUtilization;

        [ObservableProperty]
        private int _materialPiece;

        [ObservableProperty]
        private int _partPiece;

        public ObservableCollection<ProfileNestingPlanViewModel> Plans { get; set; } = [];

        public ObservableCollection<ProfileMaterialViewModel> Materials { get; set; } = [];
    }
}