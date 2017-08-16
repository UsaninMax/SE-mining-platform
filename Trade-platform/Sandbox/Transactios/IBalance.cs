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
        IList<BalanceRow> GetHistory();
        void AddTransactionCost(double value, DateTime time);
        void AddTransactionMargin(Transaction current, IList<Transaction> open, DateTime time);
    }
}
