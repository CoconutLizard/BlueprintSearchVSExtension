// Copyright (C) Coconut Lizard Limited. All rights reserved.

#pragma once

#include <Runtime/Core/Public/CoreMinimal.h>

class FImaginaryFiBDataAccessor final : protected FImaginaryFiBData
{
public:
	FImaginaryFiBDataAccessor(FImaginaryFiBDataWeakPtr InOuter, TSharedPtr< FJsonObject > InUnparsedJsonObject = TSharedPtr<FJsonObject>(), TMap<int32, FText>* InLookupTablePtr = nullptr)
		: FImaginaryFiBData(InOuter, InUnparsedJsonObject, InLookupTablePtr)
	{
	}

	FString GetInfo(const FText& Category, const FText& DisplayText);
	FImaginaryFiBDataSharedPtr GetParsedChild(const FText& DisplayText);
	bool LookUpValue(const FText& Category, const FText& DisplayText);
	FString GetValue(const FText& text);
};

