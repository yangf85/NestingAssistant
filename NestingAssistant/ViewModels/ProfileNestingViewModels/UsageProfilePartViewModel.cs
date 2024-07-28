using CommunityToolkit.Mvvm.ComponentModel;

namespace NestingAssistant.ViewModels
{
    public partial class UsageProfilePartViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private string _label;

        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _length;
    }
}