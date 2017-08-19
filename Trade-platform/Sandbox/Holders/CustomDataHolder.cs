using System.Collections.Generic;

namespace TradePlatform.Sandbox.Holders
{
    public class CustomDataHolder : ICustomDataHolder
    {

        private IDictionary<string, IEnumerable<object>> _storage = new Dictionary<string, IEnumerable<object>>();

        public void Add(string key, IEnumerable<object> data)
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

        public IEnumerable<object> Get(string key)
        {
            return _storage[key];
        }
    }
}
