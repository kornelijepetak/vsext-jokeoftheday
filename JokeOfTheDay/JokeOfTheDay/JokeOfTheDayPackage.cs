﻿//------------------------------------------------------------------------------
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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using RandomJoke;

namespace JokeOfTheDay
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(JokeOfTheDayPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideOptionPage(typeof(JokeOfTheDayDialogPage), "JokeOfTheDay", "General", 0, 0, true)]
    public sealed class JokeOfTheDayPackage : Package
    {
        /// <summary>
        /// JokeOfTheDayPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ba98182d-d752-48e2-9532-3c4f669dfa6f";

        /// <summary>
        /// Initializes a new instance of the <see cref="JokeOfTheDayPackage"/> class.
        /// </summary>
        public JokeOfTheDayPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            vs = GetService(typeof(DTE)) as DTE2;

            events = vs.Events;
            docEvents = events.DocumentEvents;
            docEvents.DocumentSaved += onSave;
        }

        private async void onSave(Document Document)
        {
            TextDocument doc = Document.Object() as TextDocument;

            var start = doc.Selection.ActivePoint.CreateEditPoint();
            start.StartOfLine();

            var end = doc.Selection.ActivePoint.CreateEditPoint();
            end.EndOfLine();

            string text = start.GetText(end).Trim();

            if(text == "//joke")
            {
                string firstName = Settings.Value<string>("JokeOfTheDay.General.FirstName");
                string lastName = Settings.Value<string>("JokeOfTheDay.General.LastName");

                string joke = await Jokes.JokeAsync(firstName, lastName);
                start.ReplaceText(end, joke, (int)(vsEPReplaceTextOptions.vsEPReplaceTextAutoformat));
            }
        }

        DTE2 vs;
        Events events;
        DocumentEvents docEvents;

        #endregion
    }

    [ComVisible(true)]
    public class JokeOfTheDayDialogPage : DialogPage
    {
        [Description("Name"), Category("Name"), DisplayName("First Name")]
        public string FirstName { get; set; }

        [Description("Last name"), Category("Name"), DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}
