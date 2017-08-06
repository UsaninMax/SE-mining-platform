using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Data.Populating;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Dispatching;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Holders;

namespace TradePlatform.Sandbox
{
    public abstract class SandboxApi : ISandbox
    {
        public CancellationToken Token => _token;
        public ICollection<IBot> Bots => _bots;
        private CancellationToken _token;
        private ICollection<IBot> _bots;
        private readonly IChartPredicatesHolder _chartPredicatesHolder;
        private readonly IChartsPopulator _chartsPopulator;
        private readonly ICustomDataHolder _customDataHolder;

        protected SandboxApi()
        {
            ContainerBuilder.Container.Resolve<IChartsPopulator>(new DependencyOverride<IEnumerable<PanelViewPredicate>>(SetUpCharts()));
            _chartPredicatesHolder = ContainerBuilder.Container.Resolve<IChartPredicatesHolder>();
            _chartsPopulator = ContainerBuilder.Container.Resolve<IChartsPopulator>();
            _customDataHolder = ContainerBuilder.Container.Resolve<ICustomDataHolder>();
        }

        public void SetToken(CancellationToken token)
        {
            _token = token;
        }

        public void SetUpBots(ICollection<IBot> bots)
        {
            _bots = bots;
        }

        public void BuildData()
        {
            if (_token.IsCancellationRequested) { return; }
            ISandboxDataProvider dataProvider = ContainerBuilder.Container.Resolve<ISandboxDataProvider>();
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            var data = dataProvider.Get(SetUpData(), _token);
            if (_token.IsCancellationRequested) { return; }
            dataHolder.Add(data);
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
                    x.ResetTransactionContext();
                    x.Execute();
                }, _token);

            }).ToList());
            continuation.Wait();
        }

        public void CleanMemory()
        {
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            dataHolder.Clean();
            _bots = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public abstract ICollection<IPredicate> SetUpData();
        public abstract void Execution();
        public abstract void AfterExecution();
        public abstract IEnumerable<PanelViewPredicate> SetUpCharts();

        public void CreateCharts()
        {
            ContainerBuilder.Container.Resolve<IChartPredicatesHolder>().Reset();
            var chartProxy = ContainerBuilder.Container.Resolve<ChartProxy>();
            chartProxy.ShowCharts(SetUpCharts(), ContainerBuilder.Container.Resolve<IChartsBuilder>());
        }

        public void PopulateCharts(ICollection<ChartPredicate> predicates)
        {
            _chartPredicatesHolder.Set(predicates);
            _chartsPopulator.Populate();
        }

        public void StoreCustomData(string key, IList<object> data)
        {
            _customDataHolder.Add(key, data);
        }

        public void CleanCustomeStorage()
        {
            _customDataHolder.CleanAll();
        }
    }
}
