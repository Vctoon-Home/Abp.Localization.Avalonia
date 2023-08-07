using System.ComponentModel;
using System.Reactive;
using Abp.Localization.Avalonia.Core;
using Avalonia;
using Avalonia.Data;
using Avalonia.Metadata;

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

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LocalizedExtension, string>(nameof(Text));

    // public static readonly StyledProperty<string?> CategoryProperty =
    //     AvaloniaProperty.Register<LocalizedBinding, string?>(nameof(Category));

    public static readonly StyledProperty<object[]?> ArgumentsProperty =
        AvaloniaProperty.Register<LocalizedExtension, object[]?>(nameof(Arguments));

    [Content]
    [MarkupExtensionDefaultOption]
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // public string? Category
    // {
    //     get => GetValue(CategoryProperty);
    //     set => SetValue(CategoryProperty, value);
    // }

    public object[]? Arguments
    {
        get => GetValue(ArgumentsProperty);
        set => SetValue(ArgumentsProperty, value);
    }

    private LocalizationManager? _localizationManager;
    private IBinding bindingImplementation;

    protected LocalizationManager? LocalizationManager
    {
        get => _localizationManager;
        set
        {
            if (_localizationManager is not null)
                _localizationManager.PropertyChanged -= LanguageChanged;

            _localizationManager = value;

            if (_localizationManager is not null)
                _localizationManager.PropertyChanged += LanguageChanged;
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
        if (sender is not LocalizationManager localizationManager)
            return;
        SetLanguageValue(localizationManager);
    }

    void SetLanguageValue(LocalizationManager localizationManager)
    {
        if (localizationManager is null)
            return;
        //
        // if (Arguments is not null && !string.IsNullOrWhiteSpace(Category))
        //     OnNext(localizationManager[Text, Category!, Arguments]);
        // else if (Arguments is not null)
        //     OnNext(localizationManager[Text, Arguments]);
        // else if (!string.IsNullOrWhiteSpace(Category))
        //     OnNext(localizationManager[Text, Category!]);
        // else
        OnNext(localizationManager[Text]);
    }
}