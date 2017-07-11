using System;
using TradePlatform.Sandbox.Transactios.Enums;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public class PostponedPositionRequest : OpenPositionRequest
    {
        public double ExpectedPrice { get; private set; }
        public Guid Id { get; private set; }

        private PostponedPositionRequest()
        {
        }

        public class Builder : OpenPositionRequest.AbstractBuilder
        {
            private double _expectedPrice;

            public Builder ExpectedPrice(double value)
            {
                _expectedPrice = value;
                return this;
            }

            public PostponedPositionRequest Build()
            {
                return new PostponedPositionRequest()
                {
                    Date = DateTime.Now,
                    ExpectedPrice = _expectedPrice,
                    Id = Guid.NewGuid(),
                    InstrumentId = _instrumentId,
                    Direction = _direction,
                    Number = _number,
                    RequestStatus = RequestStatus.Open
                };
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                   $"{nameof(Id)}: {Id}, " +
                   $"{nameof(ExpectedPrice)}: {ExpectedPrice}";
        }
    }
}
