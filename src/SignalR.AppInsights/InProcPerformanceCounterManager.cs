using System.Threading;

using Microsoft.AspNet.SignalR.Infrastructure;

namespace SignalR.AppInsights
{
    public class InProcPerformanceCounterManager : IPerformanceCounterManager
    {
        public IPerformanceCounter ConnectionsConnected { get; } = new NumberOfItemsPerformanceCounter("Connections Connected");

        public IPerformanceCounter ConnectionsReconnected { get; } = new NumberOfItemsPerformanceCounter("Connections Reconnected");

        public IPerformanceCounter ConnectionsDisconnected { get; } = new NumberOfItemsPerformanceCounter("Connections Disconnected");

        public IPerformanceCounter ConnectionsCurrentForeverFrame { get; } = new NumberOfItemsPerformanceCounter("Connections Current ForeverFrame");

        public IPerformanceCounter ConnectionsCurrentLongPolling { get; } = new NumberOfItemsPerformanceCounter("Connections Current LongPolling");

        public IPerformanceCounter ConnectionsCurrentServerSentEvents { get; } = new NumberOfItemsPerformanceCounter("Connections Current ServerSentEvents");

        public IPerformanceCounter ConnectionsCurrentWebSockets { get; } = new NumberOfItemsPerformanceCounter("Connections Current WebSockets");

        public IPerformanceCounter ConnectionsCurrent { get; } = new NumberOfItemsPerformanceCounter("Connections Current");

        public IPerformanceCounter ConnectionMessagesReceivedTotal { get; } = new NumberOfItemsPerformanceCounter("Connection Messages Received Total");

        public IPerformanceCounter ConnectionMessagesSentTotal { get; } = new NumberOfItemsPerformanceCounter("Connection Messages Sent Total");

        public IPerformanceCounter ConnectionMessagesReceivedPerSec { get; } = new RateOfCountsPerformanceCounter("Connection Messages Received/Sec");

        public IPerformanceCounter ConnectionMessagesSentPerSec { get; } = new RateOfCountsPerformanceCounter("Connection Messages Sent/Sec");

        public IPerformanceCounter MessageBusMessagesReceivedTotal { get; } = new NumberOfItemsPerformanceCounter("Message Bus Messages Received Total");

        public IPerformanceCounter MessageBusMessagesReceivedPerSec { get; } = new RateOfCountsPerformanceCounter("Message Bus Messages Received/Sec");

        public IPerformanceCounter ScaleoutMessageBusMessagesReceivedPerSec { get; } = new RateOfCountsPerformanceCounter("Scaleout Message Bus Messages Received/Sec");

        public IPerformanceCounter MessageBusMessagesPublishedTotal { get; } = new NumberOfItemsPerformanceCounter("Message Bus Messages Published Total");

        public IPerformanceCounter MessageBusMessagesPublishedPerSec { get; } = new RateOfCountsPerformanceCounter("Message Bus Messages Published/Sec");

        public IPerformanceCounter MessageBusSubscribersCurrent { get; } = new NumberOfItemsPerformanceCounter("Message Bus Subscribers Current");

        public IPerformanceCounter MessageBusSubscribersTotal { get; } = new NumberOfItemsPerformanceCounter("Message Bus Subscribers Total");

        public IPerformanceCounter MessageBusSubscribersPerSec { get; } = new RateOfCountsPerformanceCounter("Message Bus Subscribers/Sec");

        public IPerformanceCounter MessageBusAllocatedWorkers { get; } = new NumberOfItemsPerformanceCounter("Message Bus Allocated Workers");

        public IPerformanceCounter MessageBusBusyWorkers { get; } = new NumberOfItemsPerformanceCounter("Message Bus Busy Workers");

        public IPerformanceCounter MessageBusTopicsCurrent { get; } = new NumberOfItemsPerformanceCounter("Message Bus Topics Current");

        public IPerformanceCounter ErrorsAllTotal { get; } = new NumberOfItemsPerformanceCounter("Errors: All Total");

        public IPerformanceCounter ErrorsAllPerSec { get; } = new RateOfCountsPerformanceCounter("Errors: All/Sec");

        public IPerformanceCounter ErrorsHubResolutionTotal { get; } = new NumberOfItemsPerformanceCounter("Errors: Hub Resolution Total");

        public IPerformanceCounter ErrorsHubResolutionPerSec { get; } = new RateOfCountsPerformanceCounter("Errors: Hub Resolution/Sec");

        public IPerformanceCounter ErrorsHubInvocationTotal { get; } = new NumberOfItemsPerformanceCounter("Errors: Hub Invocation Total");

        public IPerformanceCounter ErrorsHubInvocationPerSec { get; } = new RateOfCountsPerformanceCounter("Errors: Hub Invocation/Sec");

        public IPerformanceCounter ErrorsTransportTotal { get; } = new NumberOfItemsPerformanceCounter("Errors: Tranport Total");

        public IPerformanceCounter ErrorsTransportPerSec { get; } = new RateOfCountsPerformanceCounter("Errors: Transport/Sec");

        public IPerformanceCounter ScaleoutStreamCountTotal { get; } = new NumberOfItemsPerformanceCounter("Scaleout Streams Total");

        public IPerformanceCounter ScaleoutStreamCountOpen { get; } = new NumberOfItemsPerformanceCounter("Scaleout Streams Open");

        public IPerformanceCounter ScaleoutStreamCountBuffering { get; } = new NumberOfItemsPerformanceCounter("Scaleout Streams Buffering");

        public IPerformanceCounter ScaleoutErrorsTotal { get; } = new NumberOfItemsPerformanceCounter("Scaleout Errors Total");

        public IPerformanceCounter ScaleoutErrorsPerSec { get; } = new RateOfCountsPerformanceCounter("Scaleout Errors/Sec");

        public IPerformanceCounter ScaleoutSendQueueLength { get; } = new NumberOfItemsPerformanceCounter("Scaleout Send Queue Length");

        public void Initialize(string instanceName, CancellationToken hostShutdownToken)
        {
            _timer.Change(1000, Timeout.Infinite);
        }

        public IPerformanceCounter LoadCounter(string categoryName, string counterName, string instanceName, bool isReadOnly)
        {
            return null;
        }

        private static readonly Timer _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);

        private static void TimerCallback(object state)
        {
            foreach (var counter in RateOfCountsPerformanceCounter.Counters)
            {
                counter.RawValue = 0;
            }

            _timer.Change(1000, Timeout.Infinite);
        }
    }
}