using System;
using System.Collections.Generic;
using NUnit.Framework;
using SEMining.Sandbox.Transactios;
using System.Linq;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace SEMining.tests.Sandbox.Transactios
{
    [TestFixture]
    public class BalanceTests
    {
        [Test]
        public void Check_set_up_balance()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(300);
            Assert.That(balance.GetTotal(), Is.EqualTo(300));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(300));
        }

        [Test]
        public void Check_Add_Transaction_Cost()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(300);
            balance.AddTransactionCost(10, DateTime.MinValue, new Guid());
            Assert.That(balance.GetTotal(), Is.EqualTo(290));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(2));
            Assert.That(balance.GetHistory().ToList()[1].TransactionCost, Is.EqualTo(-10));
            Assert.That(balance.GetHistory().ToList()[1].Total, Is.EqualTo(290));
        }

        [Test]
        public void Check_reset()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(300);
            balance.AddTransactionCost(10, DateTime.MinValue, new Guid());
            Assert.That(balance.GetTotal(), Is.EqualTo(290));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(2));
            Assert.That(balance.GetHistory().ToList()[1].TransactionCost, Is.EqualTo(-10));
            Assert.That(balance.GetHistory().ToList()[1].Total, Is.EqualTo(290));
            balance.Reset();
            Assert.That(balance.GetTotal(), Is.EqualTo(300));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(300));

        }

        [Test]
        public void Check_calculate_margine_when_open_transaction_not_exist()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(300);
            Assert.That(balance.GetTotal(), Is.EqualTo(300));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(300));

            Transaction transaction = new Transaction.Builder()
                .InstrumentId("test_1")
                .Direction(Direction.Buy)
                .ExecutedPrice(120)
                .Number(12)
                .WithDate(new DateTime())
                .Build();


            balance.AddTransactionMargin(transaction, new List<Transaction>(), DateTime.MinValue);

            Assert.That(balance.GetTotal(), Is.EqualTo(300));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(300));
        }

        [Test]
        public void Check_calculate_margine_when_open_transaction_exist()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(600);
            Assert.That(balance.GetTotal(), Is.EqualTo(600));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(600));

            Transaction transaction = new Transaction.Builder()
                .InstrumentId("test_1")
                .Direction(Direction.Sell)
                .ExecutedPrice(90)
                .Number(20)
                .WithDate(new DateTime())
                .Build();

            IEnumerable<Transaction> transactions = new List<Transaction>
            {
                new Transaction.Builder()
                    .InstrumentId("test_1")
                    .Direction(Direction.Buy)
                    .ExecutedPrice(160)
                    .Number(4)
                    .WithDate(new DateTime())
                    .Build(),
                new Transaction.Builder()
                    .InstrumentId("test_1")
                    .Direction(Direction.Buy)
                    .ExecutedPrice(40)
                    .Number(13)
                    .WithDate(new DateTime())
                    .Build()
            };


            balance.AddTransactionMargin(transaction, transactions, DateTime.MinValue);
            Assert.That(balance.GetTotal(), Is.EqualTo(970));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(2));
            Assert.That(balance.GetHistory().ToList()[1].TransactionMargin, Is.EqualTo(370));
        }

        [Test]
        public void Check_calculate_margine_when_open_transaction_exist_2()
        {
            IBalanceHolder balance = new BalanceHolder();
            balance.AddMoney(600);
            Assert.That(balance.GetTotal(), Is.EqualTo(600));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(1));
            Assert.That(balance.GetHistory().ToList()[0].Total, Is.EqualTo(600));

            Transaction transaction = new Transaction.Builder()
                .InstrumentId("test_1")
                .Direction(Direction.Sell)
                .ExecutedPrice(90)
                .Number(15)
                .WithDate(new DateTime())
                .Build();

            IEnumerable<Transaction> transactions = new List<Transaction>
            {
                new Transaction.Builder()
                    .InstrumentId("test_1")
                    .Direction(Direction.Buy)
                    .ExecutedPrice(160)
                    .Number(4)
                    .WithDate(new DateTime())
                    .Build(),
                new Transaction.Builder()
                    .InstrumentId("test_1")
                    .Direction(Direction.Buy)
                    .ExecutedPrice(40)
                    .Number(13)
                    .WithDate(new DateTime())
                    .Build()
            };


            balance.AddTransactionMargin(transaction, transactions, DateTime.MinValue);
            Assert.That(balance.GetTotal(), Is.EqualTo(870));
            Assert.That(balance.GetHistory().Count, Is.EqualTo(2));
            Assert.That(balance.GetHistory().ToList()[1].TransactionMargin, Is.EqualTo(270));
        }
    }
}
