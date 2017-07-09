using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;

namespace Trade_platform.tests.Sandbox.Bots
{
    [TestFixture]
    public class BotTests
    {
        [Test]
        public void TestExecuteDataIsEmpty()
        {
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate.Builder().InstrumentIds(new List<string>()).Build());
            bot.Execute();
            IList<Slice> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestExecuteDataHasIds_m_id_1()
        {
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate.Builder().InstrumentIds(new List<string>{ "m_id_1" }).Build());
            bot.Execute();
            IList<Slice> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(5));
        }

        [Test]
        public void TestExecuteDataHasIds_m_id_1_with_interval()
        {
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate
                .Builder()
                .From(new DateTime(2016, 9, 14, 1, 28, 0))
                .To(new DateTime(2016, 9, 16, 23, 28, 0))
                .InstrumentIds(new List<string> { "m_id_1" })
                .Build());
            bot.Execute();
            IList<Slice> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(3));
        }

        [Test]
        public void Test_execute_dataHas_Ids_m_id_1_with_interval_and_slices_are_grouped()
        {
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate
                    .Builder()
                .From(new DateTime(2016, 9, 14, 1, 28, 0))
                .To(new DateTime(2016, 9, 16, 23, 28, 0))
                .InstrumentIds(new List<string> { "m_id_1" })
                .Build());
            bot.Execute();
            IList<Slice> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(3));
            Assert.That(slices[1].Candles.Count(), Is.EqualTo(2));
            Assert.That(slices[1].Indicators.Count(), Is.EqualTo(3));
            Assert.That(slices[1].Ticks.Count(), Is.EqualTo(2));
        }

        private class TestBot : BotApi
        {
            private IList<Slice> _slices = new List<Slice>();
            public override void Execution(Slice slice)
            {
                _slices.Add(slice);
            }

            public override int Score()
            {
                throw new System.NotImplementedException();
            }

            public IList<Slice> GetSlices()
            {
                return _slices;
            }
        }

        private IList<IData> GetData()
        {
            return new List<IData>
            {
                new Candle.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build(),
                new Candle.Builder().WithDate(new DateTime(2016,9, 14, 1, 28, 0)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016,9, 16, 23, 28, 0)).WithId("m_id_1").Build(),
                new Candle.Builder().WithDate(new DateTime(2016,9, 17)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 18)).WithId("id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016,9, 19)).WithId("id_1").Build()
            };
        }
    }
}
