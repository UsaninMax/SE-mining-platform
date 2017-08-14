using System.Collections.Generic;

namespace TradePlatform.Sandbox.Holders
{
    public class CustomDataHolder : ICustomDataHolder
    {

        private IDictionary<string, IList<object>> _storage = new Dictionary<string, IList<object>>();

        public void Add(string key, IList<object> data)
        {
           if (_storage.ContainsKey(key))
            {
                _storage[key] = data;
                return;
            }
            _storage.Add(key, data);
        }

        public void CleanAll()
        {
            _storage.Clear();
        }

        public IList<object> Get(string key)
        {
            return _storage[key];
        }
    }
}
