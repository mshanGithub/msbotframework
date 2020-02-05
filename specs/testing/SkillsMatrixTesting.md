# Automated Testing of Skills
Defines the short term goals around the automated testing of skills. 

# Goal
Automate the testing matrix for Bot/Skill interactions. The full testing matrix runs every night. Failures result in emails. 

# Version of the Botbuilder SDK
1. For each test run, the latest *preview* Bot Builder SDK should be consumed from the MyGet.

2. Alternatly, a specific version of the BotBuilder SDK could be specified via an Azure Devops variable. 

# Testing Matrix
The following testing matrix needs to be fully tested
|   |        |    **Skill**        |        |   |
|---|--------|----|------------|--------|---|
|   |        | C# | Javascript | Python |   |
|   | C#     |  1  |     2       | 3       |   |
| **HOST**  | JS     |  4  |5            | 6       |   |
|   | Python | 7   |   8         |  9      |   |

1. C# Bot acting as consumer, calling a C# Skill
2. C# Bot acting as consumer, calling a JS Skill
3. C# Bot acting as consumer, calling a Pyton Skill
4. JS Bot acting as consumer, calling a C# Skill
5. JS Bot acting as consumer, calling a JS Skill
6. JS Bot acting as consumer, calling a Python Skill
7. Python Bot acting as consumer, calling a C# Skill
8. Python Bot acting as consumer, calling a JS Skill
9. Python Bot acting as consumer, calling a Python Skill

# Tests
V0 of the tests is an Echo. The Consumer bot will send a Message containing a GUID to the skill, and the skill encho that back. 

# Bots we need

3 Bots (1 in C#, one in JS, one in Python). The Bot picks up "Skill" or "Consumer" from the ENV variables, which are set as part of the bot's deployment. 

# Repo
All bots are stored in the same git repo. The repo is hosted on Azure Devops, at the following location:
https://fuselabs.visualstudio.com/DefaultCollection/SDK_v4/_git/SkillsFunctionalTesting

Note: We are not using GitHub for this, as this repo needs to be public. We may revisit this decision, as it's easier for external vendors to work in a GitHub repo. 

# CI/CD and YAML
Each test case in the matrix has a dedicated YAML file and corresponding build in Azure Devops.

The testing is kicked off each night via a trigger from our nightly CI/CD build of the SDK. 

# Problems to Solve:
1. Picking up the latest SDK version from MyGet. Unclear how best to do this using Azure Devops.
2. How best to kick-off the build? Is there an easy way to kick off the build once each of the C# / JS / Python nightly builds are complete? 
3. How to apply to Gov Datacenter? 
4. Best way to register / manage / deploy the bots? 
5. What Azure Subscription? 
6. Setup/Teardown each night, re-using the AppID + Password? Use Certs? 
7. Which Azure Resoure Group?


