using System;
using System.Collections.Generic;
using SEMining.Sandbox.Presenters;

namespace SEMining.Sandbox.Providers
{
    public interface ISandboxProvider
    {
        IEnumerable<ISandboxPresenter> Get();
        ISandbox CreateInstance(Type type);
    }
}
