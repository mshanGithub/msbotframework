# Bot Framework SDK Product Pipelines Overview
Pipelines for building and releasing Bot Framework SDK products are found in the [SDK_v4 project which is Microsoft only](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders) or the [SDK_Public project which is public](https://fuselabs.visualstudio.com/SDK_Public/_build?view=folders) on the [Azure FuseLabs web site](https://fuselabs.visualstudio.com/).

Packages are built and tested using Build pipelines [here](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders) and [here](https://fuselabs.visualstudio.com/SDK_Public/_build?view=folders).

Packages are deployed and released using Release pipelines [here](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&path=%5C) and [here](https://fuselabs.visualstudio.com/SDK_Public/_release?_a=releases&view=all&path=%5C)

## AI

**[AI daily builds for DotNet and for JS](https://fuselabs.visualstudio.com/SDK_v4/_build?_a=allDefinitions&path=%5CAI%5C&treeState=XEFJ)** are scheduled to run once a night.

(AI does not have continuous integration or pull request validation builds.)

## DotNet (C#)

**DotNet continuous integration pull request (CI-PR) builds [for Windows](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=499&_a=summary)** and **[for Linux](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=660&_a=summary)** are triggered when a pull request is created or when changes are merged to the main branch. Both builds run unit tests. In addition, the Windows build runs API compatibility validations. 

**Functional test builds** deploy a test bot to Azure and test the deployed bot. A [nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary) triggers functional tests [on a Windows platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=88) and [on a Linux platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=87). A non-signed [test setup build](https://fuselabs.visualstudio.com/SDK_v4/_build/index?definitionId=740&_a=completed) runs whenever code changes are merged to master, and also nightly. It too triggers functional tests [on a Windows platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=91) and [on a Linux platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=92).

A **[Nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary)**... 

## Java

## JS

## Python

## Samples

## Tools

## VSIX

## WeChat
