using Prism.Events;
using SEMining.DataSet.Presenters;

namespace SEMining.DataSet.Events
{
    public class RemovePresenterFromListEvent : PubSubEvent<IDataSetPresenter>
    {
    }
}
