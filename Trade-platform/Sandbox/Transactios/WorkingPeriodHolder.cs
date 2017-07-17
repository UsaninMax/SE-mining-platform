using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class WorkingPeriodHolder : IWorkingPeriodHolder
    {
        private IDictionary<string, WorkingPeriod> _periods = new Dictionary<string, WorkingPeriod>();
        private readonly IDictionary<string, DateTime> _stored = new Dictionary<string, DateTime>();

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

        public void StorePoint(string instrumentId, DateTime date)
        {
            _stored.Add(instrumentId, date);
        }

        public bool IsStoredPoint(string instrumentId, DateTime date)
        {
            if (!_stored.ContainsKey(instrumentId))
            {
                return false;
            }

            return _stored[instrumentId] == date;
        }

        public void Reset()
        {
            _stored.Clear();
        }
    }
}
