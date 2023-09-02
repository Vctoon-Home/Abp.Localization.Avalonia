using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace Abp.Localization.Avalonia;

public static class LocalizationExtensions
{
    internal static ILocalizationManager LocalizationManager { get; set; } = null!;
    internal static IStringLocalizer Localizer { get; set; } = null!;

    internal static IServiceProvider Services { get; private set; }

    public static IServiceCollection AddLocalizationManager(this IServiceCollection service)
    {
        service.AddSingleton<ILocalizationManager>(s =>
        {
            var options = s.GetRequiredService<IOptions<AbpLocalizationOptions>>();

            var defaultResourceFactory =
                s.GetRequiredService<IStringLocalizerFactory>().Create(options.Value.DefaultResourceType);

            Services = s;
            LocalizationManager = new LocalizationManager(options);
            Localizer = defaultResourceFactory;
            return LocalizationManager;
        });

        return service;
    }
}