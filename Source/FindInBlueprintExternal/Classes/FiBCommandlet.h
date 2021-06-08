#pragma once

#include "CoreMinimal.h"
#include "UObject/ObjectMacros.h"
#include "Commandlets/Commandlet.h"
#include "FiBCommandlet.generated.h"

UCLASS(MinimalAPI)
class UFiBCommandlet : public UCommandlet
{
	GENERATED_BODY()

		//~ Begin UCommandlet Interface
		virtual int32 Main(const FString& Params) override;
	//~ End UCommandlet Interface

};

