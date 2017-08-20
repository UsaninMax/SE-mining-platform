using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using SEMining.Commons.Setting;
using SEMining.Commons.Sistem;
using SEMining.DataSet.DataServices.Serialization;
using SEMining.DataSet.Models;

namespace SEMining.tests.DataSet.DataServices.Serialization
{
    [TestFixture]
    public class XmlDataSetStorageTest
    {
        [Test]
        public void TestStoreDataSet()
        {
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var serializer = new Mock<ISettingSerializer>();
            ContainerBuilder.Container.RegisterInstance(serializer.Object);

            IEnumerable<DataSetItem> dataSets = new List<DataSetItem>()
            {
                new DataSetItem.Builder().Build()
            };

            XmlDataSetStorage storage = new XmlDataSetStorage();
            storage.Store(dataSets);

            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            serializer.Verify(x => x.Serialize(dataSets, "Settings\\DataSets.xml"), Times.Once);
        }

        [Test]
        public void TestReStoreDataSetWhenFileDoesNotExist()
        {
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(false);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<ISettingSerializer>().Object);
            XmlDataSetStorage storage = new XmlDataSetStorage();

            Assert.That(storage.ReStore().Count, Is.EqualTo(0));
        }

        [Test]
        public void TestReStoreDataSetWhenFileExist()
        {
            IEnumerable<DataSetItem> dataSets = new List<DataSetItem>()
            {
                new DataSetItem.Builder().Build()
            };

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(true);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var serializer = new Mock<ISettingSerializer>();
            serializer.Setup(x => x.Deserialize<IEnumerable<DataSetItem>>("Settings\\DataSets.xml")).Returns(dataSets);
            ContainerBuilder.Container.RegisterInstance(serializer.Object);
            XmlDataSetStorage storage = new XmlDataSetStorage();
            storage.ReStore();

            Assert.That(storage.ReStore(), Is.EqualTo(dataSets));
        }
    }
}
