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
        private const string SubscriptionKey = "9967243d0ffe4d1c99f0a824d1aa5da5";
        private const string Region = "westus";

        public string result { get; set; }

        public TextRecognizer()
        {

        }

        public async Task RecoFromMicrophoneAsync(string lang)
        {

            var factory = SpeechFactory.FromSubscription(SubscriptionKey, Region);

            using (var recognizer = factory.CreateSpeechRecognizer(lang))
            {
                var recognitionResult = await recognizer.RecognizeAsync().ConfigureAwait(false);
                if (recognitionResult.RecognitionStatus != RecognitionStatus.Recognized)
                {
                    result = null;
                }
                else
                {
                    result = recognitionResult.Text;
                }
            }
        }
    }
}
