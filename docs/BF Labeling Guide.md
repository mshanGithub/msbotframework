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
|Planning|Used to describe different planning stages for the issue.| Single |`Planning:Backlog`|
|Status|Further describes the status of the issue.| Single |`Status:Stale`|
|Community|Used to describe community related issues.| Single |`Community:Help Wanted`|
|Type|Provides additional information on the issue type (if known).| Single |`Type:Bug`|
|Automation|Used to trigger github actions and workflows.| Single |`Automation:No parity`|
|DRI|Special set of tags used for DRI tracking and reporting.| Multiple |`Bot Services`|

### Area

Color: ![#5ed666](https://via.placeholder.com/15/5ed666/000000?text=+) `#5ed666`

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
|LG| TODO | `Area:` |
|Samples| TODO | `Area:` |
|Schema| TODO | `Area:` |
|Skills| TODO | `Area:` |
|Streaming| TODO | `Area:` |
|Teams| TODO | `Area:` |
|Telemetry| TODO | `Area:` |
|Testing Framework| TODO | `Area:` |

### Priority

Color: This subcategory uses different colors for each label.

Describes the priority of the issue.

|Name| Description  | Color | Example
|---|---|---|------|
|P:0| Must Fix.  Release-blocker | ![#ee0701](https://via.placeholder.com/15/ee0701/000000?text=+) `#ee0701` | `P:0` |
|P:1| Painful if we don't fix, won't block releasing | ![#ff8c00](https://via.placeholder.com/15/FF8C00/000000?text=+) `#ff8c00` | `P:1` |
|P:2| Nice to have | ![#ffff00](https://via.placeholder.com/15/ffff00/000000?text=+) `#ffff00` | `P:2` |
|P:3| Won't fix | ![#bfd4f2](https://via.placeholder.com/15/bfd4f2/000000?text=+) `#bfd4f2` | `P:3` |

Exceptions
DRI labels are used for reporting outside the bot framework repositories and the colors and names don't follow the standards described above.
