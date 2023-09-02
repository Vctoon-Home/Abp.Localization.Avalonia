using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaDemo.Views;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace AvaloniaDemo;

public partial class App : Application
{
    public App()
    {
        CreateServices();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static IServiceCollection ServiceCollection;

    public static IServiceProvider Services;

    void CreateServices()
    {
        ServiceCollection = new ServiceCollection();


        ServiceCollection.AddApplication<AvaloniaDemoModule>();

        var factory = new AbpAutofacServiceProviderFactory(new Autofac.ContainerBuilder());

        Services = factory.CreateServiceProvider(factory.CreateBuilder(ServiceCollection));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();

        Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(Services);
    }
}