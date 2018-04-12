using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using AudioToText.Helper;
using AudioToText.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AudioToText
{
    public static class SpeechToText
    {
        [FunctionName("SpeechToText")]
        [return: Queue("sttrest", Connection = "SpeechStorage")]
        public static async System.Threading.Tasks.Task<ItemSTT> RunAsync([BlobTrigger("audio/{blobname}.{extension}",
            Connection = "SpeechStorage")]Stream audioFile,
            string blobname, string extension, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed audio file \n Name:{blobname} \n Size: {audioFile.Length} Bytes -  Exension: {extension}");

            var culture = Environment.GetEnvironmentVariable("SpeechLocale");
            var urlBase = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1";
            var urlFull = $"{urlBase}?language={culture}&format=detailed";
            var subscriptionKey = Environment.GetEnvironmentVariable("SpeechSubscriptionKey");

            // TODO: Improve Exception Handling
            var recognitionResultJson = await SpeechApiHelper.ProcessSpeech(audioFile, blobname, urlFull, subscriptionKey);

            var result = ConvertToJson(recognitionResultJson, blobname);

            return result;
        }

        // process result
        private static ItemSTT ConvertToJson(string jsonToParse, string fileid)
        {
            var result = JsonConvert.DeserializeObject<RecognitionResult>(jsonToParse);

            var itemToProcess = new ItemSTT();

            foreach (var entry in result.NBest)
            {
                //TODO: Best confice may not be the best option
                    // May require to save
                // Save only the best confidence
                if(itemToProcess.Confidence < entry.Confidence)
                {
                    itemToProcess.Confidence = entry.Confidence;
                    itemToProcess.CapturedText = entry.Display;
                    itemToProcess.FileID = fileid;
                }
            }

            return itemToProcess;
        }
    }
}
