// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BlueprintSearch.Commands.CommandHandlers
{
	public class ExecuteSearchHandler
	{
		public List<BlueprintJsonObject> MakeSearch(string InSearchValue)
		{
			string Arguments = PathFinderHelper.UECommandLineFilePath + " " + PathFinderHelper.UProjectFilePath + " " + InSearchValue;
			System.Diagnostics.Process Proc = new System.Diagnostics.Process();
			Proc.StartInfo.FileName = PathFinderHelper.CommmandletFileName;
			Proc.StartInfo.WorkingDirectory = PathFinderHelper.WorkingDirectoryPath;
			Proc.StartInfo.Arguments = Arguments;
			Proc.StartInfo.UseShellExecute = true;
			Proc.Start();
			Proc.WaitForExit();
			string SearchResultsPath = Path.Combine( PathFinderHelper.WorkingDirectoryPath, "SearchResults.json");
			using (StreamReader Reader = new StreamReader(SearchResultsPath))
			{
				var SearchResults = JsonConvert.DeserializeObject<List<BlueprintJsonObject>>(Reader.ReadToEnd());
				if(SearchResults == null)
				{
					SearchResults = new List<BlueprintJsonObject>() { new BlueprintJsonObject("No Results Found") };
				}

				return SearchResults;
			}
		}
	}
}
