using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Holders
{
    public interface ISandboxDataHolder
    {
        IEnumerable<Slice> Get();
        void Add(IEnumerable<Slice> data);
        void Clean ();
    }
}
