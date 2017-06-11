using System;
using System.Runtime.Serialization;

namespace TradePlatform.Commons.BaseModels
{
    [DataContract()]
    public class DataTick
    {
        [DataMember()]
        public DateTime Date { get; set; }
        [DataMember()]
        public double  Price { get; set; }
        [DataMember()]
        public int Volume { get; set; }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(Price)}: {Price}, {nameof(Volume)}: {Volume}";
        }
    }
}
