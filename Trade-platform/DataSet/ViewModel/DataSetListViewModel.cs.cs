using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.View;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetListViewModel : BindableBase, IDataSetListViewModel
    {
        public ICommand CreateNewDataSetCommand { get; private set; }

        public DataSetListViewModel()
        {
            CreateNewDataSetCommand = new DelegateCommand(CreateNewDataSet);
            var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<CreateDataSetItem>().Subscribe(ProcessCreation, false);
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

        private void ProcessCreation(DataSetItem item)
        {
        }
    }
}
