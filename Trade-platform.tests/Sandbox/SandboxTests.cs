using System;
using System.Collections.Generic;
using System.Threading;
using Castle.Core;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;

namespace Trade_platform.tests.Sandbox
{
    [TestFixture]
    public class SandboxTests
    {

        [Test]
        public void Test_for_Build_Data()
        {
            var dataProviderMock = new Mock<IDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataProviderMock.Object);
            CancellationToken token = new CancellationToken();
            ICollection<IPredicate> predicates = new List<IPredicate>
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
            Assert.That(testSandBox.Data, Is.EqualTo(result));
        }

        [Test]
        public void Test_for_Build_Data_if_cancelation_requested()
        {
            var dataProviderMock = new Mock<IDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataProviderMock.Object);
            CancellationToken token = new CancellationToken(true);
            ICollection<IPredicate> predicates = new List<IPredicate>
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
            Assert.That(testSandBox.Data, Is.Null);
        }

        [Test]
        public void Test_Execute()
        {
            var dataProviderMock = new Mock<IDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataProviderMock.Object);
            CancellationToken token = new CancellationToken();
            ICollection<IPredicate> predicates = new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .NewId("RTS_1").Build()
            };

            var botMock_1 = new Mock<IBot>();
            var botMock_2 = new Mock<IBot>();

            ICollection<IBot> bots = new List<IBot>
            {
                botMock_1.Object,
                botMock_2.Object
            };

            var result = GetData();

            dataProviderMock.Setup(x => x.Get(predicates, token)).Returns(result);
            TestSandBox testSandBox = new TestSandBox
            {
                Predicates = predicates,
                Bots = bots
            };

            testSandBox.SetToken(token);
            testSandBox.BuildData();
            testSandBox.Execution();

            botMock_1.Verify(x => x.SetUpData(result), Times.Once);
            botMock_1.Verify(x => x.Execute(), Times.Once);

            botMock_2.Verify(x => x.SetUpData(result), Times.Once);
            botMock_2.Verify(x => x.Execute(), Times.Once);
        }

        private IList<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>> GetData()
        {
            return new List<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>>
            {
                new Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>(
                    DateTime.Now,

                    new List<IData>
                    {
                        new Candle.Builder().WithId("111").Build()
                    },

                    new List<Tick>())
            };
        }

    }

    class TestSandBox : SandboxApi
    {
        public ICollection<IPredicate> Predicates;
        public ICollection<IBot> Bots;

        public override ICollection<IPredicate> SetUpData()
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
            throw new System.NotImplementedException();
        }
    }
}
