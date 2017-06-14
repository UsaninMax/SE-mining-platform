using TradePlatform.DataSet.ViewModels;
using Microsoft.Practices.Unity;
using System.Windows;

namespace TradePlatform.DataSet.Views
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
