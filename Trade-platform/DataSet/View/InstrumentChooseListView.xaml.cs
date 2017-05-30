using TradePlatform.DataSet.ViewModel;
using Microsoft.Practices.Unity;
using System.Windows;

namespace TradePlatform.DataSet.View

{
    public partial class InstrumentChooseListView : Window
    {
        public InstrumentChooseListView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IInstrumentChooseListViewModel>();
        }
    }
}
