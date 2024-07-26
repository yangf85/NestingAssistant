using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.DependencyInjection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using NestingAssistant.Common;
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

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            GlobalExceptionHandler.Register();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };

            Ioc.Default.ConfigureServices(ConfigureServices(desktop));
        }
    }

    private IServiceProvider ConfigureServices(IClassicDesktopStyleApplicationLifetime lifetime)
    {
        var services = new ServiceCollection();

        services.AddSingleton(lifetime);
        services.AddSingleton(TopLevel.GetTopLevel(lifetime.MainWindow)!);
        services.AddMapster();

        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IMessageBoxService, MessageBoxService>();
        services.AddSingleton<IExcelService, ExcelService>();
        services.AddSingleton<IStorageService, StorageService>();

        services.AddSingleton<ProfileNestingService>();
        services.AddSingleton<ProfileNestingViewModel>();

        return services.BuildServiceProvider();
    }
}