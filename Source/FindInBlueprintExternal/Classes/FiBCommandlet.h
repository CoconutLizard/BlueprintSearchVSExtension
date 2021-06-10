#pragma once

#include <Runtime/Core/Public/CoreMinimal.h>
#include <Runtime/CoreUObject/Public/UObject/ObjectMacros.h>
#include <Runtime/Engine/Classes/Commandlets/Commandlet.h>
#include "FiBCommandlet.generated.h"

UCLASS(MinimalAPI)
class UFiBCommandlet final : public UCommandlet
{
	GENERATED_BODY()

	//~ Begin UCommandlet Interface
	virtual int32 Main(const FString& Params) override;
	//~ End UCommandlet Interface

};

