using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface IBalance
    {
        void AddMoney(double value);
        double GetTotal();
        void Reset();
        IEnumerable<BalanceRow> GetHistory();
        void AddTransactionCost(double value, DateTime time);
        void AddTransactionMargin(Transaction current, IEnumerable<Transaction> open, DateTime time);
    }
}
