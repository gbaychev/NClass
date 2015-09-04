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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NClass.DiagramEditor;
using NClass.Translations;
using NClass.DiagramEditor.ClassDiagram;

namespace NClass.GUI.Dialogs
{
	public sealed partial class OptionsDialog : Form
	{
		Style savedStyle = null;

		public event EventHandler StyleModified;

		public OptionsDialog()
		{
			InitializeComponent();
		}

		private void UpdateTexts()
		{
			this.Text = Strings.Options;
			tabGeneral.Text = Strings.General;
			tabStyle.Text = Strings.Style;
			grpGeneral.Text = Strings.General;
			grpDiagram.Text = Strings.Diagram;
			lblLanguage.Text = Strings.Language + ":";
			lblRequiresRestart.Text = Strings.RequiresRestart;
			chkRememberOpenProjects.Text = Strings.RememberOpenProjects;
			btnClearRecents.Text = Strings.ClearRecentList;
			chkUsePrecisionSnapping.Text = Strings.UseSnapping;
			lblShowChevron.Text = Strings.ShowChevron;
			radChevronAlways.Text = Strings.Always;
			radChevronAsNeeded.Text = Strings.WhenMouseHovers;
			radChevronNever.Text = Strings.Never;
			lblUseClearType.Text = Strings.UseClearType;
			radClearTypeAlways.Text = Strings.Always;
			radClearTypeWhenZoomed.Text = Strings.WhenZoomed;
			radClearTypeNever.Text = Strings.Never;
			chkClearTypeForImages.Text = Strings.UseClearTypeForImages;
			btnClear.Text = Strings.ButtonClear;
			btnLoad.Text = Strings.ButtonLoad;
			btnSave.Text = Strings.ButtonSave;
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;

			cboLanguage.Left = lblLanguage.Left + lblLanguage.Width + 3;
		}

		private void LoadStyles()
		{
			cboStyles.Items.Clear();
			foreach (Style style in Style.AvaiableStyles)
			{
				cboStyles.Items.Add(style);
				if (style.Equals(Style.CurrentStyle))
					cboStyles.SelectedItem = style;
			}
		}

		private void FillLanguages()
		{
			cboLanguage.Items.Clear();
			foreach (UILanguage language in UILanguage.AvalilableCultures)
			{
				cboLanguage.Items.Add(language);
			}
		}

		private void LoadSettings()
		{
			// General settings
			cboLanguage.SelectedItem = UILanguage.CreateUILanguage(Settings.Default.UILanguage);
			chkRememberOpenProjects.Checked = Settings.Default.RememberOpenProjects;

			// Diagram settings
			chkUsePrecisionSnapping.Checked = DiagramEditor.Settings.Default.UsePrecisionSnapping;
			if (DiagramEditor.Settings.Default.ShowChevron == ChevronMode.Always)
				radChevronAlways.Checked = true;
			else if (DiagramEditor.Settings.Default.ShowChevron == ChevronMode.AsNeeded)
				radChevronAsNeeded.Checked = true;
			else
				radChevronNever.Checked = true;

			if (DiagramEditor.Settings.Default.UseClearType == ClearTypeMode.Always)
				radClearTypeAlways.Checked = true;
			else if (DiagramEditor.Settings.Default.UseClearType == ClearTypeMode.WhenZoomed)
				radClearTypeWhenZoomed.Checked = true;
			else
				radClearTypeNever.Checked = true;
			chkClearTypeForImages.Checked = DiagramEditor.Settings.Default.UseClearTypeForImages;

			// Style settings
			savedStyle = (Style) Style.CurrentStyle.Clone();
			stylePropertyGrid.SelectedObject = Style.CurrentStyle;
		}

		private void SaveSettings()
		{
			// General settings
			Settings.Default.UILanguage = ((UILanguage) cboLanguage.SelectedItem).ShortName;
			Settings.Default.RememberOpenProjects = chkRememberOpenProjects.Checked;

			// Diagram settings
			DiagramEditor.Settings.Default.UsePrecisionSnapping = chkUsePrecisionSnapping.Checked;
			if (radChevronAlways.Checked)
				DiagramEditor.Settings.Default.ShowChevron = ChevronMode.Always;
			else if (radChevronAsNeeded.Checked)
				DiagramEditor.Settings.Default.ShowChevron = ChevronMode.AsNeeded;
			else
				DiagramEditor.Settings.Default.ShowChevron = ChevronMode.Never;

			if (radClearTypeAlways.Checked)
				DiagramEditor.Settings.Default.UseClearType = ClearTypeMode.Always;
			else if (radClearTypeWhenZoomed.Checked)
				DiagramEditor.Settings.Default.UseClearType = ClearTypeMode.WhenZoomed;
			else
				DiagramEditor.Settings.Default.UseClearType = ClearTypeMode.Never;
			DiagramEditor.Settings.Default.UseClearTypeForImages = chkClearTypeForImages.Checked;

			// Style settings
			Style.SaveCurrentStyle();
			if (savedStyle != null)
				savedStyle.Dispose();

			DiagramEditor.Settings.Default.Save();
			Settings.Default.Save();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			UpdateTexts();
			FillLanguages();
			LoadSettings();
			LoadStyles();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (this.DialogResult == DialogResult.Cancel)
				Style.CurrentStyle = savedStyle;
			savedStyle.Dispose();
		}

		private void stylePropertyGrid_PropertyValueChanged(object s,
			PropertyValueChangedEventArgs e)
		{
			cboStyles.SelectedIndex = -1;
			if (StyleModified != null)
				StyleModified(this, EventArgs.Empty);
		}

		private void btnClearRecents_Click(object sender, EventArgs e)
		{
			Settings.Default.RecentFiles.Clear();
		}

		private void cboStyles_SelectedIndexChanged(object sender, EventArgs e)
		{
			Style style = cboStyles.SelectedItem as Style;
			if (style != null)
				ChangeCurrentStyle(style);
		}

		private void ChangeCurrentStyle(Style style)
		{
			Style.CurrentStyle = style;
			stylePropertyGrid.SelectedObject = Style.CurrentStyle;
			if (StyleModified != null)
				StyleModified(this, EventArgs.Empty);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ChangeCurrentStyle(new Style());
			cboStyles.SelectedIndex = -1;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.Filter = Strings.DiagramStyle + " (*.dst)|*.dst";
				dialog.InitialDirectory = Style.StylesDirectory;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Style style = Style.Load(dialog.FileName);

					if (style == null)
					{
						MessageBox.Show(
							Strings.ErrorCouldNotLoadFile, Strings.Load,
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						cboStyles.Items.Add(style);
						cboStyles.SelectedItem = style;
					}
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.FileName = Style.CurrentStyle.Name;
				dialog.InitialDirectory = Style.StylesDirectory;
				dialog.Filter = Strings.DiagramStyle + " (*.dst)|*.dst";

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					if (Style.CurrentStyle.IsUntitled)
					{
						Style.CurrentStyle.Name = Path.GetFileNameWithoutExtension(dialog.FileName);
					}

					if (!Style.CurrentStyle.Save(dialog.FileName))
					{
						MessageBox.Show(
							Strings.ErrorCouldNotSaveFile, Strings.Save,
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						LoadStyles();
					}
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}
	}
}