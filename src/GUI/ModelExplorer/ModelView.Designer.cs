using System.Windows.Forms;
using System.ComponentModel;

namespace NClass.GUI.ModelExplorer
{
	partial class ModelView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private ContextMenuStrip contextMenu;
		private ToolStripMenuItem mnuNewProject;
		private ToolStripMenuItem mnuOpen;
		private ToolStripMenuItem mnuOpenFile;
		private ToolStripSeparator mnuSepOpenFile;
		private ToolStripMenuItem mnuRecentFile1;
		private ToolStripMenuItem mnuRecentFile2;
		private ToolStripMenuItem mnuRecentFile3;
		private ToolStripMenuItem mnuRecentFile4;
		private ToolStripMenuItem mnuRecentFile5;
		private ToolStripMenuItem mnuSaveAll;
		private ToolStripMenuItem mnuCloseAll;
		private ImageList imageList;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			if (disposing)
			{
				normalFont.Dispose();
				boldFont.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelView));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuNewProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepOpenFile = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentFile1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile3 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile4 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile5 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.lblAddProject = new System.Windows.Forms.Label();
			this.mnuSepOpen = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewProject,
            this.mnuOpen,
            this.mnuSepOpen,
            this.mnuSaveAll,
            this.mnuCloseAll});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(168, 98);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
			// 
			// mnuNewProject
			// 
			this.mnuNewProject.Image = global::NClass.GUI.Properties.Resources.Project;
			this.mnuNewProject.Name = "mnuNewProject";
			this.mnuNewProject.Size = new System.Drawing.Size(167, 22);
			this.mnuNewProject.Text = "&New Project";
			this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenFile,
            this.mnuSepOpenFile,
            this.mnuRecentFile1,
            this.mnuRecentFile2,
            this.mnuRecentFile3,
            this.mnuRecentFile4,
            this.mnuRecentFile5});
			this.mnuOpen.Image = global::NClass.GUI.Properties.Resources.Open;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.Size = new System.Drawing.Size(167, 22);
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.DropDownOpening += new System.EventHandler(this.mnuOpen_DropDownOpening);
			// 
			// mnuOpenFile
			// 
			this.mnuOpenFile.Name = "mnuOpenFile";
			this.mnuOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenFile.Size = new System.Drawing.Size(177, 22);
			this.mnuOpenFile.Text = "&New File...";
			this.mnuOpenFile.Click += new System.EventHandler(this.mnuOpenFile_Click);
			// 
			// mnuSepOpenFile
			// 
			this.mnuSepOpenFile.Name = "mnuSepOpenFile";
			this.mnuSepOpenFile.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuRecentFile1
			// 
			this.mnuRecentFile1.Name = "mnuRecentFile1";
			this.mnuRecentFile1.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile1.Tag = 0;
			this.mnuRecentFile1.Text = "Recent File 1";
			this.mnuRecentFile1.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile2
			// 
			this.mnuRecentFile2.Name = "mnuRecentFile2";
			this.mnuRecentFile2.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile2.Tag = 1;
			this.mnuRecentFile2.Text = "Recent File 2";
			this.mnuRecentFile2.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile3
			// 
			this.mnuRecentFile3.Name = "mnuRecentFile3";
			this.mnuRecentFile3.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile3.Tag = 2;
			this.mnuRecentFile3.Text = "Recent File 3";
			this.mnuRecentFile3.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile4
			// 
			this.mnuRecentFile4.Name = "mnuRecentFile4";
			this.mnuRecentFile4.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile4.Tag = 3;
			this.mnuRecentFile4.Text = "Recent File 4";
			this.mnuRecentFile4.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile5
			// 
			this.mnuRecentFile5.Name = "mnuRecentFile5";
			this.mnuRecentFile5.Size = new System.Drawing.Size(177, 22);
			this.mnuRecentFile5.Tag = 4;
			this.mnuRecentFile5.Text = "Recent File 5";
			this.mnuRecentFile5.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuSaveAll
			// 
			this.mnuSaveAll.Image = global::NClass.GUI.Properties.Resources.SaveAll;
			this.mnuSaveAll.Name = "mnuSaveAll";
			this.mnuSaveAll.Size = new System.Drawing.Size(167, 22);
			this.mnuSaveAll.Text = "Save A&ll Projects";
			this.mnuSaveAll.Click += new System.EventHandler(this.mnuSaveAll_Click);
			// 
			// mnuCloseAll
			// 
			this.mnuCloseAll.Name = "mnuCloseAll";
			this.mnuCloseAll.Size = new System.Drawing.Size(167, 22);
			this.mnuCloseAll.Text = "Close All Projects";
			this.mnuCloseAll.Click += new System.EventHandler(this.mnuCloseAll_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "project");
			this.imageList.Images.SetKeyName(1, "diagram");
			// 
			// lblAddProject
			// 
			this.lblAddProject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAddProject.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblAddProject.Location = new System.Drawing.Point(0, 0);
			this.lblAddProject.Name = "lblAddProject";
			this.lblAddProject.Size = new System.Drawing.Size(100, 23);
			this.lblAddProject.TabIndex = 0;
			this.lblAddProject.Text = "« Double click here to add new project »";
			this.lblAddProject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblAddProject.Visible = false;
			this.lblAddProject.DoubleClick += new System.EventHandler(this.lblAddProject_DoubleClick);
			// 
			// mnuSepOpen
			// 
			this.mnuSepOpen.Name = "mnuSepOpen";
			this.mnuSepOpen.Size = new System.Drawing.Size(164, 6);
			// 
			// ModelView
			// 
			this.ContextMenuStrip = this.contextMenu;
			this.ImageIndex = 0;
			this.ImageList = this.imageList;
			this.LabelEdit = true;
			this.LineColor = System.Drawing.Color.Black;
			this.SelectedImageIndex = 0;
			this.ShowRootLines = false;
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private Label lblAddProject;
		private ToolStripSeparator mnuSepOpen;
	}
}
