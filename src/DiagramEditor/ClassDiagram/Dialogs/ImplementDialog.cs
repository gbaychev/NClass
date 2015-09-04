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
using NClass.Core;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	public partial class ImplementDialog : TreeDialog
	{
		CheckBox chkImplementExplicitly = new CheckBox();

		public ImplementDialog()
		{
			chkImplementExplicitly.AutoSize = true;
			chkImplementExplicitly.Location = new Point(12, 284);
			chkImplementExplicitly.Text = Strings.ImplementExplicitly;
			chkImplementExplicitly.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.Controls.Add(chkImplementExplicitly);
		}

		public bool ImplementExplicitly
		{
			get { return chkImplementExplicitly.Checked; }
		}

		protected override void UpdateTexts()
		{
			this.Text = Strings.Implementing;
			base.UpdateTexts();
		}

		private TreeNode CreateInterfaceNode(string interfaceName)
		{
			TreeNode node = OperationTree.Nodes.Add(interfaceName);
			node.SelectedImageIndex = Icons.InterfaceImageIndex;
			node.ImageIndex = Icons.InterfaceImageIndex;

			return node;
		}

		private void AddOperations(IInterfaceImplementer implementer,
			InterfaceType _interface, TreeNode node)
		{
			if (implementer == null || _interface == null || node == null)
				return;

			foreach (InterfaceType baseInterface in _interface.Bases)
				AddOperations(implementer, baseInterface, node);

			foreach (Operation operation in _interface.Operations) {
				Operation defined = implementer.GetDefinedOperation(operation);

				if (defined == null) {
					CreateOperationNode(node, operation);
				}
				else if (defined.Type != operation.Type && 
					_interface.Language.SupportsExplicitImplementation)
				{
					TreeNode operationNode = CreateOperationNode(node, operation);
					operationNode.ForeColor = Color.Gray;
				}
			}
		}

		private void AddInterface(IInterfaceImplementer implementer, InterfaceType _interface)
		{
			if (implementer == null || _interface == null)
				return;

			TreeNode node = CreateInterfaceNode(_interface.Name);
			AddOperations(implementer, _interface, node);
		}

		public DialogResult ShowDialog(IInterfaceImplementer implementer)
		{
			if (implementer == null)
				return DialogResult.None;

			chkImplementExplicitly.Checked = false;
			chkImplementExplicitly.Visible = (implementer.Language.SupportsExplicitImplementation);

			OperationTree.Nodes.Clear();
			foreach (InterfaceType _interface in implementer.Interfaces)
				AddInterface(implementer, _interface);
			RemoveEmptyNodes();

			return ShowDialog();
		}
	}
}