using System.Threading;

namespace TradePlatform.SandboxApi
{
    public class ProxySandbox : IProxySandbox
    {
        private readonly ISandbox _sandbox;

        public ProxySandbox(ISandbox sandbox)
        {
            _sandbox = sandbox;
        }

        public void Before(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            _sandbox.Before(token);
        }

        public void Execution(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            _sandbox.Execution(token);
        }

        public void After(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            _sandbox.After(token);
        }
    }
}
