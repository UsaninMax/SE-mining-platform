using System.Windows;
using Prism.Unity;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Holders;
using TradePlatform.Main.Views;

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
            InitializeHolders();
        }

        private void InitializeHolders()
        {
            var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
            instrumentsHolder.RestoreFromSettings();
        }
    }
}
