# Bot Framework SDK Product Pipelines Overview
Most pipelines for building and releasing Bot Framework SDK products are in the [SDK_v4 project](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders) or the [SDK_Public project](https://fuselabs.visualstudio.com/SDK_Public/_build?view=folders) on the [Azure FuseLabs web site](https://fuselabs.visualstudio.com/).

Building and testing packages is done using Azure Pipelines. [Example here.](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders)

Deploying and releasing packages is done using Azure Release Pipelines. [Example here.](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&path=%5C)

## AI

AI has no continuous integration or pull request validation builds.

**[AI daily builds for DotNet and for JS](https://fuselabs.visualstudio.com/SDK_v4/_build?_a=allDefinitions&path=%5CAI%5C&treeState=XEFJ)** are scheduled to run once a night.


## DotNet (C#)

A **[DotNet continuous integration (CI) build for Windows](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=499&_a=summary)** and **[for Linux](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=660&_a=summary)** are triggered when a pull request is created or when changes are merged to the main branch. Both builds run unit tests. In addition, the Windows build runs API compatibility validations. 

**Functional test builds** deploy a test bot to Azure and then test the deployed bot. A [nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary) triggers functional tests [on a Windows platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=88) and [on a Linux platform](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&definitionId=87). A [non-signed setup build](https://fuselabs.visualstudio.com/SDK_v4/_build/index?definitionId=740&_a=completed) runs whenever code changes are merged to master, and also nightly.

A **[Nightly signed build](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=739&_a=summary)** 

## Java

## JS

## Python

## Samples

## Tools

## VSIX

## WeChat
