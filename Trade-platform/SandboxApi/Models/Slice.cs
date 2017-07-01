using System;
using System.Collections.Generic;
namespace TradePlatform.SandboxApi.Models
{
    public class Slice
    {
        public DateTime @DateTime { get; private set; }
        public IDictionary<string, Tick> Ticks { get; private set; }
        public IDictionary<string, Candle> Candles { get; private set; }
        public IDictionary<string, Indicator> Indicators { get; private set; }

        private Slice()
        {
        }

        public class Builder
        {
            private DateTime _dateTime;
            private IDictionary<string, Tick> _ticks = new Dictionary<string, Tick>();
            private IDictionary<string, Candle> _candles = new Dictionary<string, Candle>();
            private IDictionary<string, Indicator> _indicators = new Dictionary<string, Indicator>();

            public Builder WithDate(DateTime value)
            {
                _dateTime = value;
                return this;
            }

            public Builder WithTick(Tick value)
            {
                _ticks.Add(value.Id, value);
                return this;
            }

            public Builder WithCandle(Candle value)
            {
                _candles.Add(value.Id,value);
                return this;
            }

            public Builder WithIndicator(Indicator value)
            {
                _indicators.Add(value.Id, value);
                return this;
            }

            public Slice Build()
            {
                return new Slice()
                {
                    DateTime = _dateTime,
                    Ticks = _ticks,
                    Candles = _candles,
                    Indicators = _indicators
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(DateTime)}: {DateTime}, " +
                   $"{nameof(Ticks)}: {Ticks}, " +
                   $"{nameof(Candles)}: {Candles}, " +
                   $"{nameof(Indicators)}: {Indicators}";
        }
    }
}