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
        private readonly List<Transaction> _openTransactions = new List<Transaction>();
        private readonly IDictionary<string, BrokerCost> _brokerCosts;

        public TransactionHolder(IDictionary<string, BrokerCost> brokerCosts)
        {
            _brokerCosts = brokerCosts;
        }

        public void UpdateOpenTransactions(Transaction transaction)
        {
            IList<Transaction> openTransactions = GetInvertedOpenTransactions(transaction.InstrumentId, transaction.Direction);

            openTransactions.ForEach(x =>
            {
                int withdraw = Math.Min(x.RemainingNumber, transaction.RemainingNumber);
                x.RemainingNumber = x.RemainingNumber - withdraw;
                transaction.RemainingNumber = transaction.RemainingNumber - withdraw;
            });

            _openTransactions.Add(transaction);
            _openTransactions.RemoveAll(x => x.RemainingNumber == 0);
        }

        public double GetCoverage(
            IDictionary<string, Tick> ticks,
            IEnumerable<OpenPositionRequest> activeRequests)
        {
            IEnumerable<Tuple<string, Direction, double>> activeTransactions =
                activeRequests
                .Select(x => new Tuple<string, Direction, double>(x.InstrumentId, x.Direction, x.RemainingNumber))
                .ToList();

            activeTransactions = activeTransactions.Concat(_openTransactions
                .Select(x => new Tuple<string, Direction, double>(x.InstrumentId, x.Direction, x.RemainingNumber))
                .ToList());

            return activeTransactions
                .GroupBy(x => x.Item1)
                .Sum(x =>
                {
                    double buy = x.Where(y => y.Item2.Equals(Direction.Buy)).Sum(y => y.Item3);
                    double sell = x.Where(y => y.Item2.Equals(Direction.Sell)).Sum(y => y.Item3);
                    return Math.Abs(buy - sell) * ticks[x.Key].Price * _brokerCosts[x.Key].Coverage;
                });
        }

        public double GetCoverage(
            IDictionary<string, Tick> ticks,
            IEnumerable<OpenPositionRequest> activeRequests,
            OpenPositionRequest newRequest)
        {
            var temp = activeRequests.ToList();
            temp.Add(newRequest);
            return GetCoverage(ticks, temp);
        }

        public IList<Transaction> GetOpenTransactions()
        {
            return _openTransactions;
        }

        public IList<Transaction> GetInvertedOpenTransactions(string instrumentId, Direction direction)
        {
            return _openTransactions
                .Where(x => x.InstrumentId.Equals(instrumentId) && x.Direction.Equals(Invert(direction)))
                .ToList();
        }

        public IList<Transaction> GetOpenTransactions(string instrumentId, Direction direction)
        {
            return _openTransactions
                .Where(x => x.InstrumentId.Equals(instrumentId) && x.Direction.Equals(direction))
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
