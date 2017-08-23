using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SEMining.Charts.Data.Predicates;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Sandbox;
using SEMining.Sandbox.Bots;
using SEMining.Sandbox.DataProviding.Predicates;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Results.Adaptors;
using SEMining.Sandbox.Results.Storing;
using SEMining.Sandbox.Transactios.Models;

namespace TestSandbox
{
    public class TestSandbox : SandboxApi
    {
        private DateTime _from = new DateTime(2016, 3, 15);
        private DateTime _to = new DateTime(2016, 3, 25);
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
                           YSize = 700
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "EQUITY"},
                           YSize = 700
                        }
                    }
                }
            };
        }

        public override IEnumerable<IPredicate> SetUpData()
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
        private IBot _firstBot;
        private IBot _secondBot;
        private IBot _thirdBot;
        public override void Execution()
        {
            _firstBot = CreateTestBot("Test_1");
            _secondBot = CreateTestBot("Test_2");
            _thirdBot = CreateTestBot("Test_3");
            SetUpBots(new List<IBot>
            {
                _firstBot, _secondBot, _thirdBot,CreateTestBot("Test_4"),CreateTestBot("Test_5"),CreateTestBot("Test_6"),CreateTestBot("Test_7"),CreateTestBot("Test_8"),CreateTestBot("Test_9"),CreateTestBot("Test_10"),CreateTestBot("Test_11")
                ,CreateTestBot("Test_12"),CreateTestBot("Test_13"),CreateTestBot("Test_14"),CreateTestBot("Test_15"),CreateTestBot("Test_16"),CreateTestBot("Test_17"),CreateTestBot("Test_18"),CreateTestBot("Test_19"),CreateTestBot("Test_20")
            });
            Execute();
        }

        public override void AfterExecution()
        {
            IReportAdaptor reportAdaptor = new DefaultReportAdaptor();
            ReusltStoring.Store(reportAdaptor.Adopt(_firstBot.GetRequestsHistory()), " | ");
            ReusltStoring.Store(reportAdaptor.Adopt(_secondBot.GetRequestsHistory()), " | ");
            ReusltStoring.Store(reportAdaptor.Adopt(_thirdBot.GetRequestsHistory()), " | ");
            StoreCustomData("TRANSACTIONS", new List<object>(Bots.First().GetTansactionsHistory()));
            StoreCustomData("EQUITY", GetEquity());
            var from = new DateTime(2016, 3, 15, 13, 55, 00);
            var to = new DateTime(2016, 3, 25, 13, 56, 00);
            PopulateCharts(new List<ChartPredicate>
            {
                new EDPredicate
            {
                CasType = typeof(Candle),
                ChartId = "RTS_5",
                InstrumentId = "RTS_5",
                From = from,
                To = to
            },
                new EDPredicate
            {
                CasType = typeof(Indicator),
                ChartId = "MA_SHORT",
                InstrumentId = "MA_SHORT",
                Color = Brushes.DarkBlue,
                From = from,
                To = to
            },
                new EDPredicate
            {
                CasType = typeof(Indicator),
                ChartId = "MA_LONG",
                InstrumentId = "MA_LONG",
                Color = Brushes.DarkBlue,
                From = from,
                To = to
            },
                 new CDPredicate
            {
                CasType = typeof(Transaction),
                ChartId = "TRANSACTIONS",
                InstrumentId = "TRANSACTIONS",
                Color = Brushes.DarkBlue,
                From = from,
                To = to
            },
                 new CIPredicate
            {
                CasType = typeof(double),
                ChartId = "EQUITY",
                InstrumentId = "EQUITY",
                Color = Brushes.DarkBlue,
                From = 1,
                To = 1000
            }
        });
        }

        private List<object> GetEquity()
        {
            return Bots
                .First()
                .GetBalanceHistory()
                .Where(row => row.TransactionMargin != 0)
                .Select(row => row.Total)
                .Cast<object>()
                .ToList();
        }

        private TestBot CreateTestBot(string id)
        {
            TestBot bot = new TestBot(_costs);
            bot.SetUpId(id);
            bot.SetUpBalance(1000000000000);
            bot.SetUpWorkingPeriod(new Dictionary<string, WorkingPeriod>
            {
                {"RTS", new WorkingPeriod
                    {
                        Open = new TimeSpan(0, 10, 30, 0),
                        Close = new TimeSpan(0, 23, 30, 0)
                    }
                }
            });
            bot.SetUpPredicate(new BotPredicate.Builder()
                .From(_from)
                .To(_to)
                .Build());
            return bot;
        }
    }
}
