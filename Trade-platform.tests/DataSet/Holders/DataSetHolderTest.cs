using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TradePlatform;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;

namespace Trade_platform.tests.DataSet.Holders
{
    [TestFixture]
    public class DataSetHolderTest
    {

        [Test]
        public void TestStore()
        {
            var dataSetStorage = new Mock<IDataSetStorage>();
            ContainerBuilder.Container.RegisterInstance(dataSetStorage.Object);
            DataSetHolder holder = new DataSetHolder();
            holder.Store();
            dataSetStorage.Verify(x => x.Store(It.IsAny<IEnumerable<DataSetItem>>()), Times.Once);

        }

        [Test]
        public void TestRestore()
        {
            var dataSetStorage = new Mock<IDataSetStorage>();
            ContainerBuilder.Container.RegisterInstance(dataSetStorage.Object);

            DataSetItem item_1 = new DataSetItem.Builder().WithId("test_id_1").Build();
            DataSetItem item_2 = new DataSetItem.Builder().WithId("test_id_2").Build();

            dataSetStorage.Setup(x => x.ReStore()).Returns(new List<DataSetItem> { item_1, item_2 });

            DataSetHolder holder = new DataSetHolder();
            holder.Restore();

            Assert.That(holder.GetAll().Count, Is.EqualTo(2));
            Assert.That(holder.CheckIfExist("test_id_1"), Is.True);
            Assert.That(holder.CheckIfExist("test_id_2"), Is.True);
        }
    }
}
