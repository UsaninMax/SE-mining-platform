using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TradePlatform.StockData.ViewModels;

namespace TradePlatform.StockData.Views
{
    public partial class DownloadedInstrumentsView : UserControl
    {

        public DownloadedInstrumentsView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsViewModel>();
            }
        }
    }
}
