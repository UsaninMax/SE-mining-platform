using SE_mining_base.Info.Message;

namespace SEMining.Commons.Info.Model
{
    public interface IInfoTab
    {
        string TabId();
        void Add(InfoItem item);
        void Close();
        int MessageCount();
    }
}
