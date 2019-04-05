
# Testing bots (DRAFT) <!-- omit in toc -->

# Abstract <!-- omit in toc -->

Testing a conversational application is often complex and it involves several different layers. The guidance, samples and tools required to test bots are diverse and scattered in different projects, documents and samples.  

A conversational application is made of of code, cognitive models, one or many taget channels and several other external services.

This document describes the different types of tests that can be used in a conversational solution and outlines the requirements that we should address to make easier to test bots throughout their life cycle.

![Bot SDLC](images/bot-service-overview.png)

## Table of Contents

- [Tenets](#tenets)
- [Testing scenarios](#testing-scenarios)
  - [Core Scenarios](#core-scenarios)
    - [Dev Scenarios](#dev-scenarios)
    - [Language models scenarios](#language-models-scenarios)
    - [DevOps scenarios](#devops-scenarios)
    - [Release scenarios](#release-scenarios)
    - [Support scenarios](#support-scenarios)
  - [Advanced Scenarios](#advanced-scenarios)
  - [Documentation](#documentation)
- [Test types](#test-types)

# Tenets

1. It should be easy and intuitive to create coded unit tests.
2. It should be possible to write unit tests that don't rely on actual cognitive services calls.
3. It should be possible to validate the language models for a bot using tests.
4. Tests should take a relative short time to run.
5. Product owners should be able to write functional and language tests without having to write code.
6. Tests should be "scriptable" and it should be possible to execute them from the command line, the IDE of choice and/or as part of the Continuous Integration/Continuous Delivery pipelines from Azure DevOps pipelines from [Azure DevOps](https://azure.microsoft.com/en-us/services/devops/).
7. Whenever a test fails, it should be easy to understand the cause of the failure and the test output should be clean of stack traces and tech jargon (although those may still be available on demand).
8. It should be possible to possible to run tests throughout the entire bot development life cycle: Build, Test, Deployment and Operations.
9. It should be possible to test bots using different languages and locales.

# Testing scenarios

- For a list of the main scnarios for testing see the [Bot Testing](https://github.com/Microsoft/BotBuilder/projects/11?fullscreen=true) project.
- For a list of the bot tesing scenarios targeted to v4.5 see the DCRs posted under [4.5 Preview](https://github.com/Microsoft/BotBuilder/milestone/1)


# TODO (All the scenarios below will be moved to issues in the Bot testing project)

This section outlines the requirements for testing bots using the format "As a ***role*** I would like to ***requirement*** so I can ***benefit***".

## Core Scenarios

1. As a dev, I should be able to write unit tests for my bot that are run as part of builds. 
2. As a DevOps engineer, I run Functional E2E tests against my bot. 
3. As a Release Engineer, I run continuous tests against my live bot to measure health. 
4. As a Support Engineer, I have the tools and data needed to diagnose my bot when problems arise. 

### Dev Scenarios

1. As a developer, I would like to be able to use a FakeLuisRecognizer so I can programmatically set the intents understood by my bots without having to invoke the LUIS service.
2. As a developer, I would like to be able use a FakeQnaMaker service so I can programmatically set the QnA responses that will be returned by my bot.
3. As a developer, I would like to see a transcript of my tests so I can analyze how the bot responded.

    **Note**: test transcripts should be logged as .transcript files so they can be analyzed in Bot Emulator and executed using "transcript tests" at a later time if needed.
4. As a developer, I would like to have a test project template so I can quickly get started with writing unit tests for my bot conversations.

   The initial test project template should provide base code and stubs for:

   - A TestBot that gets initialized with the required fake services, fake middlewares, etc.
   - The base classes required to add other services for my test bot using the same dependency injection framework as the one used by my target language.
   - A base TestFlow that I can use to create unit test for bot dialogs.
   - A base configuration file that I can use to configure my tests.

5. As a developer, I would like to see a good example that shows me how to write effective tests for conversations.

    The BotBuilder Samples repo doesn't include any examples on how to test a bot, it would be nice to have a Test Project for one of the non trivial bots there (maybe CoreBot?) so developers can see a test project in action.
6. As a developer, I would like to be able to assert bot responses independently of the Text or Speak properties being returned.

    In many cases, the bot responses are randomized or contain dynamic data, in those cases, I would like to assert that the response is of type *XYZ* independently of the actual text being returned.

    It would be also good to be able to assert the InputHint property of the response.

7. As a developer, I would like to have any easy way of validating that complex responses that use cards and attachments are returned correctly.
8. As a developer, I would like to be able to run tests that emulate the target channels for my bot so I can make sure it will work once it is deployed.
9. As a Security engineer, I would like to use [Microsoft Security Risk Detection](https://www.microsoft.com/en-us/security-risk-detection/) to find security bugs in my bot

### Language models scenarios

1. As an NLU engineer, I would like to be able to run batch tests scripts against my language models to validate the utterances and entities my bot needs to work get resolved as expected.

    TestAdapter and FakeLuisRecognizer should help developers write tests that validate the bot functionality works without using LUIS, the natural language understanding models used by a bot evolve independently of the underlying code, the LUIS portal provides a batch testing UI but it is not scriptable and can't be used in continuous integration or continuous delivery. We need a batch testing API that can be invoked and asserted from the CLI to ensure the key utterances and entities are still resolved after the model changes.
2. As an NLU engineer, I would like to have a way of running a batch test against my updated LUIS model before it gets published to ensure I don't break the bot.
3. As an NLU engineer, I would like to be able to run batch tests against my QnAMaker models to ensure the bot is returning the correct answers.
4. As an NLU engineer, I would like to run tests in different languages so I can ensure that my bot will work as expected in the targeted locales.
5. As a DevOps engineer, I would like to have a tools in the Azure Marketplace that will allow me to configure and run NLU tests from the CI/CD pipeline.
6. As a DevOps engineer, I want to publish the live/latest NLU test results per-model and per-bot shared in a way so that I can point non-developers to a central location at any time for active status.
7. As a DevOps engineer, I want to run NLU tests in any language against any bot at any time I want so that I can see how the model performs against different bots without disrupting deployments or live bots.
8. As an NLU engineer, I want an easy to use website to create, read, update, and delete test transcript files so that I can collaborate with other non-developers on these transcripts in a managed/source-controlled way without having to download an IDE or know a language framework.
9. As an NLU engineer, I would like to be able to test a bot using speech to ensure my custom speech models (if any) are working correctly for the targeted locales.

### DevOps scenarios

1. As a DevOps engineer, I want to have clear hinting when test failures occur so that I can notify the right team member to resolve the problem in the right way (should I get a developer involved, an NLU engineer, or a sys-admin?).
2. As a tester, I would like to be able to create a work item in Azure DevOps from the Bot Framework emulator when I run a test that fails.

   The emulator should create a work item and attach the transcript for the session so the developer can repro the issue.

### Release scenarios

1. As a tester, I would like to be able to create transcript files and execute them against a deployed bot to ensure the conversation goes as expected.
2. As a tester, I would like to be able to record an exploratory test session and be able to submit a bug with a transcript of that session so the developers can run the repro and fix the error.

### Support scenarios

1. As a Support engineer, I'd like to be able to execute periodic tests that would alert me if the bot or related services services are not working or they are degraded.
2. As a Support engineer, I'd like to be able to create a work item in Azure DevOps when a production bot throws an alert.
   
   The work item should contain a transcript of the conversation that triggered the failure (if available) and other technical information like stack traces and error messages.

## Advanced Scenarios

1. I need to A/B test and Flight my bot in production. This includes language and related models.
2. As a tester, I would like to be able to load test my bot to ensure it can handle the expected number of users without overloading the Bot Service infrastructure.

## Documentation

1. As a developer, I would like to see some documentation to understand how to use TestAdapter and the Fakes provided by the framework so I can write unit tests for my bot.

    The TestAdapter is used in the bot builder code to test the different functionality in the SDK but it is not thoroughly documented. It would be nice to have additional documentation on how to use TestAdapter, Fakes and DI to create and use a bot test.
2. As a DevOps engineer, I would like to see some documentation on how to integrate tests in the Azure DevOps CI and CD pipelines so I can ensure the bot doesn't break before I publish it.

# Test types

There are several test types involved in bot development and operations:

- **Unit Tests**
  
  Are written by developers and normally executed as part of the Continuous Integration build pipeline. 
  
  Their main purpose is to ensure that the coded logic for a bot executes as expected.

- **Natural Language Understanding Tests**

    Can be written by developers, NLU engineers or Product owners and can be executed as part of the CI pipeline or when the language model for the bot changes. Their main purpose is to ensure that the bot understands what the user is asking and that there are no regressions in the language models when they are extended or modified.

    These tests typically target LUIS and QnAMaker.

- **Language Generation tests**

   TODO (write description).

- **Functional tests**

    Also called End to End tests, these tests target the entire bot and its dependent services. Non-technical audiences should be able to write and execute these type of tests.

- **Load Tests**

    These tests validate that the solution will work under the expected user load. They are typically written by testers and developers and cover end to end scenarios under a variable set of load conditions.

    **Note**: VS 2019 will be the last version of Visual Studio that will provide load testing tools. For customers requiring load testing tools, Microsoft is recommending using alternate load testing tools such as Apache JMeter, Akamai CloudTest, Blazemeter (see [Changes to load test functionality in Visual Studio](https://docs.microsoft.com/en-us/azure/devops/test/load-test/overview?view=azure-devops)).

- **Health checks**

    TODO (write description).

- **UI Testing**

    TODO (write description and include considerations for channels).

- **Flighting**

    TODO (write description).

- **A/B Testing**

    TODO (write description).
