using System;
using System.Collections.Generic;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TestSandboxModule
{
    public class TestSandbox : SandboxApi
    {
        public override ICollection<IPredicate> SetUpData()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_1")
                    .AccumulationPeriod(new TimeSpan(0,0,1))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_5")
                    .AccumulationPeriod(new TimeSpan(0,0,5))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_15")
                    .AccumulationPeriod(new TimeSpan(0,15,0))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA")
                    .Indicator(typeof(MA))
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("Si_5")
                        .ParentId("Si")
                        .AccumulationPeriod(new TimeSpan(0,0,5))
                        .From(new DateTime(2014, 1, 1))
                        .To(new DateTime(2017, 1, 1))
                        .Build())
                    .Build()
            };
        }

        public override void Execution()
        {
            TestBot bot_1 = new TestBot();
            bot_1.SetUpId("Test_1");
            bot_1.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1","Si_5", "MA" })
                .Build());

            TestBot bot_2 = new TestBot();
            bot_2.SetUpId("Test_1");
            bot_2.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());
            TestBot bot_3 = new TestBot();
            bot_3.SetUpId("Test_1");
            bot_3.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());
            TestBot bot_4 = new TestBot();
            bot_4.SetUpId("Test_1");
            bot_4.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            TestBot bot_5 = new TestBot();
            bot_5.SetUpId("Test_1");
            bot_5.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            TestBot bot_6 = new TestBot();
            bot_6.SetUpId("Test_1");
            bot_6.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            TestBot bot_7 = new TestBot();
            bot_7.SetUpId("Test_1");
            bot_7.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            TestBot bot_8 = new TestBot();
            bot_8.SetUpId("Test_1");
            bot_8.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            TestBot bot_9 = new TestBot();
            bot_9.SetUpId("Test_1");
            bot_9.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());
            TestBot bot_10 = new TestBot();
            bot_10.SetUpId("Test_1");
            bot_10.SetUpPredicate(new BotPredicate.Builder()
                .From(new DateTime(2014, 1, 1))
                .To(new DateTime(2017, 1, 1))
                .InstrumentIds(new List<string>() { "Si_1", "Si_5", "MA" })
                .Build());

            if (Token.IsCancellationRequested) { return; }

            SetUpBots(new List<IBot>
            {
                bot_1,
                bot_2
            });
            Execute();
        }

        public override void AfterExecution()
        {
            foreach (var bot in Bots)
            {
                System.Diagnostics.Debug.WriteLine("Bot name = " + bot.GetId() + " - has score " + bot.Score());
            }
        }
    }
}
