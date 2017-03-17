using System.Windows.Input;
using TradePlatform.common.viewModel;
using Prism.Mvvm;
using TradePlatform.Common.ViewModel;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using TradePlatform.Modules;
using TradePlatform.StockDataUploud.View;
using System.Windows;

namespace TradePlatform.viewModel
{
    class ShellModel : BindableBase, IViewModel
    {
        public ShellModel()
        {
            this.LoadDownloadedDataCommand = new DelegateCommand(o => this.LoadDownloadedDataPage());
        }

        public ICommand LoadDownloadedDataCommand { get; private set; }

        private void LoadDownloadedDataPage()
        {
            IUnityContainer container = ContainerBuilder.Container;
            IModule downloadedDataModule = container.Resolve<DownloadedDataModule>();
            IModule downloadNewDataModule = container.Resolve<DownloadNewDataModule>();

            container.Resolve<HistoryView>().Show();

            downloadedDataModule.Initialize();
            downloadNewDataModule.Initialize();
        }
    }
}
