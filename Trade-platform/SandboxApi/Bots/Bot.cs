using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.SandboxApi.Models;

namespace TradePlatform.SandboxApi.Bots
{
    public abstract class Bot
    {
        public string Id { get; set; }
        public IList<IData> Data { get; set; }
        public BotPredicate Predicate { get; set; }

        public void Execute()
        {
            Data.Where(m => (Predicate.From == DateTime.MinValue || m.Date() >= Predicate.From) &&
                            (Predicate.To == DateTime.MinValue || m.Date() <= Predicate.To) &&
                            Predicate.InstrumentIds.Contains(m.Id())).GroupBy(item => item.Date()).ForEach(x =>
            {

                var values = x.ToList();
                this.Execution(new Slice.Builder()
                    .WithDate(x.Key)
                    .WithCandle(values.OfType<Candle>().ToList())
                    .WithTick(values.OfType<Tick>().ToList())
                    .WithIndicator(values.OfType<Indicator>().ToList())
                    .Build());

            });

        }

        public abstract void Execution(Slice slice);
        public abstract int Score();
    }
}