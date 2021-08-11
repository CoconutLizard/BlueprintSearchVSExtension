// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHandlers;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlueprintSearch
{
	/// <summary>
	/// Interaction logic for BlueprintSearchWindowControl.
	/// </summary>
	public partial class BlueprintSearchWindowControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlueprintSearchWindowControl"/> class.
		/// </summary>
		public BlueprintSearchWindowControl()
		{
			this.InitializeComponent();
			Commands.CommandHelpers.PathFinderHelper.EnableSearchbarButton += EnableButtonHandler;
		}

		public void SearchBarGotFocus(object InSenderObject, RoutedEventArgs InEventArgs)
		{
			TextBox SearchBox = (TextBox)InSenderObject;
			SearchBox.Text = string.Empty;
			SearchBox.GotFocus -= SearchBarGotFocus;
		}

		private void OnKeyDownHandler(object InSenderObject, KeyEventArgs InEventArgs)
		{
			if(InEventArgs.Key == Key.Enter)
			{
				SearchButtonClick(InSenderObject, null);
			}
		}

		public void EnableButtonHandler(bool IsEnabled)
		{
			SearchButton.IsEnabled = IsEnabled;
			SearchValue.IsEnabled = IsEnabled;
		}

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
		private void SearchButtonClick(object InSenderObject, RoutedEventArgs InEventArgs)
		{
			ThreadHelper.ThrowIfNotOnUIThread("BlueprintSearchWindowControl.SearchButtonClick");
			ExecuteSearchHandler ExecuteSearch = new ExecuteSearchHandler();
			ResultsView.ItemsSource = ExecuteSearch.MakeSearch(SearchValue.Text);
		}
	}
}
