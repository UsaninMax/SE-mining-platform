using System;
using System.Collections.Generic;
using TradePlatform.SandboxApi;
using TradePlatform.SandboxApi.Bots;
using TradePlatform.SandboxApi.DataProviding.Predicates;

namespace TestSandboxModule
{
    public class TestSandbox : Sandbox
    {
        public override ICollection<IPredicate> PrepareData()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_1")
                    .AccumulationPeriod(new TimeSpan(0,0,1))
                    .From(new DateTime(2015, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_5")
                    .AccumulationPeriod(new TimeSpan(0,0,5))
                    .From(new DateTime(2015, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_15")
                    .AccumulationPeriod(new TimeSpan(0,15,0))
                    .From(new DateTime(2015, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA")
                    .Indicator(typeof(MA))
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("Test")
                        .ParentId("Si")
                        .AccumulationPeriod(new TimeSpan(0,0,5))
                        .From(new DateTime(2015, 1, 1))
                        .To(new DateTime(2017, 1, 1))
                        .Build())
                    .Build()
            };
        }

        public override void Execution()
        {
            _bots = new List<Bot>
            {
                new TestBot
                {
                    Id = "Test_1",
                    Predicate = new BotPredicate.Builder()
                        .From(new DateTime(2015, 1, 1))
                        .To(new DateTime(2015, 1, 1))
                        .InstrumentIds(new List<string>() {"Si", "MA"})
                        .Build()

                },
                new TestBot
                {
                    Id = "Test_2",
                    Predicate = new BotPredicate.Builder()
                        .From(new DateTime(2015, 1, 1))
                        .To(new DateTime(2015, 1, 1))
                        .InstrumentIds(new List<string>() {"Si", "MA"})
                        .Build()

                }
            };
            if(Token.IsCancellationRequested) { return; }
            Execute();
        }

        public override void AfterExecution()
        {
            foreach (var bot in _bots)
            {
                System.Diagnostics.Debug.WriteLine("Bot name = " + bot.Id + " - has score " + bot.Score());
            }
        }
    }
}
