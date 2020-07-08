# Bot Framework Labeling Guidelines (DRAFT)

This article describes the common labels used in the Bot Framework repositories to tag issues and PRs.

It also provides guidance on how to use them.

## Label format

In order to be able to parse the different labels in reports we use `:` to separate the categories and subcategories in a label. The general format of a label is:

```bash
Main category:Subcategory 1:Subcategory 2
```

There's no limit to the number of subcategories but normally 2 or 3 levels at most should be enough.

## Label usage

Some labels can be applied multiple times to the same issue and some other shouldn't. For example, an issue could be labeled as `Area:Adaptive` **and** `Area:Skills` but it should **only** have one priority label `P:0` or `P:1`.

## Label colors

Labels should use the same colors across repos to improve readability and in most cases, all labels within the same main category should use the same color.

## Main label categories

This section describes the main label categories being used:

|Category| Description  | Usage  | Example
|---|---|---|------|
|[Area](#area)|Defines a functional area or feature of the product.| Multiple |`Area:Skills`|
|[Priority](#priority)|The priority for the issue.| Single |`P:0`|
|[Status](#status)|Further describes the status of the issue throughout its lifecycle.| Single |`Status:New`|
|[Community](#community)|Used to describe community related issues.| Single |`Community:Help Wanted`|
|[Type](#type)|Provides additional information on the issue type.| Single |`Type:Bug`|
|[Size](#size)|Provides an estimate for the level of effort required to resolve the issue.| Single |`Size:M`|
|[Automation](#automation) |Used to trigger github actions and workflows.| Single |`Automation:No parity`|
|[DRI](#dri)|Special set of tags used for DRI tracking and reporting.| Multiple |`Bot Services`|

### Area

Color: All the labels in this category should use ![#1d76db](https://via.placeholder.com/15/1d76db/000000?text=+) `#1d76db`

This label is used to map a particular issue to a feature or functional area of the product.

#### Subcategories for Area

|Name| Description  | Example
|---|---|------|
|Adaptive| TODO | `Area:Adaptive` |
|Adaptive Expressions| TODO | `Area:Adaptive Expressions` |
|AI-Luis| TODO | `Area:AI-Luis` |
|AI-QnAMaker| TODO | `Area:AI-QnAMaker` |
|Authentication| TODO | `Area:Authentication` |
|Custom Adapters| TODO | `Area:Custom Adapters` |
|Docs| TODO | `Area:Docs` |
|Functional Tests| TODO | `Area:Functional Tests` |
|LG| TODO | `Area:LG` |
|Samples| TODO | `Area:Samples` |
|Schema| TODO | `Area:Schema` |
|Skills| TODO | `Area:Skills` |
|Streaming| TODO | `Area:Streaming` |
|Teams| TODO | `Area:Teams` |
|Telemetry| TODO | `Area:Telemetry` |
|Testing Framework| TODO | `Area:Testing Framework` |

### Priority

Color: This subcategory uses different colors for each label.

Describes the priority of the issue. This label is required for any issue that is in scope for an iteration. High priority issue will be addressed first.

|Name| Description  | Color | Example
|---|---|---|------|
|P:0| Must Fix.  Release-blocker | ![#ee0701](https://via.placeholder.com/15/ee0701/000000?text=+) `#ee0701` | `P:0` |
|P:1| Painful if we don't fix, won't block releasing | ![#ff8c00](https://via.placeholder.com/15/FF8C00/000000?text=+) `#ff8c00` | `P:1` |
|P:2| Nice to have | ![#ffff00](https://via.placeholder.com/15/ffff00/000000?text=+) `#ffff00` | `P:2` |
|P:3| Won't fix | ![#bfd4f2](https://via.placeholder.com/15/bfd4f2/000000?text=+) `#bfd4f2` | `P:3` |

### Status

Color: This subcategory uses different colors for each label.

Use these labels for providing additional information on the status of the issue.

|Name| Description  | Color | Example
|---|---|---|------|
|Draft| The issue definition is still being worked on and it is not ready to start development. Once the issue is ready the status should be changed to `Approved` or `Backlog`.| ![#ededed](https://via.placeholder.com/15/ededed/000000?text=+) `#ededed` | `Status:Draft` |
|New| The issue has just been created and it has not been reviewed by the team. Once the issue is reviewed the status be changed to `Approved`, `Backlog`, `Needs information` or just closed.| ![#bfd4f2](https://via.placeholder.com/15/bfd4f2/000000?text=+) `#bfd4f2` | `Status:New` |
|Needs information| The issue as described is incomplete or not well understood. It is waiting for further information before it can continue.| ![#ff8c00](https://via.placeholder.com/15/ff8c00/000000?text=+) `#ff8c00` | `Status:Needs information` |
|Backlog| The issue is out of scope for the current iteration but it will be evaluated in a future release. | ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `Status:Backlog` |
|Approved| The issue has been reviewed and is ready to start working on it, it will be added to the work queue in the current iteration. | ![#0e8a16](https://via.placeholder.com/15/0e8a16/000000?text=+) `#0e8a16` | `Status:Approved` |
|Blocked| Current progress is blocked on something else. | ![#ff8c00](https://via.placeholder.com/15/FF8C00/000000?text=+) `#ff8c00` | `Status:Blocked` |
|Stale| The issue hasn't been updated in a long time and will be automatically closed. | ![#ededed](https://via.placeholder.com/15/ededed/000000?text=+) `#ededed` | `Status:Stale` |

### Community

Color: All the labels in this category should use ![#874faf](https://via.placeholder.com/15/874faf/000000?text=+) `#874faf`.

Use these labels to tag issues that involve the community.

|Name| Description  | Example
|---|---|------|
|Help wanted| This is a good issue for a contributor to take on and submit a solution | `Community:Help wanted` |

### Type

Color: This subcategory uses different colors for each label.

Use these labels for providing additional information type of the issue.

|Name| Description  | Color | Example
|---|---|---|------|
|Bug| Your classic code defect.| ![#d73a4a](https://via.placeholder.com/15/d73a4a/000000?text=+) `#d73a4a` | `Type:Bug` |
|Feature request| This is a new feature or an enhancement to an existing one.| ![#8f31ed](https://via.placeholder.com/15/8f31ed/000000?text=+) `#8f31ed` | `Type:Feature request` |
|Customer ask| An enhancement or feature requested by a customer (use this label if the feature request was originated by a customer).| ![#8f31ed](https://via.placeholder.com/15/8f31ed/000000?text=+) `#8f31ed` | `Type:Customer ask` |
|Parity| The issue describes a gap in parity between two or more platforms.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `Type:Parity` |
|Technical debt| The issue involves refactoring existing code to make it easier to maintain, follow best practices, improved test coverage, etc.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `Type:Technical debt` |
|Team agility| An issue targeted to reduce friction to releasing new versions of the SDKs.| ![#fbca04](https://via.placeholder.com/15/fbca04/000000?text=+) `#fbca04` | `Type:Team agility` |

### Size

Color: All the labels in this category should use ![#91e3ea](https://via.placeholder.com/15/91e3ea/000000?text=+) `#91e3ea`.

Use these labels to tag issues that involve the community.

|Name| Description  | Example
|---|---|------|
|Small| The issue is simple and well understood, it will take a day or less to complete | `Size:S` |
|Medium| The issue is not very complex and it is well understood, it will take 1 to 3 days to complete | `Size:M` |
|Large| The issue complex but it is well understood, it will take 4 to 8 days to complete | `Size:L` |
|Extra Large| The issue very complex or not very well defined, it will take 9 to 14 days to complete. In this case, it is probably better to rethink the issue and break it down in smaller tasks | `Size:XL` |

### Automation

Color: All the labels in this category should use ![#cccccc](https://via.placeholder.com/15/cccccc/000000?text=+) `#cccccc`.

These labels are applied to PRs and used to trigger or disable github workflows.

|Name| Description  | Example
|---|---|------|
|no parity| PR does not need to be applied to other languages. **Note:** if you don't apply the `No parity` to a dotnet PR, the automation workflow will generate parity issues in Python, JS and Java.  | `Automation:No parity` |
|parity with dotnet| The PR needs to be ported to dotnet. | `Automation:Parity with dotnet` |
|parity with JS| The PR needs to be ported to JS. | `Automation:Parity with JS` |
|parity with Python| The PR needs to be  ported to Python. | `Automation:Parity with JS` |
|parity with Java| The PR needs to be ported to Java. | `Automation:Parity with Java` |



### DRI

Color: This subcategory uses different colors for each label.

DRI labels are used for reporting outside the bot framework repositories and their names don't follow the standards described above. They should be applied based on the current DRI guide.

|Name| Description  | Color | Example
|---|---|---|------|
|Bot Services| Required for internal Azure reporting. Do not delete. Do not change color.| ![#e99695](https://via.placeholder.com/15/e99695/000000?text=+) `#e99695` | `Bot Services` |
|customer-reported| Customer reported issues.| ![#c2e0c6](https://via.placeholder.com/15/c2e0c6/000000?text=+) `#c2e0c6` | `customer-reported` |
|customer-replied-to| Required for internal Azure reporting. Do not delete.| ![#2683a5](https://via.placeholder.com/15/2683a5/000000?text=+) `#2683a5` | `customer-replied-to` |
|ExemptFromDailyDRIReport| Use this label to exclude the issue from the DRI report.| ![#bde567](https://via.placeholder.com/15/bde567/000000?text=+) `#bde567` | `ExemptFromDailyDRIReport` |

## Label creation and approval

TODO: Describe here the process for creating new labels.

### Repo specific labels

Some repo owners may need to create custom repo tags that only apply to a particular platform. This is OK but they must be aware that these tags will be used only in their repo and won't be used in cross repo reporting and tracking. If possible, try to use the labels described in this guide before creating new ones.
