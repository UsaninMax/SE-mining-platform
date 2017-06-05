using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TradePlatform.DataSet.Models
{
    [DataContract()]
    public class DataSetItem : ICloneable
    {
        [DataMember()]
        public string Id { get; private set; }

        [DataMember()]
        public string Patch
        {
            get { return Id; }
            set { }
        }

        private DataSetItem()
        {
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
                return new DataSetItem() { Id = _id, SubInstruments = _subInstruments };
            }

        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(SubInstruments)}: {SubInstruments}";
        }

        public object Clone()
        {
            return new DataSetItem()
            {
                SubInstruments = SubInstruments.Select(s => s.Clone() as SubInstrument).ToList()
            };
        }
    }
}
