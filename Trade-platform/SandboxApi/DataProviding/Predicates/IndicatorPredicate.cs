using System;
using System.Collections.Generic;

namespace TradePlatform.SandboxApi.DataProviding.Predicates
{
    public class IndicatorPredicate
    {
        public string Id { get; private set; }
        public Type Indicator { get; private set; }
        public DataPredicate DataPredicate { get; private set; }
        public Dictionary<string, object> Parameters { get; private set; }

        private IndicatorPredicate()
        {
        }

        public class Builder
        {
            private string _id;
            private Type _indicator;
            private DataPredicate _dataPredicate;
            private Dictionary<string, object> _parameters = new Dictionary<string, object>();

            public Builder NewId(string value)
            {
                _id = value;
                return this;
            }

            public Builder Indicator(Type value)
            {
                _indicator = value;
                return this;
            }

            public Builder DataPredicate(DataPredicate value)
            {
                _dataPredicate = value;
                return this;
            }

            public Builder Parameter(string key, object value)
            {
                _parameters.Add(key, value);
                return this;
            }

            public IndicatorPredicate Build()
            {
                return new IndicatorPredicate()
                {
                    Id = _id,
                    Indicator = _indicator,
                    DataPredicate = _dataPredicate,
                    Parameters = _parameters
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Indicator)}: {Indicator}," +
                   $" {nameof(DataPredicate)}: {DataPredicate}," +
                   $" {nameof(Parameters)}: {Parameters}";
        }
    }
}
