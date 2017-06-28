using System;

namespace TradePlatform.SandboxApi.Models
{
    public class Tick
    {
        public DateTime Date { get; private set; }
        public string Id { get; private set; }
        public double Price { get; private set; }
        public double Volume { get; private set; }

        private Tick()
        {
        }

        public class Builder
        {
            private DateTime _date;
            private string _id;
            private double _price;
            private double _volume;

            public Builder WithDate(DateTime value)
            {
                _date = value;
                return this;
            }

            public Builder WithId(string value)
            {
                _id = value;
                return this;
            }

            public Builder WithPrice(double value)
            {
                _price = value;
                return this;
            }

            public Builder WithVolume(double value)
            {
                _volume = value;
                return this;
            }

            public Tick Build()
            {
                return new Tick
                {
                    Date = _date,
                    Id = _id,
                    Price = _price,
                    Volume = _volume
                };
            }
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}," +
                   $" {nameof(Id)}: {Id}," +
                   $" {nameof(Price)}: {Price}," +
                   $" {nameof(Volume)}: {Volume}";
        }
    }
}
