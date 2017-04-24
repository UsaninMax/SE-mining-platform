using System.Threading;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;
using TradePlatform;
using TradePlatform.Commons.Info.MessageEvents;
using TradePlatform.Commons.Info.Model;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Info.ViewModels;

namespace Trade_platform.tests.Commons.Info.ViewModels
{
    [TestFixture]
    public class InfoViewModelTests
    {
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void SetUp()
        {
            _eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(_eventAggregator);
        }

        [Test]
        public void PublishInfo()
        {

            InfoViewModel model = new InfoViewModel();
            _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(new DownloadInfo { Message = "first" });
            _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(new DownloadInfo { Message = "second" });
            DoEvents();
            Thread.Sleep(500);
            Assert.That(model.Tabs.Count, Is.EqualTo(1));
            Assert.That(model.Tabs[0].MessageCount(), Is.EqualTo(2));
        }

        [Test]
        public void CloseInfoTab()
        {

            InfoViewModel model = new InfoViewModel();
            _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(new DownloadInfo { Message = "first" });
            _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(new DownloadInfo { Message = "second" });
            DoEvents();
            Thread.Sleep(500);
            Assert.That(model.Tabs.Count, Is.EqualTo(1));
            Assert.That(model.Tabs[0].MessageCount(), Is.EqualTo(2));
            IInfoTab tab = model.Tabs[0];
            tab.Close();
            DoEvents();
            Thread.Sleep(500);
            Assert.That(model.Tabs.Count, Is.EqualTo(0));
        }

        private static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
