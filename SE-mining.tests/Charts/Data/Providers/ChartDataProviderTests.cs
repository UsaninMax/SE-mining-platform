using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SEMining.Sandbox.Holders;
using Microsoft.Practices.Unity;
using SE_mining_base.Sandbox.Models;


namespace SEMining.Charts.Data.Providers
{
    [TestFixture]
    public class ChartDataProviderTests
    {

        [SetUp]
        public void SetUp()
        {
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            sandboxDataHolder.Setup(x => x.Get()).Returns(GetSliceData());

            var customDataHolder = new Mock<ICustomDataHolder>();
            ContainerBuilder.Container.RegisterInstance(customDataHolder.Object);
            customDataHolder.Setup(x => x.Get("id_1")).Returns(GetCustomeData_1());
            customDataHolder.Setup(x => x.Get("m_id_1")).Returns(GetCustomeData_2());
        }

        [Test]
        public void check_get_data_from_exist_storage()
        {
            IChartDataProvider dataProvider = new ChartDataProvider();
            IEnumerable<Indicator> result = dataProvider.GetExistStorageData<Indicator>("m_id_1");
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("m_id_1"));
            Assert.That(result.First().Date(), Is.EqualTo(new DateTime(2016, 9, 15)));
        }

        [Test]
        public void check_get_data_from_exist_storage_2()
        {
            IChartDataProvider dataProvider = new ChartDataProvider();
            IEnumerable<Indicator> result = dataProvider.GetExistStorageData<Indicator>("id_1");
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void check_get_data_from_custome_storage()
        {
            IChartDataProvider dataProvider = new ChartDataProvider();
            IEnumerable<Indicator> result = dataProvider.GetCustomStorageData<Indicator>("id_1");
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Date(), Is.EqualTo(new DateTime(2016, 9, 13, 1, 28, 0)));
            Assert.That(result.First().Date(), Is.EqualTo(new DateTime(2016, 9, 13, 1, 28, 0)));
        }

        [Test]
        public void check_get_data_from_custome_storage_when_data_was_stored_not_correctly()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                new ChartDataProvider().GetCustomStorageData<Indicator>("m_id_1");
            });
        }

        private List<Slice> GetSliceData()
        {
            return new List<Slice>
            {
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 13, 1, 28, 0))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()} ,
                        { "id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>
                    {
                        { "m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 14, 1, 28, 0))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 14, 1, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 15))
                    .WithData(new Dictionary<string, IData>
                    {
                        {"m_id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()},
                        {"m_id_2",new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_2").Build()},
                        {"m_id_3",new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_3").Build()},
                        {"m_id_4",new Candle.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_4").Build()},
                        {"m_id_5",new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_5").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>
                    {
                        {"m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()},
                        {"m_id_2",new Tick.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_2").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate( new DateTime(2016,9, 16, 23, 28, 0))
                    .WithData(new Dictionary<string, IData>())
                    .WithTick(new Dictionary<string, Tick>
                    {
                        {"m_id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 16, 23, 28, 0)).WithId("m_id_1").Build()}
                    })
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 17))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "m_id_1", new Candle.Builder().WithDate(new DateTime(2016,9, 17)).WithId("m_id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 18))
                    .WithData(new Dictionary<string, IData>
                    {
                        { "id_1", new Indicator.Builder().WithDate(new DateTime(2016,9, 18)).WithId("id_1").Build()}
                    })
                    .WithTick(new Dictionary<string, Tick>())
                    .Build(),
                new Slice.Builder()
                    .WithDate(new DateTime(2016,9, 19))
                    .WithData(new Dictionary<string, IData>())
                    .WithTick(new Dictionary<string, Tick>
                    {
                        { "id_1", new Tick.Builder().WithDate(new DateTime(2016,9, 19)).WithId("id_1").Build()}
                    })
                    .Build()
            };
        }

        private List<object> GetCustomeData_1()
        {
            return new List<object>
            {
                new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("id_1").Build()
            };
        }

        private List<object> GetCustomeData_2()
        {
            return new List<object>
            {
                new Tick.Builder().WithDate(new DateTime(2016,9, 13, 1, 28, 0)).WithId("m_id_1").Build(),
                new Indicator.Builder().WithDate(new DateTime(2016,9, 15)).WithId("m_id_1").Build()
            };
        }
    }
}
