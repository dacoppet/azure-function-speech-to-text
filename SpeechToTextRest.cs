using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AudioToText
{
    public static class SpeechToTextRestBackup
    {
        [FunctionName("SpeechToTextRestBackup")]
        [return: Queue("demoaudio", Connection = "Target-Blob")]
        public static async System.Threading.Tasks.Task<string> RunAsync([BlobTrigger("demoaudio/{name}", Connection = "Target-Blob")]Stream audioFile,
            string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed audio file \n Name:{name} \n Size: {audioFile.Length} Bytes");
            var culture = Environment.GetEnvironmentVariable("BingSpeech-Locale");
            var urlBase = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1";
            var urlFull = $"{urlBase}?language={culture}&format=detailed";
            var recognitionResult = string.Empty;
            var subscriptionKey = Environment.GetEnvironmentVariable("BingSpeech-subscriptionKey");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("BingSpeech-subscriptionKey"));

                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(audioFile);
                    streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"{name}\"");
                    content.Add(streamContent, "file", name);
                    var result = await client.PostAsync(urlFull, content);
                    
                    recognitionResult = await result.Content.ReadAsStringAsync();
                    // TODO: test input
                    log.Info($"Result  - {recognitionResult}");
                }
            }

            return recognitionResult;
        }
    }
}
