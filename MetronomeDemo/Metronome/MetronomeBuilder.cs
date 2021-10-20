namespace MetronomeDemo.Metronome
{
    public interface IMetronomeBuilder
    {
        ITimer TimerImplementor { get; set; }
        IMetronomeSound SoundImplementor { get; set; }
        int MetronomeLowLimit { get; set; }
        int MetronomeHighLimit { get; set; }
    }

    public class MetronomeBuilder : IMetronomeBuilder
    {
        public ITimer TimerImplementor { get; set; }
        public IMetronomeSound SoundImplementor { get; set; }
        public int MetronomeLowLimit { get; set; }
        public int MetronomeHighLimit { get; set; }

        public MetronomeBuilder()
        {
            TimerImplementor = new TimerWin32Adapted();
            SoundImplementor = new MetronomeSound();
            MetronomeLowLimit = 30;
            MetronomeHighLimit = 240;
        }
    }
}