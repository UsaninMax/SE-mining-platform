using System.Windows.Controls;
using TradePlatform.Common.ViewModel;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadedData.xaml
    /// </summary>
    public partial class DownloadedDataView : UserControl
    {
        public DownloadedDataView(IViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
