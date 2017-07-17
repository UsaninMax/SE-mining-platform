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
        void AddTransactionCost(double value);
        void AddTransactionMargin(Transaction current, IList<Transaction> open);
    }
}
