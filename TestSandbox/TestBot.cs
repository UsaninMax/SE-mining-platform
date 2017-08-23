using System.Collections.Generic;
using System.Linq;
using SEMining.Sandbox.Bots;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace TestSandbox
{
    public class TestBot : BotAbstraction
    {
        public override void Execution(IDictionary<string, IData> value)
        {
            if (!value.ContainsKey("RTS_5"))
            {
                return;
            }
            
            double maSrort = ((Indicator)value["MA_SHORT"]).Value;
            double maLong = ((Indicator)value["MA_LONG"]).Value;

            if(maSrort < maLong)
            {
                if (!GetOpenTransactions("RTS", Direction.Sell).Any() &&
                    !GetActiveRequests("RTS", Direction.Sell).Any())
                {
                    OpenPosition(new OpenPositionRequest.Builder().Direction(Direction.Sell).InstrumentId("RTS").Number(10).Build());
                } 
            }
            if (maSrort > maLong)
            {
                if (!GetOpenTransactions("RTS", Direction.Buy).Any() &&
                    !GetActiveRequests("RTS", Direction.Buy).Any())
                {
                    OpenPosition(new OpenPositionRequest.Builder().Direction(Direction.Buy).InstrumentId("RTS").Number(10).Build());
                }
            }
        }

        public override int Score()
        {
            return 100;
        }

        public TestBot(IDictionary<string, BrokerCost> brokerCosts) : base(brokerCosts)
        {
        }
    }
}
