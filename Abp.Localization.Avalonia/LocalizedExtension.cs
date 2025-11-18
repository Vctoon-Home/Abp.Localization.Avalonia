using System;
using System.ComponentModel;
using Abp.Localization.Avalonia.Core;
using Avalonia;
using Avalonia.Data;
using Avalonia.Metadata;
using Microsoft.Extensions.Localization;

namespace Abp.Localization.Avalonia;

public class LocalizedExtension : SubjectedObject<string>, INotifyPropertyChanged
{
    static LocalizedExtension()
    {
        TextProperty.Changed.AddClassHandler<LocalizedExtension, string>((s, _) =>
        {
            var localizationManager = s._localizationManager;
            if (localizationManager is null) return;
            s.SetLanguageValue(localizationManager);
        });
        ResourceProperty.Changed.AddClassHandler<LocalizedExtension, string?>((s, _) =>
        {
            var localizationManager = s._localizationManager;
            if (localizationManager is null) return;
            s.SetLanguageValue(localizationManager);
        });
        ResourceTypeProperty.Changed.AddClassHandler<LocalizedExtension, Type?>((s, _) =>
        {
            var localizationManager = s._localizationManager;
            if (localizationManager is null) return;
            s.SetLanguageValue(localizationManager);
        });
        ArgumentsProperty.Changed.AddClassHandler<LocalizedExtension, object[]?>((s, _) =>
        {
            var localizationManager = s._localizationManager;
            if (localizationManager is null) return;
            s.SetLanguageValue(localizationManager);
        });
    }

    public LocalizedExtension(string text) : base(string.Empty)
    {
        Text = text;
    }

    public LocalizedExtension(Type resource, string text) : base(string.Empty)
    {
        Text = text;
        ResourceType = resource;
    }

    public LocalizedExtension(string text, params object[] arguments) : base(string.Empty)
    {
        Text = text;
        Arguments = arguments;
    }

    public LocalizedExtension(Type resource, string text, params object[] arguments) : base(string.Empty)
    {
        Text = text;
        Arguments = arguments;
        ResourceType = resource;
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LocalizedExtension, string>(nameof(Text));

    public static readonly StyledProperty<string?> ResourceProperty =
        AvaloniaProperty.Register<LocalizedExtension, string?>(nameof(Resource));

    public static readonly StyledProperty<object[]?> ArgumentsProperty =
        AvaloniaProperty.Register<LocalizedExtension, object[]?>(nameof(Arguments));

    public static readonly StyledProperty<Type?> ResourceTypeProperty =
        AvaloniaProperty.Register<LocalizedExtension, Type?>(nameof(ResourceType));

    [Content]
    [MarkupExtensionDefaultOption]
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Resource
    {
        get => GetValue(ResourceProperty);
        set => SetValue(ResourceProperty, value);
    }

    public object[]? Arguments
    {
        get => GetValue(ArgumentsProperty);
        set => SetValue(ArgumentsProperty, value);
    }

    public Type? ResourceType
    {
        get => GetValue(ResourceTypeProperty);
        set => SetValue(ResourceTypeProperty, value);
    }

    private ILocalizationManager? _localizationManager;
    private string _currentText = string.Empty;

    public string CurrentText
    {
        get => _currentText;
        private set
        {
            if (!string.Equals(_currentText, value, StringComparison.Ordinal))
            {
                _currentText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentText)));
            }
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected ILocalizationManager? LocalizationManager
    {
        get => _localizationManager;
        set
        {
            if (_localizationManager is not null)
                _localizationManager.CurrentCultureChanged -= LanguageChanged;

            _localizationManager = value;

            if (_localizationManager is not null)
                _localizationManager.CurrentCultureChanged += LanguageChanged;
        }
    }

    public IBinding ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = LocalizationExtensions.LocalizationManager;
        LocalizationManager = localizationManager;
        SetLanguageValue(localizationManager);
        return new Binding
        {
            Source = this,
            Path = nameof(CurrentText),
            Mode = BindingMode.OneWay
        };
    }

    void LanguageChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not ILocalizationManager localizationManager)
            return;
        SetLanguageValue(localizationManager);
    }

    void SetLanguageValue(ILocalizationManager localizationManager)
    {
        IStringLocalizer? stringLocalizer = null;
        if (ResourceType is not null)
            stringLocalizer = localizationManager.GetResource(ResourceType);
        else if (!Resource.IsNullOrEmpty())
            stringLocalizer = localizationManager.GetResource(Resource);

        if (stringLocalizer is not null)
        {
            CurrentText = Arguments is not null
                ? stringLocalizer[Text, Arguments].Value
                : stringLocalizer[Text].Value;
        }
        else
        {
            CurrentText = Arguments is not null
                ? localizationManager[Text, Arguments].Value
                : localizationManager[Text].Value;
        }
    }
}