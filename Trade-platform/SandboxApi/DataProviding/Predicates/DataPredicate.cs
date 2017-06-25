using System;

namespace TradePlatform.SandboxApi.DataProviding.Predicates
{
    public class DataPredicate
    {
        public string Id { get; private set; }
        public string ParentId { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        private DataPredicate() { }

        public class Builder
        {
            private string _id;
            private string _parentId;
            private DateTime _from;
            private DateTime _to;

            public Builder NewId(string value)
            {
                _id = value;
                return this;
            }

            public Builder ParentId(string value)
            {
                _parentId = value;
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

            public DataPredicate Build()
            {
                return new DataPredicate()
                {
                    Id = _id,
                    ParentId = _parentId,
                    From = _from,
                    To = _to
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(ParentId)}: {ParentId}, " +
                   $"{nameof(From)}: {From}, " +
                   $"{nameof(To)}: {To}";
        }
    }
}
