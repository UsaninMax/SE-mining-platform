using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using TradePlatform.Commons.Info.Views;
using HistoryInstrumentsView = TradePlatform.StockData.Views.HistoryInstrumentsView;
using TradePlatform.DataSet.Views;

namespace TradePlatform.Main.ViewModels
{
    class ShellModel : BindableBase, IShellModel
    {

        public ICommand LoadInstrumentCommand { get; set; }
        public ICommand ShowInfoCommand { get; set; }
        public ICommand ShowDataSetListCommand { get; set; }

        public ShellModel()
        {
            LoadInstrumentCommand = new DelegateCommand(HistoryInstrumentsPage);
            ShowInfoCommand = new DelegateCommand(ShowInfoPage);
            ShowDataSetListCommand = new DelegateCommand(ShowDataSetListPage);
        }

        private void HistoryInstrumentsPage()
        {
            var window = Application.Current.Windows.OfType<HistoryInstrumentsView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<HistoryInstrumentsView>().Show();
        }

        private void ShowInfoPage()
        {
            var window = Application.Current.Windows.OfType<InfoView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<InfoView>().Show();
        }

        private void ShowDataSetListPage()
        {
            var window = Application.Current.Windows.OfType<DataSetListView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<DataSetListView>().Show();
        }
    }
}
