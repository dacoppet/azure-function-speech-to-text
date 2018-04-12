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
	1. "SpeechStorage": "",
    2. "SpeechSubscriptionKey": "ReplaceWithYourSpeechApiKeyHere",
    3. "SpeechLocale": "ja-jp"


For git deployement checkout : https://docs.microsoft.com/en-us/azure/azure-functions/functions-continuous-deployment