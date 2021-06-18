// Copyright (C) Coconut Lizard Limited. All rights reserved.

#include "FindInBlueprintExternal/Classes/FiBResultsToJsonWriter.h"
#include <Runtime/Core/Public/HAL/FileManager.h>
#include <Runtime/Core/Public/Misc/DateTime.h>
#include <Runtime/Core/Public/Misc/Paths.h>
#include <Runtime/Core/Public/Serialization/Archive.h>

FiBResultsToJsonWriter::FiBResultsToJsonWriter(FArchive& Archive)
	: JsonWriter(TJsonWriterFactory<TCHAR, TPrettyJsonPrintPolicy<TCHAR>>::Create(&Archive))
{

}

FiBResultsToJsonWriter::~FiBResultsToJsonWriter()
{
	JsonWriter->Close();
}

void FiBResultsToJsonWriter::WriteDataToJson(const TArray<FSearchResult>& SearchResults, bool IsRoot)
{
	if (SearchResults.Num() > 0)
	{
		if (IsRoot)
		{
			JsonWriter->WriteArrayStart();
		}
		else
		{
			JsonWriter->WriteArrayStart("Children");
		}

		for (const FSearchResult& Item : SearchResults)
		{
			JsonWriter->WriteObjectStart();
			JsonWriter->WriteValue(TEXT("Value"), Item->GetDisplayString().ToString());
			WriteDataToJson(Item->Children, false);
			JsonWriter->WriteObjectEnd();
		}

		JsonWriter->WriteArrayEnd();
	}
}
