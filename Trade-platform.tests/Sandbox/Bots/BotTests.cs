using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
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
            bot.SetUpPredicate(new BotPredicate.Builder()
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(7));
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
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
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
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(3));
            Assert.That(slices[1].OfType<Candle>().Count(), Is.EqualTo(2));
            Assert.That(slices[1].OfType<Indicator>().Count(), Is.EqualTo(3));
            Assert.That(slices[1].OfType<Tick>().Count(), Is.EqualTo(2));
        }

        private class TestBot : BotApi
        {
            private IList<List<IData>> _slices = new List<List<IData>>();
            public override void Execution(IEnumerable<IData> slice)
            {
                _slices.Add(new List<IData>(slice));
            }

            public override int Score()
            {
                throw new System.NotImplementedException();
            }

            public IList<List<IData>> GetSlices()
            {
                return _slices;
            }
        }

        private IList<Pair<DateTime, IEnumerable<IData>>> GetData()
        {
            return new List<Pair<DateTime, IEnumerable<IData>>>
            {
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 13, 1, 28, 0),
                    new List<IData>
                {
                    new Candle.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build(),
                    new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build(),
                    new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()
                }),
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 14, 1, 28, 0),
                    new List<IData>
                    {
                        new Candle.Builder().WithDate(new DateTime(2016,9, 14, 1, 28, 0)).WithId("m_id_1").Build()
                    }),
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 15),
                    new List<IData>
                    {
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()
                    }),

                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 16, 23, 28, 0),
                    new List<IData>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 16, 23, 28, 0)).WithId("m_id_1").Build()
                    }),
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 17),
                    new List<IData>
                    {
                        new Candle.Builder().WithDate(new DateTime(2016,9, 17)).WithId("m_id_1").Build()
                    }),
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 18),
                    new List<IData>
                    {
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 18)).WithId("id_1").Build()
                    }),
                new Pair<DateTime, IEnumerable<IData>>(
                    new DateTime(2016,9, 19),
                    new List<IData>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 19)).WithId("id_1").Build()
                    })
            };
        }
    }
}
