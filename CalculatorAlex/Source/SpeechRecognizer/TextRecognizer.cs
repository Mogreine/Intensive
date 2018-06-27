using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace CalculatorAlex
{
    public class TextRecognizer
    {
        private SpeechFactory _factory;

        public string Result { get; set; }

        public TextRecognizer()
        {
            var subscriptionKey = "9967243d0ffe4d1c99f0a824d1aa5da5";
            var region = "westus";
            _factory = SpeechFactory.FromSubscription(subscriptionKey, region);
        }

        public async Task RecoFromMicrophoneAsync(string lang)
        {

            using (var recognizer = _factory.CreateSpeechRecognizer(lang))
            {
                var recognitionResult = await recognizer.RecognizeAsync().ConfigureAwait(false);

                if (recognitionResult.RecognitionStatus != RecognitionStatus.Recognized)
                {
                    Result = null;
                }
                else
                {
                    Result = recognitionResult.Text;
                }
            }

        }
    }
}
