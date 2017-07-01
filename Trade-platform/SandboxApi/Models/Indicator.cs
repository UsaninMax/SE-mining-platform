using System;

namespace TradePlatform.SandboxApi.Models
{
    public class Indicator : IData
    {
        private DateTime _date;
        public string Id { get; set; }
        public double Value { get; private set; }

        private Indicator() { }

        public class Builder
        {
            private DateTime _date;
            private double _value;

            public Builder WithDate(DateTime value)
            {
                _date = value;
                return this;
            }

            public Builder WithValue(double value)
            {
                _value = value;
                return this;
            }

            public Indicator Build()
            {
                return new Indicator()
                {
                    _date = _date,
                    Value = _value
                };
            }
        }

        public override string ToString()
        {
            return
                $" {nameof(_date)}: {_date}," +
                $" {nameof(Id)}: {Id}," +
                $" {nameof(Value)}: {Value}";
        }

        public DateTime Date()
        {
            return _date;
        }
    }
}
