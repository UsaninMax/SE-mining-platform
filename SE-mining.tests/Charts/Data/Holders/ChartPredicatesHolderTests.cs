using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SEMining.Charts.Data.Holders;
using SE_mining_base.Charts.Data.Predicates;
using SE_mining_base.Charts.Data.Predicates.Basis;

namespace SEMining.tests.Charts.Data.Holders
{
    [TestFixture]
    public class ChartPredicatesHolderTests
    {

        [Test]
        public void will_store_predicate_without_dublicates()
        {
            ChartPredicate predicate = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(predicate);
            holder.Add(predicate);

            Assert.That(holder.GetAll().Count(), Is.EqualTo(1));
        }

        [Test]
        public void will_store_predicate_without_dublicates_2()
        {
            ChartPredicate predicate_1 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId",
                From = DateTime.Now
            };

            ChartPredicate predicate_2 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(predicate_1);
            holder.Add(predicate_2);

            Assert.That(holder.GetAll().Count(), Is.EqualTo(1));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(1));
            Assert.That(holder.GetByChartId("ChartId").Cast<EDPredicate>().First().From, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void will_store_predicate_without_dublicates_3()
        {
            ChartPredicate predicate_1 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId",
                From = DateTime.Now
            };

            ChartPredicate predicate_2 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId2"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(predicate_1);
            holder.Add(predicate_2);

            Assert.That(holder.GetAll().Count(), Is.EqualTo(2));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(2));
        }

        [Test]
        public void check_update_by_list()
        {
            ChartPredicate predicate_1 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId",
                From = DateTime.Now
            };

            ChartPredicate predicate_2 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId2"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(new List<ChartPredicate>
            {
                predicate_1,
                predicate_2
            });

            Assert.That(holder.GetAll().Count(), Is.EqualTo(2));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(2));
            ChartPredicate predicate_3 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId1",
                From = DateTime.Now
            };

            ChartPredicate predicate_4 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId3"
            };

            holder.Add(new List<ChartPredicate>
            {
                predicate_3,
                predicate_4
            });
            Assert.That(holder.GetAll().Count(), Is.EqualTo(4));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(4));
        }

        [Test]
        public void check_remove()
        {
            ChartPredicate predicate_1 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId",
                From = DateTime.Now
            };

            ChartPredicate predicate_2 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId2"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(new List<ChartPredicate>
            {
                predicate_1,
                predicate_2
            });

            Assert.That(holder.GetAll().Count(), Is.EqualTo(2));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(2));
            ChartPredicate predicate_3 = new EIPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId2"
            };

            holder.Remove(predicate_3);
            Assert.That(holder.GetAll().Count(), Is.EqualTo(1));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(1));
        }

        [Test]
        public void check_clean_storage()
        {
            ChartPredicate predicate_1 = new CDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId",
                From = DateTime.Now
            };

            ChartPredicate predicate_2 = new EDPredicate()
            {
                ChartId = "ChartId",
                InstrumentId = "InstrumentId2"
            };
            IChartPredicatesHolder holder = new ChartPredicatesHolder();
            holder.Add(new List<ChartPredicate>
            {
                predicate_1,
                predicate_2
            });

            Assert.That(holder.GetAll().Count(), Is.EqualTo(2));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(2));

            holder.Reset();
            Assert.That(holder.GetAll().Count(), Is.EqualTo(0));
            Assert.That(holder.GetByChartId("ChartId").Count, Is.EqualTo(0));
        }
    }
}
