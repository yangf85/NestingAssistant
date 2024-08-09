using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using NestingAssistant.ViewModels;
using Ursa.Controls;

namespace NestingAssistant.Views;

public partial class ProfileNestingView : UserControl
{
    public ProfileNestingView()
    {
        InitializeComponent();
        Loaded += ProfileNestingView_Loaded;
    }

    private void ProfileNestingView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DataContext = Ioc.Default.GetService<ProfileNesterViewModel>();
    }
}