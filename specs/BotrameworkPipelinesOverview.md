# Bot Framework Build Pipelines Overview
Most pipelines for building and releasing Bot Framework products are on the [Azure FuseLabs web site](https://fuselabs.visualstudio.com/).

Building and testing packages is normally done using Azure Pipelines, for example [here](https://fuselabs.visualstudio.com/SDK_v4/_build?view=folders).

Deploying or releasing packages is normally done using Azure Release Pipelines, for example [here](https://fuselabs.visualstudio.com/SDK_v4/_release?_a=releases&view=all&path=%5C).

## AI pipelines

The AI continuous integration (CI) build is [here]()

|  Product | Pipeline | Description |
|:--------:|-------------|------|
|    AI    | BotBuilder-AI-JS-generator-master-daily | JS daily              |
|          | BotBuilder-AI-JS-libs-master-daily      | JS npm packages daily |
|          | BotBuilder-AI-JS-SkillsCli-master-daily | |
|          | BotBuilder-DotNet-AI-Signed-4.6-daily   | v4.6 DotNet packages  |
|          | BotBuilder-DotNet-AI-Signed-4.7-daily   | v4.7 DotNet packages  |
|          | BotBuilder-DotNet-AI-Signed-master      | master branch DotNet packages  |
|          | BotBuilder-DotNet-AI-Signed-next        | next branch DotNet packages  |
|          | BotBuilder-NetCoreTemplate-VASkills-daily | |
| DotNet   | BotBuilder-DotNet-4.0-CI-PR             |  |
|          | BotBuilder-DotNet-4.0-CI-PR             |  |
|          | BotBuilder-DotNet-4.1-CI-PR             |  |
|          | BotBuilder-DotNet-4.2-CI-PR             |  |
|          | BotBuilder-DotNet-4.5-CI-PR             |  |
|          | BotBuilder-DotNet-4.5-daily             |  |
|          | BotBuilder-DotNet-Functional-Tests-Setup-yaml |  |
|          | BotBuilder-DotNet-master-CI-PR-(MacLinux) |  |
|          | BotBuilder-DotNet-master-CI-PR          |  |
|          | BotBuilder-DotNet-Signed-yaml           |  |
