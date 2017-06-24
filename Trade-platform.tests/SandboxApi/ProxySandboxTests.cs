using System.Threading;
using Moq;
using NUnit.Framework;
using TradePlatform.SandboxApi;

namespace Trade_platform.tests.SandboxApi
{
    [TestFixture]
    public class ProxySandboxTests
    {

        [Test]
        public void TestProxyMethods()
        {
            var sanbox = new Mock<ISandbox>();
            IProxySandbox proxy = new ProxySandbox(sanbox.Object);
            CancellationToken token = new CancellationToken();
            proxy.Before(token);
            proxy.Execution(token);
            proxy.After(token);
            sanbox.Verify(x => x.Before(token), Times.Once);
            sanbox.Verify(x => x.Execution(token), Times.Once);
            sanbox.Verify(x => x.After(token), Times.Once);
        }


        [Test]
        public void TestProxyMethodsWithCancelationToken()
        {
            var sanbox = new Mock<ISandbox>();
            IProxySandbox proxy = new ProxySandbox(sanbox.Object);
            CancellationToken token = new CancellationToken(true);
            proxy.Before(token);
            proxy.Execution(token);
            proxy.After(token);
            sanbox.Verify(x => x.Before(token), Times.Never);
            sanbox.Verify(x => x.Execution(token), Times.Never);
            sanbox.Verify(x => x.After(token), Times.Never);
        }
    }
}
