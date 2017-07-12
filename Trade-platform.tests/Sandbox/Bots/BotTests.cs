using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
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
        public void TestExecuteDataWithoutInterval()
        {
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot();
            bot.SetUpData(GetData());
            bot.SetUpPredicate(new BotPredicate.Builder()
                .Build());
            bot.Execute();
            IList<IData> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(10));
        }

        [Test]
        public void TestExecuteDataHasIdsWithInterval()
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
            IList<IData> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(6));
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
            IList<IData> slices = bot.GetSlices();
            Assert.That(slices.Count, Is.EqualTo(6));
            Assert.That(slices.OfType<Candle>().Count(), Is.EqualTo(3));
            Assert.That(slices.OfType<Indicator>().Count(), Is.EqualTo(3));
        }

        private class TestBot : BotApi
        {
            private IList<IData> _slices = new List<IData>();
            public override void Execution(IDictionary<string, IData> data)
            {
                data.Values.ForEach(x => _slices.Add(x));
            }

            public override int Score()
            {
                throw new System.NotImplementedException();
            }

            public IList<IData> GetSlices()
            {
                return _slices;
            }
        }

        private List<Slice> GetData()
        {
            return new List<Slice>
            {
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 13, 1, 28, 0))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()} ,
                        { "id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>
                    {
                        { "m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 14, 1, 28, 0))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 14, 1, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 15))
                    .WithData(new Dictionary<string, IData>
                    {
                        {"m_id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()},
                        {"m_id_2",new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_2").Build()},
                        {"m_id_3",new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_3").Build()},
                        {"m_id_4",new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_4").Build()},
                        {"m_id_5",new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_5").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>
                    {
                        {"m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()},
                        {"m_id_2",new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_2").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate( new DateTime(2016,9, 16, 23, 28, 0))
                    .WithData(new Dictionary<string, IData>())
                    .WithTick(new Dictionary<string, Tick>
                    {
                        {"m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 16, 23, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 17))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 17)).WithId("m_id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 18))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 18)).WithId("id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 19))
                    .WithData(new Dictionary<string, IData>())
                    .WithTick(new Dictionary<string, Tick>
                    {
                        { "id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 19)).WithId("id_1").Build()}
                    })
                    .Build()
            };
        }
    }
}
