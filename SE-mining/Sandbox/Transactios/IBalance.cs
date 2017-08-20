using System;
using System.Collections.Generic;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Transactios
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
