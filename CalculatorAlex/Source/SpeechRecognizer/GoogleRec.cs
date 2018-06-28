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
            _credential = GoogleCredential.FromFile(@"C:\123\g.json")
                .CreateScoped(SpeechClient.DefaultScopes);
            _channel = new Channel(
                SpeechClient.DefaultEndpoint.ToString(),
                _credential.ToChannelCredentials());

            _lang = lang;
            writeLock = new object();
        }
        public async void Start()
        {
            speech = SpeechClient.Create(_channel);
            _writeMore = true;
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            lock (writeLock) _writeMore = true;
            streamingCall = speech.StreamingRecognize();
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = _lang,
                        },
                        InterimResults = true,
                    }
                });
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
            waveIn.StartRecording();
            Console.WriteLine("Speak now.");
        }
        public async Task<string> Stop()
        {
            Task<string> printResponses = Task.Run(async () =>
            {
                string res = "";
                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream
                        .Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            res += alternative.Transcript + "\n";
                        }
                    }
                }
                return res;
            });

            waveIn.StopRecording();
            lock (writeLock) _writeMore = false;
            await streamingCall.WriteCompleteAsync();
            return TextResult = await printResponses;
        }
        /*
        public async Task<string> StreamingMicRecognizeAsync(int seconds)
        { 
        }
        */
    }
}