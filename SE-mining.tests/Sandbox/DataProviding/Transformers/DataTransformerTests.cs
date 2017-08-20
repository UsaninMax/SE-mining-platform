using System;
using System.Collections.Generic;
using NUnit.Framework;
using SEMining.Sandbox.DataProviding.Predicates;
using SEMining.Sandbox.DataProviding.Transformers;
using SEMining.Sandbox.Models;
using SEMining.StockData.Models;

namespace SEMining.tests.Sandbox.DataProviding.Transformers
{
    [TestFixture]
    public class DataTransformerTests
    {

        [Test]
        public void TestTransformTiks()
        {
            TickPredicate predicate = new TickPredicate.Builder().NewId("Test_id").Build();
            DataTransformer transformer = new DataTransformer();
            List<DataTick> dataTicks = GetDataTicks();
            List<Tick> tiks = transformer.Transform(dataTicks, predicate);
            Assert.That(tiks.Count, Is.EqualTo(dataTicks.Count));

            for (int i = 0; i < dataTicks.Count; i++)
            {
                Assert.That(tiks[i].Price, Is.EqualTo(dataTicks[i].Price));
                Assert.That(tiks[i].Volume, Is.EqualTo(dataTicks[i].Volume));
                Assert.That(tiks[i].Date(), Is.EqualTo(dataTicks[i].Date));
                Assert.That(tiks[i].Id(), Is.EqualTo(predicate.Id));
            }
        }

        [Test]
        public void TestTransformTiksWithInterval()
        {
            TickPredicate predicate = new TickPredicate.Builder()
                .NewId("Test_id")
                .From(new DateTime(2016, 9, 18, 1, 1, 1))
                .To(new DateTime(2016, 9, 20, 1, 1, 1))
                .Build();
            DataTransformer transformer = new DataTransformer();
            List<DataTick> dataTicks = GetDataTicks();
            List<Tick> tiks = transformer.Transform(dataTicks, predicate);
            Assert.That(tiks.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestTransformToCandles()
        {
            DataPredicate predicate = new DataPredicate
                .Builder()
                .NewId("test_2")
                .AccumulationPeriod(new TimeSpan(0, 3, 0))
                .Build();
            DataTransformer transformer = new DataTransformer();
            IEnumerable<Tick> tiks = GetTicks();
            List<Candle> candles = transformer.Transform(tiks, predicate);

            Assert.That(candles.Count, Is.EqualTo(4));
            Assert.That(candles[0].Date(), Is.EqualTo(new DateTime(2017, 7, 17, 13, 45, 00)));
            Assert.That(candles[0].Open, Is.EqualTo(8));
            Assert.That(candles[0].Low, Is.EqualTo(8));
            Assert.That(candles[0].High, Is.EqualTo(9));
            Assert.That(candles[0].Close, Is.EqualTo(9));
            Assert.That(candles[0].Volume, Is.EqualTo(15));
            Assert.That(candles[0].Id(), Is.EqualTo(predicate.Id));

            Assert.That(candles[1].Date(), Is.EqualTo(new DateTime(2017, 7, 17, 13, 48, 00)));
            Assert.That(candles[1].Open, Is.EqualTo(11));
            Assert.That(candles[1].Low, Is.EqualTo(5));
            Assert.That(candles[1].High, Is.EqualTo(18));
            Assert.That(candles[1].Close, Is.EqualTo(16));
            Assert.That(candles[1].Volume, Is.EqualTo(24));
            Assert.That(candles[1].Id(), Is.EqualTo(predicate.Id));
        }

        [Test]
        public void TestTransformToCandlesWithInterval()
        {
            DataPredicate predicate = new DataPredicate
                    .Builder()
                .NewId("test_2")
                .AccumulationPeriod(new TimeSpan(0, 3, 0))
                .From(new DateTime(2017, 7, 17, 13, 45, 32))
                .To(new DateTime(2017, 7, 17, 13, 49, 13))
                .Build();
            DataTransformer transformer = new DataTransformer();
            IEnumerable<Tick> tiks = GetTicks();
            List<Candle> candles = transformer.Transform(tiks, predicate);
            Assert.That(candles.Count, Is.EqualTo(2));
        }


        private List<DataTick> GetDataTicks()
        {
            return new List<DataTick>
            {
                new DataTick
                {
                    Date = new DateTime(2016, 9, 17, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                },
                new DataTick
                {
                    Date = new DateTime(2016, 9, 17, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                },
                new DataTick
                {
                    Date = new DateTime(2016, 9, 18, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                },
                new DataTick
                {
                    Date = new DateTime(2016, 9, 19, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                },
                new DataTick
                {
                    Date = new DateTime(2016, 9, 20, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                },
                new DataTick
                {
                    Date = new DateTime(2016, 9, 21, 1, 1, 1),
                    Price = 1,
                    Volume = 2
                }
            };

        }
        private List<Tick> GetTicks()
        {
            return new List<Tick>
            {
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 43, 4))
                    .WithId("test_1")
                    .WithPrice(8)
                    .WithVolume(14)
                    .Build(),
               new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 44, 4))
                    .WithId("test_1")
                    .WithPrice(9)
                    .WithVolume(1)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 45, 32))
                    .WithId("test_1")
                    .WithPrice(11)
                    .WithVolume(2)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 45, 35))
                    .WithId("test_1")
                    .WithPrice(5)
                    .WithVolume(11)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 46, 33))
                    .WithId("test_1")
                    .WithPrice(12)
                    .WithVolume(3)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 47, 21))
                    .WithId("test_1")
                    .WithPrice(18)
                    .WithVolume(4)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 47, 32))
                    .WithId("test_1")
                    .WithPrice(16)
                    .WithVolume(4)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 48, 32))
                    .WithId("test_1")
                    .WithPrice(14)
                    .WithVolume(5)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 49, 13))
                    .WithId("test_1")
                    .WithPrice(15)
                    .WithVolume(6)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 50, 21))
                    .WithId("test_1")
                    .WithPrice(16)
                    .WithVolume(7)
                    .Build(),
                new Tick.Builder()
                    .WithDate(new DateTime(2017, 7, 17, 13, 51, 12))
                    .WithId("test_1")
                    .WithPrice(17)
                    .WithVolume(8)
                    .Build()
            };
        }
    }
}
