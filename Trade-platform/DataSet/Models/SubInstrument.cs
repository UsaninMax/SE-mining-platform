
using System;
using System.Runtime.Serialization;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.Models
{
    [DataContract()]
    public class SubInstrument : Instrument
    {
        [DataMember()]
        public DateTime SelectedFrom { get; set; }
        [DataMember()]
        public DateTime SelectedTo { get; set; }

        public SubInstrument(Instrument instrument) : base(instrument) { }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(SelectedFrom)}: {SelectedFrom}, {nameof(SelectedTo)}: {SelectedTo}";
        }
    }
}
