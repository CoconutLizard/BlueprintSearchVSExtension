// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include <Runtime/Core/Public/CoreMinimal.h>
#include <Runtime/Core/Public/Modules/ModuleManager.h>

class FFindInBlueprintExternalModule : public IModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
};
