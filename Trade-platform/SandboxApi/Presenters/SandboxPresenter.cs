﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using TradePlatform.Commons.Info;

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

        private readonly ISandbox _sandbox;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IInfoPublisher _infoPublisher;
        private Task _executionTask;

        public SandboxPresenter(ISandbox sandbox, string name)
        {
            DllName = name;
            _sandbox = sandbox;
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
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
                IProxySandbox proxySandbox = ContainerBuilder.Container.Resolve<IProxySandbox>(
                    new DependencyOverride<ISandbox>((ISandbox)Activator.CreateInstance(_sandbox.GetType())));

                proxySandbox.Before(_cancellationTokenSource.Token);
                proxySandbox.Execution(_cancellationTokenSource.Token);
                proxySandbox.After(_cancellationTokenSource.Token);

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