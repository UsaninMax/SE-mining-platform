namespace TradePlatform.SandboxApi.DataProviding.Models
{
    public class Indicator
    {
        public string Name { get; private set; }
        public double Value { get; private set; }

        private Indicator() { }

        public class Builder
        {
            private string _name;
            private double _value;

            public Builder WithName(string value)
            {
                _name = value;
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
                    Name = _name,
                    Value = _value
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}," +
                $" {nameof(Value)}: {Value}";
        }
    }
}
