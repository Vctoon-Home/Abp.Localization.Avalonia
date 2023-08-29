# Abp.Localization.Avalonia

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://mit-license.org/)
[![GitHub Stars](https://img.shields.io/github/stars/zyknow/Abp.Localization.Avalonia.svg)](https://github.com/zyknow/Abp.Localization.Avalonia/stargazers)
[![GitHub Issues](https://img.shields.io/github/issues/zyknow/Abp.Localization.Avalonia.svg)](https://github.com/zyknow/Abp.Localization.Avalonia/issues)

## Introduction

This project is help you to use Localization on Avalonia UI Framework in ABP Framework.

## Nuget Packages

| Name                             | Version                                                                                                                                                                      | Download                                                                                                                                                                      |
|----------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Zyknow.Abp.Localization.Avalonia | [![Zyknow.Abp.Localization.Avalonia](https://img.shields.io/nuget/v/Zyknow.Abp.Localization.Avalonia.svg)](https://www.nuget.org/packages/Zyknow.Abp.Localization.Avalonia/) | [![Zyknow.Abp.Localization.Avalonia](https://img.shields.io/nuget/dt/Zyknow.Abp.Localization.Avalonia.svg)](https://www.nuget.org/packages/Zyknow.Abp.Localization.Avalonia/) |

## Usage

1. Add Package Reference
    ```xml
    <PackageReference Include="Zyknow.Abp.Localization.Avalonia" Version="1.2.2" />
    ```

2. Add DependsOn and LocalizationManager

    ```csharp
   [DependsOn(typeof(AbpLocalizationAvaloniaModule))]
   public class YourModule : AbpModule
   {
       public override void ConfigureServices(ServiceConfigurationContext context)
       {
           var services = context.Services;
           // AvaloniaDemoResource is the resource
           services.AddLocalizationManager(s => s.GetRequiredService<IStringLocalizerFactory>()
           .Create(typeof(AvaloniaDemoResource)));
       }
   }
    ```

3. Using in axaml

    * Add xmlns

       ```xml
       xmlns:L="clr-namespace:Abp.Localization.Avalonia;assembly=Abp.Localization.Avalonia"
       ```
    * use
       ```xml
       <TextBlock FontSize="20" Text="{L:Localized Welcome}"></TextBlock>
       ```

4. Using in cs

    * Add using

       ```csharp
       using Zyknow.Abp.Localization.Avalonia;
       ```
    * use
       ```csharp
      // L is LocalizationManager,get form DI
       var localizedString = L.Localize("Welcome");
       ```

### Thanks

## Author

[Zyknow](https://github.com/zyknow)

## License

> You can check out the full license [here](https://github.com/zyknow/Abp.Localization.Avalonia/blob/master/LICENSE)

This project is licensed under the terms of the **MIT** license.
