using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TradePlatform.DataSet.Models
{
    [DataContract()]
    public class DataSet
    {
        [DataMember()]
        public string Id { get; set; }
        [DataMember()]
        public IList<SubInstrument> SubInstruments { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(SubInstruments)}: {SubInstruments}";
        }

        protected bool Equals(DataSet other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataSet) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
