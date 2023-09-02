using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Abp.Localization.Avalonia;

public interface ILocalizationManager
{
    event PropertyChangedEventHandler? CurrentCultureChanged;
    CultureInfo CurrentCulture { get; set; }

    /// <summary>
    /// this Localized from default resource
    /// </summary>
    /// <param name="resourceKey"></param>
    LocalizedString this[string resourceKey] { get; }

    /// <summary>
    /// this Localized from default resource
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <param name="arguments"></param>
    LocalizedString this[string resourceKey, params object[] arguments] { get; }

    /// <summary>
    /// Change language
    /// </summary>
    /// <param name="cultureName"></param>
    void ChangeLanguage(string cultureName);

    /// <summary>
    /// Get resource from resource type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IStringLocalizer GetResource<T>();

    /// <summary>
    ///  Get resource from resource type
    /// </summary>
    /// <param name="resourceType"></param>
    /// <returns></returns>
    IStringLocalizer GetResource(Type resourceType);

    /// <summary>
    ///  Get resource from resource name
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    IStringLocalizer GetResource(string resourceName);
}