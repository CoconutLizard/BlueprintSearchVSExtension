// Copyright (C) Coconut Lizard Limited. All rights reserved.

#pragma once

#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Runtime/Core/Public/Templates/SharedPointer.h>
#include <Runtime/Json/Public/Policies/PrettyJsonPrintPolicy.h>
#include <Runtime/Json/Public/Serialization/JsonWriter.h>

class FArchive;

class FiBResultsToJsonWriter
{
public:
	FiBResultsToJsonWriter(FArchive& Archive);
	~FiBResultsToJsonWriter();

	void WriteDataToJson(const TArray<FSearchResult>& Data, bool IsRoot = true);

private:

	TSharedRef<TJsonWriter<TCHAR, TPrettyJsonPrintPolicy<TCHAR>>> JsonWriter;
};

