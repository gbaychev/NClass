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
using System.ComponentModel;
using System.Windows.Forms;
using NClass.Core;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	public abstract partial class ListDialog : Form
	{
		bool changed = false;

		public ListDialog()
		{
			InitializeComponent();
			lstItems.SmallImageList = Icons.IconList;
			toolStrip1.Renderer = ToolStripSimplifiedRenderer.Default;
		}

		public bool Changed
		{
			get { return changed; }
			private set { changed = value; }
		}

		protected abstract void FillList();

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="text"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The <paramref name="text"/> contains a reserved name.
		/// </exception>
		protected abstract void AddToList(string text);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="text"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The <paramref name="text"/> contains a reserved name.
		/// </exception>
		protected abstract void Modify(ListViewItem item, string text);

		protected virtual void MoveUpItem(ListViewItem item)
		{
			if (item != null)
			{
				int index = item.Index;

				if (index > 0)
				{
					ListViewItem item2 = lstItems.Items[index - 1];

					SwapListItems(item, item2);
					item2.Focused = true;
					item2.Selected = true;
					Changed = true;
				}
			}
		}

		protected virtual void MoveDownItem(ListViewItem item)
		{
			if (item != null)
			{
				int index = item.Index;

				if (index < lstItems.Items.Count - 1)
				{
					ListViewItem item2 = lstItems.Items[index + 1];

					SwapListItems(item, item2);
					item2.Focused = true;
					item2.Selected = true;
					Changed = true;
				}
			}
		}

		protected virtual void Remove(ListViewItem item)
		{
			lstItems.Items.Remove(item);
			Changed = true;
		}

		private void SwapListItems(ListViewItem item1, ListViewItem item2)
		{
			string text = item1.Text;
			item1.Text = item2.Text;
			item2.Text = text;

			object tag = item1.Tag;
			item1.Tag = item2.Tag;
			item2.Tag = tag;

			Changed = true;
		}

		private void Accept()
		{
			try
			{
				if (lstItems.SelectedItems.Count == 0)
					AddToList(txtItem.Text);
				else
					Modify(lstItems.SelectedItems[0], txtItem.Text);

				ClearInput();
				Changed = true;
				txtItem.Focus();
			}
			catch (BadSyntaxException ex)
			{
				errorProvider.SetError(txtItem, ex.Message);
			}
		}

		private void ItemSelected()
		{
			toolMoveUp.Enabled = true;
			toolMoveDown.Enabled = true;
			toolDelete.Enabled = true;
			btnAccept.Text = Strings.ButtonModify;
			lblItemCaption.Text = Strings.ModifyItem;
			txtItem.Text = lstItems.SelectedItems[0].Text;
		}

		private void ClearInput()
		{
			toolMoveUp.Enabled = false;
			toolMoveDown.Enabled = false;
			toolDelete.Enabled = false;
			btnAccept.Text = Strings.ButtonAddItem;
			lblItemCaption.Text = Strings.AddNewItem;
			txtItem.Text = null;
			errorProvider.SetError(txtItem, null);
			if (lstItems.SelectedItems.Count > 0)
				lstItems.SelectedItems[0].Selected = false;
		}

		private void DeleteSelectedItem()
		{
			if (lstItems.SelectedItems.Count > 0)
			{
				int index = lstItems.SelectedItems[0].Index;

				Remove(lstItems.SelectedItems[0]);

				int count = lstItems.Items.Count;
				if (count > 0)
				{
					if (index >= count)
						index = count - 1;
					lstItems.Items[index].Selected = true;
				}
				else
				{
					txtItem.Focus();
				}
			}
		}

		private void UpdateTexts()
		{
			toolMoveUp.Text = Strings.MoveUp;
			toolMoveDown.Text = Strings.MoveDown;
			toolDelete.Text = Strings.Delete;
			lstItems.Columns[0].Text = Strings.Item;
			btnClose.Text = Strings.ButtonClose;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
			ClearInput();
			txtItem.Select();
		}

		private void txtItem_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				Accept();
			else if (e.KeyCode == Keys.Escape)
				ClearInput();
		}

		private void btnAccept_Click(object sender, EventArgs e)
		{
			Accept();
		}

		private void lstItems_ItemSelectionChanged(object sender,
			ListViewItemSelectionChangedEventArgs e)
		{
			if (lstItems.SelectedItems.Count == 0)
				ClearInput();
			else
				ItemSelected();
		}

		private void lstItems_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				DeleteSelectedItem();
		}

		private void toolMoveUp_Click(object sender, EventArgs e)
		{
			if (lstItems.SelectedItems.Count > 0)
				MoveUpItem(lstItems.SelectedItems[0]);
		}

		private void toolMoveDown_Click(object sender, EventArgs e)
		{
			if (lstItems.SelectedItems.Count > 0)
				MoveDownItem(lstItems.SelectedItems[0]);
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			DeleteSelectedItem();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}