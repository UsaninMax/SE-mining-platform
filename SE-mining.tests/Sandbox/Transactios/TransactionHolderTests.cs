using System.Collections.Generic;
using NUnit.Framework;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios;
using SEMining.Sandbox.Transactios.Enums;
using SEMining.Sandbox.Transactios.Models;
using System.Linq;

namespace SEMining.tests.Sandbox.Transactios
{
    [TestFixture]
    public class TransactionHolderTests
    {
        [Test]
        public void Check_add_transaction()
        {
            ITransactionHolder holder = new TransactionHolder(new Dictionary<string, BrokerCost>
                {
                    { "test_id", new BrokerCost{Coverage = 0.5, TransactionCost = 10}}
                }
            );

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(170)
                .Number(15)
                .Build());

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Sell)
                .ExecutedPrice(110)
                .Number(10)
                .Build());
            Assert.That(holder.GetOpenTransactions().Count, Is.EqualTo(2));
            Assert.That(holder.GetOpenTransactions().ToList()[0].RemainingNumber, Is.EqualTo(5));
            Assert.That(holder.GetOpenTransactions().ToList()[0].Direction, Is.EqualTo(Direction.Buy));
            Assert.That(holder.GetOpenTransactions().ToList()[1].RemainingNumber, Is.EqualTo(15));
            Assert.That(holder.GetOpenTransactions().ToList()[1].Direction, Is.EqualTo(Direction.Buy));

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Sell)
                .ExecutedPrice(50)
                .Number(60)
                .Build());
            Assert.That(holder.GetOpenTransactions().Count, Is.EqualTo(1));
            Assert.That(holder.GetOpenTransactions().ToList()[0].RemainingNumber, Is.EqualTo(40));
            Assert.That(holder.GetOpenTransactions().ToList()[0].Direction, Is.EqualTo(Direction.Sell));
        }


        [Test]
        public void Check_add_transaction_multiply_id()
        {
            ITransactionHolder holder = new TransactionHolder(new Dictionary<string, BrokerCost>
                {
                    { "test_id", new BrokerCost{Coverage = 0.5, TransactionCost = 10}},
                    { "test_id_2", new BrokerCost{ Coverage = 0.15, TransactionCost = 3}}
                }
            );

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_2")
                .Direction(Direction.Buy)
                .ExecutedPrice(170)
                .Number(15)
                .Build());

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Sell)
                .ExecutedPrice(110)
                .Number(10)
                .Build());
            Assert.That(holder.GetOpenTransactions().Count, Is.EqualTo(2));
            Assert.That(holder.GetOpenTransactions().ToList()[0].RemainingNumber, Is.EqualTo(5));
            Assert.That(holder.GetOpenTransactions().ToList()[0].Direction, Is.EqualTo(Direction.Buy));
            Assert.That(holder.GetOpenTransactions().ToList()[1].RemainingNumber, Is.EqualTo(15));
            Assert.That(holder.GetOpenTransactions().ToList()[1].Direction, Is.EqualTo(Direction.Buy));

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Sell)
                .ExecutedPrice(50)
                .Number(60)
                .Build());
            Assert.That(holder.GetOpenTransactions().Count, Is.EqualTo(2));
            Assert.That(holder.GetOpenTransactions().ToList()[0].InstrumentId, Is.EqualTo("test_id_2"));
            Assert.That(holder.GetOpenTransactions().ToList()[0].RemainingNumber, Is.EqualTo(15));
            Assert.That(holder.GetOpenTransactions().ToList()[0].Direction, Is.EqualTo(Direction.Buy));
            Assert.That(holder.GetOpenTransactions().ToList()[1].InstrumentId, Is.EqualTo("test_id"));
            Assert.That(holder.GetOpenTransactions().ToList()[1].RemainingNumber, Is.EqualTo(55));
            Assert.That(holder.GetOpenTransactions().ToList()[1].Direction, Is.EqualTo(Direction.Sell));

            Assert.That(holder.GetInvertedOpenTransactions("test_id_2", Direction.Sell).Count, Is.EqualTo(1));
        }

        [Test]
        public void Check_coverage()
        {
            ITransactionHolder holder = new TransactionHolder(new Dictionary<string, BrokerCost>
                {
                    { "test_id", new BrokerCost{Coverage = 0.5, TransactionCost = 10}},
                    { "test_id_2", new BrokerCost{ Coverage = 0.15, TransactionCost = 3}},
                    { "test_id_3", new BrokerCost{Coverage = 0.20, TransactionCost = 20}}
                }
            );

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_2")
                .Direction(Direction.Buy)
                .ExecutedPrice(170)
                .Number(15)
                .Build());

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_3")
                .Direction(Direction.Sell)
                .ExecutedPrice(110)
                .Number(10)
                .Build());

            IEnumerable<OpenPositionRequest> activeRequests = new List<OpenPositionRequest>
            {
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id")
                    .Direction(Direction.Sell)
                    .Number(10)
                    .Build(),
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id")
                    .Direction(Direction.Sell)
                    .Number(30)
                    .Build(),
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id_3")
                    .Direction(Direction.Buy)
                    .Number(15)
                    .Build()
            };

            IDictionary<string, Tick> ticks = new Dictionary<string, Tick>
            {
                { "test_id", new Tick.Builder().WithId("test_id").WithPrice(30).Build()},
                { "test_id_2", new Tick.Builder().WithId("test_id_2").WithPrice(40).Build()},
                { "test_id_3", new Tick.Builder().WithId("test_id_3").WithPrice(50).Build()}
            };
            Assert.That(holder.GetCoverage(ticks, activeRequests), Is.EqualTo(515));
        }

        [Test]
        public void Check_coverage_with_additional_request()
        {
            ITransactionHolder holder = new TransactionHolder(new Dictionary<string, BrokerCost>
                {
                    { "test_id", new BrokerCost{Coverage = 0.5, TransactionCost = 10}},
                    { "test_id_2", new BrokerCost{Coverage = 0.15, TransactionCost = 3}},
                    { "test_id_3", new BrokerCost{Coverage = 0.20, TransactionCost = 20}}
                }
            );

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_2")
                .Direction(Direction.Buy)
                .ExecutedPrice(170)
                .Number(15)
                .Build());

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_3")
                .Direction(Direction.Sell)
                .ExecutedPrice(110)
                .Number(10)
                .Build());

            IEnumerable<OpenPositionRequest> activeRequests = new List<OpenPositionRequest>
            {
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id")
                    .Direction(Direction.Sell)
                    .Number(10)
                    .Build(),
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id")
                    .Direction(Direction.Sell)
                    .Number(30)
                    .Build(),
                new OpenPositionRequest
                        .Builder()
                    .InstrumentId("test_id_3")
                    .Direction(Direction.Buy)
                    .Number(15)
                    .Build()
            };

            IDictionary<string, Tick> ticks = new Dictionary<string, Tick>
            {
                { "test_id", new Tick.Builder().WithId("test_id").WithPrice(30).Build()},
                { "test_id_2", new Tick.Builder().WithId("test_id_2").WithPrice(40).Build()},
                { "test_id_3", new Tick.Builder().WithId("test_id_3").WithPrice(50).Build()}
            };
            Assert.That(holder.GetCoverage(ticks, activeRequests, new OpenPositionRequest
                    .Builder()
                .InstrumentId("test_id_3")
                .Direction(Direction.Buy)
                .Number(15)
                .Build()), Is.EqualTo(665));
        }

        [Test]
        public void Check_transaction_count_for_instrument()
        {
            ITransactionHolder holder = new TransactionHolder(null);

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Buy)
                .ExecutedPrice(150)
                .Number(15)
                .Build());
            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id_2")
                .Direction(Direction.Buy)
                .ExecutedPrice(170)
                .Number(15)
                .Build());

            holder.UpdateOpenTransactions(new Transaction.Builder()
                .InstrumentId("test_id")
                .Direction(Direction.Sell)
                .ExecutedPrice(110)
                .Number(10)
                .Build());

            Assert.That(holder.GetNumberOfOpenTransactions("test_id"), Is.EqualTo(35));
            Assert.That(holder.GetNumberOfOpenTransactions("test_id_2"), Is.EqualTo(15));
        }
    }
}
