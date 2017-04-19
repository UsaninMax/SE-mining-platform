using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace TradePlatform.Commons.Info
{
    public class InfoTab : BindableBase
    {
        public string Header { get; set; }
        private ObservableCollection<InfoItem> _messages = new ObservableCollection<InfoItem>();
        public ObservableCollection<InfoItem> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged();
            }
        }
    }
}
