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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using RandomJoke;

namespace JokeOfTheDay
{

	[ComVisible(true)]
	public class JokeOfTheDayDialogPage : DialogPage
	{
		[Description("Name"), Category("Name"), DisplayName("First Name")]
		public string FirstName { get; set; }

		[Description("Last name"), Category("Name"), DisplayName("Last Name")]
		public string LastName { get; set; }
	}
}
