// Copyright (C) Coconut Lizard Limited. All rights reserved.
#include "FindInBlueprintExternal/Classes/FiBResultsToJsonWriter.h"
#include "FindInBlueprintExternal/Classes/FImaginaryFiBDataAccessor.h"
#include <Runtime/Core/Public/HAL/FileManager.h>
#include <Runtime/Core/Public/Misc/DateTime.h>
#include <Runtime/Core/Public/Misc/Paths.h>
#include <Runtime/Core/Public/Serialization/Archive.h>


FiBResultsToJsonWriter::FiBResultsToJsonWriter(FArchive& Archive)
	: JsonWriter(TJsonWriterFactory<ANSICHAR, TPrettyJsonPrintPolicy<ANSICHAR>>::Create(&Archive))
{
}

FiBResultsToJsonWriter::~FiBResultsToJsonWriter()
{
	JsonWriter->Close();
}

void FiBResultsToJsonWriter::WriteSearchResultToJson(const FSearchResult& SearchResult, const FImaginaryFiBDataSharedPtr& ImmaginaryBlueprint)
{
	JsonWriter->WriteObjectStart();
	const TSharedPtr<FImaginaryFiBDataAccessor, ESPMode::ThreadSafe> ImmaginaryBlueprintAccessor = StaticCastSharedPtr<FImaginaryFiBDataAccessor>(ImmaginaryBlueprint);
	const FText Category = SearchResult->GetCategory();
	const FText DisplayString = SearchResult->GetDisplayString();
	const FString Value = ImmaginaryBlueprintAccessor->GetInfo(Category, DisplayString);
	JsonWriter->WriteValue(TEXT("Value"), Value);
	JsonWriter->WriteArrayStart("Children");
	for (const FSearchResult& Child : SearchResult->Children)
	{
		const FText ChildDisplayString = Child->GetDisplayString();
		const FImaginaryFiBDataSharedPtr ImmaginaryBlueprintChild = ImmaginaryBlueprintAccessor->GetParsedChild(ChildDisplayString);
		WriteSearchResultToJson(Child, ImmaginaryBlueprintChild);
	}
	JsonWriter->WriteArrayEnd();
	JsonWriter->WriteObjectEnd();
}

void FiBResultsToJsonWriter::WriteDataToJson(const TArray<FSearchResult>& SearchResults, const TArray<FImaginaryFiBDataSharedPtr>& SearchResultsImmaginaryBlueprints)
{
	if (SearchResults.Num() > 0)
	{
		JsonWriter->WriteArrayStart();
		for (uint8 i = 0; i < SearchResults.Num(); i++)
		{
			WriteSearchResultToJson(SearchResults[i], SearchResultsImmaginaryBlueprints[i]);
		}
		JsonWriter->WriteArrayEnd();
	}
}
