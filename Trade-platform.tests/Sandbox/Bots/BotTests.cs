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
using TradePlatform.Sandbox.Transactios.Models;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Data.Populating;
using TradePlatform.Sandbox.Holders;

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
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            sandboxDataHolder.Setup(x => x.Get()).Returns(GetData());
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
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
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            sandboxDataHolder.Setup(x => x.Get()).Returns(GetData());
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
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            sandboxDataHolder.Setup(x => x.Get()).Returns(GetData());
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

        [Test]
        public void Test_execute_tick_data()
        {
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            sandboxDataHolder.Setup(x => x.Get()).Returns(GetData());
            bot.SetUpPredicate(new BotPredicate
                    .Builder()
                .From(new DateTime(2016, 9, 14, 1, 28, 0))
                .To(new DateTime(2016, 9, 16, 23, 28, 0))
                .Build());
            bot.Execute();
            transactionContextMock.Verify(x=>x.ProcessTick(It.IsAny<IDictionary<string, Tick>>(), It.IsAny<DateTime>()), Times.Exactly(3));
        }

        [Test]
        public void Check_set_up_working_period()
        {
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            IDictionary<string, WorkingPeriod> value = new Dictionary<string, WorkingPeriod>();
            bot.SetUpWorkingPeriod(value);
            transactionContextMock.Verify(x=>x.SetUpWorkingPeriod(value), Times.Once);
        }

        [Test]
        public void Check_set_up_balance()
        {
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            bot.SetUpBalance(1000);
            transactionContextMock.Verify(x => x.SetUpBalance(1000), Times.Once);
        }

        [Test]
        public void Check_set_up_open_position()
        {
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            OpenPositionRequest request = new OpenPositionRequest.Builder().Build();
            bot.OpenPosition(request);
            transactionContextMock.Verify(x => x.OpenPosition(request), Times.Once);
        }

        [Test]
        public void Check_is_prepared()
        {
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            bot.IsPrepared();
            transactionContextMock.Verify(x => x.IsPrepared(), Times.Once);
        }

        [Test]
        public void Check_reset()
        {
            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            var chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(chartPredicatesHolder.Object);
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var transactionContextMock = new Mock<ITransactionsContext>();
            ContainerBuilder.Container.RegisterInstance(transactionContextMock.Object);
            TestBot bot = new TestBot(new Dictionary<string, BrokerCost>());
            bot.ResetTransactionContext();
            transactionContextMock.Verify(x => x.Reset(), Times.Once);
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

            public TestBot(IDictionary<string, BrokerCost> brokerCosts) : base(brokerCosts)
            {
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
