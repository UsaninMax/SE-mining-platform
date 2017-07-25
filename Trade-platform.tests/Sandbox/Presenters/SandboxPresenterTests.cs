using System;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.Presenters;
using TradePlatform.Sandbox.Providers;

namespace Trade_platform.tests.Sandbox.Presenters
{
    [TestFixture]
    public class SandboxPresenterTests
    {

        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Test]
        public void TestExecute()
        {
            var sandboxBuilderMock = new Mock<ISandboxProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxBuilderMock.Object);
            var dataDrovider = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataDrovider.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var proxySandbox = new Mock<ISandbox>();
            ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
            sandboxBuilderMock.Setup(x => x.CreateInstance(It.IsAny<Type>())).Returns(proxySandbox.Object);
            proxySandbox.Setup(x => x.BuildData());
            proxySandbox.Setup(x => x.Execution());
            proxySandbox.Setup(x => x.AfterExecution());
            SandboxPresenter presenter = new SandboxPresenter(proxySandbox.Object, "test");
            presenter.Execute();
            Thread.Sleep(500);
            proxySandbox.Verify(x => x.BuildData(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.Execution(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.AfterExecution(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.CleanMemory(),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsDone));
        }

        [Test]
        public void TestExecuteOnlyOneTime()
        {
            var sandboxBuilderMock = new Mock<ISandboxProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxBuilderMock.Object);
            var dataDrovider = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataDrovider.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var proxySandbox = new Mock<ISandbox>();
            ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
            sandboxBuilderMock.Setup(x => x.CreateInstance(It.IsAny<Type>())).Returns(proxySandbox.Object);
            proxySandbox.Setup(x => x.Execution()).Callback(() => Thread.Sleep(200));
            SandboxPresenter presenter = new SandboxPresenter(proxySandbox.Object, "test");
            presenter.Execute();
            presenter.Execute();
            Thread.Sleep(300);
            proxySandbox.Verify(x => x.BuildData(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.Execution(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.AfterExecution(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.CleanMemory(),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsDone));
        }

        [Test]
        public void TestExecuteWithException()
        {
            var sandboxBuilderMock = new Mock<ISandboxProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxBuilderMock.Object);
            var dataDrovider = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataDrovider.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var proxySandbox = new Mock<ISandbox>();
            ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
            sandboxBuilderMock.Setup(x => x.CreateInstance(It.IsAny<Type>())).Returns(proxySandbox.Object);
            SandboxPresenter presenter = new SandboxPresenter(proxySandbox.Object, "test");
            proxySandbox.Setup(x => x.BuildData()).Throws<Exception>();
            presenter.Execute();
            Thread.Sleep(500);
            proxySandbox.Verify(x => x.BuildData(),
                Times.Exactly(1));
            proxySandbox.Verify(x => x.Execution(),
                Times.Exactly(0));
            proxySandbox.Verify(x => x.AfterExecution(),
                Times.Exactly(0));
            proxySandbox.Verify(x => x.CleanMemory(),
                Times.Exactly(0));
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.FailToExecute));
        }

        [Test]
        public void TestStopExecution()
        {
            var sandboxBuilderMock = new Mock<ISandboxProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxBuilderMock.Object);
            var dataDrovider = new Mock<ISandboxDataProvider>();
            ContainerBuilder.Container.RegisterInstance(dataDrovider.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var proxySandbox = new Mock<ISandbox>();
            ContainerBuilder.Container.RegisterInstance(proxySandbox.Object);
            sandboxBuilderMock.Setup(x => x.CreateInstance(It.IsAny<Type>())).Returns(proxySandbox.Object);
            SandboxPresenter presenter = new SandboxPresenter(proxySandbox.Object, "test");
            proxySandbox.Setup(x => x.BuildData()).Callback(() => Thread.Sleep(500));
            presenter.Execute();
            presenter.StopExecution();
            Thread.Sleep(2000);
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsCanceled));
        }
    }
}
