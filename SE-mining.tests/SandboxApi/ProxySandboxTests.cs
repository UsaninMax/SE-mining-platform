using System.Threading;
using Moq;
using NUnit.Framework;
using TradePlatform.SandboxApi;

namespace Trade_platform.tests.SandboxApi
{
    [TestFixture]
    public class ProxySandboxTests
    {

        //[Test]
        //public void TestProxyMethods()
        //{
        //    var sanbox = new Mock<Sandbox>();
        //    IProxySandbox proxy = new ProxySandbox(sanbox.Object);
        //    CancellationToken token = new CancellationToken();
        //    proxy.PrepareData(token);
        //    proxy.Execution(token);
        //    proxy.AfterExecution(token);
        //    sanbox.Verify(x => x.PrepareData(token), Times.Once);
        //    sanbox.Verify(x => x.Execution(token), Times.Once);
        //    sanbox.Verify(x => x.AfterExecution(token), Times.Once);
        //}


        //[Test]
        //public void TestProxyMethodsWithCancelationToken()
        //{
        //    var sanbox = new Mock<Sandbox>();
        //    IProxySandbox proxy = new ProxySandbox(sanbox.Object);
        //    CancellationToken token = new CancellationToken(true);
        //    proxy.PrepareData(token);
        //    proxy.Execution(token);
        //    proxy.AfterExecution(token);
        //    sanbox.Verify(x => x.PrepareData(token), Times.Never);
        //    sanbox.Verify(x => x.Execution(token), Times.Never);
        //    sanbox.Verify(x => x.AfterExecution(token), Times.Never);
        //}
    }
}
