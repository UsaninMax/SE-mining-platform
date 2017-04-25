using System.Windows;
using Microsoft.Practices.Unity;
using TradePlatform.StockData.ViewModels;

namespace TradePlatform.StockData.Views
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
