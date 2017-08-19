using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class Balance : IBalance
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

        public void AddTransactionCost(double value, DateTime time)
        {
            _currentBalance = new BalanceRow.Builder()
                .WithDate(time)
                .TransactionCost(-value)
                .Total(_currentBalance.Total - value)
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

            if(profit == 0)
            {
                return;
            }

            _currentBalance = new BalanceRow.Builder()
               .WithDate(time)
               .TransactionMargin(profit)
               .Total(_currentBalance.Total + profit)
               .Build();

            _history.Add(_currentBalance);
        }
    }
}
