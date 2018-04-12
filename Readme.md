# azure-function-speech-to-text
This is a demo sample of using the Bing Speech API with an azure function where you upload a file and it will be translated automaticaly.
Leverage the one click deploy by clicking on the Azure Deploy button:
<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fdacoppet%2Fazure-function-speech-to-text%2Fmaster%2Ffeature/continousdeploy%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fdacoppet%2Fazure-function-speech-to-text%2Fmaster%2Ffeature/continousdeploy%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>

# How to use it
- Create an Azure Function and 
- Upload an audio file in an azure storage container (audio)
- The function will process it, call the speech api and output the result in an Azure que named speech to text
- You can then process it with another function or just consume the message in your application

# How to manualy setup the solution
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

- Enable continous deploy: Clone the repo with the right branche and from Azure Functions [see documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-continuous-deployment)

