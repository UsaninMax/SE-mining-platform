using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.Presenters;
using TradePlatform.DataSet.View;
using System.Collections.ObjectModel;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetListViewModel : BindableBase, IDataSetListViewModel
    {
        public ICommand CreateNewDataSetCommand { get; private set; }

        private ObservableCollection<IDataSetPresenter> _dataSetPresenter = new ObservableCollection<IDataSetPresenter>();
        public ObservableCollection<IDataSetPresenter> DataSetPresenterInfo
        {
            get
            {
                return _dataSetPresenter;
            }
            set
            {
                _dataSetPresenter = value;
                RaisePropertyChanged();
            }
        }


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
            IDataSetPresenter presenter = ContainerBuilder.Container.Resolve<IDataSetPresenter>(
                new DependencyOverride<DataSetItem>(item));
            presenter.PrepareData();
            DataSetPresenterInfo.Add(presenter);
        }
    }
}
