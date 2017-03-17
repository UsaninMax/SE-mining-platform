using System.Windows.Controls;
using TradePlatform.Common.ViewModel;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadNewData.xaml
    /// </summary>
    public partial class DownloadNewDataView : UserControl
    {
        public DownloadNewDataView(IViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
