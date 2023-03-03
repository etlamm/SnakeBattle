using System.Timers;
using Domain.Event;

namespace Domain
{
    internal class Stopwatch
    {
        private readonly IEventBus eventBus;
        private readonly System.Diagnostics.Stopwatch stopwatch = new();
        private Timer timer;

        public Stopwatch(IEventBus eventBus)
        {
            this.eventBus = eventBus;
            ResetTimer();
        }

        private void ResetTimer()
        {
            timer = new Timer(1000);
            timer.Elapsed += SendStopwatchUpdatedEvent;
            timer.AutoReset = true;
        }

        public bool IsRunning => stopwatch.IsRunning;
        public int TotalSecondsElapsed => (int)(stopwatch.ElapsedMilliseconds / 1000);

        public void Start()
        {
            stopwatch.Start();
            timer.Enabled = true;
        }

        private void SendStopwatchUpdatedEvent(object source, ElapsedEventArgs e)
        {
            eventBus.Send(StopwatchUpdated.Instance);
        }

        public void Stop()
        {
            stopwatch.Stop();
            timer.Enabled = false;
        }

        public void Reset()
        {
            stopwatch.Reset();
            ResetTimer();
            eventBus.Send(StopwatchUpdated.Instance);
        }
    }
}