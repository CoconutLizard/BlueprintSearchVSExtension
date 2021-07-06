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
		}

		public void SearchBarGotFocus(object sender, RoutedEventArgs e)
		{
			TextBox SearchBox = (TextBox)sender;
			SearchBox.Text = string.Empty;
			SearchBox.GotFocus -= SearchBarGotFocus;
		}

		private void OnKeyDownHandler(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				SearchButtonClick(sender, null);
			}
		}

		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
		private void SearchButtonClick(object sender, RoutedEventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			ExecuteSearchHandler executeSearch = new ExecuteSearchHandler();
			ResultsView.ItemsSource = executeSearch.MakeSearch(SearchValue.Text);
		}
	}
}
