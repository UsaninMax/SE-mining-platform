using System;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public class ImmediatePositionRequest : OpenPositionRequest
    {
        private ImmediatePositionRequest()
        {
        }

        public class Builder : OpenPositionRequest.AbstractBuilder
        {
            public ImmediatePositionRequest Build()
            {
                return new ImmediatePositionRequest()
                {
                    Date = DateTime.Now,
                    InstrumentId = _instrumentId,
                    Id = Guid.NewGuid(),
                    Direction = _direction,
                    Number = _number
                };
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
