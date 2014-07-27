using System;
using System.Runtime.Serialization;
using Windows.Phone.Speech.Synthesis;
using WPCordovaClassLib.Cordova.JSON;

namespace WPCordovaClassLib.Cordova.Commands
{
    public class SpeechSynthesis : BaseCommand
    {
        private SpeechSynthesizer speechOutput;

        public SpeechSynthesis()
        {
            InitializeSpeech();
        }

        /*----------------------- TTS --------------------------------------------*/

        [DataContract]
        public class Utterance
        {
            [DataMember]
            public string text { get; set; }

            [DataMember]
            public string lang { get; set; }

            [DataMember]
            public double rate { get; set; }

            [DataMember]
            public double pitch { get; set; }
        }

        public async void speak(string options)
        {
            var parameterOptions = JsonHelper.Deserialize<string[]>(options);
            Utterance utterance = JsonHelper.Deserialize<Utterance>(parameterOptions[0]);

            // speechOutput.SetVoice

            await speechOutput.SpeakTextAsync(utterance.text);
            this.DispatchCommandResult(new PluginResult(PluginResult.Status.OK, utterance.text + " Played"));
        }

        public async void startup(string options)
        {
            
        }
        /*---------------------------TTS ------------------------------------------*/

        private void InitializeSpeech()
        {
            //TTS Object
            speechOutput = new SpeechSynthesizer();
        }
    }
}
