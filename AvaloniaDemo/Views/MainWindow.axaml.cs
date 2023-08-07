using Avalonia.Controls;
using Volo.Abp.DependencyInjection;

namespace AvaloniaDemo.Views;

public partial class MainWindow : Window, ISingletonDependency
{
    public MainWindow(MainView mainView)
    {
        InitializeComponent();

        this.Content = mainView;
    }
}