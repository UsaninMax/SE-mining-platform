using System;
using System.Text;

namespace TradePlatform.Commons.Trades
{
    public class Instrument
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Id { get; private set; }
        public string MarketId { get; private set; }
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public class Builder
        {
            private string _name;
            private string _code;
            private string _id;
            private string _marketId;
            private DateTime _from;
            private DateTime _to;
            private Instrument _parent;

            public Builder WithName(string value)
            {
                _name = value;
                return this;
            }

            public Builder WithCode(string value)
            {
                _code = value;
                return this;
            }

            public Builder WithId(string value)
            {
                _id = value;
                return this;
            }

            public Builder WithMarketId(string value)
            {
                _marketId = value;
                return this;
            }

            public Builder WithFrom(DateTime value)
            {
                _from = value;
                return this;
            }

            public Builder WithTo(DateTime value)
            {
                _to = value;
                return this;
            }

            public Builder WithParent(Instrument value)
            {
                _parent = value;
                return this;
            }

            private static string DATE_FRORMAT = "ddMMyy";

            public Instrument Build()
            {
                if (_parent != null)
                {
                    return new Instrument()
                    {
                        Name = _parent.Name,
                        Code = _parent.Code,
                        Id = _parent.Id,
                        MarketId = _parent.MarketId,
                        From = _from,
                        To = _to,
                        Path = _parent.Path,
                        FileName = new StringBuilder()
                        .Append(_parent.Name)
                        .Append("_").Append(_from.ToString(DATE_FRORMAT))
                        .Append("_").Append(_to.ToString(DATE_FRORMAT))
                        .ToString()
                    };
                }

                return new Instrument()
                {
                    Name = _name,
                    Code = _code,
                    Id = _id,
                    MarketId = _marketId,
                    From = _from,
                    To = _to,
                    Path = new StringBuilder()
                    .Append(_name)
                    .Append("_").Append(_from.ToString(DATE_FRORMAT))
                    .Append("_").Append(_to.ToString(DATE_FRORMAT))
                    .ToString(),
                    FileName = new StringBuilder()
                    .Append(_name)
                    .Append("_").Append(_from.ToString(DATE_FRORMAT))
                    .Append("_").Append(_to.ToString(DATE_FRORMAT))
                    .ToString()
                };
            }
        }
    }
}
