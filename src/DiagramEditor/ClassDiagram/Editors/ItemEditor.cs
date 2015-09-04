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
using System.Drawing;
using System.Windows.Forms;
using NClass.Translations;
using System.ComponentModel;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public abstract partial class ItemEditor : FloatingEditor
	{
		bool needValidation = false;

		public ItemEditor()
		{
			InitializeComponent();
			UpdateTexts();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
		}

		protected string DeclarationText
		{
			get { return txtDeclaration.Text; }
			set { txtDeclaration.Text = value; }
		}

		protected int SelectionStart
		{
			get { return txtDeclaration.SelectionStart; }
			set { txtDeclaration.SelectionStart = value; }
		}

		protected bool NeedValidation
		{
			get { return needValidation; }
			set { needValidation = value; }
		}

		internal override void Init(DiagramElement element)
		{
			RefreshValues();
		}

		protected virtual void UpdateTexts()
		{
			toolMoveUp.ToolTipText = Strings.MoveUp + " (Ctrl+Up)";
			toolMoveDown.ToolTipText = Strings.MoveDown + " (Ctrl+Down)";
			toolDelete.ToolTipText = Strings.Delete;
		}

		public override void ValidateData()
		{
			ValidateDeclarationLine();
			SetError(null);
		}

		protected void SetError(string message)
		{
			if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
				return;

			errorProvider.SetError(this, message);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			txtDeclaration.SelectionStart = 0;
		}

		private void txtDeclaration_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					ValidateDeclarationLine();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					HideEditor();
					e.Handled = true;
					break;

				case Keys.Up:
					if (e.Shift || e.Control)
						MoveUp();
					else
						SelectPrevious();
					e.Handled = true;
					break;

				case Keys.Down:
					if (e.Shift || e.Control)
						MoveDown();
					else
						SelectNext();
					e.Handled = true;
					break;
			}
		}

		private void txtDeclaration_TextChanged(object sender, EventArgs e)
		{
			needValidation = true;
		}

		private void txtDeclaration_Validating(object sender, CancelEventArgs e)
		{
			ValidateDeclarationLine();
		}

		private void toolMoveUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void toolMoveDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			Delete();
		}

		internal abstract override void Relocate(DiagramElement element);

		protected abstract void HideEditor();

		protected abstract void RefreshValues();

		protected abstract bool ValidateDeclarationLine();

		protected abstract void SelectPrevious();

		protected abstract void SelectNext();

		protected abstract void MoveUp();

		protected abstract void MoveDown();

		protected abstract void Delete();
	}
}
