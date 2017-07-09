using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Bots
{
    public abstract class BotApi : IBot
    {
        private string _id;
        private IList<IData> _data;
        private BotPredicate _predicate;


        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpData(IList<IData> data)
        {
            _data = data;
        }

        public void SetUpPredicate(BotPredicate predicate)
        {
            _predicate = predicate;
        }

        public void Execute()
        {
            _data.Where(m => (_predicate.From == DateTime.MinValue || m.Date() >= _predicate.From) &&
                            (_predicate.To == DateTime.MinValue || m.Date() <= _predicate.To) &&
                             _predicate.InstrumentIds.Contains(m.Id()))//.GroupBy(item => item.Date())
                             .ForEach(x =>
                {
                    //var values = x.ToList();
                    //Execution(new Slice.Builder()
                    //    .WithDate(x.Key)
                    //    .WithCandle(values.OfType<Candle>().ToList())
                    //    .WithTick(values.OfType<Tick>().ToList())
                    //    .WithIndicator(values.OfType<Indicator>().ToList())
                    //    .Build());

                    Execution(new Slice.Builder()
                        .WithDate(new DateTime())
                        .WithCandle(new List<Candle>(){new Candle.Builder().Build(), new Candle.Builder().Build() })
                        .WithTick(new List<Tick>() { new Tick.Builder().Build(), new Tick.Builder().Build() })
                        .WithIndicator(new List<Indicator>() { new Indicator.Builder().Build(), new Indicator.Builder().Build() })
                        .Build());

                });

        }

        public abstract void Execution(Slice slice);
        public abstract int Score();
    }
}