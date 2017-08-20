using System;

namespace SEMining.Sandbox.Models
{
    public class Indicator : IData
    {
        private DateTime _date;
        private string _id;
        public double Value { get; private set; }

        private Indicator() { }

        public void SetId(string value)
        {
            _id = value;
        }

        public class Builder
        {
            private DateTime _date;
            private string _id;
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

            public Builder WithId(string value)
            {
                _id = value;
                return this;
            }

            public Indicator Build()
            {
                return new Indicator()
                {
                    _date = _date,
                    _id = _id,
                    Value = _value
                };
            }
        }

        public override string ToString()
        {
            return
                $" {nameof(_date)}: {_date}," +
                $" {nameof(_id)}: {_id}," +
                $" {nameof(Value)}: {Value}";
        }

        public DateTime Date()
        {
            return _date;
        }

        public string Id()
        {
            return _id;
        }
    }
}
