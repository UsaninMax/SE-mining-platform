using System;
using System.Runtime.Serialization;
using System.Text;

namespace TradePlatform.StockData.Models
{
    [DataContract()]
    public class Instrument
    {
        private Instrument() { }

        protected Instrument(Instrument instrument)
        {
            this.Name = instrument.Name;
            this.Code = instrument.Code;
            this.Id = instrument.Id;
            this.MarketId = instrument.MarketId;
            this.DataProvider = instrument.DataProvider;
            this.Path = instrument.Path;
            this.FileName = instrument.FileName;
            this.From = instrument.From;
            this.To = instrument.To;

        }
        [DataMember()]
        public string Name { get; private set; }
        [DataMember()]
        public string Code { get; private set; }
        [DataMember()]
        public string Id { get; private set; }
        [DataMember()]
        public string MarketId { get; private set; }
        [DataMember()]
        public string DataProvider { get; private set; }
        [DataMember()]
        public string Path { get; private set; }
        [DataMember()]
        public string FileName { get; private set; }
        [DataMember()]
        public DateTime From { get; private set; }
        [DataMember()]
        public DateTime To { get; private set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}," +
                   $" {nameof(Code)}: {Code}," +
                   $" {nameof(Id)}: {Id}," +
                   $" {nameof(MarketId)}: {MarketId}," +
                   $" {nameof(DataProvider)}: {DataProvider}," +
                   $" {nameof(Path)}: {Path}," +
                   $" {nameof(FileName)}: {FileName}," +
                   $" {nameof(From)}: {From}," +
                   $" {nameof(To)}: {To}";
        }

        public class Builder
        {
            private string _name;
            private string _code;
            private string _id;
            private string _dataProvider;
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

            public Builder WithDataProvider(string value)
            {
                _dataProvider = value;
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
                        DataProvider = _parent.DataProvider,
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
                    DataProvider = _dataProvider,
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
