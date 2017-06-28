namespace TradePlatform.SandboxApi.Models
{
    public class Tick
    {
        public string Id { get; private set; }
        public double Value { get; private set; }

        private Tick()
        {
        }

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

            public Tick Build()
            {
                return new Tick()
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
