using System;

namespace TradePlatform.Sandbox.Bots
{
    public class BotPredicate
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        private BotPredicate () { }

        public class Builder
        {
            private DateTime _from;
            private DateTime _to;

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
                    From = _from,
                    To = _to
                };
            }
        }

        public override string ToString()
        {
            return 
                   $" {nameof(From)}: {From}, " +
                   $"{nameof(To)}: {To}";
        }
    }
}
