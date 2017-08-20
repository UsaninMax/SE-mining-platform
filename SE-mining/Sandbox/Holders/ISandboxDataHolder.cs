using System.Collections.Generic;
using SEMining.Sandbox.Models;

namespace SEMining.Sandbox.Holders
{
    public interface ISandboxDataHolder
    {
        IEnumerable<Slice> Get();
        void Add(IEnumerable<Slice> data);
        void Clean ();
    }
}
