//------------------------------------------------------------------------------
// <copyright file="AllJokesPanelControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace JokeOfTheDay
{
	using System.Collections.ObjectModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for AllJokesPanelControl.
	/// </summary>
	public partial class AllJokesPanelControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AllJokesPanelControl"/> class.
		/// </summary>
		public AllJokesPanelControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		public ObservableCollection<string> Jokes { get; set; } = new ObservableCollection<string>();

		public void AddJoke(string joke)
		{
			joke = joke
				.Replace("//", "")
				.Replace("\t", " ")
				.Replace("\n", " ")
				.Replace("\r", " ");

			while (joke.IndexOf("  ") > 0)
				joke = joke.Replace("  ", " ");

			Jokes.Add(joke);
		}
	}
}