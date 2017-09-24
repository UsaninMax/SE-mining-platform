using System;
using System.Collections.Generic;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public class WorkingPeriodHolder : IWorkingPeriodHolder
    {
        private IDictionary<string, WorkingPeriod> _periods = new Dictionary<string, WorkingPeriod>();

        public WorkingPeriod Get(string instrumentId)
        {
            if (!_periods.ContainsKey(instrumentId))
            {
                return null;
            }
            return _periods[instrumentId];
        }

        public void SetUp(IDictionary<string, WorkingPeriod> periods)
        {
            _periods = periods;
        }
    }
}
