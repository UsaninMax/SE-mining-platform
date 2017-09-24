using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using SEMining.Charts.Data.Holders;
using SEMining.Charts.Data.Populating;
using SEMining.Charts.Vizualization.Dispatching;
using SEMining.Commons.Info;
using SEMining.Sandbox.Bots;
using SEMining.Sandbox.DataProviding;
using SEMining.Sandbox.Holders;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Charts.Vizualization.Configurations;
using SE_mining_base.Info.Message;
using SE_mining_base.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox
{
    public abstract class SandboxAbstraction : ISandbox
    {
        private CancellationToken Token => _token;
        protected IEnumerable<IBot> Bots => _bots;
        private CancellationToken _token;
        private IEnumerable<IBot> _bots;
        private readonly IInfoPublisher _infoPublisher;

        public SandboxAbstraction()
        {
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
        }

        public void SendMessage(string message)
        {
            _infoPublisher.PublishInfo(new SandboxInfo{ Message = message });
        }

        public void SetToken(CancellationToken token)
        {
            _token = token;
        }

        public void SetUpBots(IEnumerable<IBot> bots)
        {
            _bots = bots;
        }

        public void BuildData()
        {
            if (_token.IsCancellationRequested) { return; }
            ISandboxDataProvider dataProvider = ContainerBuilder.Container.Resolve<ISandboxDataProvider>();
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            dataHolder.Add(dataProvider.Get(SetUpData(), _token));
        }

        protected void Execute()
        {
            if (_token.IsCancellationRequested) { return; }
            var continuation = Task.WhenAll(_bots?.Select(x =>
            {
                if (!x.IsPrepared())
                {
                    throw new Exception(x.GetId() + " - bot is not correctly set up");
                }

                return Task.Run(() =>
                {
                    x.Execute();
                }, _token);

            }).ToList());
            continuation.Wait(Token);
        }

        public void CleanMemory()
        {
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            dataHolder.Clean();
            _bots = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public abstract IEnumerable<IPredicate> SetUpData();
        public abstract void Execution();
        public abstract void AfterExecution();
        public abstract IEnumerable<PanelViewPredicate> SetUpCharts();

        public void CreateCharts()
        {
            ContainerBuilder.Container.Resolve<IChartPredicatesHolder>().Reset();
            var chartProxy = ContainerBuilder.Container.Resolve<ChartProxy>();
            ContainerBuilder.Container.Resolve<IChartsPopulator>(new DependencyOverride<IEnumerable<PanelViewPredicate>>(SetUpCharts()));
            chartProxy.ShowCharts(SetUpCharts(), ContainerBuilder.Container.Resolve<IChartsBuilder>());
        }

        public void PopulateCharts(IEnumerable<ChartPredicate> predicates)
        {
            ContainerBuilder.Container.Resolve<IChartPredicatesHolder>().Add(predicates);
            ContainerBuilder.Container.Resolve<IChartsPopulator>().Populate();
        }

        public void StoreCustomData(string key, IEnumerable<object> data)
        {
            ContainerBuilder.Container.Resolve<ICustomDataHolder>().Add(key, data);
        }

        public void CleanCustomeStorage()
        {
            ContainerBuilder.Container.Resolve<ICustomDataHolder>().CleanAll();
        }
    }
}
