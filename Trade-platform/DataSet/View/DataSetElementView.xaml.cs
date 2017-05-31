using TradePlatform.DataSet.ViewModel;
using Microsoft.Practices.Unity;
using System.Windows;

namespace TradePlatform.DataSet.View
{
    public partial class DataSetElementView : Window
    {
        public DataSetElementView()
        {
            this.InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IDataSetElementViewModel>();
        }
    }
}
