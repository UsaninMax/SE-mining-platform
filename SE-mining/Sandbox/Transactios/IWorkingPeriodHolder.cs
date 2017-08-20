using System;
using System.Collections.Generic;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface IWorkingPeriodHolder
    {
        WorkingPeriod Get(string instrumentId);
        void SetUp(IDictionary<string, WorkingPeriod> periods);
        void StorePoint(string instrumentId, DateTime date);
        bool IsStoredPoint(string instrumentId, DateTime date);
        void Reset();
    }
}
