// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using System.Collections.Generic;

namespace BlueprintSearch.Commands.CommandHelpers
{
	public class BlueprintJsonObject
	{
		public string Value { get; set; }

		public List<BlueprintJsonObject> Children { get; set; }
	}
}
