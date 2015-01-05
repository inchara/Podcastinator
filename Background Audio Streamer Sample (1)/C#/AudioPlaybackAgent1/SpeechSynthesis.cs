using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Foundation;
using Windows.Phone.Speech.Synthesis;

namespace AudioPlaybackAgent1
{
    public class SpeechSynthesis : IDisposable
    {
        protected SpeechSynthesizer synth;

        public SpeechSynthesis()
        {
            synth = new SpeechSynthesizer();
        }

        
        public async Task Speak(string text)
        {
            await synth.SpeakTextAsync(text);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            synth.Dispose();
        }
    }
}