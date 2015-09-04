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
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.GUI
{
	internal static class Program
	{
		public static readonly Version CurrentVersion =
			Assembly.GetExecutingAssembly().GetName().Version;
		public static readonly string AppDataDirectory =
			Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.LocalApplicationData), "NClass");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			CrashHandler.CreateGlobalErrorHandler();
			UpdateSettings();

			// Set the user interface language
			UILanguage language = UILanguage.CreateUILanguage(Settings.Default.UILanguage);
			if (language != null)
				Strings.Culture = language.Culture;

			// Some GUI settings
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ToolStripManager.VisualStylesEnabled = false;

			// Launch the application
			LoadFiles(args);
			Application.Run(new MainForm());

			// Save application settings
			DiagramEditor.Settings.Default.Save();
			Settings.Default.Save();
		}

		private static void UpdateSettings()
		{
			if (Settings.Default.CallUpgrade)
			{
				Settings.Default.Upgrade();
				Settings.Default.CallUpgrade = false;
			}

			if (Settings.Default.OpenedProjects == null)
				Settings.Default.OpenedProjects = new StringCollection();
			if (Settings.Default.RecentFiles == null)
				Settings.Default.RecentFiles = new StringCollection();
		}

		public static string GetVersionString()
		{
			if (CurrentVersion.Minor == 0)
			{
				return string.Format("NClass {0}.0", CurrentVersion.Major);
			}
			else
			{
				return string.Format("NClass {0}.{1:00}",
					CurrentVersion.Major, CurrentVersion.Minor);
			}
		}

		private static void LoadFiles(string[] args)
		{
			if (args.Length >= 1)
			{
				foreach (string filePath in args)
				{
					Workspace.Default.OpenProject(filePath);
				}
			}
			else if (Settings.Default.RememberOpenProjects)
			{
				Workspace.Default.Load();
			}
		}
	}
}