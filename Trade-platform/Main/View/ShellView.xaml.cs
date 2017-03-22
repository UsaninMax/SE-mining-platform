using System.Windows;
using TradePlatform.Main.ViewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.view
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IShellModel>();
        }
    }
}
