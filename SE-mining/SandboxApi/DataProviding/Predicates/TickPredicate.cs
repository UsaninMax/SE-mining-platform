using System;

namespace TradePlatform.SandboxApi.DataProviding.Predicates
{
    public class TickPredicate : IPredicate
    {
        public string Id { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        private TickPredicate() { }

        public class Builder
        {
            private string _id;
            private DateTime _from;
            private DateTime _to;

            public Builder NewId(string value)
            {
                _id = value;
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

            public TickPredicate Build()
            {
                return new TickPredicate()
                {
                    Id = _id,
                    From = _from,
                    To = _to
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(From)}: {From}, " +
                   $"{nameof(To)}: {To}";
        }
    }
}
