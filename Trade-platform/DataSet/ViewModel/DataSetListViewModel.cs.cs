using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using TradePlatform.DataSet.View;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetListViewModel : BindableBase, IDataSetListViewModel
    {
        public ICommand CreateNewDataSetCommand { get; private set; }

        public DataSetListViewModel()
        {
            CreateNewDataSetCommand = new DelegateCommand(CreateNewDataSet);
        }

        private void CreateNewDataSet()
        {
            var window = Application.Current.Windows.OfType<DataSetElementView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<DataSetElementView>().Show();
        }
    }
}
