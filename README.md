# Bot Builder 

Bot Builder is comprised of SDK and tools that enable you to build and connect intelligent bots that interact naturally wherever your users are talking, from text/sms to Skype, Slack, Office 365 mail and other popular services. You can build bots that converse free-form or your bot can also have more guided interactions where it provides the user choices or possible actions. The conversation can use simple text or more complex rich cards that contain text, images, and action buttons. You can add natural language interactions and questions and answers, which let your users interact with your bots in a natural way.

Bot Builder provides the most comprehensive experience for building conversation applications and includes the following SDKs and tools:

- **Bot Builder V4 SDK**
    - [**C#** (stable release)](https://github.com/microsoft/botbuilder-dotnet)
    - [**JS** (stable release)](https://github.com/microsoft/botbuilder-js)
    - [**Java** (preview release)](https://github.com/microsoft/botbuilder-java)
    - [**Python** (preview release)](https://github.com/microsoft/botbuilder-python).
- **Bot Framework Emulator** (local debugging and visualization of conversations)
    - [Bot Framework **V4 Emulator** (preview release)](https://github.com/microsoft/botframework-emulator).
    - [Bot Frameowrk **V3 Emulator** (stable release)](https://github.com/Microsoft/BotFramework-Emulator/releases/tag/v3.5.36)
- **Bot Builder CLI tools** (streamlined development, provisioning, and deployment)
    - [MSBot CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/MSBot)
    - [AZ Bot CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
    - [Dispatch CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Dispatch)
    - [LUIS CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/LUIS)
    - [Ludown CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Ludown)
    - [LuisGen CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/LUISGen)
    - [QnAMaker CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/QnAMaker)
    - [Chatdown CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Chatdown)
- **Bot Framework webchat** (embeddable and customizable open source web chat control) 
    - [Available here](https://github.com/microsoft/botframework-webchat)


Please see [here](https://aka.ms/BotBuilderOverview) for an overview of the end-to-end bot development workflow. To get started, you can create a bot with [Azure Bot Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0). Click [here](https://account.azure.com/signup) if you need a trial Azure subscription. 

## Documentation
Visit azure.com for the primary [Azure Bot Service documentation page](https://docs.microsoft.com/en-us/azure/bot-service/) to learn about building bots using Bot Builder. There is additional documentation on the SDK, oriented towards contributors. The SDK currently supports four programming language: 
- [.NET](https://github.com/Microsoft/botbuilder-dotnet/wiki)
- [JavaScript](https://github.com/microsoft/botbuilder-js/wiki)
- [Python](https://github.com/Microsoft/botbuilder-python/wiki)
- [Java](https://github.com/Microsoft/botbuilder-java/wiki)

## Samples
A rich set of samples are available at the [BotBuilder-Samples](https://github.com/microsoft/botbuilder-samples) repository.

## Questions and Help 
If you have questions about Bot Builder SDK or using Azure Bot Service, we encourage you to reach out to the community and Azure Bot Service dev team for help.
- For questions which fit the Stack Overflow format ("how does this work?"), we monitor the both [Azure-bot-service](https://stackoverflow.com/questions/tagged/azure-bot-service) and [bot framework](https://stackoverflow.com/questions/tagged/botframework) tags (search [both](https://stackoverflow.com/questions/tagged/azure-bot-service+or+botframework))
- You can also tweet/follow [@msbotframework](https://twitter.com/msbotframework) 

While we do our best to help out on a timely basis, we don't have any promise around the above resources. If you need an SLA on support from us, it's recommended you invest in an [Azure Support plan](https://azure.microsoft.com/en-us/support/options/).

## Issues and feature requests 
We track functional issues and features asks for and Bot Builder and Azure Bot Service in a variety of locations. If you have found an issue or have a feature request, please submit an issue to the below repositories.

|Item|Description|Link|
|----|-----|-----|
|SDK v4 .net| core bot runtime for .NET, connectors, middleware, dialogs, prompts, LUIS and QnA| [File an issue](https://github.com/Microsoft/botbuilder-dotnet/issues) |
|SDK v4 JavaScript| core bot runtime for JavaScript, connectors, middleware, dialogs, prompts, LUIS and QnA | [File an issue](https://github.com/Microsoft/botbuilder-js/issues) |
|SDK v4 Python| core bot runtime for Python, connectors, middleware, dialogs, prompts, LUIS and QnA | [File an issue](https://github.com/Microsoft/botbuilder-python/issues) |
|SDK v4 Java| core bot runtime for Java, connectors, middleware, dialogs, prompts, LUIS and QnA | [File an issue]( https://github.com/Microsoft/botbuilder-java/issues)|
|CLI tools| MSBot, chatdown, ludown, LUIS, LUISGen, QnA Maker, dispatch  | [File an issue](https://github.com/microsoft/botbuilder-tools/issues)|
|Emulator| Test and debug bots running locally or remote. | [File an issue](https://github.com/Microsoft/BotFramework-Emulator/issues)| 
|Webchat | Embeddable web chat control for the Microsoft Bot Framework | [File an issue](https://github.com/Microsoft/BotBuilder/issues)|
|Azure Bot Service & Channels| Bot Framework portal within Microsoft Azure, and the connectivity channels GroupMe, Facebook Messenger, Kik, Skype, Slack, Microsoft Teams, Telegram, text/SMS, Twilio, Cortana, etc. | [File an issue](https://github.com/Microsoft/BotFramework-Services/issues) |
|SDK v3 (.NET and JS)| core bot runtime, abstractions, prompts, dialogs, FormFlow, etc. | [File an issue](https://github.com/Microsoft/BotBuilder-v3/issues) |

Join the conversation on **[Gitter](https://gitter.im/Microsoft/BotBuilder)**.

See all the support options **[here](https://docs.microsoft.com/en-us/bot-framework/resources-support)**.

## Adding intelligence to your bot
With the power of [Azure Cognitive Services](https://azure.microsoft.com/services/cognitive-services/), your bots can interact with your customers in more human ways. The following are common services that bots utilize: 
- [LUIS](https://www.luis.ai)
- [QnA Maker](https://www.qnamaker.ai/)
- [Speech](https://azure.microsoft.com/services/cognitive-services/directory/speech/)

## Prior releases
- Bot Builder V3 SDK 
    - Has been migrated to the [BotBuilder-V3](https://github.com/microsoft/botbuilder-v3) repository.
- Bot Framework Emulator 
    - [Bot Framework **V3 Emulator** (stable release)](https://github.com/Microsoft/BotFramework-Emulator/releases/tag/v3.5.36)

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Reporting Security Issues
Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC) at [secure@microsoft.com](mailto:secure@microsoft.com). You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the [MSRC PGP](https://technet.microsoft.com/en-us/security/dn606155) key, can be found in the [Security TechCenter](https://technet.microsoft.com/en-us/security/default).

## License

Copyright (c) Microsoft Corporation. All rights reserved.

Licensed under the [MIT](LICENSE) License.


This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
