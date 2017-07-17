using TradePlatform.DataSet.ViewModels;
using Microsoft.Practices.Unity;

namespace TradePlatform.DataSet.Views
{
    public partial class DataSetListView
    {
        public DataSetListView()
        {
            InitializeComponent();
            DataContext = ContainerBuilder.Container.Resolve<IDataSetListViewModel>();
        }
    }
}
