// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace BlueprintSearch
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(BlueprintSearchVSPackage.PackageGuidString)]

	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideToolWindow(typeof(BlueprintSearchVSWindow))]
	public sealed class BlueprintSearchVSPackage : AsyncPackage
	{
		/// <summary>
		/// BlueprintSearchPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "d736bb7a-6a91-476d-84dc-a27acedc454b";

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
		/// <param name="progress">A provider for progress updates.</param>
		/// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
			await BlueprintSearchVSWindowCommand.InitializeAsync(this);
		}

		#endregion
	}
}
