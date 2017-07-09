using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.DataServices;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Checks;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.DataProviding.Transformers;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Providers;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.Sandbox.DataProviding
{
    [TestFixture]
    public class DataProviderTests
    {
        [Test]
        public void Should_add_tick_data()
        {
            var indicatorProviderMock = new Mock<IIndicatorBuilder>();
            ContainerBuilder.Container.RegisterInstance(indicatorProviderMock.Object);
            var predicateCheckerMock = new Mock<IPredicateChecker>();
            ContainerBuilder.Container.RegisterInstance(predicateCheckerMock.Object);
            var infoPublisherMock = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisherMock.Object);

            var dataSetSetServuceMock = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetSetServuceMock.Object);

            predicateCheckerMock.Setup(x => x.Check(It.IsAny<DataPredicate>())).Returns(true);
            predicateCheckerMock.Setup(x => x.Check(It.IsAny<IndicatorPredicate>())).Returns(true);
            var dataTicks = new List<DataTick> { new DataTick(), new DataTick() };
            dataSetSetServuceMock.Setup(x => x.Get("RTS")).Returns(dataTicks);

            var dataAggregatorMock = new Mock<ITransformer>();
            ContainerBuilder.Container.RegisterInstance(dataAggregatorMock.Object);

            var ticks = new List<Tick> { new Tick.Builder().Build(), new Tick.Builder().Build() };
            dataAggregatorMock.Setup(x => x.Transform(dataTicks, It.IsAny<TickPredicate>()))
                .Returns(ticks);
            dataAggregatorMock.Setup(x => x.Transform(ticks, It.IsAny<DataPredicate>()))
                .Returns(new List<Candle> {new Candle.Builder().Build()});

            DataProvider provider = new DataProvider();
            IList<IData> result =  provider.Get(GetPredicate(), new CancellationToken());

            dataAggregatorMock.Verify(x=> x.Transform(It.IsAny<List<DataTick>>(), It.Is<TickPredicate>(
                f => f.Id.Equals("RTS") &&
            f.From.Equals(new DateTime(2016, 1, 29)) &&
            f.To.Equals(new DateTime(2016, 2, 5)))), Times.Exactly(1));
            dataAggregatorMock.Verify(x => x.Transform(It.IsAny<List<Tick>>(), It.IsAny<DataPredicate>()), Times.Exactly(3));
            Assert.That(result.OfType<Candle>().Count, Is.EqualTo(3));
            Assert.That(result.OfType<Tick>().Count, Is.EqualTo(2));
        }


        private ICollection<IPredicate> GetPredicate()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_1")
                    .AccumulationPeriod(new TimeSpan(0,1,0))
                    .From(new DateTime(2016, 2, 1))
                    .To(new DateTime(2016, 2, 2))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_5")
                    .AccumulationPeriod(new TimeSpan(0,5,0))
                    .From(new DateTime(2016, 2, 3))
                    .To(new DateTime(2016, 2, 5))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_6")
                    .AccumulationPeriod(new TimeSpan(0,6,0))
                    .From(new DateTime(2016, 1, 29))
                    .To(new DateTime(2016, 2, 1))
                    .Build()
            };
        }
    }
}
