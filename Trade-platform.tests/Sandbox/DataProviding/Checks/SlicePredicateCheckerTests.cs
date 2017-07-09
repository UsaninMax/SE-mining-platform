using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Sandbox.DataProviding.Checks;
using TradePlatform.Sandbox.DataProviding.Predicates;

namespace Trade_platform.tests.Sandbox.DataProviding.Checks
{
    [TestFixture]
    public class SlicePredicateCheckerTests
    {

        [Test]
        public void Test_slice_checker_if_data_predicatre_empty()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            DataPredicate dataPredicate = new DataPredicate.Builder().Build();
            IPredicateChecker checker = new SlicePredicateChecker();
            Assert.That(checker.Check(dataPredicate), Is.False);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<SandboxInfo>()), Times.Exactly(2));
        }

        [Test]
        public void Test_slice_checker_if_indicator_predicatre_empty()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IndicatorPredicate predicate = new IndicatorPredicate.Builder().Build();
            IPredicateChecker checker = new SlicePredicateChecker();
            Assert.That(checker.Check(predicate), Is.False);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<SandboxInfo>()), Times.Exactly(3));
        }

        [Test]
        public void Test_slice_checker_if_indicator_predicatre_empty_data_predicate_is_not()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            DataPredicate dataPredicate = new DataPredicate.Builder().ParentId("12").NewId("22").Build();
            IndicatorPredicate predicate = new IndicatorPredicate.Builder().DataPredicate(dataPredicate).Build();
            IPredicateChecker checker = new SlicePredicateChecker();
            Assert.That(checker.Check(predicate), Is.False);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<SandboxInfo>()), Times.Exactly(2));
        }

        [Test]
        public void Test_slice_checker_if_indicator_predicate_filled()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            DataPredicate dataPredicate = new DataPredicate.Builder().ParentId("12").NewId("22").Build();
            IndicatorPredicate predicate = new IndicatorPredicate.Builder().NewId("23")
                .Indicator(typeof(SlicePredicateCheckerTests)).DataPredicate(dataPredicate).Build();
            IPredicateChecker checker = new SlicePredicateChecker();
            Assert.That(checker.Check(predicate), Is.True);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<SandboxInfo>()), Times.Never);
        }
    }
}
