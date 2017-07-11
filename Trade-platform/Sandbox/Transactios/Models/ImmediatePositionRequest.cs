using System;
using TradePlatform.Sandbox.Transactios.Enums;

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
                    Direction = _direction,
                    Number = _number,
                    RequestStatus = RequestStatus.Open
                };
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
