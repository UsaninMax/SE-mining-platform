using NUnit.Framework;
using SEMining.Charts.Vizualization.ViewModels;
using Moq;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using SEMining.Charts.Vizualization.Configurations;
using System;
using SEMining.Charts.Vizualization.Dispatching;

namespace SEMining.tests.Charts.Vizualization.Dispatching
{
    [TestFixture]
    public class ChartsConfigurationDispatcherTests
    {
        private IChartViewModel _dateChartViewModel;
        private IChartViewModel _indexChartViewModel;

        [SetUp]
        public void SetUp()
        {
            _dateChartViewModel = new Mock<IChartViewModel>().Object;
            _indexChartViewModel = new Mock<IChartViewModel>().Object;
            ContainerBuilder.Container.RegisterInstance("DateChartViewModel", _dateChartViewModel);
            ContainerBuilder.Container.RegisterInstance("IndexChartViewModel", _indexChartViewModel);
        }

        [Test]
        public void check_dispatch_test_1()
        {
            IChartsConfigurationDispatcher dispatcher = new ChartsConfigurationDispatcher();
            IDictionary<string, IChartViewModel> result = dispatcher.Dispatch(new List<PanelViewPredicate> {
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
            });
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.True(result.ContainsKey("RTS_5"));
            Assert.True(result.ContainsKey("Custom_1"));
            Assert.True(result.ContainsKey("Custom_2"));
            Assert.That(result["RTS_5"], Is.EqualTo(_dateChartViewModel));
            Assert.That(result["Custom_1"], Is.EqualTo(_indexChartViewModel));
            Assert.That(result["Custom_2"], Is.EqualTo(_indexChartViewModel));
        }

        [Test]
        public void check_dispatch_test_2()
        {
            IChartsConfigurationDispatcher dispatcher = new ChartsConfigurationDispatcher();
            IDictionary<string, IChartViewModel> result = dispatcher.Dispatch(new List<PanelViewPredicate> {
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
                           Ids = new List<string> { "Custom_1" , "Custom_2"},
                           YSize = 300
                        }
                    }
                }
            });
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.True(result.ContainsKey("RTS_5"));
            Assert.True(result.ContainsKey("Custom_1"));
            Assert.True(result.ContainsKey("Custom_2"));
            Assert.That(result["RTS_5"], Is.EqualTo(_dateChartViewModel));
            Assert.That(result["Custom_1"], Is.EqualTo(_indexChartViewModel));
            Assert.That(result["Custom_2"], Is.EqualTo(_indexChartViewModel));
        }


        [Test]
        public void check_dispatch_test_3()
        {
            IChartsConfigurationDispatcher dispatcher = new ChartsConfigurationDispatcher();
            IDictionary<string, IChartViewModel> result = dispatcher.Dispatch(new List<PanelViewPredicate> {
                new PanelViewPredicate
                {
                    ChartPredicates = new List<ChartViewPredicate>
                    {
                        new DateChartViewPredicate
                        {
                           Ids = new List<string> { "RTS_5"}
                        },
                        new IndexChartViewPredicate
                        {
                           Ids = new List<string> { "Custom_1" , "Custom_2"}
                        }
                    }
                }
            });
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.True(result.ContainsKey("RTS_5"));
            Assert.True(result.ContainsKey("Custom_1"));
            Assert.True(result.ContainsKey("Custom_2"));
            Assert.That(result["RTS_5"], Is.EqualTo(_dateChartViewModel));
            Assert.That(result["Custom_1"], Is.EqualTo(_indexChartViewModel));
            Assert.That(result["Custom_2"], Is.EqualTo(_indexChartViewModel));
        }

        [Test]
        public void check_dispatch_test_4()
        {
            IChartsConfigurationDispatcher dispatcher = new ChartsConfigurationDispatcher();
            IDictionary<string, IChartViewModel> result = dispatcher.Dispatch(new List<PanelViewPredicate> {
                new PanelViewPredicate
                {
                    ChartPredicates = new List<ChartViewPredicate>
                    {
                        new DateChartViewPredicate
                        {
                           Ids = new List<string> { "RTS_5"}
                        },
                        new IndexChartViewPredicate()
                    }
                }
            });
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.True(result.ContainsKey("RTS_5")); ;
            Assert.That(result["RTS_5"], Is.EqualTo(_dateChartViewModel));
        }
    }
}
