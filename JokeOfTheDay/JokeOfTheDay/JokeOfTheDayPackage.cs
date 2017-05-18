//------------------------------------------------------------------------------
// <copyright file="JokeOfTheDayPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Horizon;
using Horizon.Documents;
using Horizon.Settings;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using RandomJoke;

namespace JokeOfTheDay
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(JokeOfTheDayPackage.PackageGuidString)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
	[ProvideOptionPage(typeof(JokeOfTheDayDialogPage), "JokeOfTheDay", "General", 0, 0, true)]
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
		}

		private async void onSave(Document Document)
		{
			TextDocument doc = Document.AsTextDocument();

			if(doc == null)
				return;

			EditPointsSpan currentLine = doc.GetCurrentLineSpan();

			string text = currentLine.Text?.Trim();

			if(text == "//joke")
			{
				JokeOfTheDayDialogPage options = this.OptionsPage<JokeOfTheDayDialogPage>();
				currentLine.Text = await Jokes.JokeAsync(options.FirstName, options.LastName);
			}
		}
	}
}
