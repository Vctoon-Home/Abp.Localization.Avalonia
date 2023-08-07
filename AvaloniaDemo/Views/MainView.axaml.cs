using Avalonia.Controls;
using AvaloniaDemo.ViewModels;
using Volo.Abp.DependencyInjection;

namespace AvaloniaDemo.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    public MainView(MainViewModel vm)
    {
        InitializeComponent();

        this.DataContext = vm;
    }
}