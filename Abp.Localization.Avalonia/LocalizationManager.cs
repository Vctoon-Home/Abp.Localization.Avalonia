using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Abp.Localization.Avalonia;

public class LocalizationManager : ISingletonDependency, ILocalizationManager
{
    public event PropertyChangedEventHandler? CurrentCultureChanged;

    private CultureInfo _currentCulture;

    private Dictionary<string, Type> _resources = new();

    public CultureInfo CurrentCulture
    {
        get { return _currentCulture; }
        set
        {
            _currentCulture = value;
            CurrentCultureChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
        }
    }


    public LocalizationManager(IOptions<AbpLocalizationOptions> options)
    {
        _resources = options.Value.Resources.ToDictionary(v => v.Key,
            v => (v.Value as LocalizationResource).ResourceType);
        CurrentCulture = CultureInfo.CurrentCulture;
    }

    public LocalizedString this[string resourceKey] => GetValue(resourceKey);
    public LocalizedString this[string resourceKey, params object[] arguments] => GetValue(resourceKey, arguments);

    public void ChangeLanguage(string cultureName)
    {
        var cultureInfo = new CultureInfo(cultureName);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        CurrentCulture = cultureInfo;
    }


    private Dictionary<Type, IStringLocalizer> _localizers = new();

    static readonly object Lock = new();

    public IStringLocalizer GetResource<T>() => GetResource(typeof(T));

    public IStringLocalizer GetResource(Type resourceType)
    {
        lock (Lock)
        {
            if (_localizers.ContainsKey(resourceType))
            {
                return _localizers[resourceType];
            }
            else 
            {
                var localizer = LocalizationExtensions.Services.GetRequiredService<IStringLocalizerFactory>()
                    .Create(resourceType);
                _localizers.Add(resourceType, localizer);
                return localizer;
            }
        }
    }

    public IStringLocalizer GetResource(string resourceName)
    {
        if (_resources.ContainsKey(resourceName))
            return GetResource(_resources[resourceName]);

        throw new Exception($"Resource {resourceName} not found");
    }


    private LocalizedString GetValue(string resourceKey, object[] arguments = null)
    {
        if (LocalizationExtensions.Localizer == null) return default;

        if (arguments is null)
            return LocalizationExtensions.Localizer[resourceKey];
        else return LocalizationExtensions.Localizer[resourceKey, arguments];
    }
}