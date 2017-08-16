using System.Collections.Generic;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TestSandboxModule
{
    public class TestBot : BotApi
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
                if (GetOpenTransactions("RTS", Direction.Sell).Count == 0)
                {
                    OpenPosition(new OpenPositionRequest.Builder().Direction(Direction.Sell).InstrumentId("RTS").Number(10).Build());
                } 
            }
            if (maSrort > maLong)
            {
                if (GetOpenTransactions("RTS", Direction.Buy).Count == 0)
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
