using CommunityToolkit.Mvvm.ComponentModel;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingSummaryViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private int _length;

        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _utilization;

        [ObservableProperty]
        private double _totalLength;

        [ObservableProperty]
        private double _remainTotalLength;
    }
}