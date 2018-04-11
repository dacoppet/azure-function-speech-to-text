# azure-function-speech-to-text
Using the Bing Speech API

# How to setup the function
- Create an Azure Storage [New](https://ms.portal.azure.com/#create/Microsoft.StorageAccount-ARM)
	- Create a blob container name "audio"
	- Create a queue named "speechtotext"
In Azure Portal
- Create A ping speech api : [New](https://ms.portal.azure.com/#create/Microsoft.CognitiveServicesBingSpeech)
In Azure Function
- Add the following settings (while replacing the values with your own keys):
	1. "Target-Blob": "DefaultEndpointsProtocol=https;AccountName=xxxx;AccountKey=xxxx;BlobEndpoint=https://xxxx.blob.core.windows.net/;QueueEndpoint=https://xxxx.queue.core.windows.net/;TableEndpoint=https://xxxx.table.core.windows.net/;FileEndpoint=https://xxxx.file.core.windows.net/;",
    2. "BingSpeech-subscriptionKey": "ReplaceWithYourSpeechApiKeyHere",
    3. "BingSpeech-Locale": "ja-jp"


For git deployement checkout : https://docs.microsoft.com/en-us/azure/azure-functions/functions-continuous-deployment