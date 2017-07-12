using System;
using TradePlatform.Sandbox.Transactios.Enums;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public class Transaction
    {
        public DateTime Date { get; private set; }
        public string InstrumentId { get; private set; }
        public double ExecutedPrice { get; private set; }
        public int Number { get; set; }
        public Direction Direction { get; private set; }

        protected Transaction() { }

        public class Builder
        {
            private string _instrumentId;
            private double _executedPrice;
            private int _number;
            private Direction _direction;


            public Builder InstrumentId(string value)
            {
                _instrumentId = value;
                return this;
            }


            public Builder ExecutedPrice(double value)
            {
                _executedPrice = value;
                return this;
            }

            public Builder Number(int value)
            {
                _number = value;
                return this;
            }

            public Builder Direction(Direction value)
            {
                _direction = value;
                return this;
            }

            public Transaction Build()
            {
                return new Transaction()
                {
                    Date = DateTime.Now,
                    InstrumentId = _instrumentId,
                    ExecutedPrice = _executedPrice,
                    Number = _number,
                    Direction = _direction
                };
            }
        }
    }
}
