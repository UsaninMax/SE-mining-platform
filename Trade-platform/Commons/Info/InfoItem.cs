using System;
using Prism.Mvvm;

namespace TradePlatform.Commons.Info
{
    public class InfoItem : BindableBase
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }

    }
}
