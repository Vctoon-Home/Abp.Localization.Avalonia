using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Abp.Localization.Avalonia;

public class LocalizationManager : INotifyPropertyChanged, ISingletonDependency
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private CultureInfo _currentCulture;

    public CultureInfo CurrentCulture
    {
        get { return _currentCulture; }
        set
        {
            _currentCulture = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
        }
    }


    public LocalizationManager(
    )
    {
        CurrentCulture = CultureInfo.CurrentCulture;
    }

    public LocalizedString this[string resourceKey] => GetValue(resourceKey);

    public void ChangeLanguage(string cultureName)
    {
        var cultureInfo = new CultureInfo(cultureName);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        CurrentCulture = cultureInfo;
    }


    private Dictionary<Type, IStringLocalizer> _localizers = new();

    object _lock = new();

    public IStringLocalizer GetResourceLocalizer<T>()
    {
        lock (_lock)
        {
            if (_localizers.ContainsKey(typeof(T)))
            {
                return _localizers[typeof(T)];
            }
            else
            {
                var localizer = LocalizationExtensions.Services.GetRequiredService<IStringLocalizerFactory>()
                    .Create(typeof(T));
                _localizers.Add(typeof(T), localizer);
                return localizer;
            }
        }
    }


    public LocalizedString GetValue(string resourceKey)
    {
        if (LocalizationExtensions.Localizer == null) return default;
        var val = LocalizationExtensions.Localizer[resourceKey];
        return val;
    }
}