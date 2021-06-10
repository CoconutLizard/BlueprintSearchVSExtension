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
                "ApplicationCore",
                "Engine",
                "Core",
                "CoreUObject",
                "GameProjectGeneration",
				"Kismet",
				"Slate",
                "SlateCore",
                "UnrealEd"
          	}
			);
		
	}
}
