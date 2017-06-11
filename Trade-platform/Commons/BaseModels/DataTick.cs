using System;
using System.Runtime.Serialization;

namespace TradePlatform.Commons.BaseModels
{
    [DataContract(Name = "DT")]
    public class DataTick
    {
        [DataMember(Name = "D")]
        public DateTime Date { get; set; }
        [DataMember(Name = "P")]
        public double  Price { get; set; }
        [DataMember(Name = "V")]
        public int Volume { get; set; }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(Price)}: {Price}, {nameof(Volume)}: {Volume}";
        }
    }
}
