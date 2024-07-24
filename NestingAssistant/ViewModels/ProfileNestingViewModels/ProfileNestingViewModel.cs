using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public partial class ProfileNestingViewModel : ObservableValidator
    {
        public ObservableCollection<ProfilePartViewModel> ProfileParts { get; private set; } = new();

        public ObservableCollection<ProfileMaterialViewModel> ProfileMaterials { get; private set; } = new();

        public ProfileNestingViewModel()
        {
            ProfileParts = new ObservableCollection<ProfilePartViewModel>()
            {
                new ProfilePartViewModel { Category = "Category1", Label = "Label1", Piece = 1, Length = 10.5 },
                new ProfilePartViewModel { Category = "Category2", Label = "Label2", Piece = 2, Length = 20.5 },
                new ProfilePartViewModel { Category = "Category3", Label = "Label3", Piece = 3, Length = 30.5 },
                new ProfilePartViewModel { Category = "Category4", Label = "Label4", Piece = 4, Length = 40.5 },
                new ProfilePartViewModel { Category = "Category5", Label = "Label5", Piece = 5, Length = 50.5 }
            };
        }
    }
}