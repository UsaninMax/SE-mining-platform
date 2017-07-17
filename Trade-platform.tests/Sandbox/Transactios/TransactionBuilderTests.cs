using System;
using NUnit.Framework;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace Trade_platform.tests.Sandbox.Transactios
{
    [TestFixture]
    public class TransactionBuilderTests
    {
        [Test]
        public void Check_build_transaction()
        {
            ITransactionBuilder builder = new TransactionBuilder();
            OpenPositionRequest request = new OpenPositionRequest
                .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(15)
                .Build();
            Tick tick = new Tick
                .Builder()
                .WithDate(new DateTime(2017, 10, 9))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(40)
                .Build();

            Transaction transaction = builder.Build(request, tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(request.RemainingNumber));
            Assert.That(transaction.Date, Is.EqualTo(tick.Date()));
            Assert.That(transaction.Direction, Is.EqualTo(request.Direction));
            Assert.That(transaction.Number, Is.EqualTo(request.RemainingNumber));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));
        }

        [Test]
        public void Check_build_transaction_when_volume_is_empty()
        {
            ITransactionBuilder builder = new TransactionBuilder();
            OpenPositionRequest request = new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(15)
                .Build();
            Tick tick = new Tick
                    .Builder()
                .WithDate(new DateTime(2017, 10, 9))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(0)
                .Build();

            Transaction transaction = builder.Build(request, tick);

            Assert.That(transaction, Is.Null);
        }

        [Test]
        public void Check_build_transaction_when_remain_volume_is_not_enough()
        {
            ITransactionBuilder builder = new TransactionBuilder();
            OpenPositionRequest request = new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(15)
                .Build();
            Tick tick = new Tick
                    .Builder()
                .WithDate(new DateTime(2017, 10, 9))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(5)
                .Build();

            Transaction transaction = builder.Build(request, tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(2));
            Assert.That(transaction.Date, Is.EqualTo(tick.Date()));
            Assert.That(transaction.Direction, Is.EqualTo(request.Direction));
            Assert.That(transaction.Number, Is.EqualTo(2));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));
        }

        [Test]
        public void Check_build_chain_of_transactions_on_one_tick()
        {
            ITransactionBuilder builder = new TransactionBuilder();
            Tick tick = new Tick
                    .Builder()
                .WithDate(new DateTime(2017, 10, 9))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(50)
                .Build();

            Transaction transaction = builder.Build(new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(15)
                .Build(), tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(15));
            Assert.That(transaction.Number, Is.EqualTo(15));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));

            transaction = builder.Build(new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(50)
                .Build(), tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(17));
            Assert.That(transaction.Number, Is.EqualTo(17));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));
        }


        [Test]
        public void Check_build_chain_of_transactions_on_chain_of_ticks()
        {
            ITransactionBuilder builder = new TransactionBuilder();
            Tick tick = new Tick
                    .Builder()
                .WithDate(new DateTime(2017, 10, 9))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(50)
                .Build();

            Transaction transaction = builder.Build(new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(15)
                .Build(), tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(15));
            Assert.That(transaction.Number, Is.EqualTo(15));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));

            tick = new Tick
                    .Builder()
                .WithDate(new DateTime(2017, 10, 10))
                .WithId("Test_id")
                .WithPrice(140)
                .WithVolume(50)
                .Build();


            transaction = builder.Build(new OpenPositionRequest
                    .Builder()
                .InstrumentId("Test_id")
                .Direction(Direction.Buy)
                .Number(20)
                .Build(), tick);

            Assert.That(transaction.RemainingNumber, Is.EqualTo(20));
            Assert.That(transaction.Number, Is.EqualTo(20));
            Assert.That(transaction.ExecutedPrice, Is.EqualTo(tick.Price));
        }
    }
}
