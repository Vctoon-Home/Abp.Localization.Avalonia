using System.ComponentModel;
using System.Globalization;
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

    public LocalizedString GetValue(string resourceKey)
    {
        if (LocalizationExtensions.Localizer == null) return default;
        var val = LocalizationExtensions.Localizer[resourceKey];
        return val;
    }
}