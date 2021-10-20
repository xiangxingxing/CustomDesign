using System;

namespace MetronomeDemo.Metronome
{
    public enum TickTack
    {
        MetronomeTack,
        MetronomeTick
    }

    public class MetronomeBase
    {
        private const int MilliSecondsInOneMinute = 60000;
        private const int InitialTempo = 100;

        private readonly IMetronomeSound _beep;
        private readonly MetronPattern _metronomePattern;
        private readonly int _metronomeHighLimit;
        public bool IsRunning { get; internal set; }

        public string Measure => _metronomePattern.Measure;

        public int Tempo { get; set; }

        public string Pattern
        {
            get => _metronomePattern.PatternString;
            set => _metronomePattern.PatternString = value;
        }

        public string TickVisualization { get; set; }
        public ITimer Timer { get; }

        public MetronomeBase(IMetronomeBuilder appBuilder)
        {
            Timer = appBuilder?.TimerImplementor;
            _beep = appBuilder.SoundImplementor;

            _metronomeHighLimit = appBuilder.MetronomeHighLimit;

            Timer.TimerTick += Metronome_Tick;
            _metronomePattern = new MetronPattern();
            _metronomePattern.OnNextTickLoopHandler += Metronome_OnNextTickLoop;
            Tempo = InitialTempo;
        }

        private void Metronome_Tick(object sender, EventArgs e)
        {
            if (GetCurrentTackOrTick() == TickTack.MetronomeTick)
            {
                _beep.PlayHighBeep();
            }

            if (GetCurrentTackOrTick() == TickTack.MetronomeTack)
            {
                _beep.PlayLowBeep();
            }

            _metronomePattern.NextTick();
        }

        private TickTack GetCurrentTackOrTick()
        {
            return (TickTack)(int)char.GetNumericValue(_metronomePattern.CurrentTick);
        }

        private void Metronome_OnNextTickLoop(object sender, EventArgs e)
        {
        }

        public void RestartTimer()
        {
            StopTimer();
            StartTimer();
        }

        public void StartTimer()
        {
            if (!IsRunning)
            {
                Timer.StopTimer();
                Timer.Interval = TimeSpan.FromMilliseconds(MilliSecondsInOneMinute / Tempo);

                try
                {
                    Timer.StartTimer();
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Timer has not been started.");
                }

                IsRunning = true;
            }
        }

        public void StopTimer()
        {
            Timer.StopTimer();
            IsRunning = false;
            _metronomePattern.ResetPattern();
        }
    }
}