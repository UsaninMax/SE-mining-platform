using System.Windows;

using TradePlatform.view;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace TradePlatform
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override IUnityContainer CreateContainer()
        {
            return ContainerBuilder.Container;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
