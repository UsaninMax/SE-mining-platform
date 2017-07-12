using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface ITransactionsContext
    {
        bool IsPrepared();
        void SetUpCosts(IDictionary<string, BrokerCost> value);
        void SetUpBalance(double value);
        void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value);
        double GetBalance();
        void Reset();
        int AvailableNumber(string instrumentId);
        IList<Transaction> GetTransactionHistory();
        IList<BalanceRow> GetBalanceHistory();
        bool OpenPosition(ImmediatePositionRequest request);
        bool OpenPosition(PostponedPositionRequest request);
        bool ClosePosition(Guid guid);
        void ProcessTick(IDictionary<string, Tick> ticks);
    }
}
