namespace NClass.GUI
{
	partial class TabBar
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
			if (disposing)
			{
				stringFormat.Dispose();
				activeTabFont.Dispose();
			}			
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClose,
            this.mnuCloseAll,
            this.mnuCloseAllButThis});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(167, 70);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
			// 
			// mnuClose
			// 
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(166, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(mnuClose_Click);
			// 
			// mnuCloseAll
			// 
			this.mnuCloseAll.Name = "mnuCloseAll";
			this.mnuCloseAll.Size = new System.Drawing.Size(166, 22);
			this.mnuCloseAll.Text = "Close All";
			this.mnuCloseAll.Click += new System.EventHandler(mnuCloseAll_Click);
			// 
			// mnuCloseAllButThis
			// 
			this.mnuCloseAllButThis.Name = "mnuCloseAllButThis";
			this.mnuCloseAllButThis.Size = new System.Drawing.Size(166, 22);
			this.mnuCloseAllButThis.Text = "Close All But This";
			this.mnuCloseAllButThis.Click += new System.EventHandler(mnuCloseAllButThis_Click);
			// 
			// TabBar
			// 
			this.ContextMenuStrip = this.contextMenu;
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseAll;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseAllButThis;
	}
}
