using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Abp.Localization.Avalonia;

[DependsOn(typeof(AbpAutofacModule))]
public class AbpLocalizationAvaloniaModule : AbpModule
{

}