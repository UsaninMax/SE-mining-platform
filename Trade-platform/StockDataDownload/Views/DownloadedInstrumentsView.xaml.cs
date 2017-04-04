using System.Windows.Controls;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using TradePlatform.StockDataDownload.ViewModels;

namespace TradePlatform.StockDataDownload.view
{
    public partial class DownloadedInstrumentsView : UserControl
    {
        public DownloadedInstrumentsView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsViewModel>();
            }
        }
    }
}
