using System;
using System.Collections.Generic;

namespace SE_mining_base.Sandbox.Models
{
    public class Slice
    {
        public DateTime @DateTime { get; private set; }
        public IDictionary<string, Tick> Ticks { get; private set; }
        public IDictionary<string, IData> Datas { get; private set; }

        private Slice()
        {
        }

        public class Builder
        {
            private DateTime _dateTime;
            private IDictionary<string, Tick> _ticks;
            private IDictionary<string, IData> _datas;

            public Builder WithDate(DateTime value)
            {
                _dateTime = value;
                return this;
            }

            public Builder WithTick(IDictionary<string, Tick> values)
            {
                _ticks = values;
                return this;
            }

            public Builder WithData(IDictionary<string, IData> values)
            {
                _datas = values;
                return this;
            }

            public Slice Build()
            {
                return new Slice
                {
                    DateTime = _dateTime,
                    Ticks = _ticks,
                    Datas = _datas
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(DateTime)}: {DateTime}, " +
                   $"{nameof(Ticks)}: {Ticks}, " +
                   $"{nameof(Datas)}: {Datas} ";
        }
    }
}
