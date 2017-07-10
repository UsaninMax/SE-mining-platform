using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Bots
{
    public abstract class BotApi : IBot
    {
        private string _id;
        private IList<Pair<DateTime, IEnumerable<IData>>> _data;
        private BotPredicate _predicate;


        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpData(IList<Pair<DateTime, IEnumerable<IData>>> data)
        {
            _data = data;
        }

        public void SetUpPredicate(BotPredicate predicate)
        {
            _predicate = predicate;
        }

        public void Execute()
        {
            _data.Where(m => (_predicate.From == DateTime.MinValue || m.First >= _predicate.From) &&
                             (_predicate.To == DateTime.MinValue || m.First <= _predicate.To))
                .ForEach(x =>
                {

                    Execution(x.Second);

                });
        }

        public abstract void Execution(IEnumerable<IData> slice);
        public abstract int Score();
    }
}