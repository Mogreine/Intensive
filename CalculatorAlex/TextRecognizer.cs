using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace CalculatorAlex
{
    class TextRecognizer
    {
        static async Task RecoFromMicrophoneAsync()
        {
            var subscriptionKey = "9967243d0ffe4d1c99f0a824d1aa5da5";
            var region = "westus";

            var factory = SpeechFactory.FromSubscription(subscriptionKey, region);

            using (var recognizer = factory.CreateSpeechRecognizer("ru-RU"))
            {
                Console.WriteLine("Say something...");
                var result = await recognizer.RecognizeAsync();

                if (result.RecognitionStatus != RecognitionStatus.Recognized)
                {
                    Console.WriteLine($"There was an error, status {result.RecognitionStatus.ToString()}, reason {result.RecognitionFailureReason}");
                }
                else
                {
                    Console.WriteLine($"We recognized: {result.Text}");
                }
                Console.WriteLine("Please press a key to continue.");
                //Console.ReadLine();
            }
        }
    }
}
