using System;

namespace SE_mining_base.Info.Message
{
    public class InfoItem 
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
