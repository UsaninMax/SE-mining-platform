using System.Windows;
using TradePlatform.StockDataDownload.viewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.StockDataDownload.view
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
