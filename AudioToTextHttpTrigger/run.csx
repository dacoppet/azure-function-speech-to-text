using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AudioToText.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

public static HttpResponseMessage Run(HttpRequestMessage req, string name, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");
    var culture = Environment.GetEnvironmentVariable("SpeechLocale");
    var urlBase = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1";
    var urlFull = $"{urlBase}?language={culture}&format=detailed";
    var subscriptionKey = Environment.GetEnvironmentVariable("SpeechSubscriptionKey");
    var recognitionResult = string.Empty;

    // Get request body
    try
    {
        Stream audioFile = await req.Content.ReadAsStreamAsync();
        var name = Guid.NewGuid().ToString();

        recognitionResult = await SpeechApiHelper.ProcessSpeech(audioFile, name, urlFull, subscriptionKey);


        log.Info($"API result : {recognitionResult}");

    }
    catch (Exception ex)
    {
        log.Error(ex.ToString());
        throw;
    }

    if(recognitionResult == null)
        return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Call to the speech API retuned null") };

    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(recognitionResult) };

    // Fetching the name from the path parameter in the request URL
    //return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}
