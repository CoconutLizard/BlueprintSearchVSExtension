// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class FindInBlueprintExternal : ModuleRules
{
	public FindInBlueprintExternal(ReadOnlyTargetRules Target) : base(Target)
	{
		PrivateIncludePaths.Add("FindInBlueprintExternal/Classes");

		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				// ... add other public dependencies that you statically link with here ...
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
				// ... add private dependencies that you statically link with here ...	
			}
			);
		
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);
	}
}
