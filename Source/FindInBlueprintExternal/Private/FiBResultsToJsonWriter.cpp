// Copyright (C) Coconut Lizard Limited. All rights reserved.

#include "FindInBlueprintExternal/Classes/FiBResultsToJsonWriter.h"
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

void FiBResultsToJsonWriter::WriteSearchResultToJson(const FSearchResult& SearchResult)
{
	JsonWriter->WriteObjectStart();
	JsonWriter->WriteValue(TEXT("Value"), SearchResult->GetDisplayString().ToString());
	JsonWriter->WriteArrayStart("Children");
	for (const FSearchResult& Child : SearchResult->Children)
	{
		WriteSearchResultToJson(Child);
	}
	JsonWriter->WriteArrayEnd();

	JsonWriter->WriteObjectEnd();
}

void FiBResultsToJsonWriter::WriteDataToJson(const TArray<FSearchResult>& SearchResults)
{
	if (SearchResults.Num() > 0)
	{
		JsonWriter->WriteArrayStart();
		for (const FSearchResult& Item : SearchResults)
		{
			WriteSearchResultToJson(Item);
		}
		JsonWriter->WriteArrayEnd();
	}
}
