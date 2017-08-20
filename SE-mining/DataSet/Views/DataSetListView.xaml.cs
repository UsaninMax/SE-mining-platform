using SEMining.DataSet.ViewModels;
using Microsoft.Practices.Unity;

namespace SEMining.DataSet.Views
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
