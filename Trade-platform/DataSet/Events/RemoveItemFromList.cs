using Prism.Events;
using TradePlatform.DataSet.Presenters;

namespace TradePlatform.DataSet.Events
{
    public class RemovePresenterFromListEvent : PubSubEvent<IDataSetPresenter>
    {
    }
}
