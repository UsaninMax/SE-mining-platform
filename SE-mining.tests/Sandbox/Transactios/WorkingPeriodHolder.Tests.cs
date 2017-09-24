using System.Collections.Generic;
using NUnit.Framework;
using SEMining.Sandbox.Transactios;
using SE_mining_base.Transactios.Models;

namespace SEMining.tests.Sandbox.Transactios
{
    [TestFixture]
    public class WorkingPeriodHolderTests
    {

        [Test]
        public void Check_if_Working_period_not_exist()
        {
            IWorkingPeriodHolder workingPeriodHolder = new WorkingPeriodHolder();
            Assert.That(workingPeriodHolder.Get("Test"), Is.Null);
        }

        [Test]
        public void Check_if_Working_period_exist()
        {
            IWorkingPeriodHolder workingPeriodHolder = new WorkingPeriodHolder();

            WorkingPeriod workingPeriod = new WorkingPeriod();
            workingPeriodHolder.SetUp(new Dictionary<string, WorkingPeriod>
            {
                {"Test",workingPeriod  }
            });
            Assert.That(workingPeriodHolder.Get("Test"), Is.EqualTo(workingPeriod));
        }
    }
}
