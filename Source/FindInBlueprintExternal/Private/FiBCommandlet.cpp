#include "FiBCommandlet.h"
#include "AssetRegistryModule.h"
#include "Developer/AssetTools/Public/AssetToolsModule.h"
#include "FindInBlueprintManager.h"
//#include "SharedPointer.h"

int32 UFiBCommandlet::Main(const FString& Params)
{
	const TCHAR* Cmd = *Params;
	GIsRunning = true;

	FAssetRegistryModule* AssetRegistryModule = &FModuleManager::LoadModuleChecked<FAssetRegistryModule>(TEXT("AssetRegistry"));

	AssetRegistryModule->Get().SearchAllAssets(true);

	FString SearchValue = FParse::Token(Cmd, false);
	TArray<FSearchResult> OutItemsFound;
	const FStreamSearchOptions SearchOptions;

	while (!SearchValue.IsEmpty()) {

		if (SearchValue.Equals("-run=FiB"))
		{

			SearchValue = FParse::Token(Cmd, false);

		}

		FStreamSearch StreamSearch(SearchValue, SearchOptions);

		while (!StreamSearch.IsComplete()) FFindInBlueprintSearchManager::Get().Tick(.0);

		StreamSearch.GetFilteredItems(OutItemsFound);

		SearchValue = FParse::Token(Cmd, false);

	}


	UE_LOG(LogTemp, Warning, TEXT("Test text"));

    return int32();
}
