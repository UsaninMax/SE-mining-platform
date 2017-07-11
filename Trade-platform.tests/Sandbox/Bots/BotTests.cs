using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios;

namespace Trade_platform.tests.Sandbox.Bots
{
    [TestFixture]
    public class BotTests
    {
        [Test]
        public void TestExecuteDataIsEmpty()
        {
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate.Builder()
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(5));
        }

        [Test]
        public void TestExecuteDataHasIds_m_id_1_with_interval()
        {
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate
                .Builder()
                .From(new DateTime(2016, 9, 14, 1, 28, 0))
                .To(new DateTime(2016, 9, 16, 23, 28, 0))
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(2));
        }

        [Test]
        public void Test_execute_dataHas_Ids_m_id_1_with_interval_and_slices_are_grouped()
        {
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate
                    .Builder()
                .From(new DateTime(2016, 9, 14, 1, 28, 0))
                .To(new DateTime(2016, 9, 16, 23, 28, 0))
                .Build());
            bot.Execute();
            IList<List<IData>> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(2));
            Assert.That(slices[1].OfType<Candle>().Count(), Is.EqualTo(2));
            Assert.That(slices[1].OfType<Indicator>().Count(), Is.EqualTo(3));
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

        private IList<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>> GetData()
        {
            return new List<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>>
            {
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 13, 1, 28, 0),
                    new List<IData>
                {
                    new Candle.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build(),
                    new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build()
                },
                    new List<Tick>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()
                    }),
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 14, 1, 28, 0),
                    new List<IData>
                    {
                        new Candle.Builder().WithDate(new DateTime(2016,9, 14, 1, 28, 0)).WithId("m_id_1").Build()
                    },
                    new List<Tick>()


                    ),
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 15),
                    new List<IData>
                    {
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()
                    },
                    new List<Tick>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build(),
                        new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()
                    }),

                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 16, 23, 28, 0),
                    new List<IData>(),
                    new List<Tick>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 16, 23, 28, 0)).WithId("m_id_1").Build()
                    }),
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 17),
                    new List<IData>
                    {
                        new Candle.Builder().WithDate(new DateTime(2016,9, 17)).WithId("m_id_1").Build()
                    },
                    new List<Tick>()),
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 18),
                    new List<IData>
                    {
                        new Indicator.Builder().WithDate(new DateTime(2016,9, 18)).WithId("id_1").Build()
                    },
                    new List<Tick>()),
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    new DateTime(2016,9, 19),
                    new List<IData>(),
                    new List<Tick>
                    {
                        new Tick.Builder().WithDate(new DateTime(2016,9, 19)).WithId("id_1").Build()
                    })
            };
        }
    }
}
