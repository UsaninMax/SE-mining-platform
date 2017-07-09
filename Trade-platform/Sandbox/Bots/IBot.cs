using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Bots
{
    public interface IBot
    {
        string GetId();
        void SetUpId(string id);
        void SetUpData(IList<IData> data);
        void SetUpPredicate(BotPredicate predicate);
        void Execute();
        void Execution(Slice slice);
        int Score();
    }
}
