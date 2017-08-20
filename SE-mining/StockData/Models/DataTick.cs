using System;
using System.Runtime.Serialization;

namespace SEMining.StockData.Models
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

        public override bool Equals(object obj)
        {
            var item = obj as DataTick;

            if (item == null)
            {
                return false;
            }

            return Date.Equals(item.Date) &&
                Price.Equals(item.Price) &&
                Volume.Equals(item.Volume);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode() ^ Price.GetHashCode() ^ Volume.GetHashCode();
        }
    }
}
