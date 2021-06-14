// Copyright (C) Coconut Lizard Limited. All rights reserved.

using UnrealBuildTool;

public class FindInBlueprintExternal : ModuleRules
{
	public FindInBlueprintExternal(ReadOnlyTargetRules Target) : base(Target)
	{
		PrivateIncludePaths.Add("FindInBlueprintExternal/Classes");


		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
			);


		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"ApplicationCore",
				"Engine",
				"Core",
				"CoreUObject",
				"GameProjectGeneration",
				"Kismet",
				"Json",
				"Slate",
				"SlateCore",
				"UnrealEd"
			}
			);

	}
}
