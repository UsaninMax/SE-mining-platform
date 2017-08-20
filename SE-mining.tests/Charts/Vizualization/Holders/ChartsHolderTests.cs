using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.Holders;
using SEMining.Charts.Vizualization.ViewModels;

namespace SEMining.tests.Charts.Vizualization.Holders
{
    [TestFixture]
    public class ChartsHolderTests
    {
        private IChartsHolder _holder;
        private IChartViewModel _model;

        [SetUp]
        public void SetUp()
        {
            _holder = new ChartsHolder();
            _model = new DateChartViewModel(new TimeSpan());
            _holder.Set(new Dictionary<string, IChartViewModel>
            {
                { "code_1", _model},
                { "code_2", new IndexChartViewModel()},
                { "code_3", _model},
                { "code_4", new IndexChartViewModel()},
                { "code_5", new DateChartViewModel(new TimeSpan())},
                { "code_6", _model}
            });
        }

        [Test]
        public void holder_keep_models_test()
        {
            Assert.That(_holder.GetAll().Count(), Is.EqualTo(6));
        }

        [Test]
        public void exist_by_id_test()
        {
            Assert.That(_holder.Get("code_3"), Is.EqualTo(_model));
        }

        [Test]
        public void get_ids_by_model_test()
        {
            Assert.That(_holder.Get(_model), Is.EqualTo(new List<string> { "code_1", "code_3", "code_6" }));
        }

        [Test]
        public void find_by_predicate_test()
        {
            Assert.That(_holder.Get(new IndexChartViewPredicate()
            {
                Ids = new List<string> { "code_1", "code_2", "code_5" }
            }), Is.EqualTo(_model));
        }

        [Test]
        public void find_by_predicate_test_2()
        {
            Assert.Throws<Exception>(() =>
            {
                _holder.Get(new IndexChartViewPredicate());
            });
        }
    }
}
