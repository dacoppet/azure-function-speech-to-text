using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AudioToText.Helper
{
    internal static class SpeechApiHelper
    {
        /// <summary>
        /// Wrap audio stream to Speech API
        /// </summary>
        /// <param name="audioFile"></param>
        /// <param name="name"></param>
        /// <param name="urlFull"></param>
        /// <param name="subscriptionKey"></param>
        /// <returns>Raw Json API</returns>
        internal static async Task<string> ProcessSpeech(Stream audioFile, string name, string urlFull, string subscriptionKey)
        {
            var apiResult = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(audioFile);
                    streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"file\"; filename=\"{name}\"");
                    content.Add(streamContent, "file", name);
                    var httpResponse = await client.PostAsync(urlFull, content);

                    apiResult = await httpResponse.Content.ReadAsStringAsync();
                    // TODO: test input
                    Trace.TraceInformation($"Result  - {apiResult}");
                }
            }

            return apiResult;
        }
    }
}
