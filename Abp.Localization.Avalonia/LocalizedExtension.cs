using System.ComponentModel;
using System.Reactive;
using Abp.Localization.Avalonia.Core;
using Avalonia;
using Avalonia.Data;
using Avalonia.Metadata;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Abp.Localization.Avalonia;

public class LocalizedExtension : SubjectedObject<string>, IBinding
{
    static LocalizedExtension()
    {
        TextProperty.Changed.AddClassHandler<LocalizedExtension, string>((s, e) =>
        {
            if (s is null)
                return;

            var localizationManager = s._localizationManager;
            if (localizationManager is null)
                return;

            s.OnNext(localizationManager[e.NewValue.Value]);
        });
    }

    public LocalizedExtension() : base(string.Empty)
    {
    }

    public LocalizedExtension(string text) : base(string.Empty)
    {
        Text = text;
    }

    public LocalizedExtension(string text, string resource) : base(string.Empty)
    {
        Text = text;
        Resource = resource;
    }
    //
    // public LocalizedExtension(string text, string resource, params object[] arguments) : base(string.Empty)
    // {
    //     Text = text;
    //     Resource = resource;
    //     Arguments = arguments;
    // }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LocalizedExtension, string>(nameof(Text));

    public static readonly StyledProperty<string?> CategoryProperty =
        AvaloniaProperty.Register<LocalizedExtension, string?>(nameof(Resource));

    public static readonly StyledProperty<object[]?> ArgumentsProperty =
        AvaloniaProperty.Register<LocalizedExtension, object[]?>(nameof(Arguments));

    [Content]
    [MarkupExtensionDefaultOption]
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Resource
    {
        get => GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public object[]? Arguments
    {
        get => GetValue(ArgumentsProperty);
        set => SetValue(ArgumentsProperty, value);
    }

    private ILocalizationManager? _localizationManager;
    private IBinding bindingImplementation;
    private IBinding _bindingImplementation;

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

    public IBinding? ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = LocalizationExtensions.LocalizationManager;
        if (localizationManager is null)
            return default;

        LocalizationManager = localizationManager;
        SetLanguageValue(localizationManager);
        return this;
    }

    public InstancedBinding? Initiate(AvaloniaObject target, AvaloniaProperty? targetProperty, object? anchor = null,
        bool enableDataValidation = false)
    {
        var observer = Observer.Create<object?>(t => { });

        return InstancedBinding.TwoWay(this, observer);
        ;
    }


    void LanguageChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not ILocalizationManager localizationManager)
            return;
        SetLanguageValue(localizationManager);
    }

    void SetLanguageValue(ILocalizationManager localizationManager)
    {
        IStringLocalizer stringLocalizer = null;

        if (localizationManager is null)
            return;

        if (!Resource.IsNullOrEmpty())
        {
            stringLocalizer = localizationManager.GetResource(Resource);
            if (Arguments is not null)
                OnNext(stringLocalizer[Text, Arguments]);
            else
                OnNext(stringLocalizer[Text]);
        }
        else
        {
            if (Arguments is not null)
                OnNext(localizationManager[Text, Arguments]);
            else
                OnNext(localizationManager[Text]);
        }
    }
}