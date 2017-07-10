using System;
using System.Collections.Generic;
using Castle.Core;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Bots
{
    public interface IBot
    {
        string GetId();
        void SetUpId(string id);
        void SetUpData(IList<Pair<DateTime, IEnumerable<IData>>> data);
        void SetUpPredicate(BotPredicate predicate);
        void Execute();
        void Execution(IEnumerable<IData> slice);
        int Score();
    }
}
