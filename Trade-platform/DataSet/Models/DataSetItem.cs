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
        public double WarrantyCoverage { get; private set; }

        [DataMember()]
        public double StepSize { get; private set; }

        [DataMember()]
        public string Path
        {
            get { return Id; }
            set { }
        }

        public static string RootPath
        {
            get { return "DATASETS"; }
        }

        private DataSetItem()
        {
        }

        [DataMember()]
        public IEnumerable<SubInstrument> SubInstruments { get; private set; }

        public class Builder
        {
            private string _id;
            private double _warrantyCoverage;
            private double _stepSize;
            private IEnumerable<SubInstrument> _subInstruments;


            public Builder WithId(string value)
            {
                _id = value;
                return this;
            }

            public Builder WithWarrantyCoverage(double value)
            {
                _warrantyCoverage = value;
                return this;
            }

            public Builder WithStepSize(double value)
            {
                _stepSize = value;
                return this;
            }

            public Builder WithSubInstruments(IEnumerable<SubInstrument> value)
            {
                _subInstruments = value;
                return this;
            }

            public DataSetItem Build()
            {
                return new DataSetItem() {
                    Id = _id,
                    SubInstruments = _subInstruments,
                    WarrantyCoverage = _warrantyCoverage,
                    StepSize = _stepSize
                };
            }

        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }

        public object Clone()
        {
            return new DataSetItem()
            {
                StepSize = StepSize,
                WarrantyCoverage = WarrantyCoverage,
                SubInstruments = SubInstruments.Select(s => s.Clone() as SubInstrument).ToList()
            };
        }
    }
}
