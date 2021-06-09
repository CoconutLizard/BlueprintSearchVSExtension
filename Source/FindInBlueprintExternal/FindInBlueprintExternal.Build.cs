// Copyright Epic Games, Inc. All Rights Reserved.

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
				"Core",
				"CoreUObject",
				"ApplicationCore",
				"Engine",
				"UnrealEd",
				"Slate",
				"SlateCore",
				"GameProjectGeneration",
				"Kismet"
			}
			);
		
	}
}
