using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.SandboxApi.Events;

namespace TradePlatform.SandboxApi.Presenters
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

        private readonly Sandbox _sandbox;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IInfoPublisher _infoPublisher;
        private Task _executionTask;
        private IEventAggregator _eventAggregator;

        public SandboxPresenter(Sandbox sandbox, string name)
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
                Sandbox sandbox = (Sandbox) Activator.CreateInstance(_sandbox.GetType());
                sandbox.Token = _cancellationTokenSource.Token;
                sandbox.BuildData();
                sandbox.Execution();
                sandbox.AfterExecution();
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
