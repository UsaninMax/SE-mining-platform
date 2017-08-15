using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Models;

namespace TestSandboxModule
{
    public class MA : IIndicatorProvider
    {
        private List<double> _values;
        private double _sum;
        private int _length;

        private IDictionary<string, object> _parameters;

        public void SetUpParameters(IDictionary<string, object> parameters)
        {
            _parameters = parameters;
        }

        public void Initialize()
        {

            _length = Convert.ToInt32(_parameters["length"]);
            if (_length <= 0)
            {
                throw new Exception("length must be greater than zero");
            }

            _values = new List<double>(_length);
        }

        public double Get(Candle candle)
        {
            if(_values.Count == _length && _length > 0)
            {
                _sum -= _values.First();
                _values.RemoveAt(0);
            }
            _sum += candle.Close;
            _values.Add(candle.Close);
            return _sum / _values.Count;
        }
    }
}
