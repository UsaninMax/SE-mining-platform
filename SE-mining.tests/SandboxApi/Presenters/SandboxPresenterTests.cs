using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.SandboxApi;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Presenters;

namespace Trade_platform.tests.SandboxApi.Presenters
{
    [TestFixture]
    public class SandboxPresenterTests
    {

        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        //[Test]
        //public void TestExecute()
        //{
        //    var infoPublisher = new Mock<IInfoPublisher>();
        //    ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        //    var proxySandbox = new Mock<IProxySandbox>();
        //    ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
        //    SandboxPresenter presenter = new SandboxPresenter(new TestBox(), "test");
        //    presenter.Execute();
        //    Thread.Sleep(500);
        //    proxySandbox.Verify(x => x.PrepareData(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));
        //    proxySandbox.Verify(x => x.Execution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));
        //    proxySandbox.Verify(x => x.AfterExecution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));

        //    Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsDone));
        //}

        //[Test]
        //public void TestExecuteOnlyOneTime()
        //{
        //    var infoPublisher = new Mock<IInfoPublisher>();
        //    ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        //    var proxySandbox = new Mock<IProxySandbox>();
        //    ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
        //    SandboxPresenter presenter = new SandboxPresenter(new TestBox(), "test");
        //    presenter.Execute();
        //    presenter.Execute();
        //    Thread.Sleep(700);
        //    proxySandbox.Verify(x => x.PrepareData(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));
        //    proxySandbox.Verify(x => x.Execution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));
        //    proxySandbox.Verify(x => x.AfterExecution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));

        //    Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsDone));
        //}

        //[Test]
        //public void TestExecuteWithException()
        //{
        //    var infoPublisher = new Mock<IInfoPublisher>();
        //    ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        //    var proxySandbox = new Mock<IProxySandbox>();
        //    ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
        //    SandboxPresenter presenter = new SandboxPresenter(new TestBox(), "test");
        //    proxySandbox.Setup(x => x.PrepareData(It.IsAny<CancellationToken>())).Throws<Exception>();
        //    presenter.Execute();
        //    Thread.Sleep(500);
        //    proxySandbox.Verify(x => x.PrepareData(It.IsAny<CancellationToken>()),
        //        Times.Exactly(1));
        //    proxySandbox.Verify(x => x.Execution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(0));
        //    proxySandbox.Verify(x => x.AfterExecution(It.IsAny<CancellationToken>()),
        //        Times.Exactly(0));
        //    infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
        //    Assert.That(presenter.StatusMessage, Is.EqualTo(Status.FailToExecute));
        //}

        //[Test]
        //public void TestStopExecution()
        //{
        //    var infoPublisher = new Mock<IInfoPublisher>();
        //    ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        //    var proxySandbox = new Mock<IProxySandbox>();
        //    ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
        //    SandboxPresenter presenter = new SandboxPresenter(new TestBox(), "test");
        //    proxySandbox.Setup(x => x.PrepareData(It.IsAny<CancellationToken>())).Callback(() => Thread.Sleep(500));
        //    presenter.Execute();
        //    presenter.StopExecution();
        //    Thread.Sleep(1000);
        //    Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsCanceled));
        //}

        private class TestBox : Sandbox
        {
            public override ICollection<IPredicate> PrepareData()
            {
                throw new NotImplementedException();
            }

            public override void Execution()
            {
                throw new NotImplementedException();
            }

            public override void AfterExecution()
            {
                throw new NotImplementedException();
            }
        }
    }
}
