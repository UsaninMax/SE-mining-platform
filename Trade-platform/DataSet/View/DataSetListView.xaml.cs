using TradePlatform.DataSet.ViewModel;
using Microsoft.Practices.Unity;
using System.Windows;

namespace TradePlatform.DataSet.View
{
    public partial class DataSetListView : Window
    {
        public DataSetListView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IDataSetListViewModel>();
        }
    }
}
