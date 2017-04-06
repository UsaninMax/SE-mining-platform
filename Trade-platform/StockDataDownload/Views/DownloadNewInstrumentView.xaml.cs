using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TradePlatform.StockDataDownload.ViewModels;

namespace TradePlatform.StockDataDownload.Views
{
    public partial class DownloadNewInstrumentView : UserControl
    {
        public DownloadNewInstrumentView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                IDownloadNewInstrumentViewModel viewModel = ContainerBuilder.Container.Resolve<IDownloadNewInstrumentViewModel>();
                this.DataContext = viewModel;
                viewModel.UpdateSecuritiesInfo();

            }
        }
    }
}
