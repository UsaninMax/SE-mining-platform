using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Views;
using TradePlatform.DataSet.Views;
using TradePlatform.Sandbox.Events;
using TradePlatform.Sandbox.Presenters;
using TradePlatform.Sandbox.Providers;
using TradePlatform.StockData.Views;

namespace TradePlatform.Main.ViewModels
{
    public class ShellModel : BindableBase, IShellModel
    {

        public ICommand LoadInstrumentCommand { get; private set; }
        public ICommand ShowInfoCommand { get; private set; }
        public ICommand ShowDataSetListCommand { get; private set; }
        public ICommand LoadedWindowCommand { get; private set; }
        public ICommand StartCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private ObservableCollection<ISandboxPresenter> _sandboxPresenterInfo = new ObservableCollection<ISandboxPresenter>();
        public ObservableCollection<ISandboxPresenter> SandboxPresenterInfo
        {
            get
            {
                return _sandboxPresenterInfo;
            }
            set
            {
                _sandboxPresenterInfo = value;
                RaisePropertyChanged();
            }
        }

        private ISandboxPresenter _selectedSandboxPresenter;
        public ISandboxPresenter SelectedSandboxPresenter
        {
            get { return _selectedSandboxPresenter; }
            set
            {
                _selectedSandboxPresenter = value;
                RaisePropertyChanged();
                UpdateVisibilityOfContextMenu();
            }
        }

        private readonly IInfoPublisher _infoPublisher;


        public ShellModel()
        {
            StartCommand = new DelegateCommand(StartExecution, CanDoStartAction);
            CancelCommand = new DelegateCommand(CancelExecution, CanDoCancelAction);
            LoadInstrumentCommand = new DelegateCommand(HistoryInstrumentsPage);
            ShowInfoCommand = new DelegateCommand(ShowInfoPage);
            ShowDataSetListCommand = new DelegateCommand(ShowDataSetListPage);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<RefreshContextMenuEvent>().Subscribe(UpdateVisibilityOfContextMenu);
        }

        private void StartExecution()
        {
            _selectedSandboxPresenter.Execute();
            UpdateVisibilityOfContextMenu();
        }

        private void CancelExecution()
        {
            _selectedSandboxPresenter.StopExecution();
            UpdateVisibilityOfContextMenu();
        }

        private void HistoryInstrumentsPage()
        {
            var window = Application.Current.Windows.OfType<HistoryInstrumentsView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<HistoryInstrumentsView>().Show();
        }

        private void ShowInfoPage()
        {
            var window = Application.Current.Windows.OfType<InfoView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<InfoView>().Show();
        }

        private void ShowDataSetListPage()
        {
            var window = Application.Current.Windows.OfType<DataSetListView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<DataSetListView>().Show();
        }

        private void WindowLoaded()
        {
            var updateListOfSandboxesTask = new Task<ObservableCollection<ISandboxPresenter>>(() =>
            {
                var sandboxDllProvider = ContainerBuilder.Container.Resolve<ISandboxProvider>();
                return new ObservableCollection<ISandboxPresenter>(sandboxDllProvider.Get());
            });
            updateListOfSandboxesTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    SandboxPresenterInfo = t.Result;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            updateListOfSandboxesTask.Start();
        }

        private void UpdateVisibilityOfContextMenu(ISandboxPresenter presenter)
        {
            if (presenter.Equals(_selectedSandboxPresenter))
            {
                UpdateVisibilityOfContextMenu();
            }
        }

        private void UpdateVisibilityOfContextMenu()
        {
            (StartCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (CancelCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanDoStartAction()
        {
            return _selectedSandboxPresenter != null &&
                _sandboxPresenterInfo.All(x => !x.IsActive());
        }

        private bool CanDoCancelAction()
        {
            return _selectedSandboxPresenter != null &&
                _selectedSandboxPresenter.IsActive();
        }
    }
}
