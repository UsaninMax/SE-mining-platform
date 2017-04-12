using System.Windows;
using Microsoft.Practices.Unity;
using TradePlatform.Main.ViewModels;

namespace TradePlatform.Main.Views
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
