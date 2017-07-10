using System.Collections.Generic;
using System.Threading;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TradePlatform.Sandbox
{
    public interface ISandbox
    {
        void SetToken(CancellationToken token);
        ICollection<IPredicate> SetUpData();
        void SetUpBots(ICollection<IBot> bots);
        void BuildData();
        void Execution();
        void AfterExecution();
        void CleanMemory();
    }
}
