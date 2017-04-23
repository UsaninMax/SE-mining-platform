using System.Collections.ObjectModel;

namespace TradePlatform.Commons.Utils
{
    public class FixedSizeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly int _maxSize;
        public FixedSizeObservableCollection(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override void InsertItem(int index, T item)
        {
            if (Count > _maxSize)
            {
                base.RemoveAt(0);
                base.InsertItem(_maxSize, item);
            }
            else
            {
                base.InsertItem(index, item);
            }
        }
    }
}
