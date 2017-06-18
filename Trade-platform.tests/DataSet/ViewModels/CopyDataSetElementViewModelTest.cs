using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.ViewModels;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.DataSet.ViewModels
{
    [TestFixture]
    public class CopyDataSetElementViewModelTest
    {
        [Test]
        public void TestAddSelectedInstruments()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(dataSetHolder.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            IList<SubInstrument> instruments = new List<SubInstrument> {
                new SubInstrument(new Instrument.Builder().WithCode("test_1").Build()),
                new SubInstrument(new Instrument.Builder().WithCode("test_2").Build())
            };
            DataSetItem item = new DataSetItem.Builder()
                .WithSubInstruments(instruments)
                .WithId("test_id")
                .WithStepSize(12.3)
                .WithWarrantyCoverage(32.3)
                .Build();
            CopyDataSetElementViewModel model = new CopyDataSetElementViewModel();
            eventAggregator.GetEvent<CopyDataSetEvent>().Publish(item);

            Assert.That(model.InstrumentsInfo.Count, Is.EqualTo(2));
            Assert.That(model.InstrumentsInfo[0].Code, Is.EqualTo("test_1"));
            Assert.That(model.InstrumentsInfo[1].Code, Is.EqualTo("test_2"));
            Assert.That(model.UniqueId, Is.Not.EqualTo(item.Id));
            Assert.That(model.WarrantyCoverage, Is.EqualTo(item.WarrantyCoverage));
            Assert.That(model.StepSize, Is.EqualTo(item.StepSize));
        }

        [Test]
        public void TestDispose()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(dataSetHolder.Object);

            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);

            fakeEventAggregator.Setup(x => x.GetEvent<CreateDataSetItemEvent>()
                .Publish(It.IsAny<DataSetItem>()));
            fakeEventAggregator.Setup(x => x.GetEvent<AddInstrumentToDatatSetEvent>()
                .Publish(It.IsAny<IList<Instrument>>()));

            fakeEventAggregator.Setup(x => x.GetEvent<CopyDataSetEvent>()
                .Publish(It.IsAny<DataSetItem>()));

            CopyDataSetElementViewModel model = new CopyDataSetElementViewModel();

            model.Dispose();

            fakeEventAggregator.Verify(x => x.GetEvent<AddInstrumentToDatatSetEvent>()
                .Unsubscribe(It.IsAny<Action<IList<Instrument>>>()), Times.Once);

            fakeEventAggregator.Verify(x => x.GetEvent<CopyDataSetEvent>()
                .Unsubscribe(It.IsAny<Action<DataSetItem>>()), Times.Once);
        }
    }
}
