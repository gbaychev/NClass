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
using System.Net;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.GUI
{
	public static class UpdatesChecker
	{
		const string VersionUrl = "http://nclass.sourceforge.net/version.xml";

		private class VersionInfo
		{
			Version mainVersion;
			string translationVersion;
			string versionName;
			string notes;
			string downloadPageUrl;

			/// <exception cref="ArgumentException">
			/// <paramref name="version"/> is an invalid value.
			/// </exception>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="version"/>, <paramref name="translationVersion"/>, 
			/// <paramref name="versionName"/>, <paramref name="downloadPageUrl"/>, or
			/// <paramref name="notes"/> is null.
			/// </exception>
			public VersionInfo(string version, string translationVersion, string versionName,
				string downloadPageUrl, string notes)
			{
				if (version == null)
					throw new ArgumentNullException("version");
				if (translationVersion == null)
					throw new ArgumentNullException("translationVersion");
				if (versionName == null)
					throw new ArgumentNullException("versionName");
				if (downloadPageUrl == null)
					throw new ArgumentNullException("downloadPageUrl");
				if (notes == null)
					throw new ArgumentNullException("notes");

				try
				{
					this.mainVersion = new Version(version);
				}
				catch
				{
					throw new ArgumentException("Version string is invalid.", "version");
				}
				this.translationVersion = translationVersion;
				this.versionName = versionName;
				this.downloadPageUrl = downloadPageUrl;
				this.notes = notes;
			}

			public Version MainVersion
			{
				get { return mainVersion; }
			}

			public string TranslationVersion
			{
				get { return translationVersion; }
			}

			public string VersionName
			{
				get { return versionName; }
			}

			public string DownloadPageUrl
			{
				get { return downloadPageUrl; }
			}

			public string Notes
			{
				get { return notes; }
			}

			public bool IsUpdated
			{
				get
				{
					return (IsMainProgramUpdated || IsTranslationUpdated);
				}
			}

			public bool IsMainProgramUpdated
			{
				get
				{
					return (MainVersion.CompareTo(Program.CurrentVersion) > 0);
				}
			}

			public bool IsTranslationUpdated
			{
				get
				{
					string currentTranslationVersion = Strings.TranslationVersion;
					return (TranslationVersion.CompareTo(currentTranslationVersion) > 0);
				}
			}

			public override string ToString()
			{
				if (VersionName == null)
					return MainVersion.ToString();
				else
					return string.Format("{0} ({1})", VersionName, MainVersion);
			}
		}

		/// <exception cref="WebException">
		/// Could not connect to the server.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Could not read the version informations.
		/// </exception>
		private static VersionInfo GetVersionManifestInfo()
		{
			try
			{
				XmlDocument document = new XmlDocument();
				document.Load(VersionUrl);
				XmlElement root = document.DocumentElement;

				// Get main version information
				XmlElement versionElement = root["Version"];
				string version = versionElement.InnerText;

				// Get translation version information
				XmlNodeList translationElements = root.SelectNodes(
					"TranslationVersions/" + Strings.TranslationName);
				string translationVersion;
				if (translationElements.Count == 0)
					translationVersion = Strings.TranslationVersion;
				else
					translationVersion = translationElements[0].InnerText;

				// Get other informations
				string name = root["VersionName"].InnerText;
				string url = root["DownloadPageUrl"].InnerText;
				string notes = root["Notes"].InnerText.Trim();

				return new VersionInfo(version, translationVersion, name, url, notes);
			}
			catch (WebException)
			{
				throw;
			}
			catch
			{
				throw new InvalidDataException();
			}
		}

		private static void OpenUrl(string url)
		{
			System.Diagnostics.Process.Start(url);
		}

		public static void CheckForUpdates()
		{
			try
			{
				VersionInfo info = GetVersionManifestInfo();
				ShowNewVersionInfo(info);
			}
			catch (WebException)
			{
				MessageBox.Show(Strings.ErrorConnectToServer,
					Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (InvalidDataException)
			{
				MessageBox.Show(Strings.ErrorReadVersionData, Strings.Error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static void ShowNewVersionInfo(VersionInfo info)
		{
			if (info.IsUpdated)
			{
				string text = GetVersionDescription(info);
				string caption = Strings.CheckingForUpdates;

				DialogResult result = MessageBox.Show(text, caption,
					MessageBoxButtons.YesNo, MessageBoxIcon.Information);

				if (result == DialogResult.Yes)
					OpenUrl(info.DownloadPageUrl);
			}
			else
			{
				MessageBox.Show(
					Strings.NoUpdates, Strings.CheckingForUpdates,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private static string GetVersionDescription(VersionInfo info)
		{
			StringBuilder builder = new StringBuilder(512);

			if (info.IsMainProgramUpdated)
			{
				// Header text
				builder.AppendFormat("{0}: {1}\n\n",
					Strings.NewVersion, info.VersionName);

				// Main program's changes
				builder.Append(info.Notes);
				builder.Append("\n\n");
			}
			else if (info.IsTranslationUpdated)
			{
				builder.AppendFormat("{0}\n\n", Strings.TranslationUpdated);
			}

			// Download text
			builder.Append(Strings.ProgramDownload);

			return builder.ToString();
		}
	}
}
