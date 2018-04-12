#r "Newtonsoft.Json"

using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public static async Task Run(Stream audio, string name, out string textout, TraceWriter log)
{

    log.Info($"C# Blob trigger function Processed audio file \n Name:{name} \n Size: {audio.Length} Bytes");
            var culture = Environment.GetEnvironmentVariable("BingSpeech-Locale");
            var urlBase = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1";
            var urlFull = $"{urlBase}?language={culture}&format=detailed";
            var recognitionResult = string.Empty;
            var subscriptionKey = Environment.GetEnvironmentVariable("SpeechSubscriptionKey");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SpeechSubscriptionKey"));

                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(audio);
                    streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"{name}\"");
                    content.Add(streamContent, "file", name);
                    var result = await client.PostAsync(urlFull, content);
                    
                    recognitionResult = await result.Content.ReadAsStringAsync();
                    // TODO: test input
                    log.Info($"Result  - {recognitionResult}");
                }
            }

            textout = recognitionResult;
}
