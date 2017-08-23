using System;
using System.Collections.Generic;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface IBalanceHolder
    {
        void AddMoney(double value);
        double GetTotal();
        void Reset();
        IEnumerable<BalanceRow> GetHistory();
        void AddTransactionCost(double value, DateTime time);
        void AddTransactionMargin(Transaction current, IEnumerable<Transaction> open, DateTime time);
    }
}
