using System.Collections.Generic;

namespace TradePlatform.Sandbox.Holders
{
    public interface ICustomDataHolder
    {
        IList<object> Get(string key);
        void Add(string key, IList<object> data);
        void CleanAll();
    }
}
