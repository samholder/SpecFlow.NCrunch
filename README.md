# SpecFlow.NCrunch
Plugin for Specflow which allows generation of NCrunch attributes on the unit tests based on tags on the scenarios

NCrunch has [a great model](https://www.ncrunch.net/documentation/reference_runtime-framework_overview) for controlling how your tests get run in parallel by the engine. When using Specflow, then specflow engine generates the tests meaning that its hard to take advantage of these attributes as adding them to the generated code isn't simple.

This plugin for specflow allows tags in the feature files to control the generation of the NCrunch attributes in the code. The tags come in 2 basic styles, those that require no parameters, and those that require parameters. For those that don't need parameters then simply using a tag with the same name as the attribute will cause the attribute to be generated:

``` gherkin
@Isolated
Scenario: Adding a simple attribute
Given something
```

will result in this code being generated (assuming Specflow is configured to generate NUnit tests):

```c#
[NUnit.Framework.TestAttribute()]
[NUnit.Framework.DescriptionAttribute("Adding a simple attribute")]
[NCrunch.Framework.IsolatedAttribute()]
public virtual void AddingASimpleAttribute()
```

If the attribute takes a parameter or multiple parameters then the attribute should be followed by a colon and then a comma separated list of values. So this:

``` gherkin
@Timeout:1000
@ExclusivelyUses:Database,FileSystem
Scenario: Adding attributes with parameters
Given something
```

will result in this code being generated (assuming Specflow is configured to generate NUnit tests):

```c#
[NUnit.Framework.TestAttribute()]
[NUnit.Framework.DescriptionAttribute("Adding attributes with parameters")]
[NCrunch.Framework.TimeoutAttribute(1000)]
[NCrunch.Framework.ExclusivelyUsesAttribute("Database", "FileSystem")]
public virtual void AddingAttributesWithParameters()
```

This allows tags to be used to control how tests are run in parallel with each other when NCrunch is used as the running engine.

Currently only tags on scenarios are supported, tags on features don't work.
