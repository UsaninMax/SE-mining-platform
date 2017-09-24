using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using SEMining.Commons.Info;
using SEMining.DataSet.DataServices;
using SEMining.Sandbox.DataProviding;
using SEMining.Sandbox.DataProviding.Checks;
using SEMining.Sandbox.DataProviding.Transformers;
using SEMining.Sandbox.Providers;
using SEMining.StockData.Models;
using SE_mining_base.Sandbox.DataProviding.Predicates;
using SE_mining_base.Sandbox.Models;

namespace SEMining.tests.Sandbox.DataProviding
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

            var ticks = 
                new List<Tick> { new Tick.Builder().WithDate(new DateTime(2016, 2, 5)).WithId("id_1").Build(),
                new Tick.Builder().WithDate(new DateTime(2016, 2, 5)).WithId("id_2").Build() };
            dataAggregatorMock.Setup(x => x.Transform(dataTicks, It.IsAny<TickPredicate>()))
                .Returns(ticks);
            dataAggregatorMock.Setup(x => x.Transform(ticks, It.IsAny<DataPredicate>()))
                .Returns(new List<Candle> {new Candle.Builder().WithId("Id_3").WithDate(new DateTime(2016, 2, 7)).Build()});

            SandboxDataProvider provider = new SandboxDataProvider();

            IEnumerable<Slice> result =  provider.Get(GetPredicate(), new CancellationToken());

            dataAggregatorMock.Verify(x=> x.Transform(It.IsAny<List<DataTick>>(), It.Is<TickPredicate>(
                f => f.Id.Equals("RTS") &&
            f.From.Equals(new DateTime(2016, 2, 1)) &&
            f.To.Equals(new DateTime(2016, 2, 2)))), Times.Exactly(1));
            dataAggregatorMock.Verify(x => x.Transform(It.IsAny<List<Tick>>(), It.IsAny<DataPredicate>()), Times.Exactly(1));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Where(x=> x.DateTime.Equals(new DateTime(2016, 2, 5))).SelectMany(x => x.Datas).ToList().Count, Is.EqualTo(0));
            Assert.That(result.Where(x => x.DateTime.Equals(new DateTime(2016, 2, 7))).SelectMany(x => x.Datas).ToList().Count, Is.EqualTo(1));
        }

        private IEnumerable<IPredicate> GetPredicate()
        {
            return new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("RTS")
                    .NewId("RTS_1")
                    .AccumulationPeriod(new TimeSpan(0,1,0))
                    .From(new DateTime(2016, 2, 1))
                    .To(new DateTime(2016, 2, 2))
                    .Build()
            };
        }
    }
}
