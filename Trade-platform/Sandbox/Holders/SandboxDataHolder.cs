using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.Holders
{
    public class SandboxDataHolder : ISandboxDataHolder
    {
        private IList<Slice> _dataStorage = new List<Slice>();

        public void Add(IList<Slice> data)
        {
            _dataStorage = data;
        }

        public void Clean()
        {
            _dataStorage.Clear();
        }

        public IList<Slice> Get()
        {
            return _dataStorage;
        }
    }
}
