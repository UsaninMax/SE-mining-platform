using System.Threading;
using TradePlatform.SandboxApi;

namespace TestSandboxModule
{
    public class TesteSandbpx : ISandbox
    {
        public void Before(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                
            }
        }

        public void Execution(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public void After(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
