using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public class BalanceHolder : IBalanceHolder
    {
        private readonly IList<BalanceRow> _history = new List<BalanceRow>();
        private BalanceRow _currentBalance;
        private double _initMoney;


        public void AddMoney(double value)
        {
            _initMoney = value;
            _currentBalance = new BalanceRow.Builder().Total(_initMoney).Build();
            _history.Add(_currentBalance);
        }

        public double GetTotal()
        {
            return _currentBalance.Total;
        }

        public void Reset()
        {
            _currentBalance = new BalanceRow.Builder().Total(_initMoney).Build();
            _history.Clear();
            _history.Add(_currentBalance);
        }

        public IEnumerable<BalanceRow> GetHistory()
        {
            return _history;
        }

        public void AddTransactionCost(double value, DateTime time, Guid requestId)
        {
            _currentBalance = new BalanceRow.Builder()
                .WithDate(time)
                .TransactionCost(-value)
                .Total(_currentBalance.Total - value)
                .WithRequestId(requestId)
                .Build();
            _history.Add(_currentBalance);
        }

        public void AddTransactionMargin(Transaction current, IEnumerable<Transaction> open, DateTime time)
        {
            if (open.IsNullOrEmpty())
            {
                return;
            }

            var forCalculation = Math.Min(open.Sum(x=> x.RemainingNumber), current.Number);
            var closeSum = current.ExecutedPrice * forCalculation;
            double openSum = 0;
            open
                .GroupBy(x=> x.ExecutedPrice)
                .ForEach(x =>
            {
                int withdraw = Math.Min(x.Sum(y => y.RemainingNumber), forCalculation);
                forCalculation -= withdraw;
                openSum += withdraw * x.Key;
            });
            var profit = current.Direction == Direction.Sell ? closeSum - openSum : openSum - closeSum;
            _currentBalance = new BalanceRow.Builder()
               .WithDate(time)
               .TransactionMargin(profit)
               .Total(_currentBalance.Total + profit)
               .WithRequestId(current.RequestId)
               .Build();

            _history.Add(_currentBalance);
        }
    }
}
