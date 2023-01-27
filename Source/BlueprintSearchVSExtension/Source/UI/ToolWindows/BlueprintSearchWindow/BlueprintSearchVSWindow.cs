// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace BlueprintSearch
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("e49092ab-48f9-4fb9-88d4-3cd3e5fcfa36")]
	public class BlueprintSearchVSWindow : ToolWindowPane
	{
		public BlueprintSearchWindowControl WindowControl = null;

		public BlueprintSearchVSWindowVM WindowVM = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlueprintSearchVSWindow"/> class.
		/// </summary>
		public BlueprintSearchVSWindow() : base(null)
		{
			this.Caption = "BlueprintSearchWindow";

			this.WindowVM = new BlueprintSearchVSWindowVM();
			this.WindowControl = new BlueprintSearchWindowControl(this.WindowVM);

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			this.Content = this.WindowControl;
		}

		public void Shutdown()
		{
			this.WindowVM?.CancelSearch();
		}
	}
}
