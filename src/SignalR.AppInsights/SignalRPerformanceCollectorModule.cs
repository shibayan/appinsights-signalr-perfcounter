using System;
using System.Threading;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace SignalR.AppInsights
{
    public class SignalRPerformanceCollectorModule : ITelemetryModule
    {
        private readonly object _lockObject = new object();
        private readonly TimeSpan _period = TimeSpan.FromSeconds(60);

        private bool _isInitialized;
        private Timer _timer;
        private IPerformanceCounterManager _counters;
        private TelemetryClient _client;

        public void Initialize(TelemetryConfiguration configuration)
        {
            if (!_isInitialized)
            {
                lock (_lockObject)
                {
                    if (!_isInitialized)
                    {
                        _counters = new InProcPerformanceCounterManager();

                        GlobalHost.DependencyResolver.Register(typeof(IPerformanceCounterManager), () => _counters);

                        _client = new TelemetryClient(configuration);

                        _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
                        _timer.Change((int)_period.TotalMilliseconds, Timeout.Infinite);
                        _isInitialized = true;
                    }
                }
            }
        }

        private void TimerCallback(object state)
        {
            foreach (var propertyInfo in typeof(IPerformanceCounterManager).GetProperties())
            {
                var counter = (IPerformanceCounter)propertyInfo.GetValue(_counters);

                var metricTelemetry = CreateMetricTelemetry(counter.CounterName, counter.RawValue);

                _client.TrackMetric(metricTelemetry);
            }

            _timer.Change((int)_period.TotalMilliseconds, Timeout.Infinite);
        }

        private MetricTelemetry CreateMetricTelemetry(string counterName, long value)
        {
            var metricTelemetry = new MetricTelemetry
            {
                Name = counterName,
                Count = 1,
                Sum = value,
                Min = value,
                Max = value,
                StandardDeviation = 0
            };

            metricTelemetry.Properties.Add("CustomPerfCounter", "true");

            return metricTelemetry;
        }
    }
}