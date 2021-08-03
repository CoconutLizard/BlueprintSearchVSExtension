// Copyright (C) Coconut Lizard Limited. All rights reserved.

#include "FindInBlueprintExternal/Classes/FiBCommandlet.h"
#include "FindInBlueprintExternal/Classes/FiBResultsToJsonWriter.h"
#include <Developer/AssetTools/Public/AssetToolsModule.h>
#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Editor/Kismet/Public/ImaginaryBlueprintData.h>
#include <Runtime/AssetRegistry/Public/AssetRegistry/AssetRegistryModule.h>
#include <Runtime/Core/Public/Logging/LogMacros.h>

DEFINE_LOG_CATEGORY_STATIC(LogFiBCommandlet, Log, All);

int32 UFiBCommandlet::Main(const FString& Params)
{
	GIsRunning = true;
	const FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>(TEXT("AssetRegistry"));
	AssetRegistryModule.Get().SearchAllAssets(true);

	TArray<FString> Tokens;
	TArray<FString> Switches;
	ParseCommandLine(*Params, Tokens, Switches);
	TArray<FSearchResult> OutItemsFound;
	FStreamSearchOptions SearchOptions;

	for (const FString& SearchValue : Tokens)
	{
		FStreamSearch StreamSearch(SearchValue, SearchOptions);
		while (!StreamSearch.IsComplete())
		{
			FFindInBlueprintSearchManager::Get().Tick(0.0);
		}

		StreamSearch.GetFilteredItems(OutItemsFound);
	}

	TArray<FImaginaryFiBDataSharedPtr> ItemsFoundImmaginaryBlueprints;
	for (const FSearchResult& Item : OutItemsFound)
	{
		FName ItemPath = FName(*Item->DisplayText.ToString());
		FSearchData SearchData = FFindInBlueprintSearchManager::Get().GetSearchDataForAssetPath(ItemPath);
		ItemsFoundImmaginaryBlueprints.Add(SearchData.ImaginaryBlueprint);
	}

	const FString OutputFilePath = FPaths::LaunchDir() / TEXT("SearchResults.json");
	const TUniquePtr<FArchive> FileArchive = TUniquePtr<FArchive>(IFileManager::Get().CreateFileWriter(*OutputFilePath));
	if (!FileArchive)
	{
		UE_LOG(LogFiBCommandlet, Error, TEXT("Failed to create a file writer for the file at location: [%s]."), *OutputFilePath);
		return 1;
	}

	FiBResultsToJsonWriter Writer(*FileArchive);
	Writer.WriteDataToJson(OutItemsFound, ItemsFoundImmaginaryBlueprints);

	return 0;
}
