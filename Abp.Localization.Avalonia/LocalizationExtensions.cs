using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Abp.Localization.Avalonia;

public static class LocalizationExtensions
{
    internal static LocalizationManager LocalizationManager { get; set; } = null!;
    internal static IStringLocalizer Localizer { get; set; } = null!;

    internal static IServiceProvider Services { get; private set; }



    public static IServiceCollection AddLocalizationManager(this IServiceCollection service,
        [NotNull] Func<IServiceProvider, IStringLocalizer> options)
    {
        service.AddSingleton<LocalizationManager>(s =>
        {
            Services = s;
            LocalizationManager = new LocalizationManager();
            Localizer = options.Invoke(s);
            return LocalizationManager;
        });

        return service;
    }
}