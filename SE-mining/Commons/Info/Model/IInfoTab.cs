using SEMining.Commons.Info.Model.Message;

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
