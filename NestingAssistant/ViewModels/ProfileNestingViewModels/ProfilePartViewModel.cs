using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class ProfilePartViewModel : ObservableValidator
    {
        [Range(0, 10000)]
        [ObservableProperty]
        private string _category;

        [ObservableProperty]
        private string _label;

        [Range(0, 10000)]
        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _length;
    }
}