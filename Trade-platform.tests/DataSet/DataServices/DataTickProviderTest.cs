using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TradePlatform;
using TradePlatform.DataSet.DataServices;
using TradePlatform.DataSet.Models;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.DataSet.DataServices
{
    [TestFixture]
    public class DataTickProviderTest
    {
        [Test]
        public void TestGetTickDataWithEmptySelectedRange()
        {
            var parser = new Mock<IDataTickParser>();
            ContainerBuilder.Container.RegisterInstance(parser.Object);
            SubInstrument subInstrument_1 = new SubInstrument(new Instrument.Builder().Build());
            SubInstrument subInstrument_2 = new SubInstrument(new Instrument.Builder().Build());

            IEnumerable<SubInstrument> subInstruments = new List<SubInstrument>()
            {
                subInstrument_1,
                subInstrument_2
            };

            IEnumerable<DataTick> ticks_1 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            IEnumerable<DataTick> ticks_2 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 20)},
                new DataTick() { Date = new DateTime(2017, 3, 22)},
                new DataTick() { Date = new DateTime(2017, 3, 19)}
            };


            parser.Setup(x => x.Parse(subInstrument_1)).Returns(ticks_1);
            parser.Setup(x => x.Parse(subInstrument_2)).Returns(ticks_2);

            DataSetItem item = new DataSetItem.Builder().WithSubInstruments(subInstruments).Build();
            DataTickProvider provider = new DataTickProvider();
            IEnumerable<DataTick> ticks = provider.Get(item, It.IsAny<CancellationToken>());

            Assert.That(ticks.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGetTickDataWithSelectedRange()
        {
            var parser = new Mock<IDataTickParser>();
            ContainerBuilder.Container.RegisterInstance(parser.Object);
            SubInstrument subInstrument_1 = new SubInstrument(new Instrument.Builder().Build()) { SelectedFrom = new DateTime(2017, 3, 15), SelectedTo = new DateTime(2017, 3, 18) };
            SubInstrument subInstrument_2 = new SubInstrument(new Instrument.Builder().Build()) { SelectedFrom = new DateTime(2017, 3, 15), SelectedTo = new DateTime(2017, 3, 23) };

            IEnumerable<SubInstrument> subInstruments = new List<SubInstrument>()
            {
                subInstrument_1,
                subInstrument_2
            };

            IEnumerable<DataTick> ticks_1 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            IEnumerable<DataTick> ticks_2 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 20)},
                new DataTick() { Date = new DateTime(2017, 3, 22)},
                new DataTick() { Date = new DateTime(2017, 3, 19)}
            };

            IEnumerable<DataTick> expectedTicks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)},
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 19)},
                new DataTick() { Date = new DateTime(2017, 3, 20)},
                new DataTick() { Date = new DateTime(2017, 3, 22)}
            };

            parser.Setup(x => x.Parse(subInstrument_1)).Returns(ticks_1);
            parser.Setup(x => x.Parse(subInstrument_2)).Returns(ticks_2);

            DataSetItem item = new DataSetItem.Builder().WithSubInstruments(subInstruments).Build();
            DataTickProvider provider = new DataTickProvider();
            IEnumerable<DataTick> ticks = provider.Get(item, It.IsAny<CancellationToken>());

            Assert.That(ticks.Count, Is.EqualTo(6));
            Assert.IsTrue(ticks.SequenceEqual(expectedTicks));
        }

        [Test]
        public void TestGetTickDataWithSelectedRange_2()
        {
            var parser = new Mock<IDataTickParser>();
            ContainerBuilder.Container.RegisterInstance(parser.Object);
            SubInstrument subInstrument_1 = new SubInstrument(new Instrument.Builder().Build()) { SelectedFrom = new DateTime(2017, 3, 16), SelectedTo = new DateTime(2017, 3, 17) };
            SubInstrument subInstrument_2 = new SubInstrument(new Instrument.Builder().Build()) { SelectedFrom = new DateTime(2017, 3, 20), SelectedTo = new DateTime(2017, 3, 21) };

            IEnumerable<SubInstrument> subInstruments = new List<SubInstrument>()
            {
                subInstrument_1,
                subInstrument_2
            };

            IEnumerable<DataTick> ticks_1 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            IEnumerable<DataTick> ticks_2 = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 20)},
                new DataTick() { Date = new DateTime(2017, 3, 22)},
                new DataTick() { Date = new DateTime(2017, 3, 19)}
            };

            IEnumerable<DataTick> expectedTicks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 16)},
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 20)},
            };

            parser.Setup(x => x.Parse(subInstrument_1)).Returns(ticks_1);
            parser.Setup(x => x.Parse(subInstrument_2)).Returns(ticks_2);

            DataSetItem item = new DataSetItem.Builder().WithSubInstruments(subInstruments).Build();
            DataTickProvider provider = new DataTickProvider();
            IEnumerable<DataTick> ticks = provider.Get(item, It.IsAny<CancellationToken>());

            Assert.That(ticks.Count, Is.EqualTo(3));
            Assert.IsTrue(ticks.SequenceEqual(expectedTicks));
        }
    }
}
