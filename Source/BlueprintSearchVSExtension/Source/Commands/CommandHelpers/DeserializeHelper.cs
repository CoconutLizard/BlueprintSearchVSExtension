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

		public BlueprintJsonObject(string InValue)
		{
			Value = InValue;
			Children = null;
		}

		public string Value { get; set; }

		public List<BlueprintJsonObject> Children { get; set; }
	}

	public class UhtManifestJsonObject
	{
		public bool IsGameTarget { get; set; }
		public string RootLocalPath { get; set; }
		public string TargetName { get; set; }
		public string ExternalDependenciesFile { get; set; }
	}

	public class EditorTargetJsonObject
	{
		public string TargetName { get; set; }
		public string Platform { get; set; }
		public string Configuration { get; set; }
		public string TargetType { get; set; }
		public string Architecture { get; set; }
		public string Project { get; set; }
		public string Launch { get; set; }
		public string LaunchCmd { get; set; }
	}

}
