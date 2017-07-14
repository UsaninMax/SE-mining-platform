using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.Sandbox.Transactios.Enums;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public class OpenPositionRequest
    {
        public DateTime Date { get; private set; }
        public string InstrumentId { get; private set; }
        public Direction Direction { get; private set; }
        public int Number { get; private set; }
        public int RemainingNumber { get; set; }
        public Guid Id { get; private set; }
        private IList<Transaction> _transactions = new List<Transaction>();

        private OpenPositionRequest(){}

        public class Builder
        {
            private string _instrumentId;
            private Direction _direction;
            private int _number;

            public Builder InstrumentId(string value)
            {
                _instrumentId = value;
                return this;
            }

            public Builder Direction(Direction value)
            {
                _direction = value;
                return this;
            }

            public Builder Number(int value)
            {
                _number = value;
                return this;
            }

            public OpenPositionRequest Build()
            {
                return new OpenPositionRequest()
                {
                    Date = DateTime.Now,
                    InstrumentId = _instrumentId,
                    Id = Guid.NewGuid(),
                    Direction = _direction,
                    RemainingNumber = _number,
                    Number = _number
                };
            }
        }

        public IList<Transaction> GetTransactions()
        {
            return _transactions.ToList();
        }

        public void AddTransaction(Transaction transaction)
        {
           _transactions.Add(transaction);
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date}," +
                   $" {nameof(Id)}: {Id}," +
                   $" {nameof(InstrumentId)}: {InstrumentId}," +
                   $" {nameof(Direction)}: {Direction}," +
                   $" {nameof(Number)}: {Number}," +
                   $" {nameof(RemainingNumber)}: {RemainingNumber}";
        }
    }
}
