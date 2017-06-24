using System.Threading;

namespace TradePlatform.SandboxApi
{
    public interface IProxySandbox
    {
        void Before(CancellationToken token);
        void Execution(CancellationToken token);
        void After(CancellationToken token);
    }
}
