using Abp.Localization.Avalonia;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace AvaloniaDemo.ViewModels;

public class MainViewModel : ViewModelBase, ITransientDependency
{
    public MainViewModel(LocalizationManager localizationManager)
    {
        // on property changed, raise property changed for SaveText
        localizationManager.PropertyChanged += (sender, args) => this.RaisePropertyChanged(nameof(SaveText));
        localizationManager.PropertyChanged += (sender, args) => this.RaisePropertyChanged(nameof(Greeting));
    }

    public string Greeting => L["LongWelcomeMessage"];

    public string SaveText => L["Save"];
}