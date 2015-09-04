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
using System.ComponentModel;
using System.Windows.Forms;
using NClass.DiagramEditor;
using NClass.Core;
using NClass.GUI.Properties;
using NClass.Translations;

namespace NClass.GUI.ModelExplorer
{
	public partial class ModelView : TreeView
	{
		Workspace workspace = null;
		Font boldFont, normalFont;

		public event DocumentEventHandler DocumentOpening;

		public ModelView()
		{
			InitializeComponent();
			this.Controls.Add(lblAddProject);
			UpdateTexts();

			normalFont = new Font(this.Font, FontStyle.Regular);
			boldFont = new Font(this.Font, FontStyle.Bold);
		}

		private void UpdateTexts()
		{
			mnuNewProject.Text = Strings.MenuNewProject;
			mnuOpen.Text = Strings.MenuOpen;
			mnuOpenFile.Text = Strings.MenuOpenFile;
			mnuSaveAll.Text = Strings.MenuSaveAllProjects;
			mnuCloseAll.Text = Strings.MenuCloseAllProjects;

			lblAddProject.Text = Strings.DoubleClickToAddProject;
		}

		[Browsable(false)]
		public Workspace Workspace
		{
			get
			{
				return workspace;
			}
			set
			{
				if (workspace != value)
				{
					if (workspace != null)
					{
						workspace.ActiveProjectChanged -= workspace_ActiveProjectChanged;
						workspace.ProjectAdded -= workspace_ProjectAdded;
						workspace.ProjectRemoved -= workspace_ProjectRemoved;
						RemoveProjects();
					}
					workspace = value;
					if (workspace != null)
					{
						workspace.ActiveProjectChanged += workspace_ActiveProjectChanged;
						workspace.ProjectAdded += workspace_ProjectAdded;
						workspace.ProjectRemoved += workspace_ProjectRemoved;
						LoadProjects();
					}
					lblAddProject.Visible = (workspace != null && !workspace.HasProject);
				}
			}
		}

		private void AddProject(Project project)
		{
			ModelNode projectNode = new ProjectNode(project);
			Nodes.Add(projectNode);
			projectNode.AfterInitialized();

			SelectedNode = projectNode;
			projectNode.Expand();
			lblAddProject.Visible = false;

			if (project.ItemCount == 1)
			{
				foreach (IProjectItem item in project.Items)
				{
					IDocument document = item as IDocument;
					if (document != null)
						OnDocumentOpening(new DocumentEventArgs(document));
				}
			}
			if (project.IsUntitled)
			{
				projectNode.EditLabel();
			}
		}

		private void RemoveProject(Project project)
		{
			foreach (ProjectNode projectNode in Nodes)
			{
				if (projectNode.Project == project)
				{
					projectNode.Delete();
					break;
				}
			}
			if (!workspace.HasProject)
				lblAddProject.Visible = true;
		}

		private void RemoveProjects()
		{
			foreach (ModelNode node in Nodes)
			{
				node.BeforeDelete();
			}
			Nodes.Clear();
			lblAddProject.Visible = true;
		}

		private void LoadProjects()
		{
			foreach (Project project in workspace.Projects)
			{
				AddProject(project);
			}
		}

		private void workspace_ActiveProjectChanged(object sender, EventArgs e)
		{
			foreach (ProjectNode node in Nodes)
			{
				if (node.Project == Workspace.ActiveProject)
					node.NodeFont = boldFont;
				else
					node.NodeFont = normalFont;
				node.Text = node.Text; // Little hack to update the text's clipping size
			}

			if (MonoHelper.IsRunningOnMono)
				this.Refresh();
		}

		private void workspace_ProjectAdded(object sender, ProjectEventArgs e)
		{
			AddProject(e.Project);
		}

		private void workspace_ProjectRemoved(object sender, ProjectEventArgs e)
		{
			RemoveProject(e.Project);
		}

		private void lblAddProject_DoubleClick(object sender, EventArgs e)
		{
			if (workspace != null && !workspace.HasProject)
			{
				workspace.AddEmptyProject();
			}
		}

		protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			base.OnNodeMouseDoubleClick(e);
			ModelNode node = (ModelNode) e.Node;
			node.DoubleClick();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.Enter)
			{
				ModelNode selectedNode = SelectedNode as ModelNode;
				if (selectedNode != null)
					selectedNode.EnterPressed();
			}
			else if (e.KeyCode == Keys.F2)
			{
				ModelNode selectedNode = SelectedNode as ModelNode;
				if (selectedNode != null)
					selectedNode.EditLabel();
			}
		}

		protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
		{
			base.OnBeforeCollapse(e);

			// Prevent top level nodes to be collapsed
			if (e.Node.Level == 0)
				e.Cancel = true;
		}

		protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
			base.OnBeforeLabelEdit(e);

			ModelNode node = (ModelNode) e.Node;
			if (!node.EditingLabel)
				e.CancelEdit = true;
		}

		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			base.OnAfterLabelEdit(e);
			
			ModelNode node = (ModelNode) e.Node;
			node.LabelEdited();	
			if (!e.CancelEdit && e.Label != null)
			{
				node.LabelModified(e);
			}
		}

		//TODO: ez így nem szép! nem kéne internal!
		protected internal virtual void OnDocumentOpening(DocumentEventArgs e)
		{
			if (DocumentOpening != null)
				DocumentOpening(this, e);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			normalFont.Dispose();
			boldFont.Dispose();
			normalFont = new Font(this.Font, FontStyle.Regular);
			boldFont = new Font(this.Font, FontStyle.Bold);
		}

		#region Context menu event handlers

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (Workspace.Default.HasProject)
			{
				mnuSaveAll.Enabled = true;
				mnuCloseAll.Enabled = true;
			}
			else
			{
				mnuSaveAll.Enabled = false;
				mnuCloseAll.Enabled = false;
			}
		}

		private void mnuNewProject_Click(object sender, EventArgs e)
		{
			Project project = Workspace.Default.AddEmptyProject();
			Workspace.Default.ActiveProject = project;
		}

		private void mnuOpen_DropDownOpening(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in mnuOpen.DropDownItems)
			{
				if (item.Tag is int)
				{
					int index = (int) item.Tag;

					if (index < Settings.Default.RecentFiles.Count)
					{
						item.Text = Settings.Default.RecentFiles[index];
						item.Visible = true;
					}
					else
					{
						item.Visible = false;
					}
				}
			}

			mnuSepOpenFile.Visible = (Settings.Default.RecentFiles.Count > 0);
		}

		private void mnuOpenFile_Click(object sender, EventArgs e)
		{
			Workspace.Default.OpenProject();
		}

		private void OpenRecentFile_Click(object sender, EventArgs e)
		{
			int index = (int) ((ToolStripItem) sender).Tag;
			if (index >= 0 && index < Settings.Default.RecentFiles.Count)
			{
				string fileName = Settings.Default.RecentFiles[index];
				Workspace.Default.OpenProject(fileName);
			}
		}

		private void mnuSaveAll_Click(object sender, EventArgs e)
		{
			Workspace.Default.SaveAllProjects();
		}

		private void mnuCloseAll_Click(object sender, EventArgs e)
		{
			Workspace.Default.RemoveAll();
		}

		#endregion
	}
}
