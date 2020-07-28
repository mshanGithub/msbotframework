
# Bot Framework Labeling Guidelines (DRAFT) <!-- omit in toc -->

Labels help us organize and prioritize work. We use the same set of labels across the different sections in a repository, such as issues and pull requests. It is important to keep the labels consistent so that we can maintain a standard workflow. Standardized labels also help us query useful data from repos to collect customer feedback for analysis.

This article describes the naming conventions and categorization guidelines of the labels in the Bot Framework repositories to tag issues and PRs. It also provides guidance on how to use them.

This document includes:

- [Labels naming conventions and format](#labels-naming-conventions-and-format)
  - [Wording](#wording)
  - [Format](#format)
  - [Label usage](#label-usage)
  - [Label colors](#label-colors)
- [Label categories](#label-categories)
  - [Status](#status)
  - [Area](#area)
  - [Type](#type)
  - [Priority](#priority)
  - [Community](#community)
  - [Size](#size)
  - [Automation](#automation)
- [DRI labels](#dri-labels)
- [Label creation and approval](#label-creation-and-approval)
- [Repo specific labels](#repo-specific-labels)
- [Release labels](#release-labels)

# Labels naming conventions and format

There is no official GitHub documentation as to how we should name the labels. This section provides some basic guidelines and examples, which we follow in this article.  

## Wording

- Keep names clear and concise.
- Keep names short. Use acronyms when necessary and provide a description.
- Always to provide a description for a label so user know when to use them.
- Multiple-word labels should use sentence case (i.e.: use `Area: Functional tests` instead of `Area: Functional Tests`).

## Format

In order to be able to parse the different labels in reports we use a colon followed by a whitespace (`": "`) to separate the categories and subcategories in a label.

The general format of a label is:

```bash
Main category: Some subcategory
```

There's no limit to the number of subcategories in a label but it is recommended to avoid creating more than two levels.

## Label usage

Some labels can be applied multiple times to an issue and some others shouldn't. For example, an issue can be labeled as `Area: Adaptive` **and** `Area: Skills` but it should **only** have one priority label `P0` or `P1`. Check the intended usage for each label category in the sections below.

GitHub doesn't provide a way of restricting or validating the labels applied to an issue so this will be a manual check that the person updating the issue should perform.

## Label colors

Labels should use the same colors across repos to improve readability and in most cases, all labels within the same main category should use the same color.

# Label categories

We use labels to sort and describe issues, pull requests, and more. It is a good practice to categorize the issues with comprehensible types so that we can easily identify them for different purposes.

This section describes the main label categories being used in the SDK repos:

| Category | Description | Usage | Example |
|---|---|---|---|
|[Status](#status)|Describes the status of the issue throughout its lifecycle.| Single |`needs-triage`|
|[Area](#area)|Defines a functional area or feature of the product for the issue.| Multiple |`Area: Skills`|
|[Type](#type)|Provides additional information on the issue type.| Single |`bug`|
|[Priority](#priority)|The priority for the issue.| Single |`P0`|
|[Community](#community)|Used to describe community related issues.| Single |`Community: Help wanted`|
|[Size](#size)|Provides an estimate for the level of effort required to resolve the issue.| Single |`Size: M`|
|[Automation](#automation) | Labels used to trigger GitHub actions and workflows.| Single |`Automation: No parity`|
|[DRI](#dri)|This is a special set of labels used for DRI tracking and reporting on issues created by customers.| Multiple |`Bot Services`|

## Status

Use these labels for providing information on the progress of the issue. The status label is used to triage and track issues throughout its lifecycle.

Color: This subcategory uses different colors for each label.

| Name | Description | Color | Example |
|---|---|:-:|:--|
|Draft| The issue definition is still being worked on and it is not ready to start development. Once the issue is ready the status should be changed to `needs-triage`,`approved` or `backlog`.| ![#ededed](https://via.placeholder.com/15/ededed/000000?text=+) `#ededed` | `draft` |
|New| The issue has just been created and it has not been reviewed by the team. Once the issue is reviewed the status can be changed to `approved`, `backlog`, `needs-author-feedback` or just closed.| ![#f7ffa3](https://via.placeholder.com/15/f7ffa3/000000?text=+) `#f7ffa3` | `needs-triage` |
|Needs author information| The issue as described is incomplete or not well understood. It is waiting for further information before it can continue.| ![#f7ffa3](https://via.placeholder.com/15/f7ffa3/000000?text=+) `#f7ffa3` | `needs-author-feedback` |
|Needs team information| The issue has a comment from the author and needs SDK Team or service team’s attention.| ![#f7ffa3](https://via.placeholder.com/15/f7ffa3/000000?text=+) `#f7ffa3` | `needs-team-attention` |
|Approved| The issue has been reviewed and is ready to start working on it, it will be added to the work queue in the current iteration. | ![#0e8a16](https://via.placeholder.com/15/0e8a16/000000?text=+) `#0e8a16` | `approved` |
|Backlog| The issue is out of scope for the current iteration but it will be evaluated in a future release. | ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `backlog` |
|Blocked| Current progress is blocked on something else. | ![#ff8c00](https://via.placeholder.com/15/FF8C00/000000?text=+) `#ff8c00` | `blocked` |
|Stale| The issue hasn't been updated in a long time and will be automatically closed. | ![#ededed](https://via.placeholder.com/15/ededed/000000?text=+) `#ededed` | `stale` |

### Repo specific labels for status <!-- omit in toc -->

Do not create create repo specific labels for this category.

## Area

These labels are used to map issues to a feature or functional area in the product. This category informs several reports, the labels in this category should only be created by feature and product owners and documented in the table below.

All the issues labeled as `approved` should have at least one of these labels before they can be worked on.

### Area labels <!-- omit in toc -->

Color: All the labels in this category should use ![#1d76db](https://via.placeholder.com/15/1d76db/000000?text=+) `#1d76db`

| Name | Description | Example |
|---|---|---|
|Adaptive| TODO | `Area: Adaptive` |
|Adaptive Expressions| TODO | `Area: Adaptive expressions` |
|AI-LUIS| TODO | `Area: AI-LUIS` |
|AI-QnAMaker| TODO | `Area: AI-QnAMaker` |
|Authentication| TODO | `Area: Authentication` |
|Custom Adapters| TODO | `Area: Custom adapters` |
|Docs| TODO | `Area: Docs` |
|Functional Tests| TODO | `Area: Functional tests` |
|LG| TODO | `Area: LG` |
|Samples| TODO | `Area: Samples` |
|Schema| TODO | `Area: Schema` |
|Skills| TODO | `Area: Skills` |
|Streaming| TODO | `Area: Streaming` |
|Teams| TODO | `Area: Teams` |
|Telemetry| TODO | `Area: Telemetry` |
|Testing Framework| TODO | `Area: Testing framework` |

### Repo specific labels for area <!-- omit in toc -->

It is OK to create repo specific sub categories for area, for example, composer may need `Area: UX design` and BF CLI may need `Area: BF config`. 

## Type

Use these labels describe the type of the issue.

Color: This subcategory uses different colors for each label.

| Name | Description | Color | Example |
|---|---|:-:|:--|
|Bug| Indicates an unexpected problem or an unintended behavior.| ![#d73a4a](https://via.placeholder.com/15/d73a4a/000000?text=+) `#d73a4a` | `bug` |
|Feature request|  A request for new functionality.or an enhancement to an existing one.| ![#8f31ed](https://via.placeholder.com/15/8f31ed/000000?text=+) `#8f31ed` | `feature request` |
|Question| A question from customers that needs further clarification or discussion.| ![#8f31ed](https://via.placeholder.com/15/8f31ed/000000?text=+) `#8f31ed` | `question` |
|Parity| The issue describes a gap in parity between two or more platforms.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `parity` |
|Technical debt| The issue involves refactoring existing code to make it easier to maintain, follow best practices, improve test coverage, etc.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `technical debt` |
|Team agility| An issue targeted to reduce friction to SDK's development process.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `team agility` |

### Repo specific labels for type <!-- omit in toc -->

Do not create create repo specific labels for this category.

## Priority

Describes the priority of the issue. This label is required for any issue that is in scope for an iteration. High priority issue will be addressed first. All the issues with `Status: Approved` should have at least one of these labels before they can be worked on.

Color: This subcategory uses different colors for each label.

| Name | Description | Color | Example |
|---|---|:-:|:--|
|P0| Must Fix.  Release-blocker | ![#ee0701](https://via.placeholder.com/15/ee0701/000000?text=+) `#ee0701` | `P0` |
|P1| Painful if we don't fix, won't block releasing | ![#ff8c00](https://via.placeholder.com/15/FF8C00/000000?text=+) `#ff8c00` | `P1` |
|P2| Nice to have | ![#ffff00](https://via.placeholder.com/15/ffff00/000000?text=+) `#ffff00` | `P2` |
|P3| Won't fix | ![#bfd4f2](https://via.placeholder.com/15/bfd4f2/000000?text=+) `#bfd4f2` | `P3` |

### Repo specific labels for priority <!-- omit in toc -->

Do not create create repo specific labels for this category.

## Community

Use these labels to tag issues that involve the community.

Color: All the labels in this category should use ![#874faf](https://via.placeholder.com/15/874faf/000000?text=+) `#874faf`.

| Name | Description | Example |
|---|---|---|
|Help wanted| This is a good issue for a contributor to take on and submit a solution | `Community: Help wanted` |

### Repo specific labels for community <!-- omit in toc -->

Do not create create repo specific labels for this category.

## Size

Use these to assign an estimated level of effort to resolve an issue and assist with the estimation process.

Color: All the labels in this category should use ![#91e3ea](https://via.placeholder.com/15/91e3ea/000000?text=+) `#91e3ea`.

| Name | Description | Example |
|---|---|---|
|Small| The issue is simple and well understood, it will take a day or less to complete | `Size: S` |
|Medium| The issue is not very complex and it is well understood, it will take 1 to 3 days to complete | `Size: M` |
|Large| The issue complex but it is well understood, it will take 4 to 8 days to complete | `Size: L` |
|Extra Large| The issue very complex or not very well defined, it will take 9 to 14 days to complete. In this case, it is probably better to rethink the issue and break it down in smaller tasks | `Size: XL` |

### Repo specific labels for size <!-- omit in toc -->

Do not create create repo specific labels for this category.

## Automation

These labels are applied to PRs and used to trigger or disable GitHub workflows.

Color: All the labels in this category should use ![#cccccc](https://via.placeholder.com/15/cccccc/000000?text=+) `#cccccc`.

|Name| Description | Example |
|---|---|:--|
|no parity| PR does not need to be applied to other languages.<br>**Note:** if you don't apply the `No parity` to a dotnet PR, the automation workflow will generate parity issues in Python, JS and Java.  | `Automation: No parity` |
|parity with dotnet| The PR needs to be ported to dotnet. | `Automation: Parity with dotnet` |
|parity with JS| The PR needs to be ported to JS. | `Automation: Parity with JS` |
|parity with Python| The PR needs to be ported to Python. | `Automation: Parity with Python` |
|parity with Java| The PR needs to be ported to Java. | `Automation: Parity with Java` |

### Repo specific labels for Automation <!-- omit in toc -->

It is OK to create repo specific labels for this category to trigger repo specific workflows, just use prefix the label with `Automation:` (e.g.: `Automation: My action`).

# DRI labels

The DRI labels are used to support the Azure issue management process and track desired SLAs.

The DRI labels are used when an issue is opened by someone that is not a contributor of the repo.

DRI labels support reporting outside the bot framework repositories and their names don't follow the standards described above. They should be applied based on the current DRI guide.

Issues created by anyone in the community that is not a collaborator in the repositories will initially be tagged `needs-triage`, `customer-reported`, and `question` by msft-bot. Note that issues are initially assumed to be questions because that's the most common issue type.

Color: This subcategory uses different colors for each label.

| Category | Description  | Color | Labels |
|---|---|:-:|:--|
|Customer issue| Customer reported issues, it is automatically applied when the issue is created by anyone that is not a collaborator in the repository.| ![#c2e0c6](https://via.placeholder.com/15/c2e0c6/000000?text=+) `#c2e0c6` | `customer-reported` |
|Service| Required for internal Azure reporting, indicates that the issue is related to the libraries and services managed by the Conversational AI team. <br>Do not delete. <br>Do not change color.| ![#e99695](https://via.placeholder.com/15/e99695/000000?text=+) `#e99695` | `Bot Service` |
|Component| Indicates where the problem specified by the issue lies, i.e. where a fix should go.<br>- `Mgmt`: the issue is management library; management libraries provision and configure Azure resource.<br>- `Client`: the issue is in the client library; client libraries access Azure resources during application runtime.| ![#e99695](https://via.placeholder.com/15/2683a5/000000?text=+) `#2683a5` | `Client`<br> `Mgmt`<br>`Service`<br>|
|Type| Indicates what the issue type is. This is a subset of the types defined in the [tyes category](#type).<br/>Only use `bug`, `question` or `feature-request` for DRI issues.| Multiple | `bug`<br> `question`<br>`feature-request`<br>|
|Status| Indicates who needs to take the next step.<br>`needs-triage`: issue needs members of SDK Team to triage.<br>`needs-team-triage`: issue needs collective members of SDK Team to triage<br>`needs-team-attention`: the issue has a comment from the author and needs SDK Team or service team’s attention.<br>`needs-author-feedback`: more info from the issue creator is needed to address the issue.|  ![#f7ffa3](https://via.placeholder.com/15/f7ffa3/000000?text=+) `#f7ffa3` | `needs-triage`<br>`needs-team-triage`<br>`needs-team-attention`<br>`needs-author-feedback`|

OLD DRI Labels (to be removed)

| Name | Description  | Color | Example |
|---|---|---|---|
|customer-replied-to| Required for internal Azure reporting. Do not delete.| ![#2683a5](https://via.placeholder.com/15/2683a5/000000?text=+) `#2683a5` | `customer-replied-to` |
|ExemptFromDailyDRIReport| Use this label to exclude the issue from the DRI report.| ![#bde567](https://via.placeholder.com/15/bde567/000000?text=+) `#bde567` | `ExemptFromDailyDRIReport` |

### Repo specific labels for DRI <!-- omit in toc -->

Do not create create repo specific labels for this category.

# Label creation and approval

TODO: Describe here the process for creating new labels.

# Repo specific labels

Some repo owners may need to create custom repo tags that only apply to a particular platform. This is OK but you must be aware that these tags will be used only in that repo and won't be used in cross repo reporting and tracking.

It is recommended that you try to use one of the labels described in this document before creating new ones. Less is better.

# Release labels

We should not use labels to tag releases, we should use GitHub milestones instead. 
