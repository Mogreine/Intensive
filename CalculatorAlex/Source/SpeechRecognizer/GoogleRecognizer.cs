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
    class GoogleRecognizer
    {
        private bool _writeMore;
        private readonly object _writeLock;
        private readonly Channel _channel;
        private WaveInEvent _waveIn;
        private SpeechClient.StreamingRecognizeStream _streamingCall;

        private AutoResetEvent _error;

        public GoogleRecognizer(AutoResetEvent error)
        {
            var credential = GoogleCredential.FromFile(@"..\..\..\Resources\g.json").CreateScoped(SpeechClient.DefaultScopes);
            _channel = new Channel(SpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

            _error = error;

            _writeLock = new object();
            NAudioConfiguration();
        }

        public async Task Start(string lang)
        {
            var speech = SpeechClient.Create(_channel);
            _writeMore = true;
            lock (_writeLock) _writeMore = true;

            _streamingCall = speech.StreamingRecognize();
            await _streamingCall.WriteAsync(RequestConfiguration(lang));

            _waveIn.StartRecording();
            Console.WriteLine("Speak now.");
        }

        public async Task<string> Stop()
        {
            Task<string> printResponses = Task.Run(async () =>
            {
                string res = "";
                while (await _streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    res = _streamingCall.ResponseStream.Current.Results.Last().Alternatives.Last().Transcript;
                }
                return res;
            });

            _waveIn.StopRecording();
            lock (_writeLock) _writeMore = false;
            await _streamingCall.WriteCompleteAsync();

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
                                Phrases = { "миллион", "миллиард", "разделить на", "делить на", "разделить", "умножить на", "умножить", "twelve", "divide", "three" }
                            }

                        }
                    },
                    InterimResults = true,
                }
            };
        }

        private void NAudioConfiguration()
        {
            _waveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 1)
            };

            _waveIn.DataAvailable +=
                (sender, args) =>
                {
                    lock (_writeLock)
                    {
                        try
                        {
                            if (!_writeMore) return;
                            _streamingCall.WriteAsync(
                                new StreamingRecognizeRequest()
                                {
                                    AudioContent = Google.Protobuf.ByteString
                                        .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                                }).Wait();
                        }
                        catch (AggregateException)
                        {
                            _error.Set();
                        }
                        
                    }
                };
        }
    }
}