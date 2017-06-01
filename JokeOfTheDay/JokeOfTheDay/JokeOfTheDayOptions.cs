using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace JokeOfTheDay
{
	/// <summary>
	/// Represents an options dialog for JokeOfTheDay
	/// </summary>
	/// <remarks>Don't forget that [ComVisible(true)] is required</remarks>
	[ComVisible(true)]
	public class JokeOfTheDayOptions : DialogPage
	{
		[Category("Name")]
		[Description("First Name")]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Category("Name")]
		[Description("Last Name")]
		[DisplayName("Last Name")]
		public string LastName { get; set; }
	}
}
