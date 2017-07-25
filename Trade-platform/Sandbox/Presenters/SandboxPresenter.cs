using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Sandbox.Events;
using TradePlatform.Sandbox.Providers;

namespace TradePlatform.Sandbox.Presenters
{
    public class SandboxPresenter : BindableBase, ISandboxPresenter
    {
        public string DllName { get; set; }

        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }

        private readonly ISandbox _sandbox;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IInfoPublisher _infoPublisher;
        private Task _executionTask;
        private IEventAggregator _eventAggregator;

        public SandboxPresenter(ISandbox sandbox, string name)
        {
            DllName = name;
            _sandbox = sandbox;
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
        }

        public void Execute()
        {
            if (IsActive())
            {
                return;
            }

            StatusMessage = Status.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            _executionTask = new Task(() =>
            {
                var sandboxBuilder = ContainerBuilder.Container.Resolve<ISandboxProvider>();
                ISandbox sandbox = sandboxBuilder.CreateInstance(_sandbox.GetType());
                sandbox.SetToken(_cancellationTokenSource.Token);
                _infoPublisher.PublishInfo(new SandboxInfo { Message = DllName + " build data " });
                sandbox.BuildData();
                if (_cancellationTokenSource.Token.IsCancellationRequested){ return;}
                _infoPublisher.PublishInfo(new SandboxInfo { Message = DllName + " execute bots processing " });
                sandbox.CreateCharts();
                sandbox.Execution();
                if (_cancellationTokenSource.Token.IsCancellationRequested) { return; }
                _infoPublisher.PublishInfo(new SandboxInfo { Message = DllName + " gather result " });
                sandbox.AfterExecution();
                sandbox.CleanMemory();
            }, _cancellationTokenSource.Token);
            _executionTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = Status.FailToExecute;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    StatusMessage = Status.IsDone;
                }
                _eventAggregator.GetEvent<RefreshContextMenuEvent>().Publish(this);
            }, TaskScheduler.FromCurrentSynchronizationContext());
            _executionTask.Start();
        }

        public void StopExecution()
        {
            if (!IsActive())
            {
                return;
            }

            StatusMessage = Status.Canceling;
            var cancelTask = new Task(() =>
            {
                _cancellationTokenSource.Cancel();
                _executionTask.Wait();
                Thread.Sleep(500);

            });
            cancelTask.ContinueWith(t =>
            { 

             StatusMessage = Status.IsCanceled;
             _eventAggregator.GetEvent<RefreshContextMenuEvent>().Publish(this);
            }, TaskScheduler.FromCurrentSynchronizationContext());
            cancelTask.Start();
        }

        public bool IsActive()
        {
            return _executionTask != null
                   && !_executionTask.IsCompleted; ;
        }
    }
}
