// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueprintSearch.Commands.CommandHelpers
{
	public class BlueprintJsonObject
	{
		public string Value { get; set; }

		public List<BlueprintJsonObject> Children { get; set; }
	}
}
