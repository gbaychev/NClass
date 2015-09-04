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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using NClass.Core;
using NClass.CSharp;
using NClass.Java;
using NClass.DiagramEditor;
using NClass.DiagramEditor.ClassDiagram;
using NClass.GUI.Dialogs;
using NClass.Translations;

namespace NClass.GUI
{
	public sealed partial class MainForm : Form
	{
		DocumentManager docManager = new DocumentManager();
		bool showModelExplorer = true;
		bool showNavigator = true;
		DynamicMenu dynamicMenu = null;
		List<Plugin> plugins = new List<Plugin>();

		public MainForm()
		{
			InitializeComponent();

			tabbedWindow.Canvas.ZoomChanged += new EventHandler(canvas_ZoomChanged);
			tabbedWindow.DocumentManager = docManager;

			Workspace.Default.ActiveProjectChanged += delegate { UpdateTitleBar(); };
			Workspace.Default.ActiveProjectStateChanged += delegate { UpdateTitleBar(); };
			Workspace.Default.ProjectAdded += delegate { ShowModelExplorer = true; };
			docManager.ActiveDocumentChanged += docManager_ActiveDocumentChanged;
			modelExplorer.Workspace = Workspace.Default;
			tabbedWindow.DocumentManager = docManager;
			diagramNavigator.DocumentVisualizer = tabbedWindow.Canvas;

			UpdateTexts();
			UpdateStatusBar();
		}

		private bool ShowModelExplorer
		{
			get
			{
				return showModelExplorer;
			}
			set
			{
				showModelExplorer = value;

				if (!showModelExplorer)
				{
					if (showNavigator)
						toolsPanel.Panel1Collapsed = true;
					else
						windowClient.Panel2Collapsed = true;
					showModelExplorer = false;
				}
				else
				{
					toolsPanel.Panel1Collapsed = false;
					windowClient.Panel2Collapsed = false;
					if (!showNavigator)
						toolsPanel.Panel2Collapsed = true;
				}
			}
		}

		private bool ShowNavigator
		{
			get
			{
				return showNavigator;
			}
			set
			{
				showNavigator = value;

				if (!showNavigator)
				{
					if (showModelExplorer)
						toolsPanel.Panel2Collapsed = true;
					else
						windowClient.Panel2Collapsed = true;
					showNavigator = false;
				}
				else
				{
					toolsPanel.Panel2Collapsed = false;
					windowClient.Panel2Collapsed = false;
					if (!showModelExplorer)
						toolsPanel.Panel1Collapsed = true;
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadPlugins();
			LoadWindowSettings();
		}

		private void LoadPlugins()
		{
			try
			{
				string pluginsPath = Path.Combine(Application.StartupPath, "Plugins");
				if (!Directory.Exists(pluginsPath))
					return;

				DirectoryInfo directory = new DirectoryInfo(pluginsPath);

				foreach (FileInfo file in directory.GetFiles("*.dll"))
				{
					Assembly assembly = Assembly.LoadFile(file.FullName);
					LoadPlugin(assembly);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(Strings.ErrorCouldNotLoadPlugins, ex.Message),
					Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (plugins.Count > 0)
			{
				mnuPlugins.Visible = true;

				foreach (Plugin plugin in plugins)
				{
					mnuPlugins.DropDownItems.Add(plugin.MenuItem);
					plugin.MenuItem.Tag = plugin;
				}
			}
		}

		private void LoadPlugin(Assembly assembly)
		{
			try
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsSubclassOf(typeof(Plugin)))
					{
						NClassEnvironment environment =
							new NClassEnvironment(Workspace.Default, docManager);
						Plugin plugin = (Plugin) Activator.CreateInstance(type, environment);
						plugins.Add(plugin);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(Strings.ErrorCouldNotLoadPlugins, assembly.FullName + "\n" + ex.Message),
					Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadWindowSettings()
		{
			// Mono hack because of a .NET/Mono serialization difference of Point and Size classes
			if (MonoHelper.IsRunningOnMono)
				return;

			Location = WindowSettings.Default.WindowPosition;
			Size = WindowSettings.Default.WindowSize;
			if (WindowSettings.Default.IsWindowMaximized)
				WindowState = FormWindowState.Maximized;

			ShowModelExplorer = WindowSettings.Default.ShowModelExplorer;
			ShowNavigator = WindowSettings.Default.ShowNavigator;
			windowClient.SplitterDistance = WindowSettings.Default.ClientSplitterDistance;
			toolsPanel.SplitterDistance = WindowSettings.Default.ToolsSplitterDistance;
		}

		private void SaveWindowSettings()
		{
			// Mono hack because of a .NET/Mono serialization difference of Point and Size classes
			if (MonoHelper.IsRunningOnMono)
				return;

			if (WindowState == FormWindowState.Maximized)
			{
				WindowSettings.Default.IsWindowMaximized = true;
			}
			else
			{
				WindowSettings.Default.IsWindowMaximized = false;
				if (WindowState == FormWindowState.Normal)
					WindowSettings.Default.WindowSize = Size;
				if (WindowState == FormWindowState.Normal)
					WindowSettings.Default.WindowPosition = Location;
			}
			WindowSettings.Default.ClientSplitterDistance = windowClient.SplitterDistance;
			WindowSettings.Default.ToolsSplitterDistance = toolsPanel.SplitterDistance;
			WindowSettings.Default.ShowModelExplorer = ShowModelExplorer;
			WindowSettings.Default.ShowNavigator = ShowNavigator;

			WindowSettings.Default.Save();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!Workspace.Default.SaveAndClose())
			{
				e.Cancel = true;
				return;
			}
			SaveWindowSettings();
		}

		private void UpdateTexts()
		{
			// File menu
			mnuFile.Text = Strings.MenuFile;
			mnuNew.Text = Strings.MenuNew;
			mnuNewProject.Text = Strings.MenuProject;
			mnuNewCSharpDiagram.Text = Strings.MenuCSharpDiagram;
			mnuNewJavaDiagram.Text = Strings.MenuJavaDiagram;
			mnuOpen.Text = Strings.MenuOpen;
			mnuOpenFile.Text = Strings.MenuOpenFile;
			mnuSave.Text = Strings.MenuSave;
			mnuSaveAs.Text = Strings.MenuSaveAs;
			mnuSaveAll.Text = Strings.MenuSaveAllProjects;
			mnuPrint.Text = Strings.MenuPrint;
			mnuCloseProject.Text = Strings.MenuCloseProject;
			mnuCloseAllProjects.Text = Strings.MenuCloseAllProjects;
			mnuExit.Text = Strings.MenuExit;

			// Edit menu
			mnuEdit.Text = Strings.MenuEdit;
			mnuUndo.Text = Strings.MenuUndo;
			mnuRedo.Text = Strings.MenuRedo;
			mnuCut.Text = Strings.MenuCut;
			mnuCopy.Text = Strings.MenuCopy;
			mnuPaste.Text = Strings.MenuPaste;
			mnuDelete.Text = Strings.MenuDelete;
			mnuSelectAll.Text = Strings.MenuSelectAll;

			// View menu
			mnuView.Text = Strings.MenuView;
			mnuZoom.Text = Strings.MenuZoom;
			mnuAutoZoom.Text = Strings.MenuAutoZoom;
			mnuModelExplorer.Text = Strings.MenuModelExplorer;
			mnuDiagramNavigator.Text = Strings.MenuDiagramNavigator;
			mnuCloseAllDocuments.Text = Strings.MenuCloseAllDocuments;
			mnuOptions.Text = Strings.MenuOptions;

			// Plugins menu
			mnuPlugins.Text = Strings.MenuPlugins;

			// Help menu
			mnuHelp.Text = Strings.MenuHelp;
			mnuContents.Text = Strings.MenuContents;
			mnuCheckForUpdates.Text = Strings.MenuCheckForUpdates;
			mnuAbout.Text = Strings.MenuAbout;

			// Toolbar
			toolNewCSharpDiagram.Text = Strings.MenuCSharpDiagram;
			toolNewJavaDiagram.Text = Strings.MenuJavaDiagram;
			toolSave.Text = Strings.Save;
			toolPrint.Text = Strings.Print;
			toolCut.Text = Strings.Cut;
			toolCopy.Text = Strings.Copy;
			toolPaste.Text = Strings.Paste;
			toolZoomIn.Text = Strings.ZoomIn;
			toolZoomOut.Text = Strings.ZoomOut;
			toolAutoZoom.Text = Strings.AutoZoom;
		}

		private void UpdateTitleBar()
		{
			if (Workspace.Default.HasActiveProject)
			{
				string projectName = Workspace.Default.ActiveProject.Name;

				if (Workspace.Default.ActiveProject.IsDirty)
					this.Text = projectName + "* - NClass";
				else
					this.Text = projectName + " - NClass";
			}
			else
			{
				this.Text = "NClass";
			}
		}

		private void UpdateDynamicMenus()
		{
			DynamicMenu newMenu = null;

			if (docManager.HasDocument)
				newMenu = docManager.ActiveDocument.GetDynamicMenu();

			if (newMenu != dynamicMenu)
			{
				if (dynamicMenu != null)
				{
					foreach (ToolStripMenuItem menuItem in dynamicMenu)
					{
						MainMenuStrip.Items.Remove(menuItem);
					}
					ToolStrip toolStrip = dynamicMenu.GetToolStrip();
					if (toolStrip != null)
						toolStripContainer.TopToolStripPanel.Controls.Remove(toolStrip);
					dynamicMenu.SetReference(null);
				}
				if (newMenu != null)
				{
					int preferredIndex = newMenu.PreferredIndex;
					if (preferredIndex < 0)
						preferredIndex = 3;
					foreach (ToolStripMenuItem menuItem in newMenu)
					{
						MainMenuStrip.Items.Insert(preferredIndex++, menuItem);
					}
					ToolStrip toolStrip = newMenu.GetToolStrip();
					if (toolStrip != null)
					{
						toolStrip.Top = standardToolStrip.Top;
						toolStrip.Left = standardToolStrip.Right;
						toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);
					}
				}
				dynamicMenu = newMenu;
			}
		}

		private void UpdateStandardToolStrip()
		{
			toolNewCSharpDiagram.Enabled = Workspace.Default.HasActiveProject;
			toolNewJavaDiagram.Enabled = Workspace.Default.HasActiveProject;
			toolSave.Enabled = Workspace.Default.HasActiveProject;
			toolPrint.Enabled = docManager.HasDocument;
			toolZoom.Enabled = docManager.HasDocument;
			toolZoomIn.Enabled = docManager.HasDocument;
			toolZoomOut.Enabled = docManager.HasDocument;
			toolZoomValue.Enabled = docManager.HasDocument;
			toolAutoZoom.Enabled = docManager.HasDocument && !docManager.ActiveDocument.IsEmpty;
		}

		private void UpdateClipboardToolBar()
		{
			if (docManager.HasDocument)
			{
				IDocument document = docManager.ActiveDocument;
				toolCut.Enabled = document.CanCutToClipboard;
				toolCopy.Enabled = document.CanCopyToClipboard;
				toolPaste.Enabled = document.CanPasteFromClipboard;
			}
			else
			{
				toolCut.Enabled = false;
				toolCopy.Enabled = false;
				toolPaste.Enabled = false;
			}
		}

		private void UpdateStatusBar()
		{
			if (docManager.HasDocument)
			{
				lblStatus.Text = docManager.ActiveDocument.GetStatus();
				lblLanguage.Text = docManager.ActiveDocument.GetShortDescription();
			}
			else
			{
				lblStatus.Text = string.Empty;
				lblLanguage.Text = string.Empty;
			}
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
				foreach (string fileName in files)
				{
					Workspace.Default.OpenProject(fileName);
				}
			}
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.Tab)
				docManager.SwitchDocument();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (docManager.SwitchingTabs && !e.Control)
				docManager.EndSwitching();
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

		private void docManager_ActiveDocumentChanged(object sender, DocumentEventArgs e)
		{
			if (docManager.HasDocument)
			{
				Workspace.Default.ActiveProject = docManager.ActiveDocument.Project;
				docManager.ActiveDocument.Modified += ActiveDocument_Modified;
				docManager.ActiveDocument.StatusChanged += ActiveDocument_StatusChanged;
				docManager.ActiveDocument.ClipboardAvailabilityChanged += 
					ActiveDocument_ClipboardAvailabilityChanged;
			}
			else
			{
				Workspace.Default.ActiveProject = null;
			}

			IDocument oldDocument = e.Document;
			if (oldDocument != null)
			{
				oldDocument.Modified -= ActiveDocument_Modified;
				oldDocument.StatusChanged -= ActiveDocument_StatusChanged;
				oldDocument.ClipboardAvailabilityChanged -=
					ActiveDocument_ClipboardAvailabilityChanged;
			}

			UpdateStatusBar();
			UpdateDynamicMenus();
			UpdateClipboardToolBar();
			UpdateStandardToolStrip();
		}

		private void ActiveDocument_Modified(object sender, EventArgs e)
		{
			toolAutoZoom.Enabled = (docManager.HasDocument && !docManager.ActiveDocument.IsEmpty);
		}

		private void ActiveDocument_StatusChanged(object sender, EventArgs e)
		{
			UpdateStatusBar();
		}

		private void ActiveDocument_ClipboardAvailabilityChanged(object sender, EventArgs e)
		{
			UpdateClipboardToolBar();
		}

		private void canvas_ZoomChanged(object sender, EventArgs e)
		{
			toolZoom.ZoomValue = tabbedWindow.Canvas.Zoom;
			toolZoomValue.Text = tabbedWindow.Canvas.ZoomPercentage + "%";
		}

		private void modelExplorer_DocumentOpening(object sender, DocumentEventArgs e)
		{
			docManager.AddOrActivate(e.Document);
		}

		#region File menu event handlers

		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			if (Workspace.Default.HasActiveProject)
			{
				string projectName = Workspace.Default.ActiveProject.Name;
				mnuSave.Text = string.Format(Strings.MenuSaveProject, projectName);
				mnuSaveAs.Text = string.Format(Strings.MenuSaveProjectAs, projectName);
				mnuCloseProject.Text = string.Format(Strings.MenuClose, projectName);
				mnuSave.Enabled = true;
				mnuSaveAs.Enabled = true;
				mnuCloseProject.Enabled = true;
			}
			else
			{
				mnuSave.Text = Strings.MenuSave;
				mnuSaveAs.Text = Strings.MenuSaveAs;
				mnuCloseProject.Text = Strings.MenuCloseProject;
				mnuSave.Enabled = false;
				mnuSaveAs.Enabled = false;
				mnuCloseProject.Enabled = false;
			}

			if (Workspace.Default.HasProject)
			{
				mnuSaveAll.Enabled = true;
				mnuCloseAllProjects.Enabled = true;
			}
			else
			{
				mnuSaveAll.Enabled = false;
				mnuCloseAllProjects.Enabled = false;
			}

			mnuPrint.Enabled = docManager.HasDocument;
		}

		private void mnuNew_DropDownOpening(object sender, EventArgs e)
		{
			mnuNewCSharpDiagram.Enabled = Workspace.Default.HasActiveProject;
			mnuNewJavaDiagram.Enabled = Workspace.Default.HasActiveProject;
		}

		private void mnuNewProject_Click(object sender, EventArgs e)
		{
			Project project = Workspace.Default.AddEmptyProject();
			Workspace.Default.ActiveProject = project;
		}

		private void mnuNewCSharpDiagram_Click(object sender, EventArgs e)
		{
			if (Workspace.Default.HasActiveProject)
			{
				ShowModelExplorer = true;
				Diagram diagram = new Diagram(CSharpLanguage.Instance);
				Workspace.Default.ActiveProject.Add(diagram);
				Settings.Default.DefaultLanguageName = CSharpLanguage.Instance.AssemblyName;
			}
		}

		private void mnuNewJavaDiagram_Click(object sender, EventArgs e)
		{
			if (Workspace.Default.HasActiveProject)
			{
				ShowModelExplorer = true;
				Diagram diagram = new Diagram(JavaLanguage.Instance);
				Workspace.Default.ActiveProject.Add(diagram);
				Settings.Default.DefaultLanguageName = JavaLanguage.Instance.AssemblyName;
			}
		}

		private void mnuOpenFile_Click(object sender, EventArgs e)
		{
			Workspace.Default.OpenProject();
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

			sepOpenFile.Visible = (Settings.Default.RecentFiles.Count > 0);
		}

		private void mnuSave_Click(object sender, EventArgs e)
		{
			Workspace.Default.SaveActiveProject();
		}

		private void mnuSaveAs_Click(object sender, EventArgs e)
		{
			Workspace.Default.SaveActiveProjectAs();
		}

		private void mnuSaveAll_Click(object sender, EventArgs e)
		{
			Workspace.Default.SaveAllProjects();
		}

		private void mnuPrint_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				IPrintable document = docManager.ActiveDocument;
				document.ShowPrintDialog();
			}
		}

		private void mnuCloseProject_Click(object sender, EventArgs e)
		{
			Workspace.Default.RemoveActiveProject();
		}

		private void mnuCloseAllProjects_Click(object sender, EventArgs e)
		{
			Workspace.Default.RemoveAll();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Edit menu event handlers

		private void mnuEdit_DropDownOpening(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				IDocument document = docManager.ActiveDocument;

				//UNDONE:
				//mnuUndo.Enabled = document.CanUndo;
				//mnuRedo.Enabled = document.CanRedo;
				mnuCut.Enabled = document.CanCutToClipboard;
				mnuCopy.Enabled = document.CanCopyToClipboard;
				mnuPaste.Enabled = document.CanPasteFromClipboard;
				mnuDelete.Enabled = document.HasSelectedElement;
				mnuSelectAll.Enabled = !document.IsEmpty;
			}
			else
			{
				mnuUndo.Enabled = false;
				mnuRedo.Enabled = false;
				mnuCut.Enabled = false;
				mnuCopy.Enabled = false;
				mnuPaste.Enabled = false;
				mnuDelete.Enabled = false;
				mnuSelectAll.Enabled = false;
			}
		}

		private void mnuEdit_DropDownClosed(object sender, EventArgs e)
		{
			mnuUndo.Enabled = true;
			mnuRedo.Enabled = true;
			mnuCut.Enabled = true;
			mnuCopy.Enabled = true;
			mnuPaste.Enabled = true;
			mnuDelete.Enabled = true;
			mnuSelectAll.Enabled = true;
		}

		private void mnuUndo_Click(object sender, EventArgs e)
		{
			//UNDONE: mnuUndo_Click
			MessageBox.Show(Strings.NotImplemented, "Undo",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuRedo_Click(object sender, EventArgs e)
		{
			//UNDONE: mnuRedo_Click
			MessageBox.Show(Strings.NotImplemented, "Redo",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuCut_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Cut();
			}
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Copy();
			}
		}

		private void mnuPaste_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Paste();
			}
		}

		private void mnuDelete_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.DeleteSelectedElements();
			}
		}

		private void mnuSelectAll_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.SelectAll();
			}
		}

		#endregion

		#region View menu event handlers

		private void mnuView_DropDownOpening(object sender, EventArgs e)
		{
			mnuZoom.Enabled = docManager.HasDocument;
			mnuAutoZoom.Enabled = docManager.HasDocument && !docManager.ActiveDocument.IsEmpty;
			mnuCloseAllDocuments.Enabled = docManager.HasDocument;

			mnuModelExplorer.Checked = ShowModelExplorer;
			mnuDiagramNavigator.Checked = ShowNavigator;
		}

		private void mnuZoom10_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 0.1F;
		}

		private void mnuZoom25_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 0.25F;
		}

		private void mnuZoom50_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 0.5F;
		}

		private void mnuZoom100_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 1.0F;
		}

		private void mnuZoom150_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 1.5F;
		}

		private void mnuZoom200_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 2.0F;
		}

		private void mnuZoom400_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = 4.0F;
		}

		private void mnuAutoZoom_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.AutoZoom();
		}

		private void mnuModelExplorer_Click(object sender, EventArgs e)
		{
			ShowModelExplorer = mnuModelExplorer.Checked;
		}

		private void mnuDiagramNavigator_Click(object sender, EventArgs e)
		{
			ShowNavigator = mnuDiagramNavigator.Checked;
		}

		private void mnuCloseAllDocuments_Click(object sender, EventArgs e)
		{
			docManager.CloseAll();
		}

		private void mnuOptions_Click(object sender, EventArgs e)
		{
			using (OptionsDialog dialog = new OptionsDialog())
			{
				dialog.StyleModified += new EventHandler(dialog_StyleChanged);
				dialog.ShowDialog();
				if (docManager.HasDocument)
					docManager.ActiveDocument.Redraw();
			}
		}

		private void dialog_StyleChanged(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
				docManager.ActiveDocument.Redraw();
		}

		#endregion

		#region Plugins menu event handlers

		private void mnuPlugins_DropDownOpening(object sender, EventArgs e)
		{
			foreach (ToolStripItem menuItem in mnuPlugins.DropDownItems)
			{
				Plugin plugin = menuItem.Tag as Plugin;
				menuItem.Enabled = plugin.IsAvailable;
			}
		}

		#endregion

		#region Help menu event handlers

		private void mnuContents_Click(object sender, EventArgs e)
		{
			MessageBox.Show(Strings.NotImplemented, "NClass",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuCheckForUpdates_Click(object sender, EventArgs e)
		{
			UpdatesChecker.CheckForUpdates();
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			using (AboutDialog dialog = new AboutDialog())
				dialog.ShowDialog();
		}

		#endregion

		#region Standard toolbar event handlers

		private void toolNew_DropDownOpening(object sender, EventArgs e)
		{
			toolNewCSharpDiagram.Enabled = Workspace.Default.HasActiveProject;
			toolNewJavaDiagram.Enabled = Workspace.Default.HasActiveProject;
		}

		private void toolOpen_DropDownOpening(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in toolOpen.DropDownItems)
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
		}

		private void toolCut_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Cut();
			}
		}

		private void toolCopy_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Copy();
			}
		}

		private void toolPaste_Click(object sender, EventArgs e)
		{
			if (docManager.HasDocument)
			{
				docManager.ActiveDocument.Paste();
			}
		}

		private void toolZoomIn_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.ZoomIn();
		}

		private void toolZoomOut_Click(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.ZoomOut();
		}

		private void toolZoom_ZoomValueChanged(object sender, EventArgs e)
		{
			tabbedWindow.Canvas.Zoom = toolZoom.ZoomValue;
		}

		#endregion
	}
}