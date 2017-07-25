using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Holders
{
    public interface ISandboxDataHolder
    {
        IList<Slice> Get();
        void Add(IList<Slice> data);
        void Clean ();
    }
}
