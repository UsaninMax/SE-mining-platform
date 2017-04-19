using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace TradePlatform.Commons.Info.ViewModels
{
    public class InfoViewModel : BindableBase, IInfoViewModel
    {
        private ObservableCollection<InfoTab> _tabs = new ObservableCollection<InfoTab>();
        public ObservableCollection<InfoTab> Tabs
        {
            get
            {
                return _tabs;
            }
            set
            {
                _tabs = value;
                RaisePropertyChanged();
            }
        }

        public InfoViewModel()
        {
            Tabs.Add(new InfoTab {

                Header = "One",
                Messages = new ObservableCollection<InfoItem>()
            {
                new InfoItem() {Date = DateTime.Today, Message = "24werwerwrwer"},
                new InfoItem() {Date = DateTime.Today, Message = "ferfaff"},
                new InfoItem() {Date = DateTime.Today, Message = "rttryhrtyrty"}
            }
            });
            Tabs.Add(new InfoTab
            {
                Header = "Two",
                Messages = new ObservableCollection<InfoItem>()
                {
                    new InfoItem() {Date = DateTime.Today, Message = "retertert"},
                    new InfoItem() {Date = DateTime.Today, Message = "345345345345"},
                    new InfoItem() {Date = DateTime.Today, Message = "hfghfghfhfh"}
                }
            });
        }
    }
}
