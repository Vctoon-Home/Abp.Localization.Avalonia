using Abp.Localization.Avalonia;
using ReactiveUI;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Localization;

namespace AvaloniaDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase, ITransientDependency
{
    public MainWindowViewModel(ILocalizationManager localizationManager)
    {
        // on property changed, raise property changed for SaveText
        localizationManager.CurrentCultureChanged += (sender, args) =>
        {
            this.RaisePropertyChanged(nameof(SaveText));
            this.RaisePropertyChanged(nameof(Greeting));
            this.RaisePropertyChanged(nameof(IdentityName));
            this.RaisePropertyChanged(nameof(UserDeletionConfirmationMessage));
        };
    }

    public string Greeting => L["LongWelcomeMessage"];

    public string SaveText => L["Save"];

    // if you want to use the resource from another module, you can use the following code,and don't forget to add the module to the project
    public string IdentityName => L.GetResource<IdentityResource>()["DisplayName:Name"];

    public string UserDeletionConfirmationMessage
    {
        get
        {
            var resource = L.GetResource<IdentityResource>();

            // TODO: the will error
            // var value = resource["Volo.Abp.Identity:010002", new
            // {
            //     MaxUserMembershipCount = 5,
            // }];

            return resource["Volo.Abp.Identity:010002"];
        }
    }
}