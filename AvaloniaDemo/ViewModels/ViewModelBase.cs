using Abp.Localization.Avalonia;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace AvaloniaDemo.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public LocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<LocalizationManager>()!;
}