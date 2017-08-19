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
using TradePlatform.Sandbox.Results.Adaptors;
using TradePlatform.Sandbox.Results.Storing;
using TradePlatform.Sandbox.Transactios.Models;

namespace TestSandboxModule
{
    public class TestSandbox : SandboxApi
    {
        private IBot _first;
        private IBot _second;
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
            _first = CreateTestBot();
            _second = CreateTestBot();
            SetUpBots(new List<IBot> { _first, _second });
            Execute();
        }

        public override void AfterExecution()
        {
            IReportAdaptor reportAdaptor = new DefaultReportAdaptor();
            ReusltStoring.Store(reportAdaptor.Adopt(_first.GetBalanceHistory()), " | ");
            ReusltStoring.Store(reportAdaptor.Adopt(_first.GetTansactionsHistory()), " | ");
            ReusltStoring.Store(reportAdaptor.Adopt(_first.GetRequestsHistory()), " | ");
            StoreCustomData("TRANSACTIONS", new List<object>(Bots.First().GetTansactionsHistory()));
            StoreCustomData("EQUITY", GetEquity());
            var from = new DateTime(2016, 2, 1, 13, 55, 00);
            var to = new DateTime(2016, 2, 1, 13, 56, 00);
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

        private TestBot CreateTestBot()
        {
            TestBot bot = new TestBot(_costs);
            bot.SetUpId("Test_1");
            bot.SetUpBalance(10000000000);
            bot.SetUpWorkingPeriod(new Dictionary<string, WorkingPeriod>()
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
