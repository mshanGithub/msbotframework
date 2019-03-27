
# Testing bots (DRAFT) <!-- omit in toc -->

# Abstract <!-- omit in toc -->

Testing a conversational application is often complex and it involves several different layers. The guidance, samples and tools required to test bots are diverse and scattered in different projects, documents and samples.  

A conversational application is made of of code, cognitive models and several other external services.

This document describes the different types of tests that can be used in a conversational solution and outlines the requirements that we should address to make easier to test bots throughout their life cycle.

## Table of Contents

- [Tenets](#tenets)
- [Test types](#test-types)
- [Testing requirements](#testing-requirements)
  - [Unit testing](#unit-testing)
  - [Natural Language Processing](#natural-language-processing)
  - [Functional Tests](#functional-tests)
  - [Load Tests](#load-tests)
  - [Health checks](#health-checks)
  - [Documentation](#documentation)

# Tenets

1. It should be easy and intuitive to create coded unit tests.
2. It should be possible to write unit tests that don't rely on actual cognitive services calls.
3. It should be possible to validate the language models for a bot using tests.
4. Test should take a relative short time to run.
5. Product owners should be able to write functional and language tests without having to write code.
6. Tests should be "scriptable" and it should be possible to execute them from the command line, the IDE of choice and/or Azure DevOps.
7. Whenever a test fails, it should be easy to understand the cause of the failure and the test output should be clean of stack traces and tech jargon (although those may still be available on demand).

# Test types

There are several test types involved in bot development and operations:

- **Unit Tests**
  
  Are written by developers and normally executed as part of the Continuous Integration build pipeline. 
  
  Their main purpose is to ensure that the coded logic for a bot executes as expected.

- **Natural Language Understanding Tests**

    Can be written by developers, NLP engineers or Product owners and can be executed as part of the CI pipeline or when the language model for the bot changes. Their main purpose is to ensure that the bot understands what the user is asking and that there are no regressions in the language models when they are extended or modified.

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

    TODO (write description).

- **Flighting**

    TODO (write description).

- **A/B Testing**

    TODO (write description).


# Testing requirements

This section outlines the requirements for testing bots using the format "As a ***role*** I would like to ***requirement*** so I can ***benefit***".

## Unit testing

1. As a developer, I would like to be able to use a FakeLuisRecognizer so I can programmatically set the intents understood by my bots without having to invoke the LUIS service.
2. As a developer, I would like to be able use a FakeQnaMaker service so I can programmatically set the QnA responses that will be returned by my bot.
3. As a developer, I would like to see a transcript of my tests so I can analyze how the bot responded.

    **Note**: test transcripts should be logged as .transcript files so they can be analyzed in Bot Emulator and executed using "transcript tests" at a later time if needed.
4. As a developer, I would like to have a test template project so I can quickly get started with writing tests for my bot conversations.
   
   The initial test project template should provide base code and stubs for:

   - A TestBot that gets initialized with the required fake services, make middlewares, etc.
   - The base classes required to add other services for my test bot using the same dependency injection framework as the one used by my target language.
   - A base TestFlow that I can use to create unit test for bot dialogs.
   - A base configuration file that I can use to configure my tests.

5. As a developer, I would like to see a good example that shows me how to write effective tests for conversations.

    The BotBuilder Samples repo doesn't include any examples on how to test a bot, it would be nice to have a Test Project for one of the non trivial bots there (maybe CoreBot?) so developers can see a test project in action.
6. As a developer, I would like to be able to assert bot responses independently of the Text or Speak properties being returned.

    In many cases, the bot responses are randomized or contain dynamic data, in those cases, I would like to assert that the response is of type *XYZ* independently of the actual text being returned.

    It would be also good to be able to assert the InputHint property of the response.

## Natural Language Processing

1. As an NLP engineer, I would like to be able to run batch tests programmatically against my language models to validate the utterances and entities my bot expects get resolved as expected.

    TestAdapter and FakeLuisRecognizer should help developers write tests that validate the bot functionality works without using LUIS, the natural language understanding models used by a bot evolve independently of the underlying code, the LUIS portal provides a batch testing UI but it is not scriptable and can't be used in continuous integration or continuous delivery. We need a batch testing API that can be invoked and asserted from the CLI to ensure the key utterances and entities are still resolved after the model changes.
2. As an NLP engineer, I would like to have a way of running a batch test against my updated LUIS model before it gets published to ensure I don't break the bot.

## Functional Tests

1. As a tester, I would like to be able to create transcript files and execute them against a deployed bot to ensure the conversation goes as expected

## Load Tests

1. As a tester, I would like to be able to load test my bot to ensure it can handle the expected number of users without overloading the Bot Service infrastructure.

## Health checks

1. As a DevOps engineer, I'd like to be able to execute tests that would alert me if any of the services used by bot are not working or they are degraded

## Documentation

1. As a developer, I would like to see some documentation to understand how to use TestAdapter and the Fakes provided by the framework so I can write unit tests for my bot.

    The TestAdapter is used in the bot builder code to test the different functionality in the SDK but it is not thoroughly documented. It would be nice to have additional documentation on how to use TestAdapter, Fakes and DI to create and use a bot test.
2. As a DevOps engineer, I would like to see some documentation on how to integrate tests in the Azure DevOps CI and CD pipelines so I can ensure the bot doesn't break before I publish it.