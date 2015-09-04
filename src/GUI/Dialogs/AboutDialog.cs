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
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.GUI.Dialogs
{
	public partial class AboutDialog : Form
	{
		public AboutDialog()
		{
			InitializeComponent();
		}

		private void UpdateTexts()
		{
			this.Text = Strings.AboutNClass;
			lblTitle.Text = Program.GetVersionString();
			lblCopyright.Text = "Copyright (C) 2006-2009 " + Strings.Author;
			lblStatus.Text = string.Format(Strings.BetaVersion);
			lnkEmail.Text = Strings.SendEmail;
			lnkHomepage.Text = Strings.VisitHomepage;
			btnClose.Text = Strings.ButtonClose;

			lnkHomepage.Links.Clear();
			lnkEmail.Links.Clear();
			lnkHomepage.Links.Add(0, lnkHomepage.Text.Length, Properties.Resources.WebAddress);
			lnkEmail.Links.Add(0, lnkEmail.Text.Length,
				"mailto:" + Properties.Resources.MailAddress + "?subject=NClass");
			lblTranslator.Text = Strings.Translator;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
		}

		private void lnkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string target = e.Link.LinkData as string;

			if (target != null)
			{
				try
				{
					System.Diagnostics.Process.Start(target);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format("{0}\n{1}: {2}", Strings.CommandFailed,
							Strings.ErrorsReason, ex.Message),
						Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void lnkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string target = e.Link.LinkData as string;

			if (target != null)
			{
				try
				{
					System.Diagnostics.Process.Start(target);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format("{0}\n{1}: {2}", Strings.CommandFailed,
							Strings.ErrorsReason, ex.Message),
						Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}