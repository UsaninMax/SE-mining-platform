using System.Windows.Controls;
using TradePlatform.StockDataUploud.viewModel;
using Microsoft.Practices.Unity;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadNewData.xaml
    /// </summary>
    public partial class DownloadNewDataView : UserControl
    {
        public DownloadNewDataView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IDownloadNewDataViewModel>();
        }
    }
}
