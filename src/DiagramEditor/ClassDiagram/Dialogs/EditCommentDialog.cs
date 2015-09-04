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

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	public partial class EditCommentDialog : Form
	{
		public EditCommentDialog()
		{
			InitializeComponent();
		}

		public EditCommentDialog(string initText)
		{
			InitializeComponent();
			txtInput.Text = initText;
		}

		public string InputText
		{
			get { return txtInput.Text; }
		}

		private void UpdateTexts()
		{
			this.Text = Strings.EditComment;
			lblEdit.Text = Strings.EditText;
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
		}
	}
}