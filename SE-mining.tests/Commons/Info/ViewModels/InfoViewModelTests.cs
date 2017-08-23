using System.Threading;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;
using SEMining.Commons.Info.Events;
using SEMining.Commons.Info.Model;
using SEMining.Commons.Info.ViewModels;
using SE_mining_base.Info.Message;

namespace SEMining.tests.Commons.Info.ViewModels
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
            _eventAggregator.GetEvent<PuplishInfo>().Publish(new DownloadInfo { Message = "first" });
            _eventAggregator.GetEvent<PuplishInfo>().Publish(new DownloadInfo { Message = "second" });
            DoEvents();
            Thread.Sleep(500);
            Assert.That(model.Tabs.Count, Is.EqualTo(1));
            Assert.That(model.Tabs[0].MessageCount(), Is.EqualTo(2));
        }

        [Test]
        public void CloseInfoTab()
        {

            InfoViewModel model = new InfoViewModel();
            _eventAggregator.GetEvent<PuplishInfo>().Publish(new DownloadInfo { Message = "first" });
            _eventAggregator.GetEvent<PuplishInfo>().Publish(new DownloadInfo { Message = "second" });
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
