using NUnit.Framework;
using Microsoft.Practices.Unity;
using Moq;
using System.Collections.Generic;
using TradePlatform;
using TradePlatform.Commons.Setting;
using TradePlatform.Commons.Sistem;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.Commons.BaseModels;

namespace Trade_platform.tests.DataSet.DataServices.Serialization
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

            IList<DataTick> ticks = new List<DataTick>()
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
