using System;
using System.Collections.Generic;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Transactios.Models;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TestSandboxModule
{
    public class TestSandbox : SandboxApi
    {
        public override ICollection<IPredicate> SetUpData()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_1")
                    .AccumulationPeriod(new TimeSpan(0,0,1))
                    .From(new DateTime(2016, 2, 1))
                    .To(new DateTime(2016,2, 5))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_5")
                    .AccumulationPeriod(new TimeSpan(0,0,5))
                    .From(new DateTime(2016, 2, 1))
                    .To(new DateTime(2016,2, 5))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_15")
                    .AccumulationPeriod(new TimeSpan(0,15,0))
                    .From(new DateTime(2016, 2, 1))
                    .To(new DateTime(2016,2, 5))
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA")
                    .Indicator(typeof(MA))
                    .Parameter("length", 12)
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("RTS_5")
                        .ParentId("RTS")
                        .AccumulationPeriod(new TimeSpan(0,0,5))
                        .From(new DateTime(2016, 2, 1))
                        .To(new DateTime(2016,2, 5))
                        .Build())
                    .Build()
            };
        }

        public override void Execution()
        {
            var costs = new Dictionary<string, BrokerCost>();
            costs.Add("RTS", new BrokerCost());

            TestBot bot_1 = new TestBot(costs);
            bot_1.SetUpId("Test_1");
            bot_1.SetUpBalance(10000);
            bot_1.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .Build());

            TestBot bot_2 = new TestBot(costs);
            bot_2.SetUpId("Test_1");
            bot_2.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .Build());

            if (Token.IsCancellationRequested) { return; }

            SetUpBots(new List<IBot>
            {
                bot_1
                //bot_2
            });
            Execute();
            //PopulateCharts(new CandlesDataPredicate
            //{
            //    ChartId = "RTS_1",
            //    InstrumentId = "RTS_1",
            //    From = new DateTime(2016, 2, 1),
            //    To = new DateTime(2016, 2, 2),
            //    UseForAxisX = true
            //});

            PopulateCharts(new CandlesDataPredicate
            {
                ChartId = "RTS_5",
                InstrumentId = "RTS_5",
                From = new DateTime(2016, 2, 1),
                To = new DateTime(2016, 2, 5)
            });
            PopulateCharts(new IndicatorDataPredicate
            {
                ChartId = "RTS_5",
                InstrumentId = "MA",
                From = new DateTime(2016, 2, 1),
                To = new DateTime(2016, 2, 5)
            });
        }

        public override void AfterExecution()
        {
            foreach (var bot in Bots)
            {
                System.Diagnostics.Debug.WriteLine("Bot name = " + bot.GetId() + " - has score " + bot.Score());
            }
        }

        public override IEnumerable<Panel> SetUpCharts()
        {
            return new List<Panel> {
                new Panel
                {
                    Charts = new List<Chart>
                    {
                        new Chart
                        {
                           Ids = new List<string> { "RTS_5"},
                           xAxis = TimeSpan.FromSeconds(5).Ticks
                        }
                    }
                }

            };
        }
    }
}
