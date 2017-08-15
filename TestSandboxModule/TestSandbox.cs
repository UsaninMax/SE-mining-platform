using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TestSandboxModule
{
    public class TestSandbox : SandboxApi
    {
        private DateTime _from = new DateTime(2016, 2, 1);
        private DateTime _to = new DateTime(2016, 2, 5);
        private TimeSpan _period = new TimeSpan(0, 0, 5);
        private IDictionary<string, BrokerCost> _costs = new Dictionary<string, BrokerCost> { { "RTS", new BrokerCost { Coverage = 0.11, TransactionCost = 0.5 } } };

        public override IEnumerable<PanelViewPredicate> SetUpCharts()
        {
            return new List<PanelViewPredicate> {
                new PanelViewPredicate
                {
                    ChartPredicates = new List<ChartViewPredicate>
                    {
                        new DateChartViewPredicate
                        {
                           Ids = new List<string> { "RTS_5", "MA_SHORT", "MA_LONG", "TRANSACTIONS"},
                           XAxis = _period,
                           YSize = 400
                        }
                    }
                }
            };
        }

        public override ICollection<IPredicate> SetUpData()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_5")
                    .AccumulationPeriod(_period)
                    .From(_from)
                    .To(_to)
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA_SHORT")
                    .Indicator(typeof(MA))
                    .Parameter("length", 8)
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("RTS_5")
                        .ParentId("RTS")
                        .AccumulationPeriod(_period)
                        .From(_from)
                        .To(_to)
                        .Build())
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA_LONG")
                    .Indicator(typeof(MA))
                    .Parameter("length", 16)
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("RTS_5")
                        .ParentId("RTS")
                        .AccumulationPeriod(_period)
                        .From(_from)
                        .To(_to)
                        .Build())
                    .Build(),
            };
        }

        public override void Execution()
        {
            IBot first = CreateTestBot();
            IBot second = CreateTestBot();
            SetUpBots(new List<IBot> { first, second });
            Execute();
        }

        public override void AfterExecution()
        {

            StoreCustomData("TRANSACTIONS", new List<object>(Bots.First().GetTansactionsHistory()));
            PopulateCharts(new List<ChartPredicate>
            {
                new EDPredicate
            {
                CasType = typeof(Candle),
                ChartId = "RTS_5",
                InstrumentId = "RTS_5",
                From = _from,
                To = _to
            },
                new EDPredicate
            {
                CasType = typeof(Indicator),
                ChartId = "MA_SHORT",
                InstrumentId = "MA",
                Color = Brushes.DarkBlue,
                From = _from,
                To = _to
            },
                new EDPredicate
            {
                CasType = typeof(Indicator),
                ChartId = "MA_LONG",
                InstrumentId = "MA",
                Color = Brushes.DarkBlue,
                From = _from,
                To = _to
            },
                 new CDPredicate
            {
                CasType = typeof(Transaction),
                ChartId = "TRANSACTIONS",
                InstrumentId = "TRANSACTIONS",
                Color = Brushes.DarkBlue,
                From = _from,
                To = _to
            }
        });
        }

        private TestBot CreateTestBot()
        {
            TestBot bot = new TestBot(_costs);
            bot.SetUpId("Test_1");
            bot.SetUpBalance(10000);
            bot.SetUpPredicate(new BotPredicate.Builder()
                .From(_from)
                .To(_to)
                .Build());
            return bot;
        }
    }
}
