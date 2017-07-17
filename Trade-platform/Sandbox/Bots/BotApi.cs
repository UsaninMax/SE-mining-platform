using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Bots
{
    public abstract class BotApi : IBot
    {
        private string _id;
        private IList<Slice> _data;
        private BotPredicate _predicate;
        private readonly ITransactionsContext _context;

        protected BotApi(IDictionary<string, BrokerCost> brokerCosts)
        {
            _context = ContainerBuilder.Container.Resolve<ITransactionsContext>(new DependencyOverride<IDictionary<string, BrokerCost>>(brokerCosts));
        }

        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpData(IList<Slice> data)
        {
            _data = data;
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
            _data.Where(m => (_predicate.From == DateTime.MinValue || m.DateTime >= _predicate.From) &&
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
    }
}