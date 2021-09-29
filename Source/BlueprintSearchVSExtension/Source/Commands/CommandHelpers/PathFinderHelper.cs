//Copyright(C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using EnvDTE;
using EnvDTE80;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BlueprintSearch.Commands.CommandHelpers
{
	public static class PathFinderHelper
	{
		public static DTE2 DTEService;

		public static string UProjectFilePath { get; private set; } = string.Empty;

		public static string UEEditorFilePath { get; private set; } = string.Empty;

		public static string WorkingDirectoryPath { get; private set; } = string.Empty;

		public const string UnrealEditorExe = "UE4Editor.exe";

		private const char QuoteChar = '\"';


		public static bool FindPaths()
		{
			List<Project> ProjectsList = new List<Project>();
			ProjectsList.AddRange(DTEService.Solution.Projects.Cast<Project>());

			for (int i = 0; i < ProjectsList.Count; i++)
			{
				ProjectsList.AddRange(ProjectsList[i].ProjectItems.Cast<ProjectItem>().Select(x => x.SubProject).OfType<Project>());
			}

			//Check if this function is called after all the projects in the solution have been loaded
			if (ProjectsList.Count > 0)
			{
				foreach (Project Project in ProjectsList)
				{
					if(CheckProjectForUEFiles(Project))
					{
						break;
					}
				}

				if (!File.Exists(UProjectFilePath))
				{
					MessageBox.Show("BlueprintSearchVS will not work as it could not find an Unreal project in this solution.", "BlueprintSearchVS Warning");
					return false;
				}

				string UERootPath = GetUnrealCommandLineExecutablePath();
				if(UERootPath.Length == 0)
				{
					MessageBox.Show("BlueprintSearchVS will not work as it could not find a .uhtmanifest file.\nPlease build your Game Project.", "BlueprintSearchVS Warning");
					return false;
				}

				UEEditorFilePath = Path.Combine(UERootPath, "Engine\\Binaries\\Win64\\").Replace('\\', '/');
				if (!File.Exists(Path.Combine(UEEditorFilePath, UnrealEditorExe)))
				{
					MessageBox.Show("BlueprintSearchVS will not work as it could not find Unreal's editor executable.\nCheck that you have an up to date Development Editor build.", "BlueprintSearchVS Warning");
					return false;
				}

				return true;
			}

			return false;
		}

		private static string GetUnrealCommandLineExecutablePath()
		{
			string UhtManifestPath = "Intermediate\\Build\\Win64\\$ProjectName$Editor\\Development\\$ProjectName$Editor.uhtmanifest";
			string ProjectName = Path.GetFileNameWithoutExtension(UProjectFilePath);
			string UhtManifestRelativePath = Path.Combine(Path.GetFullPath(Path.Combine(UProjectFilePath, "..")), UhtManifestPath.Replace("$ProjectName$", ProjectName));
			string CommandLinePath = string.Empty;
			if(File.Exists(UhtManifestRelativePath))
			{
				using (StreamReader Reader = new StreamReader(UhtManifestRelativePath))
				{
					var UhtManifestObject = JsonConvert.DeserializeObject<UhtManifestJsonObject>(Reader.ReadToEnd());
					if(UhtManifestObject != null)
					{
						CommandLinePath = UhtManifestObject.RootLocalPath;
					}
				}
			}

			return CommandLinePath;
		}

		public static string AddQuotes(string InStringToQuote)
		{
			return QuoteChar + InStringToQuote + QuoteChar;
		}

		private static bool CheckProjectForUEFiles(Project Project)
		{
			bool OutIsUnrealProject = false;
			foreach (ProjectItem FilterOrFile in Project.ProjectItems)
			{
				const string UProjectExtension = ".uproject";
				if (Path.GetExtension(FilterOrFile.Name) == UProjectExtension)
				{
					UProjectFilePath = FilterOrFile.GetFullPath().Replace('\\', '/');
					OutIsUnrealProject = true;
				}
			}
			return OutIsUnrealProject;
		}
	}

}
