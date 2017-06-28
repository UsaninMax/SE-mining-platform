namespace TradePlatform.SandboxApi.Models
{
    public class Indicator
    {
        public string Id { get; private set; }
        public double Value { get; private set; }

        private Indicator() { }

        public class Builder
        {
            private string _id;
            private double _value;

            public Builder WithName(string value)
            {
                _id = value;
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
                    Id = _id,
                    Value = _value
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}," +
                $" {nameof(Value)}: {Value}";
        }
    }
}
