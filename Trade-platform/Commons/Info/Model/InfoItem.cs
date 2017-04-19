using System;
using Prism.Mvvm;

namespace TradePlatform.Commons.Info
{
    public class InfoItem : BindableBase
    {
        public string TabId { get; private set; }
        public DateTime Date { get; private set; }
        public string Message { get; set; }

        public InfoItem (string tabId)
        {
            Date = DateTime.Now;
            TabId = tabId;
        }
    }
}
