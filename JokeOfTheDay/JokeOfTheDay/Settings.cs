using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace JokeOfTheDay
{
	public static class Settings
	{
		public static T Value<T>(string options)
		{
			try
			{
				string[] parts = options.Split('.');
				string category = parts[0];
				string page = parts[1];
				string property = parts[2];

				DTE2 vs = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;
				Properties props = vs.get_Properties(category, page);
				dynamic val = props.Item(property).Value;
				return (T)val;
			}
			catch
			{
				return default(T);
			}
		}
	}
}
