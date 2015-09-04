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
using System.Collections.Generic;
using System.Windows.Forms;
using NClass.Core;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	public abstract partial class TreeDialog : Form
	{
		bool checkingLocked = false;

		public TreeDialog()
		{
			InitializeComponent();
			treOperations.ImageList = Icons.IconList;
		}

		protected TreeView OperationTree
		{
			get { return treOperations; }
		}

        public IEnumerable<Operation> GetSelectedOperations()
        {
            for (int parent = 0; parent < treOperations.Nodes.Count; parent++) {
                TreeNode parentNode = treOperations.Nodes[parent];

                for (int child = 0; child < parentNode.Nodes.Count; child++) {
                    if (parentNode.Nodes[child].Tag is Operation &&
                        parentNode.Nodes[child].Checked) {
                        yield return (Operation) parentNode.Nodes[child].Tag;
                    }
                }
            }
        }

        /// <exception cref="ArgumentNullException">
		/// <paramref name="parentNode"/> is null.-or-
		/// <paramref name="operation"/> is null.
		/// </exception>
		protected TreeNode CreateOperationNode(TreeNode parentNode, Operation operation)
		{
			if (parentNode == null)
				throw new ArgumentNullException("parentNode");
			if (operation == null)
				throw new ArgumentNullException("operation");

			TreeNode child = parentNode.Nodes.Add(operation.GetUmlDescription());
			int imageIndex = Icons.GetImageIndex(operation);

			child.Tag = operation;
			child.ImageIndex = imageIndex;
			child.SelectedImageIndex = imageIndex;
			child.ToolTipText = operation.ToString();

			return child;
		}

		protected void RemoveEmptyNodes()
		{
			for (int i = 0; i < OperationTree.Nodes.Count; i++) {
				if (OperationTree.Nodes[i].Nodes.Count == 0)
					OperationTree.Nodes.RemoveAt(i--);
			}
		}

		protected virtual void UpdateTexts()
		{
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTexts();
		}

		private void treMembers_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (!checkingLocked) {
				checkingLocked = true;

				TreeNode node = e.Node;
				TreeNode parentNode = e.Node.Parent;

				if (parentNode != null) {
					if (!node.Checked) {
						parentNode.Checked = false;
					}
					else {
						bool allChecked = true;

						foreach (TreeNode neighbour in parentNode.Nodes) {
							if (!neighbour.Checked) {
								allChecked = false;
								break;
							}
						}
						parentNode.Checked = allChecked;
					}
				}

				foreach (TreeNode child in node.Nodes)
					child.Checked = node.Checked;

				checkingLocked = false;
			}
		}
	}
}