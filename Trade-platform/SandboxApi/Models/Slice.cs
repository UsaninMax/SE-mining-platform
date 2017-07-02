using System;
using System.Collections.Generic;

namespace TradePlatform.SandboxApi.Models
{
    public class Slice
    {
        public DateTime @DateTime { get; private set; }
        public IList<Tick> Ticks { get; private set; }
        public IList<Candle> Candles { get; private set; }
        public IList<Indicator> Indicators { get; private set; }

        private Slice()
        {
        }

        public class Builder
        {
            private DateTime _dateTime;
            private IList<Tick> _ticks;
            private IList<Candle> _candles;
            private IList<Indicator> _indicators;

            public Builder(int tickSize, int candleSize, int indicatorSize)
            {
                _ticks = new List<Tick>(tickSize);
                _candles = new List<Candle>(candleSize);
                _indicators = new List<Indicator>(indicatorSize);
            }


            public Builder WithDate(DateTime value)
            {
                _dateTime = value;
                return this;
            }

            public Builder WithTick(Tick value)
            {
                _ticks.Add(value);
                return this;
            }

            public Builder WithCandle(Candle value)
            {
                _candles.Add(value);
                return this;
            }

            public Builder WithIndicator(Indicator value)
            {
                _indicators.Add(value);
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