using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Sistem;
using TradePlatform.Main.ViewModels;
using TradePlatform.SandboxApi;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Presenters;
using TradePlatform.SandboxApi.Services;

namespace Trade_platform.tests.Main.ViewModels
{
    [TestFixture]
    public class ShellModelTests
    {
        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }


        [Test]
        public void TestStartExecution()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var selectedSandboxPresenter = new Mock<ISandboxPresenter>();
            ShellModel model = new ShellModel();
            model.SelectedSandboxPresenter = selectedSandboxPresenter.Object;
            model.StartCommand.Execute(null);
            selectedSandboxPresenter.Verify(x => x.Execute(), Times.Once);
        }

        [Test]
        public void TestCancelExecution()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var selectedSandboxPresenter = new Mock<ISandboxPresenter>();
            ShellModel model = new ShellModel();
            model.SelectedSandboxPresenter = selectedSandboxPresenter.Object;
            model.CancelCommand.Execute(null);
            selectedSandboxPresenter.Verify(x => x.StopExecution(), Times.Once);

        }

        [Test]
        public void TestLoadWindow()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var sandboxDllProvider = new Mock<ISandboxDllProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxDllProvider.Object);
            ISandboxPresenter presenter = new SandboxPresenter(new TestBox(),"test");
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>
            {
                presenter
            };

            sandboxDllProvider.Setup(x => x.Get()).Returns(presenters);
            ShellModel model = new ShellModel();
            model.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            Assert.That(model.SandboxPresenterInfo.Count, Is.EqualTo(1));
            Assert.That(model.SandboxPresenterInfo[0], Is.EqualTo(presenter));
        }

        [Test]
        public void TestLoadWindowWhenError()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var sandboxDllProvider = new Mock<ISandboxDllProvider>();
            ContainerBuilder.Container.RegisterInstance(sandboxDllProvider.Object);
            ISandboxPresenter presenter = new SandboxPresenter(new TestBox(), "test");
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>
            {
                presenter
            };

            sandboxDllProvider.Setup(x => x.Get()).Throws<Exception>();
            ShellModel model = new ShellModel();
            model.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            Assert.That(model.SandboxPresenterInfo.Count, Is.EqualTo(0));
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
        }

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
