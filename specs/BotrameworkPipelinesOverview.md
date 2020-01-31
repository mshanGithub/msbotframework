# Bot Framework SDK Product Pipelines Overview
Pipelines for building and releasing Bot Framework SDK products are found in the [SDK_v4 project](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders) (which is Microsoft only) or the [SDK_Public project](https://fuselabs.visualstudio.com/SDK_Public/_build?view=folders) (which is public facing) on the [Azure FuseLabs web site](https://fuselabs.visualstudio.com/).

Packages are built and tested using Build pipelines [here](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders) and [here](https://fuselabs.visualstudio.com/SDK_Public/_build?view=folders).

Packages are deployed and released using Release pipelines [here](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&path=%5C) and [here](https://fuselabs.visualstudio.com/SDK_Public/_release?_a=releases&view=all&path=%5C)

## AI

**[AI daily pipelines for DotNet and for JS](https://fuselabs.visualstudio.com/SDK_v4/_build?_a=allDefinitions&path=%5CAI%5C&treeState=XEFJ)** are scheduled to run once a night.

AI does not have continuous integration or pull request validation pipelines.

## DotNet (C#)

**DotNet continuous integration pull request (CI-PR) pipelines [for Windows](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=499&_a=summary)** and **[for Linux](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=660&_a=summary)** are triggered when a pull request is created or when changes are merged to the main branch. 
Those builds run unit tests. The Windows pipeline additionally runs API compatibility validations. 

**Functional test pipelines** deploy a test bot to Azure, then run tests against the bot. A [nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary) triggers functional test builds [on a Windows platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=88) and [on a Linux platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=87). A non-signed [test setup build](https://fuselabs.visualstudio.com/SDK_v4/_build/index?definitionId=740&_a=completed) runs whenever code changes are merged to master, and also nightly. It too triggers functional tests [on a Windows platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=91) and [on a Linux platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=92).

A **[Nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary)** produces preview versions which are automatically published to MyGet. It can optionally create release versions if queued by hand, which are also automatically pushed to MyGet. Releases may be pushed to Nuget.org by manually queuing [this release pipeline]().

## Java

## JS

## Python

## Samples

## Tools

## VSIX

## WeChat
