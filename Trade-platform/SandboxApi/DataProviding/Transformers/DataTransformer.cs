using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using TradePlatform.StockData.Models;


namespace TradePlatform.SandboxApi.DataProviding.Transformers
{
    public class DataTransformer : ITransformer
    {
        public List<Candle> Transform(List<DataTick> tiks, DataPredicate predicate)
        {
            return tiks.GroupBy(item => item.Date.Ticks / predicate.AccumulationPeriod.Ticks)
                .Select(x =>
                {
                    var values = x.ToList();
                    return new Candle.Builder()
                    .WithDate(new DateTime(x.Key * predicate.AccumulationPeriod.Ticks))
                    .WithId(predicate.Id)
                    .WithOpen(values.First().Price)
                    .WithClose(values.Last().Price)
                    .WithHigh(values.Max(y => y.Price))
                    .WithLow(values.Min(y => y.Price))
                    .WithVolume(values.Sum(y => y.Volume)).Build();
                }).ToList();
        }

        public List<Tick> Transform(List<DataTick> tiks, TickPredicate predicate)
        {
            return tiks.Select(c => new Tick.Builder()
            .WithDate(c.Date)
            .WithPrice(c.Price)
            .WithId(predicate.Id)
            .WithVolume(c.Volume)
            .Build())
            .ToList();
        }
    }
}
