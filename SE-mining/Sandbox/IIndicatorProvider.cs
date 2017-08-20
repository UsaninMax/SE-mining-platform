using System.Collections.Generic;
using SEMining.Sandbox.Models;

namespace SEMining.Sandbox
{
    public interface IIndicatorProvider
    {
        void SetUpParameters(IDictionary<string, object> parameters);
        void Initialize();
        double Get(Candle candle);
    }
}
