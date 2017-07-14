using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionHolder : ITransactionHolder
    {
        private List<Transaction> _openTransactions = new List<Transaction>();
        private IDictionary<string, BrokerCost> _brokerCosts;

        public TransactionHolder(IDictionary<string, BrokerCost> brokerCosts)
        {
            _brokerCosts = brokerCosts;
        }

        public void UpdateOpenTransactions(Transaction transaction)
        {
            IList<Transaction> openTransactions = GetOpenTransactions(transaction.InstrumentId, transaction.Direction);

            openTransactions.ForEach(x =>
            {
                int withdraw = Math.Min(x.RemainingNumber, transaction.RemainingNumber);
                x.RemainingNumber = x.RemainingNumber - withdraw;
                transaction.RemainingNumber = transaction.RemainingNumber - withdraw;
            });

            _openTransactions.Add(transaction);
            _openTransactions.RemoveAll(x => x.RemainingNumber == 0);
        }

        public double GetCoverage(IDictionary<string, Tick> ticks)
        {
            return _openTransactions.GroupBy(x => x.InstrumentId)
                .Sum(x => x.Sum(y => y.RemainingNumber * ticks[x.Key].Price * _brokerCosts[x.Key].Coverage));
        }

        public IList<Transaction> GetOpenTransactions()
        {
           return _openTransactions;
        }

        public IList<Transaction> GetOpenTransactions(string instrumentId, Direction direction)
        {
            return _openTransactions
                .Where(x => x.InstrumentId.Equals(instrumentId) && x.Direction.Equals(Invert(direction)))
                .ToList();
        }

        public IList<Transaction> GetOpenTransactions(string instrumentId)
        {
            return _openTransactions
                .Where(x => x.InstrumentId.Equals(instrumentId))
                .ToList();
        }

        public int GetSize()
        {
            return _openTransactions.Count;
        }

        public void Reset()
        {
            _openTransactions.Clear();
        }

        private Direction Invert(Direction direction)
        {
            return direction == Direction.Buy ? Direction.Sell : Direction.Buy;
        }
    }
}
