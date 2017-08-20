using System.Windows;
using Microsoft.Practices.Unity;
using SEMining.StockData.ViewModels;

namespace SEMining.StockData.Views
{
    public partial class HistoryInstrumentsView : Window
    {
        public HistoryInstrumentsView()
        {
            InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IHistoryInstrumentsViewModel>();
        }
    }
}
