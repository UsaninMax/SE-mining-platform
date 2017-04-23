using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TradePlatform.Commons.Utils;

namespace Trade_platform.tests.Commons.Utils
{
    [TestFixture]
    public class FixedSizeObservableCollectionTests
    {
        [Test]
        public void CheckFixedSize()
        {
            ObservableCollection<int> col = new FixedSizeObservableCollection<int>(10);

            for (int i = 0; i < 20; i++)
            {
                col.Add(i);
            }
            
            Assert.That(col.Count, Is.EqualTo(11));
            int res = col[10];
            Assert.That(res, Is.EqualTo(19));
        }

        [Test]
        public void CheckFixedSizeIfOutOfBorder()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ObservableCollection<int> col = new FixedSizeObservableCollection<int>(10);
                for (int i = 0; i < 20; i++)
                {
                    col.Add(i);
                }

                int res = col[11];
            });
        }
    }
}
