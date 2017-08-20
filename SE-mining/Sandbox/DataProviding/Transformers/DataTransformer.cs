using System;
using System.Collections.Generic;
using System.Linq;
using SEMining.Sandbox.DataProviding.Predicates;
using SEMining.Sandbox.Models;
using SEMining.StockData.Models;

namespace SEMining.Sandbox.DataProviding.Transformers
{
    public class DataTransformer : ITransformer
    {
        public List<Candle> Transform(IEnumerable<Tick> tiks, DataPredicate predicate)
        {
            return tiks.Where(m => (predicate.From == DateTime.MinValue || m.Date() >= predicate.From) &&
                                   (predicate.To == DateTime.MinValue || m.Date() <= predicate.To))
                                   .GroupBy(item => item.Date().Ticks / predicate.AccumulationPeriod.Ticks)
                .Select(x =>
                {
                    var values = x.ToList();
                    return new Candle.Builder()
                    .WithDate(new DateTime(x.Key * predicate.AccumulationPeriod.Ticks + predicate.AccumulationPeriod.Ticks))
                    .WithId(predicate.Id)
                    .WithOpen(values.First().Price)
                    .WithClose(values.Last().Price)
                    .WithHigh(values.Max(y => y.Price))
                    .WithLow(values.Min(y => y.Price))
                    .WithVolume(values.Sum(y => y.Volume)).Build();
                }).ToList();
        }

        public List<Tick> Transform(IEnumerable<DataTick> tiks, TickPredicate predicate)
        {
            return tiks.Where(m => (predicate.From == DateTime.MinValue || m.Date >= predicate.From) &&
                                    (predicate.To == DateTime.MinValue || m.Date <= predicate.To))
                                    .Select(c => new Tick.Builder()
            .WithDate(c.Date)
            .WithPrice(c.Price)
            .WithId(predicate.Id)
            .WithVolume(c.Volume)
            .Build())
            .ToList();
        }
    }
}
