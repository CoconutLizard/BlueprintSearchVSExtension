#include "Runtime/AssetRegistry/Public/AssetRegistry/AssetRegistryModule.h"
#include "Developer/AssetTools/Public/AssetToolsModule.h"
#include "FiBCommandlet.h"
#include "FindInBlueprintManager.h"

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

    return 0;
}
