using System.Windows.Controls;
using TradePlatform.StockDataUploud.viewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadedData.xaml
    /// </summary>
    public partial class DownloadedDataView : UserControl
    {
        public DownloadedDataView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IDownloadedDataViewModel>();
        }
    }
}
