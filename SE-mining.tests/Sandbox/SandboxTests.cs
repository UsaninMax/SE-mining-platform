using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using SEMining.Sandbox;
using SEMining.Sandbox.Bots;
using SEMining.Sandbox.DataProviding;
using SEMining.Sandbox.Holders;
using SEMining.Charts.Data.Populating;
using SE_mining_base.Charts.Vizualization.Configurations;
using SE_mining_base.Sandbox.DataProviding.Predicates;
using SE_mining_base.Sandbox.Models;

namespace SEMining.tests.Sandbox
{
    [TestFixture]
    public class SandboxTests
    {

        [Test]
        public void Test_for_Build_Data()
        {
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var dataProviderMock = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataProviderMock.Object);
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            CancellationToken token = new CancellationToken();
            IEnumerable<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            var result = GetData();

            dataProviderMock.Setup(x => x.Get(predicates, token)).Returns(result);
            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates
            };

            testSandBox.SetToken(token);
            testSandBox.BuildData();
            sandboxDataHolder.Verify(x => x.Add(result), Times.Once);
        }

        [Test]
        public void Test_for_Build_Data_if_cancelation_requested()
        {
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            var dataProviderMock = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataProviderMock.Object);
            var sandboxDataHolder = new Mock<ISandboxDataHolder>();
            ContainerBuilder.Container.RegisterInstance(sandboxDataHolder.Object);
            IEnumerable<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates
            };

            testSandBox.SetToken(new CancellationToken(true));
            testSandBox.BuildData();

            dataProviderMock.Verify(x => x.Get(It.IsAny<IEnumerable<IPredicate>>(), It.IsAny<CancellationToken>()), Times.Never);
            sandboxDataHolder.Verify(x => x.Add(It.IsAny<IEnumerable<Slice>>()), Times.Never);

        }

        [Test]
        public void Test_Execute()
        {
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            CancellationToken token = new CancellationToken();
            IEnumerable<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            var botMock_1 = new Mock<IBot>();
            var botMock_2 = new Mock<IBot>();

            botMock_1.Setup(x => x.IsPrepared()).Returns(true);
            botMock_2.Setup(x => x.IsPrepared()).Returns(true);
            IEnumerable<IBot> bots = new List<IBot>
            {
                botMock_1.Object,
                botMock_2.Object
            };

          
            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates,
                Bots = bots
            };

            testSandBox.SetToken(token);
            testSandBox.Execution();

            botMock_1.Verify(x => x.Execute(), Times.Once);
            botMock_1.Verify(x => x.ResetTransactionContext(), Times.Once);
            botMock_2.Verify(x => x.Execute(), Times.Once);
            botMock_2.Verify(x => x.ResetTransactionContext(), Times.Once);
        }

        [Test]
        public void Test_Execute_when_cancelation_is_active()
        {
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            CancellationToken token = new CancellationToken(true);
            IEnumerable<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            var botMock_1 = new Mock<IBot>();
            var botMock_2 = new Mock<IBot>();

            botMock_1.Setup(x => x.IsPrepared()).Returns(true);
            botMock_2.Setup(x => x.IsPrepared()).Returns(true);
            IEnumerable<IBot> bots = new List<IBot>
            {
                botMock_1.Object,
                botMock_2.Object
            };


            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates,
                Bots = bots
            };

            testSandBox.SetToken(token);
            testSandBox.Execution();

            botMock_1.Verify(x => x.Execute(), Times.Never);
            botMock_1.Verify(x => x.ResetTransactionContext(), Times.Never);
            botMock_2.Verify(x => x.Execute(), Times.Never);
            botMock_2.Verify(x => x.ResetTransactionContext(), Times.Never);
        }

        [Test]
        public void Test_Execute_when_bots_are_not_prepared()
        {
            var chartsPopulator = new Mock<IChartsPopulator>();
            ContainerBuilder.Container.RegisterInstance(chartsPopulator.Object);
            CancellationToken token = new CancellationToken();
            IEnumerable<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            var botMock_1 = new Mock<IBot>();
            var botMock_2 = new Mock<IBot>();

            botMock_1.Setup(x => x.IsPrepared()).Returns(false);
            botMock_2.Setup(x => x.IsPrepared()).Returns(false);
            IEnumerable<IBot> bots = new List<IBot>
            {
                botMock_1.Object,
                botMock_2.Object
            };


            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates,
                Bots = bots
            };

            testSandBox.SetToken(token);

            Assert.Throws<Exception>(() =>
            {
                testSandBox.Execution();
            });
        }

        private IEnumerable<Slice> GetData()
        {
            IDictionary<string, IData> datas = new Dictionary<string, IData>();
            datas.Add("111", new Candle.Builder().WithId("111").Build());

            return new List<Slice>
            {
                new Slice.Builder()
                .WithDate(DateTime.Now)
                .WithData(datas)
                .WithTick(new Dictionary<string, Tick>())
                .Build()
            };
        }
    }

    class TestSandBox : SandboxAbstraction
    {
        public IEnumerable<IPredicate> Predicates;
        public IEnumerable<IBot> Bots;

        public override IEnumerable<IPredicate> SetUpData()
        {
            return Predicates;
        }

        public override void Execution()
        {
            SetUpBots(Bots);
            Execute();
        }

        public override void AfterExecution()
        {
 
        }

        public override IEnumerable<PanelViewPredicate> SetUpCharts()
        {
            return new List<PanelViewPredicate>();
        }
    }
}
