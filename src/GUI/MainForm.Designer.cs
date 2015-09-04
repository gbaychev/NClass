using System;

namespace NClass.GUI
{
	sealed partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblLanguage = new System.Windows.Forms.ToolStripStatusLabel();
			this.windowClient = new System.Windows.Forms.SplitContainer();
			this.tabbedWindow = new NClass.GUI.TabbedWindow();
			this.toolsPanel = new System.Windows.Forms.SplitContainer();
			this.modelExplorer = new NClass.GUI.ModelExplorer.ModelView();
			this.diagramNavigator = new NClass.GUI.DiagramNavigator();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNewProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepProject = new System.Windows.Forms.ToolStripSeparator();
			this.mnuNewCSharpDiagram = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNewJavaDiagram = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.sepOpenFile = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRecentFile1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile3 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile4 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentFile5 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepOpen = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepSaveAll = new System.Windows.Forms.ToolStripSeparator();
			this.mnuPrint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepExport = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCloseProject = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseAllProjects = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepClose = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepReso = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepDelete = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom10 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom25 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom50 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep50 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuZoom150 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom200 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoom400 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAutoZoom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepAutoZoom = new System.Windows.Forms.ToolStripSeparator();
			this.mnuModelExplorer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDiagramNavigator = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepDiagramNavigator = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCloseAllDocuments = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPlugins = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuContents = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSepUpdates = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.standardToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolNew = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolNewProject = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSepProject = new System.Windows.Forms.ToolStripSeparator();
			this.toolNewCSharpDiagram = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewJavaDiagram = new System.Windows.Forms.ToolStripMenuItem();
			this.toolOpen = new System.Windows.Forms.ToolStripSplitButton();
			this.toolRecentFile1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolRecentFile2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolRecentFile3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolRecentFile4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolRecentFile5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSave = new System.Windows.Forms.ToolStripButton();
			this.toolPrint = new System.Windows.Forms.ToolStripButton();
			this.toolSepPrint = new System.Windows.Forms.ToolStripSeparator();
			this.toolCut = new System.Windows.Forms.ToolStripButton();
			this.toolCopy = new System.Windows.Forms.ToolStripButton();
			this.toolPaste = new System.Windows.Forms.ToolStripButton();
			this.toolSepPaste = new System.Windows.Forms.ToolStripSeparator();
			this.toolUndo = new System.Windows.Forms.ToolStripButton();
			this.toolRedo = new System.Windows.Forms.ToolStripButton();
			this.toolSepRedo = new System.Windows.Forms.ToolStripSeparator();
			this.toolZoomValue = new System.Windows.Forms.ToolStripLabel();
			this.toolZoomOut = new System.Windows.Forms.ToolStripButton();
			this.toolZoom = new NClass.GUI.ZoomingToolStrip();
			this.toolZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolAutoZoom = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer.ContentPanel.SuspendLayout();
			this.toolStripContainer.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.windowClient.Panel1.SuspendLayout();
			this.windowClient.Panel2.SuspendLayout();
			this.windowClient.SuspendLayout();
			this.toolsPanel.Panel1.SuspendLayout();
			this.toolsPanel.Panel2.SuspendLayout();
			this.toolsPanel.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.standardToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer
			// 
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			this.toolStripContainer.ContentPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.toolStripContainer.ContentPanel.Controls.Add(this.windowClient);
			this.toolStripContainer.ContentPanel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(892, 595);
			this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer.Name = "toolStripContainer";
			this.toolStripContainer.Size = new System.Drawing.Size(892, 666);
			this.toolStripContainer.TabIndex = 0;
			this.toolStripContainer.Text = "toolStripContainer1";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.standardToolStrip);
			// 
			// statusStrip
			// 
			this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblLanguage});
			this.statusStrip.Location = new System.Drawing.Point(0, 0);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(892, 22);
			this.statusStrip.TabIndex = 0;
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(823, 17);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "Status";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLanguage
			// 
			this.lblLanguage.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblLanguage.Name = "lblLanguage";
			this.lblLanguage.Size = new System.Drawing.Size(54, 17);
			this.lblLanguage.Text = "Language";
			this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// windowClient
			// 
			this.windowClient.BackColor = System.Drawing.SystemColors.Control;
			this.windowClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.windowClient.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.windowClient.Location = new System.Drawing.Point(0, 0);
			this.windowClient.Name = "windowClient";
			// 
			// windowClient.Panel1
			// 
			this.windowClient.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.windowClient.Panel1.Controls.Add(this.tabbedWindow);
			this.windowClient.Panel1.Padding = new System.Windows.Forms.Padding(1);
			this.windowClient.Panel1MinSize = 200;
			// 
			// windowClient.Panel2
			// 
			this.windowClient.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.windowClient.Panel2.Controls.Add(this.toolsPanel);
			this.windowClient.Panel2MinSize = 100;
			this.windowClient.Size = new System.Drawing.Size(892, 595);
			this.windowClient.SplitterDistance = 650;
			this.windowClient.TabIndex = 0;
			// 
			// tabbedWindow
			// 
			this.tabbedWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.tabbedWindow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabbedWindow.DocumentManager = null;
			this.tabbedWindow.Location = new System.Drawing.Point(1, 1);
			this.tabbedWindow.Name = "tabbedWindow";
			this.tabbedWindow.Size = new System.Drawing.Size(648, 593);
			this.tabbedWindow.TabIndex = 0;
			// 
			// toolsPanel
			// 
			this.toolsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolsPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.toolsPanel.Location = new System.Drawing.Point(0, 0);
			this.toolsPanel.Name = "toolsPanel";
			this.toolsPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// toolsPanel.Panel1
			// 
			this.toolsPanel.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.toolsPanel.Panel1.Controls.Add(this.modelExplorer);
			this.toolsPanel.Panel1.Padding = new System.Windows.Forms.Padding(1);
			this.toolsPanel.Panel1MinSize = 100;
			// 
			// toolsPanel.Panel2
			// 
			this.toolsPanel.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.toolsPanel.Panel2.Controls.Add(this.diagramNavigator);
			this.toolsPanel.Panel2.Padding = new System.Windows.Forms.Padding(1);
			this.toolsPanel.Panel2MinSize = 100;
			this.toolsPanel.Size = new System.Drawing.Size(238, 595);
			this.toolsPanel.SplitterDistance = 405;
			this.toolsPanel.TabIndex = 0;
			// 
			// modelExplorer
			// 
			this.modelExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.modelExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelExplorer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
			this.modelExplorer.ImageIndex = 0;
			this.modelExplorer.Indent = 22;
			this.modelExplorer.ItemHeight = 18;
			this.modelExplorer.LabelEdit = true;
			this.modelExplorer.Location = new System.Drawing.Point(1, 1);
			this.modelExplorer.Name = "modelExplorer";
			this.modelExplorer.SelectedImageIndex = 0;
			this.modelExplorer.ShowRootLines = false;
			this.modelExplorer.Size = new System.Drawing.Size(236, 403);
			this.modelExplorer.TabIndex = 0;
			this.modelExplorer.Workspace = null;
			this.modelExplorer.DocumentOpening += new NClass.DiagramEditor.DocumentEventHandler(this.modelExplorer_DocumentOpening);
			// 
			// diagramNavigator
			// 
			this.diagramNavigator.BackColor = System.Drawing.SystemColors.Window;
			this.diagramNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramNavigator.DocumentVisualizer = null;
			this.diagramNavigator.Location = new System.Drawing.Point(1, 1);
			this.diagramNavigator.Name = "diagramNavigator";
			this.diagramNavigator.Size = new System.Drawing.Size(236, 184);
			this.diagramNavigator.TabIndex = 0;
			this.diagramNavigator.Text = "diagramNavigator";
			// 
			// menuStrip
			// 
			this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView,
            this.mnuPlugins,
            this.mnuHelp});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(892, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSepOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuSaveAll,
            this.mnuSepSaveAll,
            this.mnuPrint,
            this.mnuSepExport,
            this.mnuCloseProject,
            this.mnuCloseAllProjects,
            this.mnuSepClose,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(35, 20);
			this.mnuFile.Text = "&File";
			this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
			// 
			// mnuNew
			// 
			this.mnuNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewProject,
            this.mnuSepProject,
            this.mnuNewCSharpDiagram,
            this.mnuNewJavaDiagram});
			this.mnuNew.Image = global::NClass.GUI.Properties.Resources.NewDocument;
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.Size = new System.Drawing.Size(167, 22);
			this.mnuNew.Text = "&New";
			this.mnuNew.DropDownOpening += new System.EventHandler(this.mnuNew_DropDownOpening);
			// 
			// mnuNewProject
			// 
			this.mnuNewProject.Image = global::NClass.GUI.Properties.Resources.Project;
			this.mnuNewProject.Name = "mnuNewProject";
			this.mnuNewProject.Size = new System.Drawing.Size(149, 22);
			this.mnuNewProject.Text = "Project";
			this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
			// 
			// mnuSepProject
			// 
			this.mnuSepProject.Name = "mnuSepProject";
			this.mnuSepProject.Size = new System.Drawing.Size(146, 6);
			// 
			// mnuNewCSharpDiagram
			// 
			this.mnuNewCSharpDiagram.Name = "mnuNewCSharpDiagram";
			this.mnuNewCSharpDiagram.Size = new System.Drawing.Size(149, 22);
			this.mnuNewCSharpDiagram.Text = "&C# diagram";
			this.mnuNewCSharpDiagram.Click += new System.EventHandler(this.mnuNewCSharpDiagram_Click);
			// 
			// mnuNewJavaDiagram
			// 
			this.mnuNewJavaDiagram.Name = "mnuNewJavaDiagram";
			this.mnuNewJavaDiagram.Size = new System.Drawing.Size(149, 22);
			this.mnuNewJavaDiagram.Text = "&Java diagram";
			this.mnuNewJavaDiagram.Click += new System.EventHandler(this.mnuNewJavaDiagram_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenFile,
            this.sepOpenFile,
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
			this.mnuOpenFile.Size = new System.Drawing.Size(153, 22);
			this.mnuOpenFile.Text = "&File...";
			this.mnuOpenFile.Click += new System.EventHandler(this.mnuOpenFile_Click);
			// 
			// sepOpenFile
			// 
			this.sepOpenFile.Name = "sepOpenFile";
			this.sepOpenFile.Size = new System.Drawing.Size(150, 6);
			// 
			// mnuRecentFile1
			// 
			this.mnuRecentFile1.Name = "mnuRecentFile1";
			this.mnuRecentFile1.Size = new System.Drawing.Size(153, 22);
			this.mnuRecentFile1.Tag = 0;
			this.mnuRecentFile1.Text = "Recent File 1";
			this.mnuRecentFile1.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile2
			// 
			this.mnuRecentFile2.Name = "mnuRecentFile2";
			this.mnuRecentFile2.Size = new System.Drawing.Size(153, 22);
			this.mnuRecentFile2.Tag = 1;
			this.mnuRecentFile2.Text = "Recent File 2";
			this.mnuRecentFile2.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile3
			// 
			this.mnuRecentFile3.Name = "mnuRecentFile3";
			this.mnuRecentFile3.Size = new System.Drawing.Size(153, 22);
			this.mnuRecentFile3.Tag = 2;
			this.mnuRecentFile3.Text = "Recent File 3";
			this.mnuRecentFile3.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile4
			// 
			this.mnuRecentFile4.Name = "mnuRecentFile4";
			this.mnuRecentFile4.Size = new System.Drawing.Size(153, 22);
			this.mnuRecentFile4.Tag = 3;
			this.mnuRecentFile4.Text = "Recent File 4";
			this.mnuRecentFile4.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuRecentFile5
			// 
			this.mnuRecentFile5.Name = "mnuRecentFile5";
			this.mnuRecentFile5.Size = new System.Drawing.Size(153, 22);
			this.mnuRecentFile5.Tag = 4;
			this.mnuRecentFile5.Text = "Recent File 5";
			this.mnuRecentFile5.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// mnuSepOpen
			// 
			this.mnuSepOpen.Name = "mnuSepOpen";
			this.mnuSepOpen.Size = new System.Drawing.Size(164, 6);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = global::NClass.GUI.Properties.Resources.Save;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(167, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(167, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuSaveAll
			// 
			this.mnuSaveAll.Image = global::NClass.GUI.Properties.Resources.SaveAll;
			this.mnuSaveAll.Name = "mnuSaveAll";
			this.mnuSaveAll.Size = new System.Drawing.Size(167, 22);
			this.mnuSaveAll.Text = "Save A&ll Projects";
			this.mnuSaveAll.Click += new System.EventHandler(this.mnuSaveAll_Click);
			// 
			// mnuSepSaveAll
			// 
			this.mnuSepSaveAll.Name = "mnuSepSaveAll";
			this.mnuSepSaveAll.Size = new System.Drawing.Size(164, 6);
			// 
			// mnuPrint
			// 
			this.mnuPrint.Image = global::NClass.GUI.Properties.Resources.Print;
			this.mnuPrint.Name = "mnuPrint";
			this.mnuPrint.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.mnuPrint.Size = new System.Drawing.Size(167, 22);
			this.mnuPrint.Text = "&Print...";
			this.mnuPrint.Click += new System.EventHandler(this.mnuPrint_Click);
			// 
			// mnuSepExport
			// 
			this.mnuSepExport.Name = "mnuSepExport";
			this.mnuSepExport.Size = new System.Drawing.Size(164, 6);
			// 
			// mnuCloseProject
			// 
			this.mnuCloseProject.Name = "mnuCloseProject";
			this.mnuCloseProject.Size = new System.Drawing.Size(167, 22);
			this.mnuCloseProject.Text = "Close Project";
			this.mnuCloseProject.Click += new System.EventHandler(this.mnuCloseProject_Click);
			// 
			// mnuCloseAllProjects
			// 
			this.mnuCloseAllProjects.Name = "mnuCloseAllProjects";
			this.mnuCloseAllProjects.Size = new System.Drawing.Size(167, 22);
			this.mnuCloseAllProjects.Text = "Close All Projects";
			this.mnuCloseAllProjects.Click += new System.EventHandler(this.mnuCloseAllProjects_Click);
			// 
			// mnuSepClose
			// 
			this.mnuSepClose.Name = "mnuSepClose";
			this.mnuSepClose.Size = new System.Drawing.Size(164, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(167, 22);
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndo,
            this.mnuRedo,
            this.mnuSepReso,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuDelete,
            this.mnuSepDelete,
            this.mnuSelectAll});
			this.mnuEdit.Name = "mnuEdit";
			this.mnuEdit.Size = new System.Drawing.Size(37, 20);
			this.mnuEdit.Text = "&Edit";
			this.mnuEdit.DropDownOpening += new System.EventHandler(this.mnuEdit_DropDownOpening);
			this.mnuEdit.DropDownClosed += new System.EventHandler(this.mnuEdit_DropDownClosed);
			// 
			// mnuUndo
			// 
			this.mnuUndo.Image = global::NClass.GUI.Properties.Resources.Undo;
			this.mnuUndo.Name = "mnuUndo";
			this.mnuUndo.ShortcutKeyDisplayString = "Ctrl+Z";
			this.mnuUndo.Size = new System.Drawing.Size(167, 22);
			this.mnuUndo.Text = "&Undo";
			this.mnuUndo.Visible = false;
			this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
			// 
			// mnuRedo
			// 
			this.mnuRedo.Image = global::NClass.GUI.Properties.Resources.Redo;
			this.mnuRedo.Name = "mnuRedo";
			this.mnuRedo.ShortcutKeyDisplayString = "Ctrl+Y";
			this.mnuRedo.Size = new System.Drawing.Size(167, 22);
			this.mnuRedo.Text = "&Redo";
			this.mnuRedo.Visible = false;
			this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
			// 
			// mnuSepReso
			// 
			this.mnuSepReso.Name = "mnuSepReso";
			this.mnuSepReso.Size = new System.Drawing.Size(164, 6);
			this.mnuSepReso.Visible = false;
			// 
			// mnuCut
			// 
			this.mnuCut.Image = global::NClass.GUI.Properties.Resources.Cut;
			this.mnuCut.Name = "mnuCut";
			this.mnuCut.ShortcutKeyDisplayString = "Ctrl+X";
			this.mnuCut.Size = new System.Drawing.Size(167, 22);
			this.mnuCut.Text = "Cu&t";
			this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
			// 
			// mnuCopy
			// 
			this.mnuCopy.Image = global::NClass.GUI.Properties.Resources.Copy;
			this.mnuCopy.Name = "mnuCopy";
			this.mnuCopy.ShortcutKeyDisplayString = "Ctrl+C";
			this.mnuCopy.Size = new System.Drawing.Size(167, 22);
			this.mnuCopy.Text = "&Copy";
			this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
			// 
			// mnuPaste
			// 
			this.mnuPaste.Image = global::NClass.GUI.Properties.Resources.Paste;
			this.mnuPaste.Name = "mnuPaste";
			this.mnuPaste.ShortcutKeyDisplayString = "Ctrl+V";
			this.mnuPaste.Size = new System.Drawing.Size(167, 22);
			this.mnuPaste.Text = "&Paste";
			this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Image = global::NClass.GUI.Properties.Resources.Delete;
			this.mnuDelete.Name = "mnuDelete";
			this.mnuDelete.ShortcutKeyDisplayString = "Del";
			this.mnuDelete.Size = new System.Drawing.Size(167, 22);
			this.mnuDelete.Text = "&Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// mnuSepDelete
			// 
			this.mnuSepDelete.Name = "mnuSepDelete";
			this.mnuSepDelete.Size = new System.Drawing.Size(164, 6);
			// 
			// mnuSelectAll
			// 
			this.mnuSelectAll.Name = "mnuSelectAll";
			this.mnuSelectAll.ShortcutKeyDisplayString = "";
			this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.mnuSelectAll.Size = new System.Drawing.Size(167, 22);
			this.mnuSelectAll.Text = "Select &All";
			this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoom,
            this.mnuAutoZoom,
            this.mnuSepAutoZoom,
            this.mnuModelExplorer,
            this.mnuDiagramNavigator,
            this.mnuSepDiagramNavigator,
            this.mnuCloseAllDocuments,
            this.toolStripSeparator3,
            this.mnuOptions});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(41, 20);
			this.mnuView.Text = "&View";
			this.mnuView.DropDownOpening += new System.EventHandler(this.mnuView_DropDownOpening);
			// 
			// mnuZoom
			// 
			this.mnuZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoom10,
            this.mnuZoom25,
            this.mnuZoom50,
            this.mnuSep50,
            this.mnuZoom100,
            this.toolStripSeparator4,
            this.mnuZoom150,
            this.mnuZoom200,
            this.mnuZoom400});
			this.mnuZoom.Image = global::NClass.GUI.Properties.Resources.Zoom;
			this.mnuZoom.Name = "mnuZoom";
			this.mnuZoom.Size = new System.Drawing.Size(177, 22);
			this.mnuZoom.Text = "&Zoom";
			// 
			// mnuZoom10
			// 
			this.mnuZoom10.Name = "mnuZoom10";
			this.mnuZoom10.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom10.Text = "10%";
			this.mnuZoom10.Click += new System.EventHandler(this.mnuZoom10_Click);
			// 
			// mnuZoom25
			// 
			this.mnuZoom25.Name = "mnuZoom25";
			this.mnuZoom25.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom25.Text = "25%";
			this.mnuZoom25.Click += new System.EventHandler(this.mnuZoom25_Click);
			// 
			// mnuZoom50
			// 
			this.mnuZoom50.Name = "mnuZoom50";
			this.mnuZoom50.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom50.Text = "50%";
			this.mnuZoom50.Click += new System.EventHandler(this.mnuZoom50_Click);
			// 
			// mnuSep50
			// 
			this.mnuSep50.Name = "mnuSep50";
			this.mnuSep50.Size = new System.Drawing.Size(111, 6);
			// 
			// mnuZoom100
			// 
			this.mnuZoom100.Image = global::NClass.GUI.Properties.Resources.ActualSize;
			this.mnuZoom100.Name = "mnuZoom100";
			this.mnuZoom100.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom100.Text = "100%";
			this.mnuZoom100.Click += new System.EventHandler(this.mnuZoom100_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(111, 6);
			// 
			// mnuZoom150
			// 
			this.mnuZoom150.Name = "mnuZoom150";
			this.mnuZoom150.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom150.Text = "150%";
			this.mnuZoom150.Click += new System.EventHandler(this.mnuZoom150_Click);
			// 
			// mnuZoom200
			// 
			this.mnuZoom200.Name = "mnuZoom200";
			this.mnuZoom200.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom200.Text = "200%";
			this.mnuZoom200.Click += new System.EventHandler(this.mnuZoom200_Click);
			// 
			// mnuZoom400
			// 
			this.mnuZoom400.Name = "mnuZoom400";
			this.mnuZoom400.Size = new System.Drawing.Size(114, 22);
			this.mnuZoom400.Text = "400%";
			this.mnuZoom400.Click += new System.EventHandler(this.mnuZoom400_Click);
			// 
			// mnuAutoZoom
			// 
			this.mnuAutoZoom.Image = global::NClass.GUI.Properties.Resources.AutoZoom;
			this.mnuAutoZoom.Name = "mnuAutoZoom";
			this.mnuAutoZoom.Size = new System.Drawing.Size(177, 22);
			this.mnuAutoZoom.Text = "&Auto Zoom";
			this.mnuAutoZoom.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// mnuSepAutoZoom
			// 
			this.mnuSepAutoZoom.Name = "mnuSepAutoZoom";
			this.mnuSepAutoZoom.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuModelExplorer
			// 
			this.mnuModelExplorer.Checked = true;
			this.mnuModelExplorer.CheckOnClick = true;
			this.mnuModelExplorer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuModelExplorer.Name = "mnuModelExplorer";
			this.mnuModelExplorer.Size = new System.Drawing.Size(177, 22);
			this.mnuModelExplorer.Text = "Model &Explorer";
			this.mnuModelExplorer.Click += new System.EventHandler(this.mnuModelExplorer_Click);
			// 
			// mnuDiagramNavigator
			// 
			this.mnuDiagramNavigator.Checked = true;
			this.mnuDiagramNavigator.CheckOnClick = true;
			this.mnuDiagramNavigator.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuDiagramNavigator.Name = "mnuDiagramNavigator";
			this.mnuDiagramNavigator.Size = new System.Drawing.Size(177, 22);
			this.mnuDiagramNavigator.Text = "Diagram &Navigator";
			this.mnuDiagramNavigator.Click += new System.EventHandler(this.mnuDiagramNavigator_Click);
			// 
			// mnuSepDiagramNavigator
			// 
			this.mnuSepDiagramNavigator.Name = "mnuSepDiagramNavigator";
			this.mnuSepDiagramNavigator.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuCloseAllDocuments
			// 
			this.mnuCloseAllDocuments.Name = "mnuCloseAllDocuments";
			this.mnuCloseAllDocuments.Size = new System.Drawing.Size(177, 22);
			this.mnuCloseAllDocuments.Text = "&Close All Documens";
			this.mnuCloseAllDocuments.Click += new System.EventHandler(this.mnuCloseAllDocuments_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(174, 6);
			// 
			// mnuOptions
			// 
			this.mnuOptions.Image = global::NClass.GUI.Properties.Resources.Options;
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(177, 22);
			this.mnuOptions.Text = "&Options...";
			this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
			// 
			// mnuPlugins
			// 
			this.mnuPlugins.Name = "mnuPlugins";
			this.mnuPlugins.Size = new System.Drawing.Size(52, 20);
			this.mnuPlugins.Text = "&Plugins";
			this.mnuPlugins.Visible = false;
			this.mnuPlugins.DropDownOpening += new System.EventHandler(this.mnuPlugins_DropDownOpening);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContents,
            this.mnuCheckForUpdates,
            this.mnuSepUpdates,
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(40, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mnuContents
			// 
			this.mnuContents.Image = global::NClass.GUI.Properties.Resources.Help;
			this.mnuContents.Name = "mnuContents";
			this.mnuContents.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.mnuContents.Size = new System.Drawing.Size(174, 22);
			this.mnuContents.Text = "&Contents";
			this.mnuContents.Visible = false;
			this.mnuContents.Click += new System.EventHandler(this.mnuContents_Click);
			// 
			// mnuCheckForUpdates
			// 
			this.mnuCheckForUpdates.Image = global::NClass.GUI.Properties.Resources.SearchWeb;
			this.mnuCheckForUpdates.Name = "mnuCheckForUpdates";
			this.mnuCheckForUpdates.Size = new System.Drawing.Size(174, 22);
			this.mnuCheckForUpdates.Text = "Check for &Updates";
			this.mnuCheckForUpdates.Click += new System.EventHandler(this.mnuCheckForUpdates_Click);
			// 
			// mnuSepUpdates
			// 
			this.mnuSepUpdates.Name = "mnuSepUpdates";
			this.mnuSepUpdates.Size = new System.Drawing.Size(171, 6);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(174, 22);
			this.mnuAbout.Text = "&About NClass...";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// standardToolStrip
			// 
			this.standardToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.standardToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.standardToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolOpen,
            this.toolSave,
            this.toolPrint,
            this.toolSepPrint,
            this.toolCut,
            this.toolCopy,
            this.toolPaste,
            this.toolSepPaste,
            this.toolUndo,
            this.toolRedo,
            this.toolSepRedo,
            this.toolZoomValue,
            this.toolZoomOut,
            this.toolZoom,
            this.toolZoomIn,
            this.toolAutoZoom});
			this.standardToolStrip.Location = new System.Drawing.Point(3, 24);
			this.standardToolStrip.Name = "standardToolStrip";
			this.standardToolStrip.Size = new System.Drawing.Size(396, 25);
			this.standardToolStrip.TabIndex = 1;
			// 
			// toolNew
			// 
			this.toolNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewProject,
            this.toolSepProject,
            this.toolNewCSharpDiagram,
            this.toolNewJavaDiagram});
			this.toolNew.Image = global::NClass.GUI.Properties.Resources.NewDocument;
			this.toolNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNew.Name = "toolNew";
			this.toolNew.Size = new System.Drawing.Size(29, 22);
			this.toolNew.DropDownOpening += new System.EventHandler(this.toolNew_DropDownOpening);
			// 
			// toolNewProject
			// 
			this.toolNewProject.Image = global::NClass.GUI.Properties.Resources.Project;
			this.toolNewProject.Name = "toolNewProject";
			this.toolNewProject.Size = new System.Drawing.Size(149, 22);
			this.toolNewProject.Text = "Project";
			this.toolNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
			// 
			// toolSepProject
			// 
			this.toolSepProject.Name = "toolSepProject";
			this.toolSepProject.Size = new System.Drawing.Size(146, 6);
			// 
			// toolNewCSharpDiagram
			// 
			this.toolNewCSharpDiagram.Enabled = false;
			this.toolNewCSharpDiagram.Name = "toolNewCSharpDiagram";
			this.toolNewCSharpDiagram.Size = new System.Drawing.Size(149, 22);
			this.toolNewCSharpDiagram.Text = "C# diagram";
			this.toolNewCSharpDiagram.Click += new System.EventHandler(this.mnuNewCSharpDiagram_Click);
			// 
			// toolNewJavaDiagram
			// 
			this.toolNewJavaDiagram.Enabled = false;
			this.toolNewJavaDiagram.Name = "toolNewJavaDiagram";
			this.toolNewJavaDiagram.Size = new System.Drawing.Size(149, 22);
			this.toolNewJavaDiagram.Text = "Java diagram";
			this.toolNewJavaDiagram.Click += new System.EventHandler(this.mnuNewJavaDiagram_Click);
			// 
			// toolOpen
			// 
			this.toolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolRecentFile1,
            this.toolRecentFile2,
            this.toolRecentFile3,
            this.toolRecentFile4,
            this.toolRecentFile5});
			this.toolOpen.Image = global::NClass.GUI.Properties.Resources.Open;
			this.toolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.Size = new System.Drawing.Size(32, 22);
			this.toolOpen.ButtonClick += new System.EventHandler(this.mnuOpenFile_Click);
			this.toolOpen.DropDownOpening += new System.EventHandler(this.toolOpen_DropDownOpening);
			// 
			// toolRecentFile1
			// 
			this.toolRecentFile1.Name = "toolRecentFile1";
			this.toolRecentFile1.Size = new System.Drawing.Size(145, 22);
			this.toolRecentFile1.Tag = 0;
			this.toolRecentFile1.Text = "Recent file 1";
			this.toolRecentFile1.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// toolRecentFile2
			// 
			this.toolRecentFile2.Name = "toolRecentFile2";
			this.toolRecentFile2.Size = new System.Drawing.Size(145, 22);
			this.toolRecentFile2.Tag = 1;
			this.toolRecentFile2.Text = "Recent file 2";
			this.toolRecentFile2.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// toolRecentFile3
			// 
			this.toolRecentFile3.Name = "toolRecentFile3";
			this.toolRecentFile3.Size = new System.Drawing.Size(145, 22);
			this.toolRecentFile3.Tag = 2;
			this.toolRecentFile3.Text = "Recent file 3";
			this.toolRecentFile3.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// toolRecentFile4
			// 
			this.toolRecentFile4.Name = "toolRecentFile4";
			this.toolRecentFile4.Size = new System.Drawing.Size(145, 22);
			this.toolRecentFile4.Tag = 3;
			this.toolRecentFile4.Text = "Recent file 4";
			this.toolRecentFile4.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// toolRecentFile5
			// 
			this.toolRecentFile5.Name = "toolRecentFile5";
			this.toolRecentFile5.Size = new System.Drawing.Size(145, 22);
			this.toolRecentFile5.Tag = 4;
			this.toolRecentFile5.Text = "Recent file 5";
			this.toolRecentFile5.Click += new System.EventHandler(this.OpenRecentFile_Click);
			// 
			// toolSave
			// 
			this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSave.Enabled = false;
			this.toolSave.Image = global::NClass.GUI.Properties.Resources.Save;
			this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSave.Name = "toolSave";
			this.toolSave.Size = new System.Drawing.Size(23, 22);
			this.toolSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// toolPrint
			// 
			this.toolPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPrint.Enabled = false;
			this.toolPrint.Image = global::NClass.GUI.Properties.Resources.Print;
			this.toolPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPrint.Name = "toolPrint";
			this.toolPrint.Size = new System.Drawing.Size(23, 22);
			this.toolPrint.Click += new System.EventHandler(this.mnuPrint_Click);
			// 
			// toolSepPrint
			// 
			this.toolSepPrint.Name = "toolSepPrint";
			this.toolSepPrint.Size = new System.Drawing.Size(6, 25);
			// 
			// toolCut
			// 
			this.toolCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolCut.Enabled = false;
			this.toolCut.Image = global::NClass.GUI.Properties.Resources.Cut;
			this.toolCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCut.Name = "toolCut";
			this.toolCut.Size = new System.Drawing.Size(23, 22);
			this.toolCut.Click += new System.EventHandler(this.toolCut_Click);
			// 
			// toolCopy
			// 
			this.toolCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolCopy.Enabled = false;
			this.toolCopy.Image = global::NClass.GUI.Properties.Resources.Copy;
			this.toolCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCopy.Name = "toolCopy";
			this.toolCopy.Size = new System.Drawing.Size(23, 22);
			this.toolCopy.Click += new System.EventHandler(this.toolCopy_Click);
			// 
			// toolPaste
			// 
			this.toolPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPaste.Enabled = false;
			this.toolPaste.Image = global::NClass.GUI.Properties.Resources.Paste;
			this.toolPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPaste.Name = "toolPaste";
			this.toolPaste.Size = new System.Drawing.Size(23, 22);
			this.toolPaste.Click += new System.EventHandler(this.toolPaste_Click);
			// 
			// toolSepPaste
			// 
			this.toolSepPaste.Name = "toolSepPaste";
			this.toolSepPaste.Size = new System.Drawing.Size(6, 25);
			// 
			// toolUndo
			// 
			this.toolUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolUndo.Enabled = false;
			this.toolUndo.Image = global::NClass.GUI.Properties.Resources.Undo;
			this.toolUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolUndo.Name = "toolUndo";
			this.toolUndo.Size = new System.Drawing.Size(23, 22);
			this.toolUndo.Visible = false;
			// 
			// toolRedo
			// 
			this.toolRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolRedo.Enabled = false;
			this.toolRedo.Image = global::NClass.GUI.Properties.Resources.Redo;
			this.toolRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolRedo.Name = "toolRedo";
			this.toolRedo.Size = new System.Drawing.Size(23, 22);
			this.toolRedo.Visible = false;
			// 
			// toolSepRedo
			// 
			this.toolSepRedo.Name = "toolSepRedo";
			this.toolSepRedo.Size = new System.Drawing.Size(6, 25);
			this.toolSepRedo.Visible = false;
			// 
			// toolZoomValue
			// 
			this.toolZoomValue.AutoSize = false;
			this.toolZoomValue.Enabled = false;
			this.toolZoomValue.Name = "toolZoomValue";
			this.toolZoomValue.Size = new System.Drawing.Size(36, 22);
			this.toolZoomValue.Text = "100%";
			this.toolZoomValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// toolZoomOut
			// 
			this.toolZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolZoomOut.Enabled = false;
			this.toolZoomOut.Image = global::NClass.GUI.Properties.Resources.ZoomOut;
			this.toolZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolZoomOut.Name = "toolZoomOut";
			this.toolZoomOut.Size = new System.Drawing.Size(23, 22);
			this.toolZoomOut.Click += new System.EventHandler(this.toolZoomOut_Click);
			// 
			// toolZoom
			// 
			this.toolZoom.Enabled = false;
			this.toolZoom.Name = "toolZoom";
			this.toolZoom.Size = new System.Drawing.Size(100, 22);
			this.toolZoom.ZoomValueChanged += new System.EventHandler(this.toolZoom_ZoomValueChanged);
			// 
			// toolZoomIn
			// 
			this.toolZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolZoomIn.Enabled = false;
			this.toolZoomIn.Image = global::NClass.GUI.Properties.Resources.ZoomIn;
			this.toolZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolZoomIn.Name = "toolZoomIn";
			this.toolZoomIn.Size = new System.Drawing.Size(23, 22);
			this.toolZoomIn.Click += new System.EventHandler(this.toolZoomIn_Click);
			// 
			// toolAutoZoom
			// 
			this.toolAutoZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAutoZoom.Enabled = false;
			this.toolAutoZoom.Image = global::NClass.GUI.Properties.Resources.AutoZoom;
			this.toolAutoZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAutoZoom.Name = "toolAutoZoom";
			this.toolAutoZoom.Size = new System.Drawing.Size(23, 22);
			this.toolAutoZoom.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 666);
			this.Controls.Add(this.toolStripContainer);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NClass";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer.ContentPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.PerformLayout();
			this.toolStripContainer.ResumeLayout(false);
			this.toolStripContainer.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.windowClient.Panel1.ResumeLayout(false);
			this.windowClient.Panel2.ResumeLayout(false);
			this.windowClient.ResumeLayout(false);
			this.toolsPanel.Panel1.ResumeLayout(false);
			this.toolsPanel.Panel2.ResumeLayout(false);
			this.toolsPanel.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.standardToolStrip.ResumeLayout(false);
			this.standardToolStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStrip standardToolStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuNew;
		private System.Windows.Forms.ToolStripMenuItem mnuNewCSharpDiagram;
		private System.Windows.Forms.ToolStripMenuItem mnuNewJavaDiagram;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenFile;
		private System.Windows.Forms.ToolStripSeparator sepOpenFile;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolStripSeparator mnuSepSaveAll;
		private System.Windows.Forms.ToolStripMenuItem mnuPrint;
		private System.Windows.Forms.ToolStripSeparator mnuSepExport;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuContents;
		private System.Windows.Forms.ToolStripSeparator mnuSepUpdates;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFile1;
		private System.Windows.Forms.ToolStripSplitButton toolOpen;
		private System.Windows.Forms.ToolStripButton toolSave;
		private System.Windows.Forms.ToolStripSeparator toolSepPrint;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFile2;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFile3;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFile4;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFile5;
		private System.Windows.Forms.ToolStripMenuItem toolRecentFile1;
		private System.Windows.Forms.ToolStripMenuItem toolRecentFile2;
		private System.Windows.Forms.ToolStripMenuItem toolRecentFile3;
		private System.Windows.Forms.ToolStripMenuItem toolRecentFile4;
		private System.Windows.Forms.ToolStripMenuItem toolRecentFile5;
		private System.Windows.Forms.ToolStripButton toolZoomIn;
		private System.Windows.Forms.ToolStripButton toolZoomOut;
		private System.Windows.Forms.ToolStripButton toolAutoZoom;
		private System.Windows.Forms.ToolStripStatusLabel lblLanguage;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripButton toolPrint;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuUndo;
		private System.Windows.Forms.ToolStripMenuItem mnuRedo;
		private System.Windows.Forms.ToolStripSeparator mnuSepReso;
		private System.Windows.Forms.ToolStripMenuItem mnuCut;
		private System.Windows.Forms.ToolStripMenuItem mnuPaste;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
		private System.Windows.Forms.ToolStripSeparator mnuSepDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
		private System.Windows.Forms.ToolStripMenuItem mnuCheckForUpdates;
		private System.Windows.Forms.ToolStripMenuItem mnuPlugins;
		private ZoomingToolStrip toolZoom;
		private System.Windows.Forms.ToolStripLabel toolZoomValue;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAll;
		private System.Windows.Forms.ToolStripMenuItem mnuCopy;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripButton toolCut;
		private System.Windows.Forms.ToolStripButton toolCopy;
		private System.Windows.Forms.ToolStripButton toolPaste;
		private System.Windows.Forms.ToolStripSeparator toolSepRedo;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoZoom;
		private System.Windows.Forms.ToolStripButton toolUndo;
		private System.Windows.Forms.ToolStripButton toolRedo;
		private System.Windows.Forms.ToolStripSeparator toolSepPaste;
		private System.Windows.Forms.SplitContainer windowClient;
		private System.Windows.Forms.SplitContainer toolsPanel;
		private System.Windows.Forms.ToolStripMenuItem mnuModelExplorer;
		private System.Windows.Forms.ToolStripMenuItem mnuDiagramNavigator;
		private NClass.GUI.ModelExplorer.ModelView modelExplorer;
		private NClass.GUI.DiagramNavigator diagramNavigator;
		private TabbedWindow tabbedWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuNewProject;
		private System.Windows.Forms.ToolStripSeparator mnuSepProject;
		private System.Windows.Forms.ToolStripDropDownButton toolNew;
		private System.Windows.Forms.ToolStripMenuItem toolNewProject;
		private System.Windows.Forms.ToolStripSeparator toolSepProject;
		private System.Windows.Forms.ToolStripMenuItem toolNewCSharpDiagram;
		private System.Windows.Forms.ToolStripMenuItem toolNewJavaDiagram;
		private System.Windows.Forms.ToolStripSeparator mnuSepClose;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseProject;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseAllProjects;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseAllDocuments;
		private System.Windows.Forms.ToolStripSeparator mnuSepOpen;
		private System.Windows.Forms.ToolStripSeparator mnuSepAutoZoom;
		private System.Windows.Forms.ToolStripSeparator mnuSepDiagramNavigator;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom10;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom25;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom50;
		private System.Windows.Forms.ToolStripSeparator mnuSep50;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom100;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom150;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom200;
		private System.Windows.Forms.ToolStripMenuItem mnuZoom400;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
	}
}