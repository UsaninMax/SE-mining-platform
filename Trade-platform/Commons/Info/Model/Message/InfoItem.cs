using System;
using Prism.Mvvm;

namespace TradePlatform.Commons.Info.Model.Message
{
    public class InfoItem : BindableBase
    {
        public string TabId { get; private set; }
        public DateTime Date { get; private set; }
        public string Message { get; set; }

        protected InfoItem (string tabId)
        {
            Date = DateTime.Now;
            TabId = tabId;
        }
    }
}
