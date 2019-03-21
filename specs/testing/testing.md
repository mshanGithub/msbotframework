# Testing bots (DRAFT)

## Abstract

Testing a conversational application is often complex and it involves several different layers. The guidance, samples and tools required to test bots are diverse and scattered in different projects, documents and samples.  

A conversational application is made of of code, cognitive models and several other external services.

This document describes the different types of tests that can be employed in developing a conversational solution and outlines the requirements that we should address to make easier to test bots throughout their life cycle.

## Table of Contents

<!-- TOC -->

- [Testing bots (DRAFT)](#testing-bots-draft)
  - [Abstract](#abstract)
  - [Table of Contents](#table-of-contents)
  - [Tenets](#tenets)
  - [Test types](#test-types)
  - [Gaps and new functionality](#gaps-and-new-functionality)
  - [Testing requirements](#testing-requirements)
    - [Unit testing](#unit-testing)
    - [Natural Language Processing](#natural-language-processing)
    - [Functional Tests](#functional-tests)
    - [Load Tests](#load-tests)
    - [Health checks](#health-checks)
    - [Documentation](#documentation)

<!-- /TOC -->

## Tenets

- It should be easy and intuitive to create coded unit tests
- It should be possible to write unit tests that don't rely on actual cognitive services calls
- It should be possible to validate the language models for a bot using tests
- Test should take a relative short time to run
- Product owners should be able to write functional and language tests without having to write code
- Tests should be "scriptable" and it should be possible to execute them from the command line, the IDE of choice and/or Azure DevOps

## Test types

There are several layers involved in testing bots

- **Unit Tests**. Are written by the developers and normally executed as part of the Continuous Integration build pipeline. Their main purpose is to ensure that the coded logic for a bot executes as expected.
- **Natural Language Understanding Tests**. Are written by developers or business owners and can be executed as part of the CI pipeline or when the language model for the bot changes. Their main purpose is to ensure that the understands what the user is traying to say and that there are no regressions in the language models when they are extended or modified. Typically, these tests target LUIS and QnAMaker.
- **Functional tests** or End to End tests,

## Gaps and new functionality

This section describes what needs to be done in order to create and execute tests on bots

- TestAdapter documentation, the TestAdapter is used in the bot builder code to test the different functionality in the SDK but it is not thoroughly documented. It would be nice to have additional documentation on how to use TestAdapter, Fakes and DI to create and use a bot test.
- Test Project Sample, BotBuilder samples doesn't include any examples on how to test a bot, it would be nice to write a Test Project for one of the non trivial bots in Builder Samples 'TBD' so developers and see a test project in action for reference.
- Batch testing APIs for LUIS, TestAdapter and FakeLuisRecognizer should help developers write tests that validate the bot functionality works without using LUIS, the natural language understanding models used by a bot evolve independently of the underlying code, the LUIS portal provides a batch testing UI but it is not scriptable and can't be used in continuous integration or continues delivery. We need a batch testing API that can be invoked and asserted from the CLI to ensure the key utterances and entities are still resolved after model changes.
- Extend the Build Builder templates to include a Test template.
- Azure DevOps tools, we should provide tools 
- Transcript driven tests. 
- Deterministic VS random responses
- Health checks?
  - AppInsight alerts
- Run human logs against existing bot

## Testing requirements

This section outlines the requirements for testing bots using the format "As a ***role*** I would like to ***requirement*** so I can ***benefit***".

### Unit testing

- As a developer, I would like to be able to use a FakeLuisRecognizer so I can programmatically set the intents understood by my bots so I don't have to invoke the LUIS service.
- As a developer, I would like to be able use a FakeQnaMaker service so I can programmatically set the QnA responses that will be returned by my bot.
- As a developer, I would like to see a transcript of my test so I can analyze how bot responded.
    **Note**: test transcripts should be logged as .transcript files so they can be analyzed in Bot Emulator and executed using "transcript tests" at a later date if needed.
- As a developer, I would like to have a test template project so I can quickly get started with writing tests for my bot conversations.
- As a developer, I would like to see a good example that shows me how to write effective tests for conversations
- As a developer, I would like to be able to assert bot responses independently if the Text or Speak being returned
    In many cases, the bot responses are randomized or contain dynamic data, it those cases, I would like to assert that response is if type XYZ independently of the actual text being returned

### Natural Language Processing

- As an NLP engineer, I would like to be able to run batch tests programmatically against my language models to validate the utterances and entities my bot expects get resolved as expected.
- As an NLP engineer, I would like to have a way of running a batch test against my updated LUIS model before it gets published to ensure I don't break the bot.

### Functional Tests

- As a tester, I would like to be able to create transcript files and execute them against a deployed bot to ensure the conversation goes as expected

### Load Tests

- TODO

### Health checks

- TODO

### Documentation

- As a developer, I would like to see some documentation to understand how to use TestAdapter and the Fakes provided by the framework so I can write unit tests for my bot
- As a DevOps engineer, I would like to see some documentation on how to integrate tests in the Azure DevOps CI and CD pipelines so I can ensure the bot doesn't break before I publish it.