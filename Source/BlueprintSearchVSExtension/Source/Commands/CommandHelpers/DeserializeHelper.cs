// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using System.Collections.Generic;

namespace BlueprintSearch.Commands.CommandHelpers
{
	public class BlueprintJsonObject
	{

		public BlueprintJsonObject()
		{
			Value = string.Empty;
			Children = new List<BlueprintJsonObject>();
		}

		public BlueprintJsonObject(string value)
		{
			Value = value;
			Children = null;
		}

		public string Value { get; set; }

		public List<BlueprintJsonObject> Children { get; set; }
	}
}
