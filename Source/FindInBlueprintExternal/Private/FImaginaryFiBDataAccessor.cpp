#include "FindInBlueprintExternal/Classes/FImaginaryFiBDataAccessor.h"
#include <Editor/Kismet/Public/FindInBlueprintManager.h>
#include <Editor/Kismet/Public/FindInBlueprints.h>
#include <Editor/Kismet/Public/ImaginaryBlueprintData.h>

#define LOCTEXT_NAMESPACE "FindInBlueprintsCommandlet"

FText FindInBlueprintsHelpers::AsFText(int32 InValue, const TMap<int32, FText>& InLookupTable)
{
	if (const FText* LookupText = InLookupTable.Find(InValue))
	{
		return *LookupText;
	}
	// Let's never get here.
	return LOCTEXT("FiBSerializationError", "There was an error in serialization!");
}

/** Returns the display text to use for this item */
FText FSearchableValueInfo::GetDisplayText(const TMap<int32, FText>& InLookupTable) const
{
	FText Result;
	if (!DisplayText.IsEmpty() || LookupTableKey == -1)
	{
		Result = DisplayText;
	}
	else
	{
		Result = FindInBlueprintsHelpers::AsFText(LookupTableKey, InLookupTable);
	}
	return Result;
}

FString FImaginaryFiBDataAccessor::GetInfo(const FText& Category, const FText& DisplayText)
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
			const FString CommentMetadata = FString::Printf(TEXT("%s: %s"), *Child.Key.Text.ToString(), *Child.Value.GetDisplayText(*LookupTablePtr).ToString());
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
		if (SearchableValue->GetDisplayText(*LookupTablePtr).CompareTo(DisplayText) == 0)
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

FString FImaginaryFiBDataAccessor::GetValue(const FText& Key)
{
	FString OutValue;
	if (const FSearchableValueInfo* SearchableValue = ParsedTagsAndValues.Find(Key))
	{
		OutValue = SearchableValue->GetDisplayText(*LookupTablePtr).ToString();
	}
	return OutValue;
}
#undef LOCTEXT_NAMESPACE
