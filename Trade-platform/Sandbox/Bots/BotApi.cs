using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios;
using TradePlatform.Sandbox.Transactios.Models;
using TradePlatform.Sandbox.Holders;
using TradePlatform.Vizualization.Populating.Holders;
using TradePlatform.Vizualization.Populating;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Sandbox.Bots
{
    public abstract class BotApi : IBot
    {
        private string _id;
        private string _sandboxId;
        private BotPredicate _predicate;
        private readonly ITransactionsContext _context;
        private readonly IChartPredicatesHolder _chartPredicatesHolder;
        private readonly IChartsPopulator _chartsPopulator;
        private readonly ICustomDataHolder _customDataHolder;

        protected BotApi(IDictionary<string, BrokerCost> brokerCosts)
        {
            _context = ContainerBuilder.Container.Resolve<ITransactionsContext>(new DependencyOverride<IDictionary<string, BrokerCost>>(brokerCosts));
            _chartPredicatesHolder = ContainerBuilder.Container.Resolve<IChartPredicatesHolder>();
            _chartsPopulator = ContainerBuilder.Container.Resolve<IChartsPopulator>();
            _customDataHolder = ContainerBuilder.Container.Resolve<ICustomDataHolder>();
        }

        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpPredicate(BotPredicate predicate)
        {
            _predicate = predicate;
        }

        public void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value)
        {
            _context.SetUpWorkingPeriod(value);
        }

        public void SetUpBalance(double value)
        {
            _context.SetUpBalance(value);
        }

        public void OpenPosition(OpenPositionRequest request)
        {
            _context.OpenPosition(request);
        }

        public bool IsPrepared()
        {
            return _context.IsPrepared();
        }

        public void ResetTransactionContext()
        {
            _context.Reset();
        }

        public void Execute()
        {
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            dataHolder.Get().Where(m => (_predicate.From == DateTime.MinValue || m.DateTime >= _predicate.From) &&
                             (_predicate.To == DateTime.MinValue || m.DateTime <= _predicate.To))
                .ForEach(x =>
                {
                    _context.ProcessTick(x.Ticks, x.DateTime);
                    if (!x.Datas.IsNullOrEmpty())
                    {
                        Execution(x.Datas);
                    }
                });
        }

        public abstract void Execution(IDictionary<string, IData> data);
        public abstract int Score();

        public void SetUpSandboxId(string id)
        {
            _sandboxId = id;
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