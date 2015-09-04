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

using System.IO;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using NClass.Core;
using NClass.CSharp;
using NClass.Translations;

namespace NClass.GUI
{
	public sealed partial class Settings
	{
		const int MaxRecentFileCount = 5;

		public Language GetDefaultLanguage()
		{
			Language defaultLanguage = Language.GetLanguage(DefaultLanguageName);

			return defaultLanguage ?? CSharpLanguage.Instance;
		}

		public void AddRecentFile(string recentFile)
		{
			if (!File.Exists(recentFile))
				return;

			int index = RecentFiles.IndexOf(recentFile);

			if (index < 0)
			{
				if (RecentFiles.Count < MaxRecentFileCount)
					RecentFiles.Add(string.Empty);

				for (int i = RecentFiles.Count - 2; i >= 0; i--)
					RecentFiles[i + 1] = RecentFiles[i];
				RecentFiles[0] = recentFile;
			}
			else if (index > 0)
			{
				string temp = RecentFiles[index];
				for (int i = index; i > 0; i--)
					RecentFiles[i] = RecentFiles[i - 1];
				RecentFiles[0] = temp;
			}
		}
	}
}