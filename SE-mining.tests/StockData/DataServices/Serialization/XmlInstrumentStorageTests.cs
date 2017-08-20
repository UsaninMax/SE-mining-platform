using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using SEMining.Commons.Setting;
using SEMining.Commons.Sistem;
using SEMining.StockData.DataServices.Serialization;
using SEMining.StockData.Models;

namespace SEMining.tests.StockData.DataServices.Serialization
{
    [TestFixture]
    public class XmlInstrumentStorageTests
    {
        [Test]
        public void StoreInstrumentCheck()
        {
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var settingSerializer = new Mock<ISettingSerializer>();
            ContainerBuilder.Container.RegisterInstance(settingSerializer.Object);
            IEnumerable<Instrument> instruments = new List<Instrument>
            {
                new Instrument.Builder().Build(),
                new Instrument.Builder().Build()
            };

            var xmlInstrumentStorage = new XmlInstrumentStorage();
            xmlInstrumentStorage.Store(instruments);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            settingSerializer.Verify(x => x.Serialize(instruments, It.IsAny<string>()), Times.Once);

        }

        [Test]
        public void ReStoreInstrumentCheck()
        {
            IEnumerable<Instrument> instruments = new List<Instrument>
            {
                new Instrument.Builder().Build(),
                new Instrument.Builder().Build()
            };

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(true);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var settingSerializer = new Mock<ISettingSerializer>();
            settingSerializer.Setup(x => x.Deserialize<IEnumerable<Instrument>>(It.IsAny<string>())).Returns(instruments);
            ContainerBuilder.Container.RegisterInstance(settingSerializer.Object);

            var xmlInstrumentStorage = new XmlInstrumentStorage();
            IEnumerable<Instrument> result = xmlInstrumentStorage.ReStore();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.EqualTo(instruments));
            settingSerializer.Verify(x => x.Deserialize<IEnumerable<Instrument>>(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ReStoreInstrumentCheckIfPathNoExist()
        {
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(false);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var settingSerializer = new Mock<ISettingSerializer>();
            ContainerBuilder.Container.RegisterInstance(settingSerializer.Object);
            var xmlInstrumentStorage = new XmlInstrumentStorage();
            IEnumerable<Instrument> result = xmlInstrumentStorage.ReStore();

            Assert.That(result.Count, Is.EqualTo(0));
            settingSerializer.Verify(x => x.Deserialize<IEnumerable<Instrument>>(It.IsAny<string>()), Times.Never);


        }
    }
}
