using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Horizon;
using Horizon.Documents;
using Horizon.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using RandomJoke;

namespace JokeOfTheDay
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid(PackageGuidString)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
	[ProvideOptionPage(typeof(JokeOfTheDayOptions), "JokeOfTheDay", "General", 0, 0, true)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideToolWindow(typeof(AllJokesPanel))]
	public sealed class JokeOfTheDayPackage : Package
	{
		public const string PackageGuidString = "ba98182d-d752-48e2-9532-3c4f669dfa6f";

		private Events events;
		private DocumentEvents docEvents;

		/// <summary>
		/// Initialization of the package
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// This two objects must be cached. 
			// Otherwise GC will collect them.
			events = VS.Instance.Events;
			docEvents = events.DocumentEvents;

			docEvents.DocumentSaved += onSave;

            ReplaceSelectionWithJokeCommand.Initialize(this);
            AllJokesPanelCommand.Initialize(this);
        }

        private async void onSave(Document document)
		{
			try
			{
				EditPointsSpan currentLine =
					document
					.AsTextDocument()
					.GetCurrentLineSpan();

				string text = currentLine.Text?.Trim();

				if (text == "//joke")
				{
					JokeOfTheDayOptions options =
						this.OptionsPage<JokeOfTheDayOptions>();

					string joke =
						await Jokes.JokeAsync(options.FirstName, options.LastName);

					currentLine.Text = joke;
					addJokeToList(joke);
				}
			}
			catch
			{
			}
		}

		public void ReplaceSelectionWithJoke()
		{
			try
			{
				TextSelection selection = VS.Instance.ActiveDocument.Selection;
				JokeOfTheDayOptions options = this.OptionsPage<JokeOfTheDayOptions>();
				string joke = Jokes.Joke(options.FirstName, options.LastName);
				selection.Text = joke;

				addJokeToList(joke);
			}
			catch { }
		}

		private void addJokeToList(string joke)
		{
			AllJokesPanelControl window =
				FindToolWindow(typeof(AllJokesPanel), 0, true).Content as AllJokesPanelControl;

			window.AddJoke(joke);
		}
	}
}
