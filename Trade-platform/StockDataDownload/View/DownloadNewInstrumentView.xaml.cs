using System.Windows.Controls;
using TradePlatform.StockDataDownload.viewModel;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace TradePlatform.StockDataDownload.view
{
    public partial class DownloadNewInstrumentView : UserControl
    {
        public DownloadNewInstrumentView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = ContainerBuilder.Container.Resolve<IDownloadNewInstrumentViewModel>();
            }
        }
    }
}
