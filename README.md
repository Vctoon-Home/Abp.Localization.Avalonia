# Abp.Localization.Avalonia

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://mit-license.org/)
[![GitHub Stars](https://img.shields.io/github/stars/zyknow/Abp.Localization.Avalonia.svg)](https://github.com/zyknow/Abp.Localization.Avalonia/stargazers)
[![GitHub Issues](https://img.shields.io/github/issues/zyknow/Abp.Localization.Avalonia.svg)](https://github.com/zyknow/Abp.Localization.Avalonia/issues)

## Introduction

This project is help you to use Localization on Avalonia UI Framework in ABP Framework.

## Nuget Packages

| Name                             | Version                                                                                                                                                                      | Download                                                                                                                                                                      |
|----------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Abp.Localization.Avalonia | [![Abp.Localization.Avalonia](https://img.shields.io/nuget/v/Abp.Localization.Avalonia.svg)](https://www.nuget.org/packages/Abp.Localization.Avalonia/) | [![Abp.Localization.Avalonia](https://img.shields.io/nuget/dt/Abp.Localization.Avalonia.svg)](https://www.nuget.org/packages/Abp.Localization.Avalonia/) |

## Usage

1. Add Package Reference
    ```xml
    <PackageReference Include="Abp.Localization.Avalonia" Version="1.2.2" />
    ```

2. Add DependsOn and LocalizationManager

    ```csharp
   // DependsOn(typeof(AbpLocalizationAvaloniaModule)) is not required 
   [DependsOn(typeof(AbpLocalizationAvaloniaModule))]
   public class YourModule : AbpModule
   {
       public override void ConfigureServices(ServiceConfigurationContext context)
       {
           var services = context.Services;
           // AvaloniaDemoResource is the resource
           services.AddLocalizationManager();
       }
   }
    ```

3. Using in axaml

    * Add xmlns

       ```xml
       xmlns:L="clr-namespace:Abp.Localization.Avalonia;assembly=Abp.Localization.Avalonia"
       ```
    * use, more simple please see `AvaloniaDemo`
       ```xml
        <TextBlock FontSize="20" Text="{L:Localized Submit}"></TextBlock>
      
        <TextBlock FontSize="20" Text="{L:Localized {x:Type localization:IdentityResource},UserName}"></TextBlock>

        <TextBlock FontSize="20" Text="{L:Localized {x:Type localization:IdentityResource},Volo.Abp.Identity:UserNameNotFound,test}">
        </TextBlock>
       ```

4. Using in cs

    * Add using

       ```csharp
       using Abp.Localization.Avalonia;
       ```
    * use
       ```csharp
      // L is LocalizationManager,get form DI
       var localizedString = L.Localize("Welcome");
       ```

### Thanks

## Author

[Zyknow](https://github.com/zyknow)

## Credits
[LocalizationManager.Avalonia](https://github.com/MicroSugarDeveloperOrg/LocalizationManager.Avalonia)

## License

> You can check out the full license [here](https://github.com/zyknow/Abp.Localization.Avalonia/blob/master/LICENSE)

This project is licensed under the terms of the **MIT** license.
