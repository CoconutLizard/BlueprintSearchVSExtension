// Copyright (C) Coconut Lizard Limited. All rights reserved.

#pragma once

#include <Runtime/Core/Public/CoreMinimal.h>
#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Runtime/Json/Public/Policies/PrettyJsonPrintPolicy.h>
#include <Runtime/Json/Public/Serialization/JsonWriter.h>

class FArchive;

class FiBResultsToJsonWriter
{
public:
	FiBResultsToJsonWriter(FArchive& Archive);
	~FiBResultsToJsonWriter();
	void WriteDataToJson(const TArray<FSearchResult>& SearchResults);
	void WriteSearchResultToJson(const FSearchResult& SearchResult);

private:
	TSharedRef<TJsonWriter<ANSICHAR, TPrettyJsonPrintPolicy<ANSICHAR >>> JsonWriter;
};

