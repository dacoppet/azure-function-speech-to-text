#r "Microsoft.WindowsAzure.Storage"
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

public static async Task Run(Stream myBlob, string name, TraceWriter log)
{
    log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

     log.Info($"C# Blob trigger function Processed audio file \n Name:{name} \n Size: {myBlob.Length} Bytes");
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
                    var streamContent = new StreamContent(myBlob);
                    streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"{name}\"");
                    content.Add(streamContent, "file", name);
                    var result = await client.PostAsync(urlFull, content);
                    
                    recognitionResult = await result.Content.ReadAsStringAsync();
                    // TODO: test input
                    log.Info($"Result  - {recognitionResult}");
                }
            }
}
