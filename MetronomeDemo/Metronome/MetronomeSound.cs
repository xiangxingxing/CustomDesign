using System.IO;
using System.Media;
using System.Reflection;

namespace MetronomeDemo.Metronome
{
    public interface IMetronomeSound
    {
        void PlayHighBeep();
        void PlayLowBeep();
    }

    public class MetronomeSound : IMetronomeSound
    {
        private readonly SoundPlayer _lowFreqPlayer;
        private readonly SoundPlayer _highFreqPlayer;

        public MetronomeSound()
        {
            var lowPath = GetWaveFilePath("sticks_low");
            _lowFreqPlayer = new SoundPlayer(lowPath);

            var highPath = GetWaveFilePath("sticks_high");
            _highFreqPlayer = new SoundPlayer(highPath);
        }

        private string GetWaveFilePath(string name)
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Audio\metronome\", name) + ".wav";
        }

        public void PlayHighBeep()
        {
            _highFreqPlayer.Play();
        }

        public void PlayLowBeep()
        {
            _lowFreqPlayer.Play();
        }
    }
}