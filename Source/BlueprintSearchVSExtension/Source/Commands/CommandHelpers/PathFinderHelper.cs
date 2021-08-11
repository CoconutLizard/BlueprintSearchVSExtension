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

		public static string UProjectFilePath { get; private set; }

		public static string UECommandLineFilePath { get; private set; }

		public static string WorkingDirectoryPath { get; private set; }

		public const string CommmandletFileName = "RunSearchCommandlet.bat";

		private const char QuoteChar = '\"';

		public delegate void EnableButton(bool IsEnabled);

		public static event EnableButton EnableSearchbarButton;

		public static void FindPaths()
		{
			List<Project> ProjectsList = new List<Project>();
			ProjectsList.AddRange(DTEService.Solution.Projects.Cast<Project>());

			for (int i = 0; i < ProjectsList.Count; i++)
			{
				ProjectsList.AddRange(ProjectsList[i].ProjectItems.Cast<ProjectItem>().Select(x => x.SubProject).OfType<Project>());
			}

			if (ProjectsList.Count > 0)
			{
				foreach (Project project in ProjectsList)
				{
					if (project.Name.Equals("UE4"))
					{
						//We know that we are in a ue4 project, so we analyze all the ue4 metadata we need.
						const string UProjectExtension = ".uproject";
						UProjectFilePath = Path.GetFullPath(Path.ChangeExtension(DTEService.Solution.FullName, UProjectExtension)).Replace('\\', '/');
						break;
					}
				}

				if (!File.Exists(UProjectFilePath))
				{
					MessageBox.Show("BlueprintSearchVS could not find an Unreal project in this solution.", "BlueprintSearchVS Warning");
					return;
				}

				string UERootPath = GetUnrealCommandLineExecutablePath();
				UECommandLineFilePath = Path.Combine(UERootPath, "Engine\\Binaries\\Win64\\UE4Editor-Cmd.exe").Replace('\\', '/');
				if (!File.Exists(UECommandLineFilePath))
				{
					MessageBox.Show("BlueprintSearchVS could not find Unreal's command line executable.\nCheck that you have an up to date Development Editor build.", "BlueprintSearchVS Warning");
					return;
				}

				string ExtensionRootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				WorkingDirectoryPath = Path.Combine(ExtensionRootFolder, "Source\\Scripts\\");
				if (!File.Exists(Path.Combine(WorkingDirectoryPath, CommmandletFileName)))
				{
					MessageBox.Show("BlueprintSearchVS could not find CommandletScript.", "BlueprintSearchVS Warning");
					return;
				}

				EnableSearchbarButton(true);
			}
		}

		private static string GetUnrealCommandLineExecutablePath()
		{
			string UhtManifestPath = "Intermediate\\Build\\Win64\\$ProjectName$Editor\\Development\\$ProjectName$Editor.uhtmanifest";
			string ProjectName = Path.GetFileNameWithoutExtension(DTEService.Solution.FileName);
			string UhtManifestRelativePath = Path.Combine(Path.GetFullPath(Path.Combine(DTEService.Solution.FullName, "..")), UhtManifestPath.Replace("$ProjectName$", ProjectName));
			using (StreamReader Reader = new StreamReader(UhtManifestRelativePath))
			{
				var UhtManifestObject = JsonConvert.DeserializeObject<UhtManifestJsonObject>(Reader.ReadToEnd());
				return UhtManifestObject.RootLocalPath;
			}
		}

		public static string GetScriptPath()
		{
			string RootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string OutPath = Path.Combine(RootPath, "Source\\Scripts\\");
			if (!File.Exists(Path.Combine(OutPath, CommmandletFileName)))
			{
				OutPath = "";
				MessageBox.Show("BlueprintSearchVS could not find CommandletScript.", "BlueprintSearchVS Warning");
			}
			return OutPath;
		}

		public static string AddQuotes(string InStringToQuote)
		{
			return QuoteChar + InStringToQuote + QuoteChar;
		}
	}

}
