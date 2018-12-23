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
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NClass.GUI.Dialogs;
using NClass.Translations;
using Octokit;

namespace NClass.GUI
{
    public static class UpdatesChecker
    {
        private class VersionInfo
        {
            /// <exception cref="ArgumentException">
            /// <paramref name="version"/> is an invalid value.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="versionName"/>, <paramref name="downloadPageUrl"/>, or
            /// <paramref name="notes"/> is null.
            /// </exception>
            public VersionInfo(string version, string versionName, string downloadPageUrl, string notes)
            {
                if (version == null)
                    throw new ArgumentNullException("version");
                try
                {
                    this.MainVersion = new Version(version);
                }
                catch
                {
                    throw new ArgumentException("Version string is invalid.", "version");
                }

                this.VersionName = versionName ?? throw new ArgumentNullException("versionName");
                this.DownloadPageUrl = downloadPageUrl ?? throw new ArgumentNullException("downloadPageUrl");
                this.Notes = notes ?? throw new ArgumentNullException("notes");
            }

            public Version MainVersion { get; }

            public string VersionName { get; }

            public string DownloadPageUrl { get; }

            public string Notes { get; }

            public bool IsUpdated => (MainVersion.CompareTo(Program.CurrentVersion) > 0);

            public override string ToString()
            {
                if (VersionName == null)
                    return MainVersion.ToString();
                else
                    return $"{VersionName} ({MainVersion})";
            }
        }

        /// <exception cref="WebException">
        /// Could not connect to the server.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Could not read the version informations.
        /// </exception>
        private static async Task<VersionInfo> GetVersionManifestInfo()
        {
            try
            {
                var githubClient = new GitHubClient(new ProductHeaderValue("NClass"));
                var latestRelease = await githubClient.Repository.Release.GetLatest("gbaychev", "nclass");

                // semver
                var versionRegex = new Regex(@"^releases/v(?<majorminor>\d{1,}\.\d{1,})\.(?<patch>\d{1,})(-beta|-pre)?$", RegexOptions.ExplicitCapture);
                var match = versionRegex.Match(latestRelease.TagName);
                // seriously, whoever retard at microsoft decided that 'build' should be before 'revision'
                // in System.Version should be stoned to death. 'build' must not be patched in the assembly version
                var version = $"{match.Groups["majorminor"].Value}.0.{match.Groups["patch"].Value}";

                // Get other informations
                var name = latestRelease.Name;
                var url = latestRelease.HtmlUrl;
                var notes = ConvertMarkdownToHtml(latestRelease.Body);

                return new VersionInfo(version, name, url, notes);
            }
            catch (Octokit.NotFoundException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch
            {
                throw new InvalidDataException();
            }
        }

        private static string ConvertMarkdownToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
            {
                return string.Empty;
            }

            var html = new StringBuilder();

            using (var reader = new StringReader(markdown))
            using (var writer = new StringWriter(html))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }

            return html.ToString();
        }

        private static void OpenUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        public static async Task CheckForUpdates()
        {
            try
            {
                VersionInfo info = await GetVersionManifestInfo();
                ShowNewVersionInfo(info);
            }
            catch (Octokit.NotFoundException)
            {
                MessageBox.Show(Strings.ErrorReleasesNotFound, Strings.Error, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (HttpRequestException)
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
                var updateDialog = new UpdateDialog(Strings.CheckingForUpdates, info.VersionName,  info.Notes);
                var result = updateDialog.ShowDialog();

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
    }
}
