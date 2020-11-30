# Orchestrator (PREVIEW)

Conversational AI applications today are built using disparate technologies to fulfill language understanding (LU) needs, such as [LUIS][1] and [QnA Maker][2]. Often, conversational AI applications are also built by accessing various skill bots, such as [Virtual Assistant skills][3], each of which handle a specific conversation topic and can be built using different LU technologies. Hence, conversational AI applications typically require LU to route an incoming user request to an appropriate skill or to dispatch to a specific sub-component.

<!-- Coining the term "orchestration" doesn't seem necessary. -->

<!-- Discussing the underlying technology also doesn't seem necessary here. Mention in the technical overview, instead. -->

Orchestrator is an LU solution optimized for conversational AI applications. It is built ground-up to run locally with your bot. See the [technical overview][18] for additional details.

## Scenarios

**Routing**: For bots, Orchestrator can replace the [LUIS Dispatch tool][5]. You can use Orchestrator instead of Dispatch to aggregate multiple [LUIS][1] and [QnA Maker][2] applications. With Orchestrator, you are likely to see:

- Improved classification accuracy
- Higher resilience to data imbalance across your LUIS and QnA Maker authoring data.
- Ability to correctly dispatch from relatively little authoring data.

**Intent recognition**: You can use Orchestrator as an intent recognizer with [adaptive dialogs][6] to route user input to an appropriate skill or sub-component.

**Entity extraction** is not yet supported.

## Authoring experience

Orchestrator can be used in different development environments:

- [Bot Framework SDK][24]: Orchestrator can be integrated into your code project by replacing LUIS for intent recognition, such as for skill delegation or dispatching to subsequent language understanding services. See the [SDK integration](#sdk-integration) section for more information. <!--We don't yet document Orchestrator in the SDK docs. Do we need to?-->
- [Bot Framework Composer][19]: Orchestrator can be selected as a recognizer within Bot Framework Composer. At this point there are limitations to using Orchestrator in Composer, primarily around importing of existing models and tuning recognition performance. (To use Orchestrator, enable the feature flag in your Composer settings.) See the [Composer integration](#composer-integration) section for more information.

In most cases, the [Bot Framework CLI][7] and [Bot Framework CLI Orchestrator plugin][11] is required to prepare and optimize the model for your domain. The [BF Orchestrator command usage][23] page describes how to create, evaluate, and use an Orchestrator model. This diagram illustrates the first part of that process. <!--The diagram leaves off steps 4 and 5.-->

<p align="center">
  <img width="350" src="./docs/media/authoring.png" />
</p>

To use the CLI, install the [Bot Framework CLI Orchestrator plugin][11].

On the [BF Orchestrator command usage][23] page:

- Steps 1-3 describe how to prepare and optimize a snapshot of your Orchestrator model.
- Step 4 describes how to evaluate and improve the performance of your snapshot.
- Step 5 describes how to integrate the Orchestrator language recognizer with your bot.

<!--Is there a reason to summarize this process here, instead of just linking to the **BF Orchestrator Command Usage** topic?

### To prepare and optimize your Orchestrator model

* Pre-requisite: Install [BF CLI Orchestrator plugin][11] first.

1. Author Intent-utterances example based .lu definition referred to as a *label file* using the Language Understanding practices as described in [Language Understanding][2] for dispatch (e.g. author .lu file or within the [Composer][3] GUI experience). 
   * Alternatively, [export][8] your LUIS application and [convert][9] to .lu format or [export][10] your QnA Maker KB to .qna format.
   * See also the [.lu file format][21] to author a .lu file from scratch. 
2. Download Natural Language Representation ([NLR][20]) base Model (will be referred to as the *basemodel*) using the `bf orchestrator:basemodel:get` command. 
   * See `bf orchestrator:basemodel:list` for alternate models. You may need to experiment with the different models to find which performs best for your language domain.
3. Combine the label file .lu from (1) with the base model from (2) to create a *snapshot* file with a .blu extension.
   * Use [`bf orchestrator:create`][16] to create just a single .blu snapshot file for all Lu/json/qna tsv files for dispatch scenario.
-->

<!--This is insufficient information, but again the other article covers this.

### To evaluate the performance of your Orchestrator model

* Create another test .lu file similar to (1) with utterances that are similar but are not identical to the ones specified in the example based .lu definition in (1). This is typically variations on end-user utterances. 
* Test quality of utterance to intent recognition. 
* Examine report to ensure that the recognition quality is satisfactory. See more in [Report Interpretation][22].
* If not, adjust the label file in (1) and repeat this cycle.
-->

## SDK integration

To use Orchestrator in place of Dispatch in an existing bot:

- Create an _Orchestrator recognizer_ and provide it the path to the base model and your snapshot.
- Use the recognizer's _recognize_ method to recognize user input.

### In a C\# bot

- Install the `Microsoft.Bot.Builder.AI.Orchestrator` NuGet package.
- Set your project to target `x64` platform.
- Install the latest supported version of the [Visual C++ redistributable package](https://support.microsoft.com/help/2977003/the-latest-supported-visual-c-downloads).

```csharp
using Microsoft.Bot.Builder.AI.Orchestrator;

// Get Model and Snapshot path.
string modelPath = Path.GetFullPath(OrchestratorConfig.ModelPath);
string snapshotPath = Path.GetFullPath(OrchestratorConfig.SnapshotPath);

// Create OrchestratorRecognizer.
OrchestratorRecognizer orc = new OrchestratorRecognizer()
{
    ModelPath = modelPath,
    SnapshotPath = snapshotPath
};

// Recognize user input.
var recoResult = await orc.RecognizeAsync(turnContext, cancellationToken);
```

### In a JavaScript bot

- Install the `botbuilder-ai-orchestrator` npm package to your bot.

```javascript
const { OrchestratorRecognizer } = require('botbuilder-ai-orchestrator');

// Create OrchestratorRecognizer.
const dispatchRecognizer = new OrchestratorRecognizer().configure({
            modelPath: process.env.ModelPath, 
            snapshotPath: process.env.SnapShotPath
});
// To recognize user input
const recoResult = await dispatchRecognizer.recognize(context);
```

## Composer integration

Orchestrator can be used as recognizer in [Bot Framework Composer][19]. To specify Orchestrator as a dialog recognizer:

1. Enable the Orchestrator feature in Composer's **Application Settings** page.
2. Select **Orchestrator** in the **Recognizer Type** drop-down menu for your bot.
3. Review, evaluate and adjust examples in language data as you would normally for LUIS to ensure recognition quality.

This enables basic intent recognition. For more advanced scenarios follow the steps above to import and tune up routing quality. For more information about recognizers in Composer, see the discussion of [recognizers](https://docs.microsoft.com/composer/concept-dialog#recognizer) with respect to dialogs in Composer.

## Limitations
<!--Assuming this applies to the entire article, not just the Composer integration section.-->

> **Important**:
> Orchestrator is limited to intents only. Entity definitions are ignored and no entity extraction is performed during recognition.
> Only the *default* base model is available to Orchestrator solutions.

See the [School skill navigator](https://github.com/microsoft/BotBuilder-Samples/tree/main/experimental/orchestrator/Composer/01.school-skill-navigator#school-skill-navigator-bot) for an example of using Orchestrator commandlets to improve the quality of a .lu training set and using Composer to build a bot from examples in .lu format.

## Additional Reading

- [Tech overview][18]
- [API reference][14] <!--broken link-->
- [Roadmap](./docs/Overview.md#Roadmap)
- [BF CLI Orchestrator plugin][11]
- [C# samples][12]
- [NodeJS samples][13]
- [BF Orchestrator Command Usage][23]

[1]:https://luis.ai
[2]:https://qnamaker.ai
[3]:https://microsoft.github.io/botframework-solutions/index
[4]:https://en.wikipedia.org/wiki/Transformer_(machine_learning_model)
[5]:https://docs.microsoft.com/azure/bot-service/bot-builder-tutorial-dispatch?tabs=cs
[6]:https://aka.ms/adaptive-dialogs
[7]:https://github.com/microsoft/botframework-cli
[8]:https://github.com/microsoft/botframework-cli/tree/master/packages/luis#bf-luisversionexport
[9]:https://github.com/microsoft/botframework-cli/tree/master/packages/luis#bf-luisconvert
[10]:https://github.com/microsoft/botframework-cli/tree/master/packages/qnamaker#bf-qnamakerkbexport
[11]:https://github.com/microsoft/botframework-cli/tree/beta/packages/orchestrator
[12]:https://github.com/microsoft/BotBuilder-Samples/tree/main/experimental/orchestrator/csharp_dotnetcore
[13]:https://github.com/microsoft/BotBuilder-Samples/tree/main/experimental/orchestrator/javascript_nodejs
[14]:./docs/API_reference.md <!--broken link-->
<!--[15]: TBD/AvailableIndex unused-->
[16]:https://github.com/microsoft/botframework-cli/tree/beta/packages/orchestrator#bf-orchestratorcreate
<!--[17]:TBD/AvailableIndex unused-->
[18]:./docs/Overview.md
[19]: https://docs.microsoft.com/composer/introduction
[20]: https://aka.ms/NLRModels "Natural Language Representation Models"
[21]:https://docs.microsoft.com/azure/bot-service/file-format/bot-builder-lu-file-format "LU file format"
[22]:./docs/BFOrchestratorReport.md "report interpretation"
[23]: ./docs/BFOrchestratorUsage.md "BF Orchestrator command usage"
[24]:https://docs.microsoft.com/azure/bot-service/index-bf-sdk
