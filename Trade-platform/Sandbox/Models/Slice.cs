using System;
using System.Collections.Generic;

namespace TradePlatform.Sandbox.Models
{
    public class Slice
    {
        public DateTime @DateTime { get; private set; }
        public IEnumerable<Tick> Ticks { get; private set; }
        public IEnumerable<Candle> Candles { get; private set; }
        public IEnumerable<Indicator> Indicators { get; private set; }

        private Slice()
        {
        }

        public class Builder
        {
            private DateTime _dateTime;
            private ICollection<Tick> _ticks;
            private ICollection<Candle> _candles;
            private ICollection<Indicator> _indicators;

            public Builder WithDate(DateTime value)
            {
                _dateTime = value;
                return this;
            }

            public Builder WithTick(ICollection<Tick> values)
            {
                _ticks = values;
                return this;
            }

            public Builder WithCandle(ICollection<Candle> values)
            {
                _candles = values;
                return this;
            }

            public Builder WithIndicator(ICollection<Indicator> values)
            {
                _indicators = values;
                return this;
            }

            public Slice Build()
            {
                return new Slice
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