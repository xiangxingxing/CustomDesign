using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MetronomeDemo.Metronome
{
    public class MetronomeViewModel : MetronomeBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly int metronomeLowLimit;
        private readonly int metronomeHighLimit;

        public MetronomeViewModel(IMetronomeBuilder appBuilder) : base(appBuilder)
        {
            metronomeLowLimit = appBuilder.MetronomeLowLimit;
            metronomeHighLimit = appBuilder.MetronomeHighLimit;

            base.Timer.TimerTick += MetronomeViewModel_MetronomeTick;
        }

        private void MetronomeViewModel_MetronomeTick(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TickVisualization));
        }

        public void Run()
        {
            base.StartTimer();
        }

        public void Stop()
        {
            base.StopTimer();
        }

        public void ChangePattern()
        {
            if (base.IsRunning)
            {
                base.RestartTimer();
            }
        }

        public void TempoSliderMoved()
        {
            if (base.IsRunning)
            {
                base.RestartTimer();
            }
        }

        public int Tempo
        {
            get => base.Tempo;
            set
            {
                if ((value >= metronomeLowLimit) && (value <= metronomeHighLimit))
                {
                    base.Tempo = value;
                    OnPropertyChanged(nameof(Tempo));
                }
                else if (value < metronomeLowLimit)
                {
                    base.Tempo = metronomeLowLimit;
                }
                else if (value > metronomeHighLimit)
                {
                    base.Tempo = metronomeHighLimit;
                }
            }
        }

        public string Pattern
        {
            get => base.Pattern;
            set
            {
                base.Pattern = value;
                OnPropertyChanged(nameof(Pattern));
                OnPropertyChanged(nameof(Measure));
            }
        }
    }
}