using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using SEMining.StockData.ViewModels;

namespace SEMining.StockData.Views
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
