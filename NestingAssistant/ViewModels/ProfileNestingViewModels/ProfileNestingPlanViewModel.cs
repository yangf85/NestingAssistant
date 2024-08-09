using CommunityToolkit.Mvvm.ComponentModel;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingPlanViewModel : ObservableObject
    {
        [ObservableProperty]
        private double _length;

        [ObservableProperty]
        private int _materialPiece;

        [ObservableProperty]
        private string _nestingPlan;

        [ObservableProperty]
        private int _partPiece;

        [ObservableProperty]
        private double _remainLength;

        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private double _utilization;
    }
}