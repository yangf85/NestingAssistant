using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileMaterialViewModel : ObservableValidator
    {
        [ObservableProperty]
        private string _category;

        [ObservableProperty]
        private int _piece;

        [ObservableProperty]
        private double _length;
    }
}