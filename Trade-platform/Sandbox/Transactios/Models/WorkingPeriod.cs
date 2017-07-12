using System;

namespace TradePlatform.Sandbox.Transactios.Models
{
    public class WorkingPeriod
    {
        public string InstrumentId { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
    }
}
