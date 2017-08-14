using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TradePlatform.StockData.ViewModels;

namespace TradePlatform.StockData.Views
{
    public partial class DownloadNewInstrumentView : UserControl
    {
        public DownloadNewInstrumentView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                IDownloadNewInstrumentViewModel viewModel = ContainerBuilder.Container.Resolve<IDownloadNewInstrumentViewModel>();
                DataContext = viewModel;
                viewModel.UpdateSecuritiesInfo();

            }
        }
    }
}
