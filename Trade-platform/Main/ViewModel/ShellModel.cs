using System.Windows.Input;
using TradePlatform.common.viewModel;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using TradePlatform.Main.ViewModel;
using TradePlatform.StockDataDownload.view;

namespace TradePlatform.viewModel
{
    class ShellModel : BindableBase, IShellModel
    {
        public ShellModel()
        {
            this.LoadInstrumentCommand = new DelegateCommand(o => this.HistoryInstrumentsPage());
        }

        public ICommand LoadInstrumentCommand { get; private set; }

        private void HistoryInstrumentsPage()
        {
            IUnityContainer container = ContainerBuilder.Container;
            container.Resolve<HistoryInstrumentsView>().ShowDialog();
        }
    }
}
