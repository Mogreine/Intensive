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
        private string _lang;
        private GoogleCredential _credential;
        private Channel _channel;
        private bool _writeMore;
        private SpeechClient speech;
        private SpeechClient.StreamingRecognizeStream streamingCall;
        private WaveInEvent waveIn;
        private object writeLock;

        public string TextResult { get; set; }

        public GoogleRec(string lang)
        {
            _credential = GoogleCredential.FromFile(@"..\..\..\Resources\g.json").CreateScoped(SpeechClient.DefaultScopes);
            _channel = new Channel(SpeechClient.DefaultEndpoint.ToString(), _credential.ToChannelCredentials());

            _lang = lang;
            writeLock = new object();

            NAudioConfiguration();
        }

        public async Task Start()
        {
            speech = SpeechClient.Create(_channel);
            _writeMore = true;
            lock (writeLock) _writeMore = true;

            streamingCall = speech.StreamingRecognize();
            await streamingCall.WriteAsync(ConfigRequest(_lang));

            waveIn.StartRecording();
            Console.WriteLine("Speak now.");
        }

        private StreamingRecognizeRequest ConfigRequest(string lang)
        {
            return new StreamingRecognizeRequest()
            {
                StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = new RecognitionConfig()
                    {
                        Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                        SampleRateHertz = 16000,
                        LanguageCode = lang,
                        //SpeechContexts = { new SpeechContext() { Phrases = {"млн", "млрд", "+", "-", "*", "/" } } }
                    },
                    InterimResults = true,
                }
            };
        }

        private void NAudioConfiguration()
        {
            waveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 1)
            };
            waveIn.DataAvailable +=
                (object sender, WaveInEventArgs args) =>
                {
                    lock (writeLock)
                    {
                        if (!_writeMore) return;
                        streamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };
        }

        public async Task<string> Stop()
        {
            Task<string> printResponses = Task.Run(async () =>
            {
                string res = "";
                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    res = streamingCall.ResponseStream.Current.Results.Last().Alternatives.Last().Transcript;
                }
                return res;
            });

            waveIn.StopRecording();
            lock (writeLock) _writeMore = false;
            await streamingCall.WriteCompleteAsync();
            var textResult = await printResponses;
            return textResult;
        }

    }
}