# BlueprintSearchVSExtension

A visual studio extension and associated UE plugin code to do a blueprint search without having to load up the editor.

Tested on engine versions: 4.26

##### Contents

* [Installing BlueprintSearchVS](#installing-blueprintsearchvs)

* [Instruction for Using BlueprintSearchVS](#using-blueprintsearchvs)

* [Instruction for Using Blueprint Search Filters](#using-blueprint-search-filters)

## Installing BlueprintSearchVS

1. Clone the repository inside your Unreal Engine plugins (<project path>/Engine/Plugins) folder using your favourite Git client or by clicking the download ZIP button inside the Green Code submenu.

2. Run GenerateProjectFiles from your Unreal Engine root folder and Rebuild your engine.

3. Close any open instance of Visual Studio.

4. Open BlueprintSearch.vsix.

5. Check the relevant product(s) you want to install the extension to.

6. Open Visual Studio and Rebuild your UE4 project.

## Using BlueprintSearchVS

1. Open an Unreal Engine 4 project built against a version of the engine with the BlueprintSearch plugin installed.

2. Open the Search Window from the Extensions drop down menu or by clicking Alt + Shift + Control + G.

3. Type the search terms inside the text box.

4. Click the search button or press Enter.

## Using Blueprint Search Filters

Epic implemented a list of filters which allows you to search for specific subsets of data inside a Blueprint. A full list of filters can be found following this link: https://docs.unrealengine.com/4.26/en-US/ProgrammingAndScripting/Blueprints/Search/.

To call a filter, type the filter name followed by a set of open and closed parentheses that will enclose a list of Tags and Subfilters. 
Tags must follow one of these syntaxes TagName=TagValue which is equal to TagName==TagValue which will return all the results that match the TagValue,
or TagName!=TagValue which will return all the results that DON'T match the Tag value.
On every filter, except for Blueprint, the Name tag can be searched, also, without being explicitly specified, i.e. Nodes(Test) is the same as writing Nodes(Name=Test).
Multiple Filters and Tags can be chained together using a logical AND operator(&&), or a logical OR operator(||). When filters are chained together using an AND the search query will return only items that satisfy both of them, when using an OR instead the query will return items that matches at least one of them.

NOTES:
* When using any subfilter, they are always applied to Blueprint even if not explicitly specified, i.e. Pins(MyPin) && Pins(MyOtherPin) will be the same as using Blueprint(Pins(MyPin) && Pins(MyOtherPin)), in this case the query wont return all the Pins called MyPin and MyOtherPin, but only the one that are both present inside a Blueprint.
* Despite being grouped inside Pins, Variables/Properties and Categories, the Tag IsSCSComponent will fail to produce any results when used with the Pins filter, regardless of its value. On a similar note the Tag DefaultValue will fail to produce any results when used with Components.
* Of the three boolean Tags (IsArray, IsReference, IsSCSComponent), IsReference needs a integer boolean instead of a literal one, i.e. IsReference=0 for False, IsReference!=0 for True.
* When using the Name Tag inside the Blueprint filter don't include its own file extension or it will fail to produce results.

Examples on how to use filters:

Blueprint(Name=MyBlueprint && Nodes(MyNode) && Functions(MyFunction)) will return all the nodes called MyNode and all the functions called MyFunction located inside Blueprints that contain MyBlueprint in their name.
Pins(PinCategory=int && IsReference=1) will return all the int references.

