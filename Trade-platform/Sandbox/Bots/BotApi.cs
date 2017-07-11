using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
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
        private IList<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>> _data;
        private BotPredicate _predicate;
        private ITransactionsContext _context;

        protected BotApi()
        {
            _context = ContainerBuilder.Container.Resolve<ITransactionsContext>();
        }

        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpData(IList<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>> data)
        {
            _data = data;
        }

        public void SetUpPredicate(BotPredicate predicate)
        {
            _predicate = predicate;
        }

        public void SetUpCosts(IEnumerable<BrokerCost> value)
        {
            _context.SetUpCosts(value);
        }

        public void SetUpBalance(double value)
        {
            _context.SetUpBalance(value);
        }

        public void OpenPosition(ImmediatePositionRequest request)
        {
            _context.OpenPosition(request);
        }

        public Guid OpenPosition(PostponedPositionRequest request)
        {
            return _context.OpenPosition(request);
        }

        public void Execute()
        {
            _data.Where(m => (_predicate.From == DateTime.MinValue || m.Item1 >= _predicate.From) &&
                             (_predicate.To == DateTime.MinValue || m.Item1 <= _predicate.To))
                .ForEach(x =>
                {
                    _context.ProcessTick(x.Item3);
                    if (!x.Item2.IsNullOrEmpty())
                    {
                        Execution(x.Item2);
                    }
                });
        }

        public abstract void Execution(IEnumerable<IData> slice);
        public abstract int Score();
    }
}