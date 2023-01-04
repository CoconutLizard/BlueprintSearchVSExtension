// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHandlers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BlueprintSearch
{
	/// <summary>
	/// Interaction logic for BlueprintSearchWindowControl.
	/// </summary>
	public partial class BlueprintSearchWindowControl : UserControl
	{
		BlueprintSearchVSWindowVM ViewModel = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlueprintSearchWindowControl"/> class.
		/// </summary>
		public BlueprintSearchWindowControl(BlueprintSearchVSWindowVM inViewModel)
		{
			ViewModel = inViewModel;
			this.DataContext = ViewModel;
			this.InitializeComponent();
		}

		private void OnKeyDownHandler(object InSenderObject, KeyEventArgs InEventArgs)
		{
			ThreadHelper.ThrowIfNotOnUIThread("BlueprintSearchWindowControl.OnKeyDownHandler");
			if (InEventArgs.Key == Key.Enter)
			{
				SearchButtonClick(InSenderObject, null);
			}
		}

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		private void SearchButtonClick(object InSenderObject, RoutedEventArgs InEventArgs)
		{
			ThreadHelper.ThrowIfNotOnUIThread("BlueprintSearchWindowControl.SearchButtonClick");
			ViewModel.HandleSearch();
		}

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		private void CancelButtonClick(object InSenderObject, RoutedEventArgs InEventArgs)
		{
			ThreadHelper.ThrowIfNotOnUIThread("BlueprintSearchWindowControl.SearchButtonClick");
			ViewModel.CancelSearch();
		}
	}
}
