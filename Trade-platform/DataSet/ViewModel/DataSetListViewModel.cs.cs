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
using System.Threading.Tasks;
using Prism.Commands;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.Holders;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetListViewModel : BindableBase, IDataSetListViewModel
    {
        public ICommand CreateNewDataSetCommand { get; private set; }
        public ICommand RemoveDataSetCommand { get; private set; }
        public ICommand LoadedWindowCommand { get; private set; }
        private readonly IInfoPublisher _infoPublisher;

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
            RemoveDataSetCommand = new DelegateCommand<IDataSetPresenter>(RemoveDataSet);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<CreateDataSetItem>().Subscribe(ProcessCreation, false);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
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

        private void RemoveDataSet(IDataSetPresenter item)
        {
            item?.DeleteData();
        }

        private void WindowLoaded()
        {
            var updateHistory = new Task<ObservableCollection<IDataSetPresenter>>(() =>
            {
                var dataSetHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
                return new ObservableCollection<IDataSetPresenter>(dataSetHolder.GetAll()
                    .Select(i =>
                    {
                        var presenter = ContainerBuilder.Container
                            .Resolve<IDataSetPresenter>(new DependencyOverride<DataSetItem>(i));
                        presenter.CheckData();
                        return presenter;
                    })
                    .ToList());
            });
            updateHistory.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    DataSetPresenterInfo = t.Result;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            updateHistory.Start();
        }
    }
}
