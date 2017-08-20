using System.Collections.Generic;

namespace SEMining.Sandbox.Holders
{
    public interface ICustomDataHolder
    {
        IEnumerable<object> Get(string key);
        void Add(string key, IEnumerable<object> data);
        void CleanAll();
    }
}
