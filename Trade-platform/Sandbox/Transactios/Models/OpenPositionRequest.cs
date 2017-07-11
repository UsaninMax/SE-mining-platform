using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Transactios.Enums;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public abstract class OpenPositionRequest
    {
        public DateTime Date { get; protected set; }
        public string InstrumentId { get; protected set; }
        public Direction Direction { get; protected set; }
        public int Number { get; protected set; }
        public int RemainingNumber { get; set; }
        public RequestStatus RequestStatus { get; set; }
        private IList<Transaction> _transactions = new List<Transaction>();

        protected OpenPositionRequest(){}

        public abstract class AbstractBuilder
        {
            protected string _instrumentId;
            protected Direction _direction;
            protected int _number;

            public AbstractBuilder InstrumentId(string value)
            {
                _instrumentId = value;
                return this;
            }

            public AbstractBuilder Direction(Direction value)
            {
                _direction = value;
                return this;
            }

            public AbstractBuilder Number(int value)
            {
                _number = value;
                return this;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}," +
                   $" {nameof(InstrumentId)}: {InstrumentId}," +
                   $" {nameof(Direction)}: {Direction}," +
                   $" {nameof(Number)}: {Number}," +
                   $" {nameof(RemainingNumber)}: {RemainingNumber}," +
                   $" {nameof(RequestStatus)}: {RequestStatus}";
        }
    }
}
