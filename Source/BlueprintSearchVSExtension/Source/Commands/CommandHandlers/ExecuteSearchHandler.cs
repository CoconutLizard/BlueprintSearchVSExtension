// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlueprintSearch.Commands.CommandHandlers
{
	public class ExecuteSearchHandler
	{
		public List<BlueprintJsonObject> Results = new List<BlueprintJsonObject>();

		public List<string> ErrorMessages = new List<string>();

		private const string CommandLineArguments = "-NoShaderCompile -SILENT -run=FiB";

		public async Task MakeSearchAsync(string InSearchValue, CancellationToken CancellationToken)
		{
			Results.Clear();

			if (string.IsNullOrEmpty(InSearchValue))
			{
				ErrorMessages.Add("BlueprintSearchVS has not been given a valid search term to search for.");
			}
			else if (string.IsNullOrEmpty(PathFinderHelper.UProjectFilePath))
			{
				ErrorMessages.Add("BlueprintSearchVS has not found a valid .uproject file from the loaded list of projects, please ensure you're in the correct solution and it has been built successfully.");
			}
			else
			{
				string Arguments = PathFinderHelper.UProjectFilePath + " " + CommandLineArguments + " " + PathFinderHelper.AddQuotes(InSearchValue);
				System.Diagnostics.Process Proc = new System.Diagnostics.Process();
				Proc.StartInfo.FileName = PathFinderHelper.UnrealEditorExe;
				Proc.StartInfo.WorkingDirectory = PathFinderHelper.UEEditorFilePath;
				Proc.StartInfo.Arguments = Arguments;
				Proc.StartInfo.UseShellExecute = true;
				Proc.Start();

				while (!Proc.HasExited && !CancellationToken.IsCancellationRequested)
				{
					// need to check back periodically to see if the task is finished/cancelled without doing a busy wait.
					await Task.Delay(20);
				}

				if (CancellationToken.IsCancellationRequested)
				{
					Proc.Kill();
					Results = new List<BlueprintJsonObject>() { new BlueprintJsonObject("Search cancelled") };
					return;
				}

				string SearchResultsPath = Path.Combine(PathFinderHelper.UEEditorFilePath, "SearchResults.json");
				if (File.Exists(SearchResultsPath))
				{
					using (StreamReader Reader = new StreamReader(SearchResultsPath))
					{
						Results = JsonConvert.DeserializeObject<List<BlueprintJsonObject>>(await Reader.ReadToEndAsync());
						if (Results == null || Results.Count == 0)
						{
							Results = new List<BlueprintJsonObject>() { new BlueprintJsonObject("No Results Found") };
						}
					}
					File.Delete(SearchResultsPath);
				}
				else
				{
					ErrorMessages.Add("BlueprintSearchVS couldn't find SearchResults.json.\nPlease install FindInBlueprintsExternal Plugin inside \"/Engine/Plugins\"");
				}
			}
		}
	}
}
