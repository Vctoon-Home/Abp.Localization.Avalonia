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
            if (localizationManager is null) { s.CurrentText = s.Text ?? string.Empty; return; }
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
        // 绑定参数变化时也要刷新
        Arg0Property.Changed.AddClassHandler<LocalizedExtension, object?>((s, _) => s.TryRefresh());
        Arg1Property.Changed.AddClassHandler<LocalizedExtension, object?>((s, _) => s.TryRefresh());
        Arg2Property.Changed.AddClassHandler<LocalizedExtension, object?>((s, _) => s.TryRefresh());
        Arg3Property.Changed.AddClassHandler<LocalizedExtension, object?>((s, _) => s.TryRefresh());
        Arg4Property.Changed.AddClassHandler<LocalizedExtension, object?>((s, _) => s.TryRefresh());
    }

    public LocalizedExtension() : base(string.Empty)
    {
        _currentText = string.Empty;
    }

    public LocalizedExtension(string text) : base(string.Empty)
    {
        Text = text;
        _currentText = text ?? string.Empty; // 回退初始显示
    }

    public LocalizedExtension(Type resource, string text) : base(string.Empty)
    {
        Text = text;
        ResourceType = resource;
        _currentText = text ?? string.Empty; // 回退初始显示
    }

    public LocalizedExtension(string text, params object[] arguments) : base(string.Empty)
    {
        Text = text;
        Arguments = arguments;
        _currentText = text ?? string.Empty; // 回退初始显示
    }

    public LocalizedExtension(Type resource, string text, params object[] arguments) : base(string.Empty)
    {
        Text = text;
        Arguments = arguments;
        ResourceType = resource;
        _currentText = text ?? string.Empty; // 回退初始显示
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LocalizedExtension, string>(nameof(Text));

    public static readonly StyledProperty<string?> ResourceProperty =
        AvaloniaProperty.Register<LocalizedExtension, string?>(nameof(Resource));

    public static readonly StyledProperty<object[]?> ArgumentsProperty =
        AvaloniaProperty.Register<LocalizedExtension, object[]?>(nameof(Arguments));

    public static readonly StyledProperty<Type?> ResourceTypeProperty =
        AvaloniaProperty.Register<LocalizedExtension, Type?>(nameof(ResourceType));

    // 支持绑定的单个参数位（当未提供 Arguments 时生效）
    public static readonly StyledProperty<object?> Arg0Property =
        AvaloniaProperty.Register<LocalizedExtension, object?>(nameof(Arg0));
    public static readonly StyledProperty<object?> Arg1Property =
        AvaloniaProperty.Register<LocalizedExtension, object?>(nameof(Arg1));
    public static readonly StyledProperty<object?> Arg2Property =
        AvaloniaProperty.Register<LocalizedExtension, object?>(nameof(Arg2));
    public static readonly StyledProperty<object?> Arg3Property =
        AvaloniaProperty.Register<LocalizedExtension, object?>(nameof(Arg3));
    public static readonly StyledProperty<object?> Arg4Property =
        AvaloniaProperty.Register<LocalizedExtension, object?>(nameof(Arg4));

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

    public object? Arg0 { get => GetValue(Arg0Property); set => SetValue(Arg0Property, value); }
    public object? Arg1 { get => GetValue(Arg1Property); set => SetValue(Arg1Property, value); }
    public object? Arg2 { get => GetValue(Arg2Property); set => SetValue(Arg2Property, value); }
    public object? Arg3 { get => GetValue(Arg3Property); set => SetValue(Arg3Property, value); }
    public object? Arg4 { get => GetValue(Arg4Property); set => SetValue(Arg4Property, value); }

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

    // 关键：MarkupExtension 的 ProvideValue 必须返回 object
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = LocalizationExtensions.LocalizationManager;
        if (localizationManager is not null)
        {
            LocalizationManager = localizationManager;
            SetLanguageValue(localizationManager);
        }
        else
        {
            // 未初始化时回退为原始文本，避免 NRE
            CurrentText = Text ?? string.Empty;
        }

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

    void TryRefresh()
    {
        if (_localizationManager is null)
        {
            CurrentText = Text ?? string.Empty;
            return;
        }
        SetLanguageValue(_localizationManager);
    }

    void SetLanguageValue(ILocalizationManager localizationManager)
    {
        if (localizationManager is null)
        {
            CurrentText = Text ?? string.Empty;
            return;
        }

        // Text 为空直接回退，避免索引器抛异常
        if (string.IsNullOrWhiteSpace(Text))
        {
            CurrentText = string.Empty;
            return;
        }

        IStringLocalizer? stringLocalizer = null;
        try
        {
            if (ResourceType is not null)
                stringLocalizer = localizationManager.GetResource(ResourceType);
            else if (!Resource.IsNullOrEmpty())
                stringLocalizer = localizationManager.GetResource(Resource);
        }
        catch
        {
            // 忽略资源解析异常，回退到默认本地化器
            stringLocalizer = null;
        }

        // 汇总参数：优先使用 Arguments；否则使用 Arg0..Arg4 中非空值
        object[]? args = Arguments;
        if (args is null)
        {
            var list = new System.Collections.Generic.List<object>(5);
            if (Arg0 is not null) list.Add(Arg0);
            if (Arg1 is not null) list.Add(Arg1);
            if (Arg2 is not null) list.Add(Arg2);
            if (Arg3 is not null) list.Add(Arg3);
            if (Arg4 is not null) list.Add(Arg4);
            args = list.Count > 0 ? list.ToArray() : null;
        }

        string? value = null;
        if (stringLocalizer is not null)
        {
            value = args is not null
                ? stringLocalizer[Text, args].Value
                : stringLocalizer[Text].Value;
        }
        else
        {
            value = args is not null
                ? localizationManager[Text, args].Value
                : localizationManager[Text].Value;
        }

        CurrentText = value ?? (Text ?? string.Empty);
    }
}
