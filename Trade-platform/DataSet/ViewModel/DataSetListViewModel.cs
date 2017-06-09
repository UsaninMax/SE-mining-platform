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

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetListViewModel : BindableBase, IDataSetListViewModel
    {
        public ICommand CreateNewDataSetCommand { get; private set; }
        public ICommand RemoveDataSetCommand { get; private set; }
        public ICommand LoadedWindowCommand { get; private set; }
        public ICommand OpenFolderCommand { get; private set; }
        public ICommand CopyDataSetCommand { get; private set; }
        public ICommand ShowStructureCommand { get; private set; }
        private readonly IInfoPublisher _infoPublisher;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<IDataSetPresenter> _dataSetPresenterInfo = new ObservableCollection<IDataSetPresenter>();
        public ObservableCollection<IDataSetPresenter> DataSetPresenterInfo
        {
            get
            {
                return _dataSetPresenterInfo;
            }
            set
            {
                _dataSetPresenterInfo = value;
                RaisePropertyChanged();
            }
        }


        private IDataSetPresenter _selectedSetPresenter;
        public IDataSetPresenter SelectedSetPresenter
        {
            get { return _selectedSetPresenter; }
            set
            {
                _selectedSetPresenter = value;
                RaisePropertyChanged();
                UpdateVisibilityOfContextMenu();
            }
        }

        public DataSetListViewModel()
        {
            CreateNewDataSetCommand = new DelegateCommand(CreateNewDataSet);
            RemoveDataSetCommand = new DelegateCommand(RemoveDataSet, CanDoAction);
            CopyDataSetCommand = new DelegateCommand(CopyDataSet, CanDoAction);
            OpenFolderCommand = new DelegateCommand(OpenFolder, CanDoAction);
            ShowStructureCommand = new DelegateCommand(ShowStructure, CanDoAction);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<CreateDataSetItemEvent>().Subscribe(ProcessCreation, false);
            _eventAggregator.GetEvent<RemovePresenterFromListEvent>().Subscribe(RemovePresenmterFromList, false);
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
            DataSetPresenterInfo.Add(presenter);
            presenter.PrepareData();
        }

        private void RemovePresenmterFromList(IDataSetPresenter presenter)
        {
            DataSetPresenterInfo.Remove(presenter);
        }

        private void RemoveDataSet()
        {
            _selectedSetPresenter?.DeleteData();
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

        private void CopyDataSet()
        {
            Application.Current.Windows.OfType<CopyDataSetElementView>().SingleOrDefault(x => x.IsInitialized)?.Close();
            ContainerBuilder.Container.Resolve<CopyDataSetElementView>().Show();
            _eventAggregator.GetEvent<CopyDataSetEvent>().Publish(_selectedSetPresenter.DataSet().Clone() as DataSetItem);
        }

        private void OpenFolder()
        {
            _selectedSetPresenter?.ShowDataInFolder();
        }

        private void ShowStructure()
        {
            ContainerBuilder.Container.Resolve<ShowDataSetElementView>().Show();
            _eventAggregator.GetEvent<ShowDataSetEvent>().Publish(_selectedSetPresenter.DataSet());
        }

        private void UpdateVisibilityOfContextMenu()
        {
            (RemoveDataSetCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (CopyDataSetCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (OpenFolderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ShowStructureCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanDoAction()
        {
            return _selectedSetPresenter != null;
        }
    }
}
