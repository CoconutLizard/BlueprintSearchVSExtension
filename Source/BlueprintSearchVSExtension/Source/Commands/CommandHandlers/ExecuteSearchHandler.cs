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

		public void MakeSearch(string InSearchValue)
		{

			//PathFinderHelper.FindPaths();
			string Arguments = PathFinderHelper.UECommandLineFilePath + " " + PathFinderHelper.UProjectFilePath + " " + InSearchValue;

			System.Diagnostics.Process Proc = new System.Diagnostics.Process();
			Proc.StartInfo.FileName = "RunSearchCommandlet.sh";
			Proc.StartInfo.WorkingDirectory = Path.GetFullPath("../..") + "\\Source\\Scripts\\";
			Proc.StartInfo.Arguments = Arguments;
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
