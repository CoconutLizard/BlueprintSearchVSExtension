//Copyright(C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using EnvDTE;
using EnvDTE80;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BlueprintSearch.Commands.CommandHelpers
{
	public static class PathFinderHelper
	{
		public static DTE2 DTEService;

		public static string UProjectFilePath { get; private set; }

		public static string UECommandLineFilePath { get; private set; }

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
					MessageBox.Show("BlueprintSearchVS could not find Unreal's command line executable.", "BlueprintSearchVS Warning");
				}
			}
		}

		private static string GetUnrealCommandLineExecutablePath()
		{
			string UhtManifestPath = Path.Combine(Path.GetFullPath(Path.Combine(DTEService.Solution.FullName, "..")), "Intermediate\\Build\\Win64\\VehicleGameEditor\\Development\\VehicleGameEditor.uhtmanifest");
			using (StreamReader Reader = new StreamReader(UhtManifestPath))
			{
				var UthManifestObject = JsonConvert.DeserializeObject<UthManifestJsonObject>(Reader.ReadToEnd());
				return UthManifestObject.RootLocalPath;
			}
		}

	}

}
