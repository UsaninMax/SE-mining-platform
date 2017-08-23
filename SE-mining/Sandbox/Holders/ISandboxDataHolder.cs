using System.Collections.Generic;
using SE_mining_base.Sandbox.Models;

namespace SEMining.Sandbox.Holders
{
    public interface ISandboxDataHolder
    {
        IEnumerable<Slice> Get();
        void Add(IEnumerable<Slice> data);
        void Clean ();
    }
}
