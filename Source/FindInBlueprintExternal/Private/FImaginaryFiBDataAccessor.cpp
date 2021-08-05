// Copyright (C) Coconut Lizard Limited. All rights reserved.

#include "FindInBlueprintExternal/Classes/FImaginaryFiBDataAccessor.h"
#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Editor/Kismet/Public/FindInBlueprints.h>

#define LOCTEXT_NAMESPACE "FindInBlueprintsCommandlet"

class FSearchableValueInfoHelper final : public FSearchableValueInfo
{
public:
	FSearchableValueInfoHelper(const FSearchableValueInfo& InValueInfo, TMap<int32, FText>* const InLookupTable)
		: FSearchableValueInfo(InValueInfo), LookupTablePtr(InLookupTable)
	{
	}

	FText GetDisplayText() const
	{
		FText Result;
		if (!DisplayText.IsEmpty() || LookupTableKey == -1)
		{
			Result = DisplayText;
		}
		else
		{
			Result = *LookupTablePtr->Find(LookupTableKey);
		}

		return Result;
	}

	FString GetDisplayString() const
	{
		return GetDisplayText().ToString();
	}

private:
	TMap<int32, FText>* LookupTablePtr;
};

FString FImaginaryFiBDataAccessor::GetInfo(const FText& Category, const FText& DisplayText) const
{
	FString OutInfo;
	if (!Category.IsEmpty())
	{
		if (Category.CompareTo(LOCTEXT("PinText", "Pin")) == 0)
		{
			const FString PinCategory = GetValue(FFindInBlueprintSearchTags::FiB_PinCategory);
			FString PinSubCategory = GetValue(FFindInBlueprintSearchTags::FiB_PinSubCategory);
			if (PinSubCategory.IsEmpty())
			{
				PinSubCategory = GetValue(FFindInBlueprintSearchTags::FiB_ObjectClass);
			}
			if (PinSubCategory.IsEmpty())
			{
				PinSubCategory = TEXT("None");
			}
			OutInfo = FString::Printf(TEXT("%s\"%s\" %s: %s"), *PinCategory, *PinSubCategory, *Category.ToString(), *DisplayText.ToString());
		}
		else
		{
			OutInfo = FString::Printf(TEXT("%s: %s"), *Category.ToString(), *DisplayText.ToString());
		}
	}
	else
	{
		OutInfo = DisplayText.ToString();
	}

	return OutInfo;
}

FImaginaryFiBDataSharedPtr FImaginaryFiBDataAccessor::GetParsedChild(const FText& DisplayText)
{
	FImaginaryFiBDataSharedPtr OutChild;
	for (const FImaginaryFiBDataSharedPtr& Child : ParsedChildData)
	{
		TSharedPtr<FImaginaryFiBDataAccessor, ESPMode::ThreadSafe> ChildAccessor = StaticCastSharedPtr<FImaginaryFiBDataAccessor>(Child);
		if (ChildAccessor->LookUpValue(FFindInBlueprintSearchTags::FiB_Name, DisplayText))
		{
			OutChild = Child;
			break;
		}
	}

	if (!OutChild)
	{
		for (const TPair<FindInBlueprintsHelpers::FSimpleFTextKeyStorage, FSearchableValueInfo>& Child : ParsedTagsAndValues)
		{
			const FSearchableValueInfoHelper ValueHelper(Child.Value, LookupTablePtr);
			const FString CommentMetadata = FString::Printf(TEXT("%s: %s"), *Child.Key.Text.ToString(), *ValueHelper.GetDisplayString());
			if (CommentMetadata.Equals(DisplayText.ToString()))
			{
				OutChild = AsShared();
				break;
			}
		}
	}

	return OutChild;
}

bool FImaginaryFiBDataAccessor::LookUpValue(const FText& Category, const FText& DisplayText)
{
	bool IsTagOrValueFound = false;
	if (const FSearchableValueInfo* SearchableValue = ParsedTagsAndValues.Find(Category))
	{
		const FSearchableValueInfoHelper ValueHelper(*SearchableValue, LookupTablePtr);
		if (ValueHelper.GetDisplayText().CompareTo(DisplayText) == 0)
		{
			IsTagOrValueFound = true;
		}
	}
	else
	{
		if (AsShared()->IsCategory())
		{
			const TSharedPtr<FCategorySectionHelper, ESPMode::ThreadSafe> CategoryObj = StaticCastSharedRef<FCategorySectionHelper>(AsShared());
			if (CategoryObj->GetCategoryFunctionName().Equals(DisplayText.ToString()))
			{
				IsTagOrValueFound = true;
			}
		}
	}
	return IsTagOrValueFound;
}

FString FImaginaryFiBDataAccessor::GetValue(const FText& Key) const
{
	FString OutValue;
	if (const FSearchableValueInfo* SearchableValue = ParsedTagsAndValues.Find(Key))
	{
		const FSearchableValueInfoHelper ValueHelper(*SearchableValue, LookupTablePtr);
		OutValue = ValueHelper.GetDisplayString();
	}
	return OutValue;
}
#undef LOCTEXT_NAMESPACE
