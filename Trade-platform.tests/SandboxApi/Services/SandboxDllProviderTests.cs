using System;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Commons.Sistem;
using TradePlatform.SandboxApi.Services;

namespace Trade_platform.tests.SandboxApi.Services
{
    [TestFixture]
    public class SandboxDllProviderTests
    {
        [Test]
        public void TestGetListOfPresentersIfDirectoryIsNotExist()
        {
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            fileManager.Setup(x => x.IsDirectoryExist(It.IsAny<string>())).Returns(false);
            SandboxDllProvider provider = new SandboxDllProvider();
            Assert.Throws<Exception>(() =>
            {
                provider.Get();
            });
        }
    }
}
