using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Speech2Text
{
    public class Speech2TextHelper
    {
        public string Recognize(string key, string path, string locale)
        {
            var request = CreateRequest(key, locale);
            SendFile(path, request);
            return GetResponse(request).DisplayText;
        }

        private static HttpWebRequest CreateRequest(string key, string locale)
        {
            var endpoint = $"https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1?language={locale}";
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.SendChunked = true;
            request.Accept = @"application/json;text/xml";
            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version11;
            request.ContentType = @"audio/wav; codec=audio/pcm; samplerate=16000";
            request.Headers["Ocp-Apim-Subscription-Key"] = key;
            return request;
        }

        private static void SendFile(string path, HttpWebRequest request)
        {
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = null;
                var bytesRead = 0;
                using (var stream = request.GetRequestStream())
                {
                    buffer = new Byte[checked((uint)Math.Min(1024, (int)file.Length))];
                    while ((bytesRead = file.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                    stream.Flush();
                }
            }
        }

        private static ServiceResult GetResponse(HttpWebRequest request)
        {
            using (var response = request.GetResponse())
            {
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    var json = stream.ReadToEnd();
                    return JsonConvert.DeserializeObject<ServiceResult>(json);
                }
            }
        }

        public partial class ServiceResult
        {
            [JsonProperty("RecognitionStatus")]
            public string RecognitionStatus { get; set; }

            [JsonProperty("DisplayText")]
            public string DisplayText { get; set; }

            [JsonProperty("Offset")]
            public long Offset { get; set; }

            [JsonProperty("Duration")]
            public long Duration { get; set; }
        }
    }
}
