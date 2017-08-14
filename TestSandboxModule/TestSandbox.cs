﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.ResultStoring;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

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

            IEnumerable<Dictionary<string, string>> data = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                },
                new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                },
               new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                }
            };

            ReusltStoring.Store(data);



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
            });
            Execute();


            StoreCustomData("Custom_1", new List<object> { 22d, 33d, 44d, 55d, 66d });
            StoreCustomData("Custom_2", new List<object> {

            new Transaction.Builder().Direction(Direction.Buy).ExecutedPrice(72860).WithDate(new DateTime(2016, 2, 4, 23, 49, 33)).Build(),
             new Transaction.Builder().Direction(Direction.Buy).ExecutedPrice(72860).WithDate(new DateTime(2016, 2, 4, 23, 49, 33)).Build(),
              new Transaction.Builder().Direction(Direction.Buy).ExecutedPrice(72860).WithDate(new DateTime(2016, 2, 4, 23, 49, 33)).Build(),
            new Transaction.Builder().Direction(Direction.Sell).ExecutedPrice(72880).WithDate(new DateTime(2016, 2, 4, 23, 49, 41)).Build()

            });

            PopulateCharts(new List<ChartPredicate>
            {
                new EDPredicate
            {
                CasType = typeof(Candle),
                ChartId = "RTS_5",
                InstrumentId = "RTS_5",
                From = new DateTime(2016, 2, 1, 13, 55, 00),
                To = new DateTime(2016, 2, 1, 13, 56, 00)
            },
                new EDPredicate
            {
                CasType = typeof(Indicator),
                ChartId = "RTS_5",
                InstrumentId = "MA",
                Color = Brushes.DarkBlue,
                From = new DateTime(2016, 2, 1, 13, 55, 00),
                To = new DateTime(2016, 2, 1, 13, 56, 00)
            },
                new CIPredicate
                {
                CasType = typeof(double),
                ChartId = "Custom_1",
                InstrumentId = "Custom_1",
                Color = Brushes.DarkBlue,
                From = 0,
                To = 999
                }
                ,
                new CDPredicate
                {
                CasType = typeof(Transaction),
                ChartId = "RTS_5",
                InstrumentId = "Custom_2",
                From = new DateTime(2016, 2, 1, 13, 55, 00),
                To = new DateTime(2016, 2, 1, 13, 56, 00)
                }
            });
        }

        public override void AfterExecution()
        {
            foreach (var bot in Bots)
            {
                System.Diagnostics.Debug.WriteLine("Bot name = " + bot.GetId() + " - has score " + bot.Score());
            }
        }

        public override IEnumerable<PanelViewPredicate> SetUpCharts()
        {
            return new List<PanelViewPredicate> {
                new PanelViewPredicate
                {
                    ChartPredicates = new List<ChartViewPredicate>
                    {
                        new DateChartViewPredicate
                        {
                           Ids = new List<string> { "RTS_5"},
                           XAxis = TimeSpan.FromSeconds(5),
                           YSize = 400
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "Custom_1"},
                           YSize = 300
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "Custom_2"},
                           YSize = 300
                        }
                    }
                }
            };
        }
    }
}
