using NUnit.Framework;
using Microsoft.Practices.Unity;
using Moq;
using System.Collections.Generic;
using SEMining.Commons.Setting;
using SEMining.Commons.Sistem;
using SEMining.DataSet.DataServices.Serialization;
using SEMining.StockData.Models;

namespace SEMining.tests.DataSet.DataServices.Serialization
{
    [TestFixture]
    public class XmlDataTickStorageTest
    {
        [Test]
        public void TestStoreDataTick()
        {
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var serializer = new Mock<ISettingSerializer>();
            ContainerBuilder.Container.RegisterInstance(serializer.Object);

            IEnumerable<DataTick> ticks = new List<DataTick>()
            {
                new DataTick()
            };

            XmlDataTickStorage storage = new XmlDataTickStorage();
            storage.Store(ticks, "path", "file");
            serializer.Verify(x => x.Serialize(ticks, "path" + "\\" + "file" + ".xml"));
        }

        //[Test]
        //public void TestReStoreDataTick()
        //{
        //    Assert.Ignore("Not implemented. Omitting.");
        //}
    }
}
