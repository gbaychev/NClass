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
	public partial class OverrideDialog : TreeDialog
	{
		protected override void UpdateTexts()
		{
			this.Text = Strings.OverrideMembers;
			base.UpdateTexts();
		}

		private TreeNode CreateClassNode(string className)
		{
			TreeNode node = OperationTree.Nodes.Add(className);
			node.SelectedImageIndex = Icons.ClassImageIndex;
			node.ImageIndex = Icons.ClassImageIndex;

			return node;
		}

		private void RemoveSimilarNode(Operation operation)
		{
			if (operation == null)
				return;

			for (int i = 0; i < OperationTree.Nodes.Count; i++) {
				for (int j = 0; j < OperationTree.Nodes[i].Nodes.Count; j++) {
					if (operation.HasSameSignatureAs(
						OperationTree.Nodes[i].Nodes[j].Tag as Operation))
					{
						OperationTree.Nodes[i].Nodes.RemoveAt(j);
						break;
					}
				}
			}
		}

		private void AddOperations(SingleInharitanceType derivedClass,
			SingleInharitanceType baseClass)
		{
			if (derivedClass == null || baseClass == null)
				return;

			AddOperations(derivedClass, baseClass.Base);

			TreeNode node = CreateClassNode(baseClass.Name);
			foreach (Operation operation in baseClass.OverridableOperations) {
				if (derivedClass.GetDefinedOperation(operation) != null)
					continue;
				RemoveSimilarNode(operation);
				CreateOperationNode(node, operation);
			}
		}

		public DialogResult ShowDialog(SingleInharitanceType inheritedClass)
		{
			if (inheritedClass == null)
				return DialogResult.None;

			OperationTree.Nodes.Clear();
			AddOperations(inheritedClass, inheritedClass.Base);
			RemoveEmptyNodes();

			return ShowDialog();
		}
	}
}