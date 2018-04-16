#r "Microsoft.WindowsAzure.Storage"

using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");    
    var containerName = "audio";
    var connectionString = Environment.GetEnvironmentVariable("SpeechStorage");
    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

    // Create the destination blob client
    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
    CloudBlobContainer container = blobClient.GetContainerReference(containerName);
    container.Create();
}