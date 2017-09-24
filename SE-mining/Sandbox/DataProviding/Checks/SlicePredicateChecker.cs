using System;
using Microsoft.Practices.Unity;
using SEMining.Commons.Info;
using SE_mining_base.Info.Message;
using SE_mining_base.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox.DataProviding.Checks
{
    public class SlicePredicateChecker : IPredicateChecker
    {
        private readonly IInfoPublisher _infoPublisher;

        public SlicePredicateChecker()
        {
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
        }

        public bool Check(DataPredicate predicate)
        {
            bool isOk = true;
            if (String.IsNullOrEmpty(predicate.ParentId))
            {
                _infoPublisher.PublishInfo(new SandboxInfo{ Message = predicate + "- ParentId can not be null or empty" });
                isOk = false;
            }

            if (String.IsNullOrEmpty(predicate.Id))
            {
                _infoPublisher.PublishInfo(new SandboxInfo{ Message = predicate + "- Id can not be null or empty" });
                isOk = false;
            }
            return isOk;
        }

        public bool Check(IndicatorPredicate indicatorPredicate)
        {
            bool isOk = true;

            if (String.IsNullOrEmpty(indicatorPredicate.Id))
            {
                _infoPublisher.PublishInfo(new SandboxInfo { Message = indicatorPredicate + "- Id can not be null or empty" });
                isOk = false;
            }

            if (indicatorPredicate.Indicator == null)
            {
                _infoPublisher.PublishInfo(new SandboxInfo { Message = indicatorPredicate + "- Indicator can not be null" });
                isOk = false;
            }

            if (indicatorPredicate.DataPredicate == null)
            {
                _infoPublisher.PublishInfo(new SandboxInfo { Message = indicatorPredicate + "- DataPredicate can not be null" });
                isOk = false;
            }

            return isOk && Check(indicatorPredicate.DataPredicate);
        }
    }
}
