using System.Threading;

namespace TradePlatform.SandboxApi
{
    public interface ISandbox
    {
        void Before(CancellationToken token);
        void Execution(CancellationToken token);
        void After(CancellationToken token);
    }
}
