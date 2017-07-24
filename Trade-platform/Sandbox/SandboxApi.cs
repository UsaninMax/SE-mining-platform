using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.Populating.Predicates;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating;

namespace TradePlatform.Sandbox
{
    public abstract class SandboxApi : ISandbox
    {
        public IList<Slice> Data => _data;
        public CancellationToken Token => _token;
        public ICollection<IBot> Bots => _bots;
        private IList<Slice> _data;
        private CancellationToken _token;
        private ICollection<IBot> _bots;
        private IChartsPopulator _chartsPopulator;


        protected SandboxApi()
        {
            _chartsPopulator = ContainerBuilder.Container.Resolve<IChartsPopulator>();
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
            IDataProvider dataProvider = ContainerBuilder.Container.Resolve<IDataProvider>();
            _data = dataProvider.Get(SetUpData(), _token);
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
                    x.SetUpData(Data);
                    x.ResetTransactionContext();
                    x.Execute();
                }, _token);

            }).ToList());
            continuation.Wait();
        }

        
        public void PopulateCharts(IEnumerable<ChartPredicate> predicates)
        {
            _chartsPopulator.Populate(predicates);
        }

        public void CleanMemory()
        {
            _data = null;
            _bots = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public abstract ICollection<IPredicate> SetUpData();
        public abstract void Execution();
        public abstract void AfterExecution();

        public abstract IEnumerable<Panel> SetUpCharts();
    }
}
