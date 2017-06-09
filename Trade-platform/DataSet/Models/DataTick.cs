using System;
using System.Runtime.Serialization;

namespace TradePlatform.DataSet.Models
{
    [DataContract()]
    public class DataTick
    {
        [DataMember()]
        public DateTime Date { get; set; }
        [DataMember()]
        public double  Value { get; set; }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}, {nameof(Value)}: {Value}";
        }
    }
}
