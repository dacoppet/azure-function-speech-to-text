using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AudioToText.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AudioToText
{
    public static class SpeechToTextRest
    {
        [FunctionName("SpeechToTextRest")]
        [return: Queue("sttrest", Connection = "SpeechStorage")]
        public static async Task<string> RunAsync([BlobTrigger("audio/{name}", Connection = "SpeechStorage")]Stream audioFile,
            string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed audio file \n Name:{name} \n Size: {audioFile.Length} Bytes");
            var culture = Environment.GetEnvironmentVariable("SpeechLocale");
            var urlBase = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1";
            var urlFull = $"{urlBase}?language={culture}&format=detailed";
            var subscriptionKey = Environment.GetEnvironmentVariable("SpeechSubscriptionKey");

            var recognitionResult = await SpeechApiHelper.ProcessSpeech(audioFile, name, urlFull, subscriptionKey);

            return recognitionResult;
        }

    }
}
