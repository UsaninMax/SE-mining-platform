using NUnit.Framework;
using Moq;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Vizualization.Holders;
using TradePlatform;
using TradePlatform.Charts.Data.Providers;
using TradePlatform.Charts.Vizualization.Dispatching;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Vizualization.Configurations;
using System.Collections.Generic;
using System;
using TradePlatform.Charts.Data.Populating;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace Trade_platform.tests.Charts.Data.Populating
{
    [TestFixture]
    public class ChartsPopulatorTests
    {

        private IEnumerable<PanelViewPredicate> _configuration;
        private Mock<IChartsConfigurationDispatcher> _chartsConfigurationDispatcher;
        private Mock<IChartPredicatesHolder> _chartPredicatesHolder;
        private Mock<IChartProxy> _chartProxy;
        private Mock<IChartDataProvider> _chartDataProvider;
        private Mock<IChartsHolder> _chartsHolder;

        [SetUp]
        public void SetUp()
        {
            _chartsHolder = new Mock<IChartsHolder>();
            ContainerBuilder.Container.RegisterInstance(_chartsHolder.Object);
            _chartDataProvider = new Mock<IChartDataProvider>();
            ContainerBuilder.Container.RegisterInstance(_chartDataProvider.Object);
            _chartProxy = new Mock<IChartProxy>();
            ContainerBuilder.Container.RegisterInstance(_chartProxy.Object);
            _chartPredicatesHolder = new Mock<IChartPredicatesHolder>();
            ContainerBuilder.Container.RegisterInstance(_chartPredicatesHolder.Object);
            _chartsConfigurationDispatcher = new Mock<IChartsConfigurationDispatcher>();
            ContainerBuilder.Container.RegisterInstance(_chartsConfigurationDispatcher.Object);
            _configuration = SetUpChartPredicate();
        }

        [Test]
        public void when_create_chart_populator_chart_will_create_also()
        {
            new ChartsPopulator(_configuration);
            _chartsConfigurationDispatcher.Verify(x => x.Dispatch(_configuration), Times.Once);
        }

        [Test]
        public void Before_populate_chart_will_clean_previous_data()
        {
            var chartPredicate = new CDPredicate { ChartId = "Chart_id_1" };
            var chartPredicate_2 = new EDPredicate { ChartId = "Chart_id_2" };
            var model_1 = new IndexChartViewModel();
            var model_2 = new DateChartViewModel(new TimeSpan());

            _chartPredicatesHolder.Setup(x => x.GetAll()).Returns(new List<ChartPredicate>
            {
                chartPredicate,
                chartPredicate_2
            });

            _chartsHolder.Setup(x => x.Get("Chart_id_1")).Returns(model_1);
            _chartsHolder.Setup(x => x.Get("Chart_id_2")).Returns(model_2);

            IChartsPopulator populator = new ChartsPopulator(_configuration);
            populator.Populate();

            _chartProxy.Verify(x => x.Clear(model_1), Times.Once);
            _chartProxy.Verify(x => x.Clear(model_2), Times.Once);

            populator.Populate(model_1, 1, 2);
            _chartProxy.Verify(x => x.Clear(model_1), Times.Exactly(2));
            _chartProxy.Verify(x => x.Clear(model_2), Times.Once);

            populator.Populate(model_2, new DateTime(), new DateTime());
            _chartProxy.Verify(x => x.Clear(model_1), Times.Exactly(2));
            _chartProxy.Verify(x => x.Clear(model_2), Times.Exactly(2));
        }

        [Test]
        public void Chart_populate_will_call_chart_predicate_holder()
        {
            var chartPredicate = new CDPredicate { ChartId = "Chart_id_1" };
            var chartPredicate_2 = new EDPredicate { ChartId = "Chart_id_2" };
            _chartPredicatesHolder.Setup(x => x.GetAll()).Returns(new List<ChartPredicate>
            {
                chartPredicate,
                chartPredicate_2
            });

            IChartsPopulator populator = new ChartsPopulator(_configuration);
            populator.Populate();

            _chartPredicatesHolder.Verify(x => x.GetAll(), Times.Exactly(2));
            _chartsHolder.Verify(x => x.Get(chartPredicate.ChartId), Times.Exactly(2));
            _chartsHolder.Verify(x => x.Get(chartPredicate_2.ChartId), Times.Exactly(2));
        }

        [Test]
        public void Chart_populate_single_chart_date_time_call()
        {
            var chartPredicate = new CDPredicate { ChartId = "Chart_id_1" };
            var chartPredicate_2 = new EDPredicate { ChartId = "Chart_id_2" };
            var model = new DateChartViewModel(new TimeSpan());
            DateTime from = new DateTime(1983, 3, 5);
            DateTime to = new DateTime(1986, 5, 6);

            IEnumerable<string> chartIds = new List<string> { "Chart_id_1" };
            _chartsHolder.Setup(x => x.Get(model)).Returns(chartIds);
            _chartPredicatesHolder.Setup(x => x.GetByChartId("Chart_id_1")).Returns(new List<ChartPredicate> { chartPredicate, chartPredicate_2 });


            IChartsPopulator populator = new ChartsPopulator(_configuration);
            populator.Populate(model, from, to);

            _chartsHolder.Verify(x => x.Get(model), Times.Exactly(1));
            _chartPredicatesHolder.Verify(x => x.GetByChartId("Chart_id_1"), Times.Exactly(1));
            Assert.That(chartPredicate.From, Is.EqualTo(from));
            Assert.That(chartPredicate.To, Is.EqualTo(to));
            Assert.That(chartPredicate_2.From, Is.EqualTo(from));
            Assert.That(chartPredicate_2.To, Is.EqualTo(to));
        }

        [Test]
        public void Chart_populate_single_chart_index_call()
        {
            var chartPredicate = new CIPredicate { ChartId = "Chart_id_1" };
            var chartPredicate_2 = new EIPredicate { ChartId = "Chart_id_2" };
            var model = new DateChartViewModel(new TimeSpan());


            IEnumerable<string> chartIds = new List<string> { "Chart_id_1" };
            _chartsHolder.Setup(x => x.Get(model)).Returns(chartIds);
            _chartPredicatesHolder.Setup(x => x.GetByChartId("Chart_id_1")).Returns(new List<ChartPredicate> { chartPredicate, chartPredicate_2 });


            IChartsPopulator populator = new ChartsPopulator(_configuration);
            populator.Populate(model, 4, 8);

            _chartsHolder.Verify(x => x.Get(model), Times.Exactly(1));
            _chartPredicatesHolder.Verify(x => x.GetByChartId("Chart_id_1"), Times.Exactly(1));
            Assert.That(chartPredicate.From, Is.EqualTo(4));
            Assert.That(chartPredicate.To, Is.EqualTo(8));
            Assert.That(chartPredicate_2.From, Is.EqualTo(4));
            Assert.That(chartPredicate_2.To, Is.EqualTo(8));
        }


        [Test]
        public void When_try_tp_call_chart_populate_for_date_with_index_predicate()
        {
            var chartPredicate = new CDPredicate { ChartId = "Chart_id_1" };
            var model = new DateChartViewModel(new TimeSpan());
            _chartsHolder.Setup(x => x.Get(model)).Returns(new List<string> { "Chart_id_1" });
            _chartPredicatesHolder.Setup(x => x.GetByChartId("Chart_id_1")).Returns(new List<ChartPredicate> { chartPredicate });

            Assert.Throws<InvalidCastException>(() =>
            {
                IChartsPopulator populator = new ChartsPopulator(_configuration);
                populator.Populate(model, 4, 8);
            });
        }


        [Test]
        public void When_try_tp_call_chart_populate_for_index_with_date_predicate()
        {
            var chartPredicate = new CIPredicate { ChartId = "Chart_id_1" };
            var model = new DateChartViewModel(new TimeSpan());
            _chartsHolder.Setup(x => x.Get(model)).Returns(new List<string> { "Chart_id_1" });
            _chartPredicatesHolder.Setup(x => x.GetByChartId("Chart_id_1")).Returns(new List<ChartPredicate> { chartPredicate });

            Assert.Throws<InvalidCastException>(() =>
            {
                IChartsPopulator populator = new ChartsPopulator(_configuration);
                populator.Populate(model, new DateTime(), new DateTime());
            });
        }

        [Test]
        public void Data_will_pushed_to_chart_proxy()
        {
            IChartViewModel model = new DateChartViewModel(new TimeSpan());
            ChartsPopulator populator = new ChartsPopulator(_configuration);
            ChartPredicate chartPredicate = new CIPredicate { InstrumentId = "InstrumentId_1", CasType = typeof(double) };
            _chartDataProvider.Setup(x => x.GetCustomStorageData<double>(chartPredicate.InstrumentId)).Returns(GetCustomeDoubleData());
            populator.Populate(model, chartPredicate, 3, 9);
            _chartProxy.Verify(x => x.Push(model, new List<double> { 6, 7, 8, 9, 0, 1 }, chartPredicate), Times.Exactly(1));
        }

        [Test]
        public void Data_will_not_pushed_to_chart_proxy_when_use_wrong_cast_type()
        {
            IChartViewModel model = new DateChartViewModel(new TimeSpan());
            ChartsPopulator populator = new ChartsPopulator(_configuration);
            ChartPredicate chartPredicate = new CIPredicate { InstrumentId = "InstrumentId_1", CasType = typeof(int) };
            _chartDataProvider.Setup(x => x.GetCustomStorageData<double>(chartPredicate.InstrumentId)).Returns(GetCustomeDoubleData());
            populator.Populate(model, chartPredicate, 3, 9);
            _chartProxy.Verify(x => x.Push(It.IsAny<IChartViewModel>(), It.IsAny<List<double>>(), It.IsAny<ChartPredicate>()), Times.Exactly(0));
        }

        private IEnumerable<PanelViewPredicate> SetUpChartPredicate()
        {
            return new List<PanelViewPredicate>
            {
                 new PanelViewPredicate
                {
                    ChartPredicates = new List<ChartViewPredicate>
                    {
                        new DateChartViewPredicate
                        {
                           Ids = new List<string> { "RTS_5"},
                           XAxis = TimeSpan.FromSeconds(5),
                           YSize = 400
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "Custom_1"},
                           YSize = 300
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "Custom_2"},
                           YSize = 300
                        }
                    }
                }
            };
        }

        private IEnumerable<double> GetCustomeDoubleData()
        {
            return new List<double> { 3,4,5,6,7,8,9,0,1,2,3,4,5,6};
        }
    }
}
