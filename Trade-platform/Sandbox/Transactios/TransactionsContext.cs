using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionsContext : ITransactionsContext
    {
        public void SetUpCosts(IEnumerable<BrokerCost> value)
        {
            throw new NotImplementedException();
        }

        public void SetUpBalance(double value)
        {
            throw new NotImplementedException();
        }

        public double GetBalance()
        {
            throw new NotImplementedException();
        }

        public int AvailableNumber(string instrumentId)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetTransactionHistory()
        {
            throw new NotImplementedException();
        }

        public IList<BalanceRow> GetBalanceHistory()
        {
            throw new NotImplementedException();
        }

        public void OpenPosition(ImmediatePositionRequest request)
        {
            throw new NotImplementedException();
        }

        public Guid OpenPosition(PostponedPositionRequest request)
        {
            throw new NotImplementedException();
        }

        public void ProcessTick(IEnumerable<Tick> ticks)
        {
            throw new NotImplementedException();
        }
    }
}
