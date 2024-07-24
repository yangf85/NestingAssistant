using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NestingAssistant.Services;
using NestingAssistant.ViewModels;
using NestingAssistant.Views;
using System;

namespace NestingAssistant;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Ioc.Default.ConfigureServices(ConfigureServices());

        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<NestingOption>();
        services.AddTransient<ProfilePartViewModel>();
        services.AddTransient<PlacedProfilePartViewModel>();
        services.AddTransient<ProfileMaterialViewModel>();
        services.AddTransient<ProfileNestingResultViewModel>();
        services.AddSingleton<ProfileNestingViewModel>();

        services.AddSingleton<IExcelService, ExcelService>();

        return services.BuildServiceProvider();
    }
}