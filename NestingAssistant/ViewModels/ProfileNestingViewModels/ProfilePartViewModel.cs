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
        [Required(ErrorMessage = "类型名称是必须的")]
        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private string _label;

        [Range(1, 10000)]
        [ObservableProperty]
        private int _piece;

        [Range(1, 99999)]
        [ObservableProperty]
        private double _length;

        [Range(0, 999999)]
        [ObservableProperty]
        private int _index;
    }
}