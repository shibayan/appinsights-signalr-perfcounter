using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Microsoft.AspNet.SignalR.Infrastructure;

namespace SignalR.AppInsights
{
    public class RateOfCountsPerformanceCounter : IPerformanceCounter
    {
        public RateOfCountsPerformanceCounter(string counterName)
        {
            CounterName = counterName;

            Counters.Add(this);
        }

        internal static List<RateOfCountsPerformanceCounter> Counters { get; } = new List<RateOfCountsPerformanceCounter>();

        private long _rawValue;

        public string CounterName { get; }

        public long RawValue
        {
            get => _rawValue;
            set => Interlocked.Exchange(ref _rawValue, value);
        }

        public void Close()
        {
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref _rawValue);
        }

        public long Increment()
        {
            return Interlocked.Increment(ref _rawValue);
        }

        public long IncrementBy(long value)
        {
            return Interlocked.Add(ref _rawValue, value);
        }

        public CounterSample NextSample()
        {
            return CounterSample.Empty;
        }

        public void RemoveInstance()
        {
        }
    }
}