using System;
using System.Collections.Generic;

namespace TradePlatform.Sandbox.Bots
{
    public class BotPredicate
    {
        public ICollection<string> InstrumentIds { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        private BotPredicate () { }

        public class Builder
        {
            private ICollection<string> _instrumentIds;
            private DateTime _from;
            private DateTime _to;

            public Builder InstrumentIds(ICollection<string> values)
            {
                _instrumentIds = values;
                return this;
            }

            public Builder From(DateTime value)
            {
                _from = value;
                return this;
            }

            public Builder To(DateTime value)
            {
                _to = value;
                return this;
            }

            public BotPredicate Build()
            {
                return new BotPredicate()
                {
                    InstrumentIds = _instrumentIds,
                    From = _from,
                    To = _to
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(InstrumentIds)}: {InstrumentIds}," +
                   $" {nameof(From)}: {From}, " +
                   $"{nameof(To)}: {To}";
        }
    }
}
