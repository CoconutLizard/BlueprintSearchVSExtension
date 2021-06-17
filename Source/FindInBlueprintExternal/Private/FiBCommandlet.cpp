// Copyright (C) Coconut Lizard Limited. All rights reserved.

#include "FindInBlueprintExternal/Classes/FiBCommandlet.h"
#include <Developer/AssetTools/Public/AssetToolsModule.h>
#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Runtime/AssetRegistry/Public/AssetRegistry/AssetRegistryModule.h>
#include "FindInBlueprintExternal/Classes/FiBResultsToJsonWriter.h"

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
	const FStreamSearchOptions SearchOptions;

	for (const FString& SearchValue : Tokens)
	{
		FStreamSearch StreamSearch(SearchValue, SearchOptions);
		while (!StreamSearch.IsComplete())
		{
			FFindInBlueprintSearchManager::Get().Tick(0.0);
		}

		StreamSearch.GetFilteredItems(OutItemsFound);
	}

	const FString OutputFilePath = FPaths::LaunchDir() / TEXT("SearchResults.json");
	const TUniquePtr<FArchive> FileArchive = TUniquePtr<FArchive>(IFileManager::Get().CreateFileWriter(*OutputFilePath));
	if (!FileArchive)
	{
		UE_LOG(LogFiBCommandlet, Error, TEXT("Failed to create a file writer for the file at location: [%s]."), *OutputFilePath);
		return 1;
	}

	FiBResultsToJsonWriter Writer(*FileArchive.Get());
	Writer.WriteDataToJson(OutItemsFound);

	return 0;
}
