using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using TradePlatform.StockData.Models;


namespace TradePlatform.SandboxApi.DataProviding.Transformers
{
    public class DataTransformer : ITransformer
    {
        public List<Candle> Transform(List<DataTick> tiks, DataPredicate predicate)
        {
            List<Candle> candles = new List<Candle>();
          
            var groupedTiks = from dt in tiks
                group dt by dt.Date.Ticks / predicate.AccumulationPeriod.Ticks
                into g
                select new { Date = new DateTime(g.Key * predicate.AccumulationPeriod.Ticks), Values = g.ToList() };

            groupedTiks.ForEach(x =>
            {
                Candle.Builder builder = new Candle.Builder();
                builder.WithDate(x.Date);
                builder.WithId(predicate.Id);
                builder.WithOpen(x.Values.First().Price);
                builder.WithClose(x.Values.Last().Price);
                builder.WithHigh(x.Values.Max(y => y.Price));
                builder.WithLow(x.Values.Min(y => y.Price));
                builder.WithVolume(x.Values.Sum(y => y.Volume));
                candles.Add(builder.Build());
            });

            return candles;
        }

        public List<Tick> Transform(List<DataTick> tiks, TickPredicate predicate)
        {
            List<Tick> result = new List<Tick>();

            var groupedTiks = from dt in tiks
                group dt by dt.Date.Ticks 
                into g
                select new { Date = new DateTime(g.Key), Values = g.ToList() };

            groupedTiks.ForEach(x =>
            {
                Tick.Builder builder = new Tick.Builder();
                builder.WithDate(x.Date);
                builder.WithId(predicate.Id);
                builder.WithPrice(x.Values.Last().Price);
                builder.WithVolume(x.Values.Sum(y => y.Volume));
                result.Add(builder.Build());
            });

            return result;
        }
    }
}
