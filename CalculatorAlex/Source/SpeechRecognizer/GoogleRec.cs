using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using Google.Apis;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Grpc.Core;
using NAudio.Wave;
using System.Windows.Controls;

namespace CalculatorAlex
{
    class GoogleRec
    {
        private bool WriteMore;
        private object WriteLock;
        private readonly Channel _channel;
        private WaveInEvent WaveIn;
        private SpeechClient.StreamingRecognizeStream StreamingCall;

        public GoogleRec()
        {
            var credential = GoogleCredential.FromFile(@"..\..\..\Resources\g.json").CreateScoped(SpeechClient.DefaultScopes);
            _channel = new Channel(SpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());
            
            WriteLock = new object();

            NAudioConfiguration();
        }

        public async Task Start(string lang)
        {
            var speech = SpeechClient.Create(_channel);
            WriteMore = true;
            lock (WriteLock) WriteMore = true;

            StreamingCall = speech.StreamingRecognize();
            await StreamingCall.WriteAsync(RequestConfiguration(lang));

            WaveIn.StartRecording();
            Console.WriteLine("Speak now.");
        }

        public async Task<string> Stop()
        {
            Task<string> printResponses = Task.Run(async () =>
            {
                string res = "";
                while (await StreamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    res = StreamingCall.ResponseStream.Current.Results.Last().Alternatives.Last().Transcript;
                }
                return res;
            });

            WaveIn.StopRecording();
            lock (WriteLock) WriteMore = false;
            await StreamingCall.WriteCompleteAsync();

            var textResult = await printResponses;
            return textResult;
        }

        private StreamingRecognizeRequest RequestConfiguration(string lang)
        {
            return new StreamingRecognizeRequest()
            {
                StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = new RecognitionConfig()
                    {
                        Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                        SampleRateHertz = 16000,
                        LanguageCode = lang,
                        SpeechContexts =
                        {
                            new SpeechContext()
                            {
                                Phrases = { "миллион", "миллиард", "разделить на", "делить на", "разделить", "умножить на", "умножить", "twelve", "divide" }
                            }

                        }
                    },
                    InterimResults = true,
                }
            };
        }

        private void NAudioConfiguration()
        {
            WaveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 1)
            };

            WaveIn.DataAvailable +=
                (sender, args) =>
                {
                    lock (WriteLock)
                    {
                        if (!WriteMore) return;
                        StreamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };
        }

        

    }
}