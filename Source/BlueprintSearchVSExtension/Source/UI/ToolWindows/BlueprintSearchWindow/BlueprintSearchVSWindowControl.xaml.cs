// Copyright (C) Coconut Lizard Limited. All rights reserved.

// ---------------------------------------------------------

using BlueprintSearch.Commands.CommandHandlers;
using Microsoft.VisualStudio.Shell;
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
		/// <summary>
		/// Initializes a new instance of the <see cref="BlueprintSearchWindowControl"/> class.
		/// </summary>
		public BlueprintSearchWindowControl()
		{
			this.InitializeComponent();
			SearchPathsCommand.EnableSearchBar += EnableSearchHandler;
			EnableSearchHandler(Commands.CommandHelpers.PathFinderHelper.EnableButtons);
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

		public void EnableSearchHandler(bool IsEnabled)
		{
			if (IsEnabled)
			{
				SearchButton.IsEnabled = true;
				SearchValue.IsEnabled = true;
				SearchValue.Text = "Type something to search";
				SearchValue.BorderThickness = new Thickness(0.1);
				SearchValue.BorderBrush = Brushes.Transparent;
			}
			else
			{
				SearchButton.IsEnabled = false;
				SearchValue.IsEnabled = false;
				SearchValue.Text = Commands.CommandHelpers.PathFinderHelper.SearchBoxErrorText;
				SearchValue.BorderThickness = new Thickness(1.0);
				SearchValue.BorderBrush = Brushes.Red;
			}
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
