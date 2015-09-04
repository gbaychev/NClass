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
using System.Collections.Generic;
using System.Windows.Forms;
using NClass.Core;
using NClass.Translations;
using NClass.DiagramEditor.ClassDiagram;

namespace NClass.GUI.ModelExplorer
{
	public sealed class EmptyProjectNode : ModelNode
	{
		Project project;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public EmptyProjectNode(Project project)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			this.project = project;
			project.ItemAdded += new ProjectItemEventHandler(project_ItemAdded);

			this.Text = Strings.DoubleClickToAddDiagram;
			this.ImageKey = "diagram";
			this.SelectedImageKey = "diagram";
		}

		protected internal override void AfterInitialized()
		{
			base.AfterInitialized();
			NodeFont = new Font(TreeView.Font, FontStyle.Italic);
		}

		private void project_ItemAdded(object sender, ProjectItemEventArgs e)
		{
			this.Delete();
		}

		public override void LabelModified(NodeLabelEditEventArgs e)
		{
			e.CancelEdit = true;
		}

		private void AddEmptyDiagram()
		{
			TreeNode parent = Parent;

			this.Delete();
			Diagram diagram = new Diagram(Settings.Default.GetDefaultLanguage());
			project.Add(diagram);
		}

		public override void DoubleClick()
		{
			AddEmptyDiagram();
		}

		public override void EnterPressed()
		{
			AddEmptyDiagram();
		}

		public override void BeforeDelete()
		{
			project.ItemAdded -= new ProjectItemEventHandler(project_ItemAdded);
			NodeFont.Dispose();
			base.BeforeDelete();
		}
	}
}
