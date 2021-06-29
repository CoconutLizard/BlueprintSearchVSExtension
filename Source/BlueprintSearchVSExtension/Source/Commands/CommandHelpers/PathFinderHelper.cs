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

		public static string UProjectFilePath { get; set; }

		public static string UECommandLineFilePath { get; set; }

		public static void FindPaths()
		{
			List<Project> ProjectsList = new List<Project>();
			ProjectsList.AddRange(DTEService.Solution.Projects.Cast<Project>());

			for (int i = 0; i < ProjectsList.Count; i++)
			{
				ProjectsList.AddRange(ProjectsList[i].ProjectItems.Cast<ProjectItem>().Select(x => x.SubProject).OfType<Project>());
			}

			for (int i = 0; i < ProjectsList.Count; ++i)
			{

				if (ProjectsList[i].Name.Equals("UE4"))
				{
					// This solution contains a ue4 project, find the path!
					string EngineIncludePath = GetUnrealCommandLineExecutablePathFromProject(ProjectsList[i]);
					UECommandLineFilePath = EngineIncludePath;
					if (EngineIncludePath.Equals(string.Empty))
					{
						MessageBox.Show("BlueprintSearchVS could not find an unreal project in this solution.", "BlueprintSearchVS Warning");
					}

					//We know that we are in a ue4 project, so we analyze all the ue4 metadata we need.
					const string UProjectExtension = ".uproject";
					UProjectFilePath = Path.GetFullPath(Path.ChangeExtension(DTEService.Solution.FullName, UProjectExtension)).Replace('\\', '/');

					return;
				}
			}

			UECommandLineFilePath = string.Empty;
			MessageBox.Show("BlueprintSearchVS could not find an unreal project in this solution.", "BlueprintSearchVS Warning");
		}

		private static string GetUnrealCommandLineExecutablePathFromProject(EnvDTE.Project Prj)
		{
			string SourceDirectoryPath;
			if (Prj.FullName.Contains("Engine\\Intermediate\\ProjectFiles\\")) // this is a valid check to know that the unreal source directory is located at: ../../Source/
			{
				SourceDirectoryPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Prj.FullName), "..", "..")) + "\\Binaries\\Win64\\UE4Editor-Cmd.exe";
				var DirInfo2 = new DirectoryInfo(Path.GetDirectoryName(SourceDirectoryPath));
				if (DirInfo2.Exists)
				{ 
					return SourceDirectoryPath.Replace('\\', '/');
				}
			}

			SourceDirectoryPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Prj.FullName), "..", "..", "..")) + "\\Engine\\Binaries\\Win64\\UE4Editor-Cmd.exe";
			var DirInfo = new DirectoryInfo(Path.GetDirectoryName(SourceDirectoryPath));
			if (DirInfo.Exists)
			{
				return SourceDirectoryPath.Replace('\\', '/');
			}

			SourceDirectoryPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Prj.FullName), "..", "..", "..", "..")) + "\\Engine\\Binaries\\Win64\\UE4Editor-Cmd.exe";
			DirInfo = new DirectoryInfo(Path.GetDirectoryName(SourceDirectoryPath));
			if (DirInfo.Exists)
			{
				return SourceDirectoryPath.Replace('\\', '/');
			}

			return string.Empty;
		}

	}

}
