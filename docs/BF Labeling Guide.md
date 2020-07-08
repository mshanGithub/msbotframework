# Bot Framework Labeling Guidelines

This article describes the common labels used in the Bot Framework repositories to tag issues and PRs.

It also provides guidance on how to use them.

## Label format

In order to be able to parse the different labels in reports we use `:` to separate the categories and subcategories in a label. The general format of a label is:

```bash
Main category:Subcategory 1:Subcategory 2
```

There's no limit to the number of subcategories but normally 2 or 3 levels at most should be enough.

## Label usage

Same labels can be applied multiple to times to the same issue and some other shouldn't. For example, an issue could be labeled as `Area:Adaptive` and `Area:Skills` but it should only have one priority label `P:0` or `P:1`. 

## Label colors

Labels should use the same colors across repos to improve readability and in most cases, all labels within the same main category should use the same color.

## Main label categories

This section describes the main label categories being used:

|Category| Description  | Usage  | Example
|---|---|---|------|
|Area|Defines a functional area or feature of the product.| Multiple |<span style="background-color:#5ed666">Area:Skills</span>|
|Priority|The priority for the issue.| Single |P:0|
|Planning|Used to describe different planning stages for the issue.| Single |Planning:Backlog|
|Status|Further describes the status of the issue.| Single |Status:Stale|
|Community|Used to describe community related issues.| Single |Community:Help Wanted|
Type
Automation
DRI

<style
  type="text/css">
tag {color:blue;}
</style>
<tag>okay</tag>

### Area

Color: ![#5ed666](https://via.placeholder.com/15/5ed666/000000?text=+) `#5ed666`

Color: ![#5ed666](https://via.placeholder.com/200x30/5ed666/000000?text=+How+will+this) `#5ed666`
Subcategories for area

`#5ed666`

Adaptive
Adaptive Expressions
AI-Luis
AI-QnAMaker
Authentication
Custom Adapters
Docs
Functional Tests
LG
Schema
Skills
Streaming
Teams
Telemetry
Testing Framework


<span style="background-color:#5ed666"> Status: Backlog </span>.
<span style="color:white;background-color:#ee0701"> Status: Blocked </span>.

Colors
Except for the ones for DRI, 

Exceptions
DRI labels are used for reporting outside the bot framework repositories and the colors and names don't follow the standards described above.
