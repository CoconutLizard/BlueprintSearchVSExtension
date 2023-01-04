using BlueprintSearch.Commands.CommandHandlers;
using BlueprintSearch.Commands.CommandHelpers;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;

namespace BlueprintSearch
{
	public class BlueprintSearchVSWindowVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		bool isSearching = false;
		public bool IsSearching
		{
			get => isSearching;
			set
			{
				isSearching = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(IsSearching)));
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CanSearch)));
			}
		}

		string searchText = string.Empty;
		public string SearchText
		{
			get => searchText;
			set
			{
				searchText = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SearchText)));
			}
		}

		ObservableCollection<BlueprintJsonObject> searchResults = new ObservableCollection<BlueprintJsonObject>();
		public ObservableCollection<BlueprintJsonObject> SearchResults
		{
			get => searchResults;
			set
			{
				searchResults = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));
			}
		}

		CancellationTokenSource cancellationTokenSource = null;
		public CancellationTokenSource CancellationSource
		{
			get => cancellationTokenSource;
			set
			{
				cancellationTokenSource = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CancellationSource)));
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CanCancel)));
			}
		}

		// a custom IValueConverter that implements invertible boolean to visibility conversion is preferable for this long term.
		public bool CanSearch => !IsSearching;
		public bool CanCancel => CancellationSource != null;

		public void CancelSearch()
		{
			CancellationSource?.Cancel();
		}

		public void HandleSearch()
		{
			if (IsSearching)
			{
				CancelSearch();
			}
			else if (string.IsNullOrEmpty(SearchText) == false)
			{
				IsSearching = true;
				CancellationSource = new CancellationTokenSource();
				Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.RunAsync(() => SearchForSymbolAsync());
			}
		}

		private async Task SearchForSymbolAsync()
		{
			if (await EnsureProjectSetupAsync())
			{
				if (CancellationSource != null && !CancellationSource.IsCancellationRequested)
				{
					try
					{
						// switch off main thread to run the search, avoid UI hang
						await TaskScheduler.Default;
						ExecuteSearchHandler search = new ExecuteSearchHandler();
						try
						{
							await search.MakeSearchAsync(SearchText, CancellationSource.Token);
						}
						catch (Exception e)
						{
							search.ErrorMessages.Add($"Search failed due to: {e.Message}");
						}

						// switch back to main thread once the search has completed so we can update UI element bindings safely.
						await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

						if (search.ErrorMessages.Count == 0)
						{
							SearchResults.Clear();
							search.Results.ForEach(result => SearchResults.Add(result));
						}
						else
						{
							MessageBox.Show(string.Join(",", search.ErrorMessages), "BlueprintSearchVS Error");
						}
					}
					catch (Exception e)
					{
						MessageBox.Show($"BlueprintSearchVS Error: {e.Message}"); ;
					}
				}
			}

			IsSearching = false;
			CancellationSource = null;
		}

		private async Task<bool> EnsureProjectSetupAsync()
		{
			await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			if (string.IsNullOrEmpty(PathFinderHelper.UEEditorFilePath))
			{
				if (PathFinderHelper.FindUEProject(out string UEProjectPath))
				{
					if (PathFinderHelper.FindPaths(UEProjectPath) == false)
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}

			return true;
		}
	}
}
