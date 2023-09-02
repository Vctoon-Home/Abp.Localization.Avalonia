using Avalonia.Controls;
using AvaloniaDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace AvaloniaDemo.Views;

public partial class MainWindow : Window, ISingletonDependency
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = App.Services.GetRequiredService<MainWindowViewModel>();
    }
}