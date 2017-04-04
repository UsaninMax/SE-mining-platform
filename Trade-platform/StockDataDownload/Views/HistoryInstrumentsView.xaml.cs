using System.Windows;
using Microsoft.Practices.Unity;
using TradePlatform.StockDataDownload.ViewModels;

namespace TradePlatform.StockDataDownload.Views
{
    public partial class HistoryInstrumentsView : Window
    {
        public HistoryInstrumentsView()
        {
            InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IHistoryInstrumentsViewModel>();
        }
    }
}
