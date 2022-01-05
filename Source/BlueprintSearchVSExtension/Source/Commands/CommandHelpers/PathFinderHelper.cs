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

		public static string UnrealEditorExe { get; private set; } = string.Empty;

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

				string ProjectName = Path.GetFileNameWithoutExtension(UProjectFilePath);

				string UnrealManifestPath = GetUnrealManifestPath(ProjectName);
				UhtManifestJsonObject UnrealManifest = GetUnrealManifest(UnrealManifestPath);
				if(UnrealManifest == null)
				{
					MessageBox.Show($"BlueprintSearchVS will not work as it could not find/parse a .uhtmanifest file., expected in \"{UnrealManifestPath}\". \nPlease build your Game Project.", "BlueprintSearchVS Warning");
					return false;
				}

				string UERootPath = UnrealManifest.RootLocalPath;
				if (UERootPath.Length == 0)
				{
					MessageBox.Show($"BlueprintSearchVS will not work as it could not find the project root path from the .uhtmanifest file, loaded from \"{UnrealManifestPath}\".\nPlease build your Game Project.", "BlueprintSearchVS Warning");
					return false;
				}

				string EditorTargetPath = GetEditorTargetPath(ProjectName);
				EditorTargetJsonObject EditorTarget = GetEditorTarget(EditorTargetPath);
				if(EditorTarget == null)
				{
					MessageBox.Show($"BlueprintSearchVS will not work as it could not find/parse a .target file for the Editor, expected in \"{EditorTargetPath}\". \nPlease build your Game Project.", "BlueprintSearchVS Warning");
					return false;
				}

				string EditorPath = EditorTarget.Launch;
				if (EditorPath.Length == 0)
				{
					MessageBox.Show($"BlueprintSearchVS will not work as it could not find a the Editor executable, expected to be in the \"Launch\" value of Editor Target file, loaded from \"{EditorTargetPath}\".\nPlease build your Game Project.", "BlueprintSearchVS Warning");
					return false;
				}

				EditorPath = EditorPath.Replace("$(EngineDir)/", string.Empty);
				string EngineDir = Path.Combine(UERootPath, "Engine\\");

				UEEditorFilePath = Path.Combine(EngineDir, EditorPath).Replace('\\', '/');
				if (!File.Exists(UEEditorFilePath))
				{
					MessageBox.Show($"BlueprintSearchVS will not work as it could not find Unreal's editor executable, expected in \"{UEEditorFilePath}\".\nCheck that you have an up to date Development Editor build.", "BlueprintSearchVS Warning");
					return false;
				}

				UnrealEditorExe = Path.GetFileName(UEEditorFilePath);
				UEEditorFilePath = Path.GetDirectoryName(UEEditorFilePath);
				return true;
			}

			return false;
		}

		private static string GetUnrealManifestPath(string ProjectName)
		{
			string UhtManifestRelativePath = "Intermediate\\Build\\Win64\\$ProjectName$Editor\\Development\\$ProjectName$Editor.uhtmanifest";
			return Path.Combine(Path.GetFullPath(Path.Combine(UProjectFilePath, "..")), UhtManifestRelativePath.Replace("$ProjectName$", ProjectName));
		}

		private static UhtManifestJsonObject GetUnrealManifest(string UhtManifestRelativePath)
		{
			UhtManifestJsonObject UhtManifestObject = null;
			if(File.Exists(UhtManifestRelativePath))
			{
				using (StreamReader Reader = new StreamReader(UhtManifestRelativePath))
				{
					UhtManifestObject = JsonConvert.DeserializeObject<UhtManifestJsonObject>(Reader.ReadToEnd());
				}
			}

			return UhtManifestObject;
		}

		private static string GetEditorTargetPath(string ProjectName)
		{
			string EditorTargetPath = "Binaries\\Win64\\$ProjectName$Editor.target";
			return Path.Combine(Path.GetFullPath(Path.Combine(UProjectFilePath, "..")), EditorTargetPath.Replace("$ProjectName$", ProjectName));
		}

		private static EditorTargetJsonObject GetEditorTarget(string EditorTargetRelativePath)
		{
			EditorTargetJsonObject EditorTargetObject = null;
			if (File.Exists(EditorTargetRelativePath))
			{
				using (StreamReader Reader = new StreamReader(EditorTargetRelativePath))
				{
					EditorTargetObject = JsonConvert.DeserializeObject<EditorTargetJsonObject>(Reader.ReadToEnd());
				}
			}

			return EditorTargetObject;
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
