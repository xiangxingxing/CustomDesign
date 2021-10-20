using System;

namespace MetronomeDemo.Metronome
{
    public interface ITimer
    {
        event EventHandler TimerTick;
        void StartTimer();
        void StopTimer();
        TimeSpan Interval { get; set; }
    }

    public class TimerWin32Adapted : ITimer
    {
        public event EventHandler TimerTick;

        private readonly TimerWin32Wrapper _timer;

        public TimerWin32Adapted()
        {
            _timer = new TimerWin32Wrapper();
            _timer.Elapsed += Metronome_Tick;
        }

        void Metronome_Tick(object sender, EventArgs e)
        {
            TimerTick?.Invoke(this, e);
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimer()
        {
            if(_timer.IsRunning)
            {
                _timer.Stop();
            }
        }

        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(_timer.Interval);
            set => _timer.Interval = (int)value.TotalMilliseconds;
        }
    }
}