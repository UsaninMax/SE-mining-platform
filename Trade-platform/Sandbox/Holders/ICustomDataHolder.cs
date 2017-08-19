using System.Collections.Generic;

namespace TradePlatform.Sandbox.Holders
{
    public interface ICustomDataHolder
    {
        IEnumerable<object> Get(string key);
        void Add(string key, IEnumerable<object> data);
        void CleanAll();
    }
}
