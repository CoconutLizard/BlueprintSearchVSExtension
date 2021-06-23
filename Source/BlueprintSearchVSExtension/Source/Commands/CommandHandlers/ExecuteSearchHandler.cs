// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BlueprintSearch.Commands.CommandHandlers
{
	public class ExecuteSearchHandler
	{
		public void MakeSearch(string InSearchValue)
		{
			Process Proc = new Process();
			Proc.StartInfo.FileName = "RunSearchCommandlet.sh";
			Proc.StartInfo.WorkingDirectory = Path.GetFullPath("../..") + "\\Source\\Scripts\\";
			Proc.StartInfo.Arguments = InSearchValue;
			Proc.StartInfo.UseShellExecute = true;
			Proc.Start();
			Proc.WaitForExit();

			string SearchResultsPath = Path.GetFullPath("../..") + "\\Source\\Scripts\\SearchResults.json";
			using (StreamReader Reader = new StreamReader(SearchResultsPath))
			{
				List<BlueprintJsonObject> SearchResults = JsonConvert.DeserializeObject<List<BlueprintJsonObject>>(Reader.ReadToEnd());
			}
		}
	}
}
