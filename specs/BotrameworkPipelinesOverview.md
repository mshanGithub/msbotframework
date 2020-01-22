# Bot Framework Product Pipelines Overview
Most pipelines for building and releasing Bot Framework products are on the [Azure FuseLabs web site](https://fuselabs.visualstudio.com/).

Building and testing packages is done using Azure Pipelines. [Example](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders).

Deploying and releasing packages is done using Azure Release Pipelines. [Example](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&path=%5C).

## AI

AI has no continuous integration or pull request validation builds.

[AI daily builds for DotNet and for JS](https://fuselabs.visualstudio.com/SDK_v4/_build?_a=allDefinitions&path=%5CAI%5C&treeState=XEFJ) are scheduled to run once every night.


## DotNet

The DotNet continuous integration (CI) builds [for DotNet](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=499&_a=summary) and [for JS](https://fuselabs.visualstudio.com/SDK_v4/_build?definitionId=499&_a=summary) are triggered when a pull request is created and when a PR is merged to the main branch.


