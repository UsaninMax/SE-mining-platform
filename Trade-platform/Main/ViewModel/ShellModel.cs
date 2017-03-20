using System.Windows.Input;
using TradePlatform.common.viewModel;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using TradePlatform.Main.ViewModel;
using TradePlatform.StockDataUploud.view;

namespace TradePlatform.viewModel
{
    class ShellModel : BindableBase, IShellModel
    {
        public ShellModel()
        {
            this.LoadDownloadedDataCommand = new DelegateCommand(o => this.LoadDownloadedDataPage());
        }

        public ICommand LoadDownloadedDataCommand { get; private set; }

        private void LoadDownloadedDataPage()
        {
            IUnityContainer container = ContainerBuilder.Container;
            container.Resolve<HistoryView>().Show();
        }
    }
}
