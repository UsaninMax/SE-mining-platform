using System.Windows;
using TradePlatform.Main.ViewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IShellModel>();
        }
    }
}
