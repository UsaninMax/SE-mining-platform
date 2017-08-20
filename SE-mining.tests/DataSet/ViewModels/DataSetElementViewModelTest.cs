using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using SEMining.Commons.Info;
using SEMining.DataSet.Events;
using SEMining.DataSet.Holders;
using SEMining.DataSet.Models;
using SEMining.DataSet.ViewModels;
using SEMining.StockData.Models;

namespace SEMining.tests.DataSet.ViewModels
{
    [TestFixture]
    public class DataSetElementViewModelTest
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
            IEnumerable<Instrument> instruments = new List<Instrument> {
                new Instrument.Builder().WithCode("test_1").Build(),
                new Instrument.Builder().WithCode("test_2").Build()
            };

            DataSetElementViewModel model = new DataSetElementViewModel();
            eventAggregator.GetEvent<AddInstrumentToDatatSetEvent>().Publish(instruments);
            Assert.That(model.InstrumentsInfo.Count, Is.EqualTo(2));
            Assert.That(model.InstrumentsInfo[0].Code, Is.EqualTo("test_1"));
            Assert.That(model.InstrumentsInfo[1].Code, Is.EqualTo("test_2"));
        }

        [Test]
        public void TestCreateNew()
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
                .Publish(It.IsAny<IEnumerable<Instrument>>()));

            dataSetHolder.Setup(x => x.CheckIfExist(It.IsAny<String>())).Returns(false);

            bool isHit = false;

            DataSetElementViewModel model = new DataSetElementViewModel
            {
                UniqueId = "test_id_",
                StepSize = 12.3,
                WarrantyCoverage = 22.2
            };
            model.CloseWindowNotification += (s, e) =>
            {
                isHit = true;
            };
            model.InstrumentsInfo.Add(new SubInstrument(new Instrument.Builder().WithCode("test_1").Build()));
            model.InstrumentsInfo.Add(new SubInstrument(new Instrument.Builder().WithCode("test_2").Build()));
            model.CreateNewCommand.Execute(null);

            fakeEventAggregator.Verify(x => x.GetEvent<CreateDataSetItemEvent>()
                .Publish(It.Is<DataSetItem>(
                    m =>
                        model.UniqueId.Equals(m.Id) &&
                        model.StepSize.Equals(m.StepSize) &&
                        model.WarrantyCoverage.Equals(m.WarrantyCoverage) &&
                        model.InstrumentsInfo.Equals(m.SubInstruments)
                )), Times.Once);


            Assert.IsTrue(isHit);
        }

        [Test]
        public void TestCreateNewWhenUniqueIdIsWrong()
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
                .Publish(It.IsAny<IEnumerable<Instrument>>()));

            dataSetHolder.Setup(x => x.CheckIfExist(It.IsAny<String>())).Returns(true);

            bool isHit = false;

            DataSetElementViewModel model = new DataSetElementViewModel
            {
                UniqueId = "test_id_",
                StepSize = 12.3,
                WarrantyCoverage = 22.2
            };
            model.CloseWindowNotification += (s, e) =>
            {
                isHit = true;
            };
            model.InstrumentsInfo.Add(new SubInstrument(new Instrument.Builder().WithCode("test_1").Build()));
            model.InstrumentsInfo.Add(new SubInstrument(new Instrument.Builder().WithCode("test_2").Build()));
            model.CreateNewCommand.Execute(null);

            fakeEventAggregator.Verify(x => x.GetEvent<CreateDataSetItemEvent>()
                .Publish(It.Is<DataSetItem>(
                    m =>
                        model.UniqueId.Equals(m.Id) &&
                        model.StepSize.Equals(m.StepSize) &&
                        model.WarrantyCoverage.Equals(m.WarrantyCoverage) &&
                        model.InstrumentsInfo.Equals(m.SubInstruments)
                )), Times.Never);
            infoPublisher.Verify(x => x.PublishException(It.IsAny<Exception>()), Times.Once);

            Assert.IsFalse(isHit);
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
                .Publish(It.IsAny<IEnumerable<Instrument>>()));

            DataSetElementViewModel model = new DataSetElementViewModel();
 
            model.Dispose();

            fakeEventAggregator.Verify(x => x.GetEvent<AddInstrumentToDatatSetEvent>()
                .Unsubscribe(It.IsAny<Action<IEnumerable<Instrument>>>()), Times.Once);
        }
    }
}
