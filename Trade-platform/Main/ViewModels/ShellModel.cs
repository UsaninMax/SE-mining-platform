using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using TradePlatform.Commons.BaseModels;
using HistoryInstrumentsView = TradePlatform.StockDataDownload.Views.HistoryInstrumentsView;

namespace TradePlatform.Main.ViewModels
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
