// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace NClass.Translations
{
	public class UILanguage
	{
		static List<UILanguage> availableCultures;

		static UILanguage()
		{
			// Load localization resources
			Assembly assembly = Assembly.GetExecutingAssembly();
			string resourceDir = Path.GetDirectoryName(assembly.Location);

			// Search for localized cultures
			try
			{
				DirectoryInfo resource = new DirectoryInfo(resourceDir);
				DirectoryInfo[] directories = resource.GetDirectories("*",
					SearchOption.TopDirectoryOnly);
				availableCultures = new List<UILanguage>(directories.Length + 2);

				foreach (DirectoryInfo directory in directories)
				{
					if (directory.Name != "Plugins" && directory.Name != "Templates")
					{
						string cultureName = directory.Name;
						UILanguage language = CreateUILanguage(cultureName);
						if (language != null)
							availableCultures.Add(language);
					}
				}
			}
			catch
			{
				availableCultures = new List<UILanguage>(2);
			}

			availableCultures.Add(CreateDefaultUILanguage());
			availableCultures.Add(CreateUILanguage("en"));
			availableCultures.Sort(delegate(UILanguage c1, UILanguage c2)
			{
				return c1.Name.CompareTo(c2.Name);
			});
		}

		CultureInfo culture;
		bool isDefault;

		private UILanguage()
		{
		}

		private UILanguage(CultureInfo culture)
		{
			this.culture = culture;
			this.isDefault = false;
		}

		public string Name
		{
			get
			{
				if (IsDefault)
					return "[Default]";
				else
					return culture.EnglishName;
			}
		}

		public string ShortName
		{
			get
			{
				if (IsDefault)
					return "default";
				else
					return culture.Name;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				if (IsDefault)
					return CultureInfo.CurrentUICulture;
				else
					return culture;
			}
		}

		public bool IsDefault
		{
			get { return isDefault; }
		}

		public static IEnumerable<UILanguage> AvalilableCultures
		{
			get { return availableCultures; }
		}

		public static UILanguage CreateDefaultUILanguage()
		{
			UILanguage language = new UILanguage();
			language.isDefault = true;

			return language;
		}

		public static UILanguage CreateUILanguage(string cultureName)
		{
			if (cultureName == "default")
				return CreateDefaultUILanguage();

			try
			{
				CultureInfo culture = new CultureInfo(cultureName);
				return new UILanguage(culture);
			}
			catch (ArgumentException)
			{
				return null;
			}
		}

		public override bool Equals(object obj)
		{
			UILanguage other = obj as UILanguage;

			if (other == null)
				return false;
			else if (this.IsDefault && other.IsDefault)
				return true;
			else if (!this.IsDefault && !other.IsDefault)
				return (this.culture.Equals(other.culture));
			else
				return false;
		}

		public override int GetHashCode()
		{
			if (IsDefault)
				return 0;
			else
				return culture.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}