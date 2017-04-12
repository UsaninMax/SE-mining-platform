using System.Linq;
using System.Windows;
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
            LoadInstrumentCommand = new DelegateCommand(o => HistoryInstrumentsPage());
        }

        public ICommand LoadInstrumentCommand { get; set; }

        private void HistoryInstrumentsPage()
        {
            var window = Application.Current.Windows.OfType<HistoryInstrumentsView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            IUnityContainer container = ContainerBuilder.Container;
            container.Resolve<HistoryInstrumentsView>().Show();
        }
    }
}
