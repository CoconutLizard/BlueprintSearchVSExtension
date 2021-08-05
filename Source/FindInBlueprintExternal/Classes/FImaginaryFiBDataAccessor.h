// Copyright (C) Coconut Lizard Limited. All rights reserved.

#pragma once

#include <Runtime/Core/Public/CoreMinimal.h>
#include <Editor/Kismet/Public/ImaginaryBlueprintData.h>

class FImaginaryFiBDataAccessor final : public FImaginaryFiBData
{
public:
	FImaginaryFiBDataAccessor(FImaginaryFiBDataWeakPtr InOuter, TSharedPtr< FJsonObject > InUnparsedJsonObject = TSharedPtr<FJsonObject>(), TMap<int32, FText>* InLookupTablePtr = nullptr)
		: FImaginaryFiBData(InOuter, InUnparsedJsonObject, InLookupTablePtr)
	{
	}

	FString GetInfo(const FText& Category, const FText& DisplayText) const;
	FImaginaryFiBDataSharedPtr GetParsedChild(const FText& DisplayText);
	bool LookUpValue(const FText& Category, const FText& DisplayText);
	FString GetValue(const FText& Key) const;
};
