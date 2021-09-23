// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BlueprintSearch.Commands.CommandHandlers
{
	public class ExecuteSearchHandler
	{
		private const string CommandLineArguments = "-NoShaderCompile -SILENT -run=FiB";

		public List<BlueprintJsonObject> MakeSearch(string InSearchValue)
		{
			List<BlueprintJsonObject> SearchResults = new List<BlueprintJsonObject>() { new BlueprintJsonObject(string.Empty) };
			bool PathsFound = true;
			if (PathFinderHelper.UEEditorFilePath.Length == 0 || PathFinderHelper.UProjectFilePath.Length == 0 || PathFinderHelper.WorkingDirectoryPath.Length == 0)
			{
				PathsFound = PathFinderHelper.FindPaths();
			}

			if (PathsFound)
			{
				string Arguments = PathFinderHelper.UProjectFilePath + " " + CommandLineArguments + " " + PathFinderHelper.AddQuotes(InSearchValue);
				System.Diagnostics.Process Proc = new System.Diagnostics.Process();
				Proc.StartInfo.FileName = PathFinderHelper.UnrealEditorExe;
				Proc.StartInfo.WorkingDirectory = PathFinderHelper.UEEditorFilePath;
				Proc.StartInfo.Arguments = Arguments;
				Proc.StartInfo.UseShellExecute = true;
				Proc.Start();
				Proc.WaitForExit();
				string SearchResultsPath = Path.Combine(PathFinderHelper.UEEditorFilePath, "SearchResults.json");
				if (File.Exists(SearchResultsPath))
				{
					using (StreamReader Reader = new StreamReader(SearchResultsPath))
					{
						SearchResults = JsonConvert.DeserializeObject<List<BlueprintJsonObject>>(Reader.ReadToEnd());
						if (SearchResults == null)
						{
							SearchResults = new List<BlueprintJsonObject>() { new BlueprintJsonObject("No Results Found") };
						}
					}
					File.Delete(SearchResultsPath);
				}
				else
				{
					MessageBox.Show("BlueprintSearchVS couldn't find SearchResults.json.\nPlease install FindInBlueprintsExternal Plugin inside \"/Engine/Plugins\"", "BlueprintSearchVS Warning");
				}
			}

			return SearchResults;
		}
	}
}
