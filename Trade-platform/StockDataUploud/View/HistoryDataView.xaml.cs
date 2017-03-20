using System.Windows;
using TradePlatform.StockDataUploud.viewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Window
    {
        public HistoryView()
        {
            InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IHistoryDataViewModel>();
        }
    }
}
