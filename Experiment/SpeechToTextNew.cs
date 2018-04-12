using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bing.Speech;

namespace AudioToText
{
    public static class SpeechToTextNew
    {
        /// <summary>
        /// Short phrase mode URL
        /// </summary>
        private static readonly Uri ShortPhraseUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition");

        /// <summary>
        /// The long dictation URL
        /// </summary>
        private static readonly Uri LongDictationUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition/continuous");

        /// <summary>
        /// Cancellation token used to stop sending the audio.
        /// </summary>
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        [FunctionName("SpeechToTextNew")]
        public static async System.Threading.Tasks.Task RunAsync([Disable(), BlobTrigger("audio/{name}", Connection = "Target-Blob")]Stream audioFile, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {audioFile.Length} Bytes");


            /// <param name="audioFile">The audio file.</param>
            /// <param name="locale">The locale.</param>
            /// <param name="serviceUrl">The service URL.</param>
            /// <param name="subscriptionKey">The subscription key.</param>
            var locale = "ja-JP"; // en-US
            var serviceUrl = ShortPhraseUrl;
            var subscriptionKey = Environment.GetEnvironmentVariable("BingSpeech-subscriptionKey");

            var preferences = new Preferences(locale, serviceUrl, new CognitiveServicesAuthorizationProvider(subscriptionKey));

            // Create a a speech client
            using (var speechClient = new SpeechClient(preferences))
            {
                /// Invoked when the speech client receives a partial recognition hypothesis from the server.
                //speechClient.SubscribeToPartialResult((recoPartialResult) =>
                //{
                //    // Print the partial response recognition hypothesis.
                //    log.Info($"--- Partial result received by OnPartialResult ---\n {recoPartialResult.DisplayText}");
                //    return CompletedTask;
                //});


                /// Invoked when the speech client receives a phrase recognition result(s) from the server.
                speechClient.SubscribeToRecognitionResult((recoResult) =>
                {
                    log.Info("--- Phrase result received by OnRecognitionResult ---");

                    // Print the recognition status.
                    log.Info($"***** Phrase Recognition Status = [{recoResult.RecognitionStatus}] ***");

                    if (recoResult.Phrases != null)
                    {
                        log.Info($"***** Phrase Recognition Phrases Count: {recoResult.Phrases.Count} ***");
                        foreach (var result in recoResult.Phrases)
                        {
                            // Print the recognition phrase display text.
                            log.Info($"{result.DisplayText} (Confidence:{ result.Confidence})");
                        }
                    }
                    return CompletedTask;
                });

                // create an audio content and pass it a stream
                var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.HolographicOrVR, NetworkType.Ethernet, OsName.Windows, "1607", "Hololens", "v1");
                var applicationMetadata = new ApplicationMetadata("SpeechToTextFunction", "1.0.0");
                var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata, "AudioToTextFunctionApp");

                await speechClient.RecognizeAsync(new SpeechInput(audioFile, requestMetadata), cts.Token).ConfigureAwait(false);

            }
           
        }

        /// <summary>
        /// A completed task
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(true);


    }
}
