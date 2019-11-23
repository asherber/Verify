<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> ObjectApproval

[![Build status](https://ci.appveyor.com/api/projects/status/qt5bqw30vp7ywgh3/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/ObjectApproval)
[![NuGet Status](https://img.shields.io/nuget/v/ObjectApproval.svg?cacheSeconds=86400)](https://www.nuget.org/packages/ObjectApproval/)

Extends [ApprovalTests](https://github.com/approvals/ApprovalTests.Net) to allow simple approval of complex models using [Json.net](https://www.newtonsoft.com/json).

<!-- toc -->
## Contents

  * [Scrubbers](#scrubbers)
  * [File extension](#file-extension)
  * [Diff Tool](#diff-tool)
    * [Visual Studio](#visual-studio)
<!-- endtoc -->


## Scrubbers

Scrubbers run on the final string prior to doing the verification action.

They can be defined at three levels:

 * Method: Will run the verification in the current test method.
 * Class: Will run for all verifications in all test methods for a test class.
 * Global: Will run for test methods on all tests.

Multiple scrubbers can bee defined at each level.

Scrubber are excited in reveres order. So the most recent added method scrubber through to earlies added global scrubber.

Global scrubbers should be defined only once at appdomain startup.

Usage:

<!-- snippet: scrubberssample.cs -->
<a id='snippet-scrubberssample.cs'/></a>
```cs
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class ScrubbersSample :
    VerifyBase
{
    [Fact]
    public async Task Simple()
    {
        AddScrubber(s => s.Replace("Two", "B"));
        await VerifyText("One Two Three");
    }

    public ScrubbersSample(ITestOutputHelper output) :
        base(output)
    {
        AddScrubber(s => s.Replace("Three", "C"));
    }

    static ScrubbersSample()
    {
        Global.AddScrubber(s => s.Replace("One", "A"));
    }
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/ScrubbersSample.cs#L1-L26) / [anchor](#snippet-scrubberssample.cs)</sup>
<!-- endsnippet -->

Result:

<!-- snippet: ScrubbersSample.Simple.verified.txt -->
<a id='snippet-ScrubbersSample.Simple.verified.txt'/></a>
```txt
A B C
```
<sup>[snippet source](/src/Verify.Xunit.Tests/ScrubbersSample.Simple.verified.txt#L1-L1) / [anchor](#snippet-ScrubbersSample.Simple.verified.txt)</sup>
<!-- endsnippet -->


## File extension

The default file extension is `.txt`. So the resulting verified file will be `TestClass.TestMethod.verified.txt`.

It can be overridden at two levels:

 * Method: Change the extension for the current test method.
 * Class: Change the extension all verifications in all test methods for a test class.

Usage:

<!-- snippet: ExtensionSample.cs -->
<a id='snippet-ExtensionSample.cs'/></a>
```cs
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class ExtensionSample :
    VerifyBase
{
    [Fact]
    public async Task AtMethod()
    {
        UseExtensionForText(".xml");
        await VerifyText(@"<note>
<to>Joe</to>
<from>Kim</from>
<heading>Reminder</heading>
</note>");
    }

    [Fact]
    public async Task InheritedFromClass()
    {
        await VerifyText(@"{
    ""fruit"": ""Apple"",
    ""size"": ""Large"",
    ""color"": ""Red""
}");
    }

    public ExtensionSample(ITestOutputHelper output) :
        base(output)
    {
        UseExtensionForText(".json");
    }
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/ExtensionSample.cs#L1-L35) / [anchor](#snippet-ExtensionSample.cs)</sup>
<!-- endsnippet -->

Result in two files:

<!-- snippet: ExtensionSample.InheritedFromClass.verified.json -->
<a id='snippet-ExtensionSample.InheritedFromClass.verified.json'/></a>
```json
{
    "fruit": "Apple",
    "size": "Large",
    "color": "Red"
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/ExtensionSample.InheritedFromClass.verified.json#L1-L5) / [anchor](#snippet-ExtensionSample.InheritedFromClass.verified.json)</sup>
<!-- endsnippet -->

<!-- snippet: ExtensionSample.AtMethod.verified.xml -->
<a id='snippet-ExtensionSample.AtMethod.verified.xml'/></a>
```xml
<note>
<to>Joe</to>
<from>Kim</from>
<heading>Reminder</heading>
</note>
```
<sup>[snippet source](/src/Verify.Xunit.Tests/ExtensionSample.AtMethod.verified.xml#L1-L5) / [anchor](#snippet-ExtensionSample.AtMethod.verified.xml)</sup>
<!-- endsnippet -->


## Diff Tool

Controlled via environment variables.

 * `VerifyDiffProcess`: The process name. Short name if the tool exists in the current path, otherwise the full path.
 * `VerifyDiffArguments`: The argument syntax to pass to the process. Must contain the strings `{receivedPath}` and `{verifiedPath}`.


### Visual Studio

```
setx VerifyDiffProcess "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe"
setx VerifyDiffArguments "/diff {receivedPath} {verifiedPath}"
```


## Release Notes

See [closed milestones](../../milestones?state=closed).



## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).
