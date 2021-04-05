# Frequently Asked Questions

### What are the primary scenarios supported by Orchestrator?
Primary scenario currently supported is routing/dispatching (intent only).  We are working on supporting entities extraction for future releases.


### Can you use both Orchestrator and LUIS in the same bot?
Yes, definitely.  You could use Orchestrator (OrchestratorRecognizer) to route incoming utterances to the right child applications (i.e., LUISRecognizer, QnAMakerRecognizer).


### What are the guidelines for when Orchestrator correctly predicts incoming utterance for sub intent (app/skill) but the sub app fails to return the right intent?
We recommend tuning the sub app models (i.e. LUIS/QnAMaker/Orchestrator) using the recommended best practices for the app.  
- To improve LUIS model, follow instructions [here][1].
- To improve QnAMaker kb, follow guidelines [here][2].  
- To improve Orchestrator, follow guidelines [here][3].


### Which languages will be supported in Orchestrator multilingual models?
The model supports 100 languages (full list can be found at [XLMR Supported Languages][4]). 8 languages (EN, ES, DE, FR, IT, JA, PT, and ZH) are fine-tuned with additional data.  We are constantly looking to expand the number of supported languages and if a language is not yet supported, Azure translation could be used to translate incoming utterance to English first before passing it to Orchestrator.  

### Does Orchestrator replace LUIS Dispatch?
LUIS based [Dispatch][5] is on the path to be deprecated, although the exact timeline is not yet finalized.  Now is a good time to start evaluating Orchestrator as an alternative to Dispatch.


### Does Orchestrator support entity extraction?
Not at the moment, but we are working on it for future releases.


### What is the guidelines in choosing Orchestrator vs LUIS?
We recommend evaluating each solution against your data and see which performs better.  Both LUIS based [Dispatch CLI][5] and [BF Orchestrator CLI][6] come with a command to evaluate training and test data.

### Could you combine Orchestrator to predict intent and LUIS for entity?
Unfortunately not.  However, Orchestrator Recognizer could take external entity recognizers.  Look [here][7] for supported entity recognizers.

### References
* [Orchestrator][8]
* [Composer][9]
* [LUIS][10]
* [QnAMaker][11]

[1]:https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-use-dashboard
[2]:https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/test-knowledge-base?tabs=v1
[3]:https://github.com/microsoft/BotBuilder-Samples/blob/main/experimental/orchestrator/CLI/ModelTuning/README.md
[4]:https://github.com/pytorch/fairseq/tree/master/examples/xlmr#introduction
[5]:https://github.com/microsoft/botbuilder-tools/tree/master/packages/Dispatch
[6]:https://github.com/microsoft/botframework-cli/tree/main/packages/Orchestrator
[7]:https://github.com/microsoft/botbuilder-dotnet/tree/main/libraries/Microsoft.Bot.Builder.Dialogs.Adaptive/Recognizers/EntityRecognizers
[8]:https://aka.ms/bf-orchestrator
[9]:https://github.com/microsoft/BotFramework-Composer
[10]:https://docs.microsoft.com/en-us/azure/cognitive-services/luis/what-is-luis
[11]:https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/overview/overview