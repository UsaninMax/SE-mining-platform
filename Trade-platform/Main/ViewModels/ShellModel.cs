using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using TradePlatform.Commons.BaseModels;
using TradePlatform.Commons.Info.Views;
using HistoryInstrumentsView = TradePlatform.StockData.Views.HistoryInstrumentsView;
using TradePlatform.DataSet.View;

namespace TradePlatform.Main.ViewModels
{
    class ShellModel : BindableBase, IShellModel
    {

        public ICommand LoadInstrumentCommand { get; set; }
        public ICommand ShowInfoCommand { get; set; }
        public ICommand ShowDataSetListCommand { get; set; }

        public ShellModel()
        {
            LoadInstrumentCommand = new DelegateCommand(o => HistoryInstrumentsPage());
            ShowInfoCommand = new DelegateCommand(o => ShowInfoPage());
            ShowDataSetListCommand = new DelegateCommand(o => ShowDataSetListPage());
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
