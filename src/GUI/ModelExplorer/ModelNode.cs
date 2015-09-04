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

namespace NClass.GUI.ModelExplorer
{
	public abstract class ModelNode : TreeNode
	{
		bool editingLabel = false;
		bool deleted = false;

		protected ModelNode()
		{
		}

		public ModelView ModelView
		{
			get { return this.TreeView as ModelView; }
		}

		public bool EditingLabel
		{
			get { return editingLabel; }
		}

		public virtual void BeforeDelete()
		{
			foreach (ModelNode node in Nodes)
			{
				node.BeforeDelete();
			}
		}

		public void Delete()
		{
			if (!deleted)
			{
				BeforeDelete();
				Remove();
				deleted = true;
			}
		}

		public void EditLabel()
		{
			if (!editingLabel)
			{
				editingLabel = true;
				this.BeginEdit();
			}
		}

		internal void LabelEdited()
		{
			editingLabel = false;
		}

		public virtual void LabelModified(NodeLabelEditEventArgs e)
		{
		}

		public virtual void DoubleClick()
		{
		}

		public virtual void EnterPressed()
		{
		}

		protected internal virtual void AfterInitialized()
		{
			foreach (ModelNode node in Nodes)
				node.AfterInitialized();
		}
	}
}
