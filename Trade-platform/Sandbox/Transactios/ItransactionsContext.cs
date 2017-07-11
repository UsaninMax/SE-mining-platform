using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface ITransactionsContext
    {
        void SetUpCosts(IEnumerable<BrokerCost> value);
        void SetUpBalance(double value);
        double GetBalance();
        int AvailableNumber(string instrumentId);
        IList<Transaction> GetTransactionHistory();
        IList<BalanceRow> GetBalanceHistory();
        void OpenPosition(ImmediatePositionRequest request);
        Guid OpenPosition(PostponedPositionRequest request);
        void ProcessTick(IEnumerable<Tick> ticks);
    }
}
