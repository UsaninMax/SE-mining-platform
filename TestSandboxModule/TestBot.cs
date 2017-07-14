using System.Collections.Generic;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TestSandboxModule
{
    public class TestBot : BotApi
    {
        public override void Execution(IDictionary<string, IData> value)
        {
            System.Diagnostics.Debug.WriteLine("Bot name = " + GetId() + " -receive slice " + value);
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
