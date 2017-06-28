using System;
using System.Collections.Generic;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.SandboxApi.DataProviding.Transformers
{
    public class DataTransformer : ITransformer
    {
        public Queue<Candle> Transform(List<DataTick> tiks, DataPredicate predicate)
        {
            tiks.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            Queue<Candle> candles = new Queue<Candle>();

            DateTime openTime = DateTime.MaxValue;
            double high = 0;
            double low = Double.MaxValue;
            double volume = 0;
            Candle.Builder builder = null;

            for (int i = 0; i < tiks.Count; i++)
            {
                if (openTime.Ticks <= tiks[i].Date.Ticks)
                {
                    candles.Enqueue(builder
                        .WithClose(tiks[i].Price)
                        .WithHigh(Math.Max(high, tiks[i].Price))
                        .WithLow(Math.Min(low, tiks[i].Price))
                        .WithVolume(volume)
                        .WithDate(openTime)
                        .Build());
                    builder = null;
                }
                if (builder == null)
                {
                    openTime = tiks[i].Date.AddMilliseconds(predicate.AccumulationPeriod);
                    builder = new Candle.Builder()
                        .WithId(predicate.Id)
                        .WithOpen(tiks[i].Price);
                    high = 0;
                    low = Double.MaxValue;
                    volume = 0;
                }

                high = Math.Max(high, tiks[i].Price);
                low = Math.Min(low, tiks[i].Price);
                volume = volume + tiks[i].Volume;
            }
            return candles;
        }

        public Queue<Tick> Transform(List<DataTick> tiks, TickPredicate predicate)
        {
            tiks.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            Queue<Tick> transformedTiks = new Queue<Tick>();
            double volume = 0;
            DateTime openTime = DateTime.MaxValue;
            Tick.Builder builder = null;

            for (int i = 0; i < tiks.Count; i++)
            {
                if (openTime.Ticks < tiks[i].Date.Ticks)
                {
                    transformedTiks.Enqueue(new Tick.Builder()
                        .WithId(predicate.Id)
                        .WithDate(tiks[i].Date)
                        .WithPrice(tiks[i].Price)
                        .WithVolume(volume)
                        .Build());
                    builder = null;
                }
                if (builder == null)
                {
                    builder = new Tick.Builder();
                    openTime = tiks[i].Date;
                    volume = 0;
                }
                volume = volume + tiks[i].Volume;
            }
            return transformedTiks;
        }
    }
}
