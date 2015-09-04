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
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor;
using NClass.Translations;

namespace NClass.GUI
{
	internal static class CrashHandler
	{
		public static void CreateGlobalErrorHandler()
		{
#if !DEBUG
			AppDomain.CurrentDomain.UnhandledException += 
				new UnhandledExceptionEventHandler(AppDomain_UnhandledException);
#endif
		}

		private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				string crashDir = Path.Combine(Program.AppDataDirectory, "crash");
				Directory.CreateDirectory(crashDir);

				CreateBackups(crashDir);
				Exception ex = (Exception) e.ExceptionObject;
				CreateCrashLog(crashDir, ex);

				MessageBox.Show(
					Strings.ProgramTerminates, Strings.CriticalError,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				System.Diagnostics.Process.Start(crashDir);
				System.Diagnostics.Process.GetCurrentProcess().Kill();
				// Goodbye!
			}
		}

		private static void CreateBackups(string directory)
		{
			int untitledCount = 0;			
			foreach (Project project in Workspace.Default.Projects)
			{
				if (project.IsDirty){
				try
				{
					string fileName = project.FileName;
					if (project.IsUntitled)
					{
						untitledCount++;
						fileName = project.Name + untitledCount + ".ncp";
					}
					string filePath = Path.Combine(directory, fileName);
					
					project.Save(filePath);
				}
				catch
				{
				}
				}
			}
		}

		private static void CreateCrashLog(string directory, Exception exception)
		{
			StreamWriter writer = null;

			try
			{
				string filePath = Path.Combine(directory, "crash.log");
				writer = new StreamWriter(filePath);

				writer.WriteLine(string.Format(
					Strings.SendLogFile, Properties.Resources.MailAddress));
				writer.WriteLine();
				writer.WriteLine("Version: {0}", Program.GetVersionString());
				writer.WriteLine("Mono: {0}", MonoHelper.IsRunningOnMono ? "yes" : "no");
				if (MonoHelper.IsRunningOnMono)
					writer.WriteLine("Mono version: {0}", MonoHelper.Version);
				writer.WriteLine("OS: {0}", Environment.OSVersion.VersionString);

				writer.WriteLine();
				writer.WriteLine(exception.Message);
				Exception innerException = exception.InnerException;
				while (innerException != null)
				{
					writer.WriteLine(innerException.Message);
					innerException = innerException.InnerException;
				}

				writer.WriteLine();
				writer.WriteLine(exception.StackTrace);
			}
			catch
			{
				// Do nothing
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}
	}
}