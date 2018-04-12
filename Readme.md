# azure-function-speech-to-text
This ia a demo sample of using the Bing Speech API with an azure function

# How to use it
- Create an Azure Function and Clone the repo and do continuous deploy from Azure Functions [see documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-continuous-deployment)
- Upload an audio file in an azure storage container (audio)
- The function will process it, call the speech api and output the result in an Azure que named speech to text
- You can then process it with another function or just consume the message in your application

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