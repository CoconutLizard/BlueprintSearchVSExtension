//Copyright(C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using EnvDTE;
using EnvDTE80;
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

			foreach (Project project in ProjectsList)
			{
				if (project.Name.Equals("UE4"))
				{
					// This solution contains a ue4 project, find the path!
					string EngineIncludePath = GetUnrealCommandLineExecutablePathFromProject(project);
					if (string.IsNullOrEmpty(EngineIncludePath))
					{
						MessageBox.Show("BlueprintSearchVS could not find an unreal project in this solution.", "BlueprintSearchVS Warning");
					}
					else
					{
						//We know that we are in a ue4 project, so we analyze all the ue4 metadata we need.
						UECommandLineFilePath = EngineIncludePath;
						const string UProjectExtension = ".uproject";
						UProjectFilePath = Path.GetFullPath(Path.ChangeExtension(DTEService.Solution.FullName, UProjectExtension)).Replace('\\', '/');
					}

					return;
				}
			}
		}

		private static string GetUnrealCommandLineExecutablePathFromProject(EnvDTE.Project Prj)
		{
			string SourceDirectoryPath = Path.GetDirectoryName(Prj.FullName);
			string UE4CmdRelativePath = "Engine\\Binaries\\Win64\\UE4Editor-Cmd.exe";

			while (Path.GetFullPath(SourceDirectoryPath) != Path.GetPathRoot(SourceDirectoryPath))
			{
				string SearchPath = Path.Combine(Path.GetFullPath(SourceDirectoryPath), UE4CmdRelativePath);
				var DirInfo = new DirectoryInfo(Path.GetDirectoryName(SearchPath));
				if (DirInfo.Exists)
				{
					return SearchPath.Replace('\\', '/');
				}

				SourceDirectoryPath = Path.Combine(SourceDirectoryPath, "..");
			}

			return string.Empty;
		}

	}

}
