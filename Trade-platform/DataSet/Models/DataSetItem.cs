﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TradePlatform.DataSet.Models
{
    [DataContract()]
    public class DataSetItem
    {
        [DataMember()]
        public string Id { get; private set; }

        [DataMember()]
        public string Patch {
            get { return Id; }
        }

        [DataMember()]
        public IList<SubInstrument> SubInstruments { get; private set; }

        public class Builder
        {
            private string _id;
            private IList<SubInstrument> _subInstruments;


            public Builder WithId(string value)
            {
                _id = value;
                return this;
            }

            public Builder WithSubInstruments(IList<SubInstrument> value)
            {
                _subInstruments = value;
                return this;
            }

            public DataSetItem Build()
            {
                return new DataSetItem() {Id = _id, SubInstruments = _subInstruments};
            }

        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(SubInstruments)}: {SubInstruments}";
        }

        protected bool Equals(DataSetItem other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataSetItem) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
