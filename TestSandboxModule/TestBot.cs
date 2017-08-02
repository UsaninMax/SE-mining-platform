using System;
using System.Collections.Generic;
using System.Threading;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;
using TradePlatform.Vizualization.Populating.Predicates;

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

            DateTime date = value["RTS_5"].Date();
            PopulateCharts(new List<ChartPredicate>
            {
                new CandlesDataPredicate
            {
                DateTo = date,
                ChartId = "RTS_5",
                InstrumentId = "RTS_5"
            },
                new IndicatorDataPredicate
            {
                DateTo = date,
                ChartId = "RTS_5",
                InstrumentId = "MA"
            }
            });

            Thread.Sleep(2000);
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
