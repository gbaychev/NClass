namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	partial class ItemEditor
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
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolDelete = new System.Windows.Forms.ToolStripButton();
			this.toolMoveDown = new System.Windows.Forms.ToolStripButton();
			this.toolMoveUp = new System.Windows.Forms.ToolStripButton();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.txtDeclaration = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolDelete,
            this.toolMoveDown,
            this.toolMoveUp});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 28);
			this.toolStrip.TabIndex = 4;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolDelete
			// 
			this.toolDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDelete.Image = global::NClass.DiagramEditor.Properties.Resources.Delete;
			this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDelete.Name = "toolDelete";
			this.toolDelete.Size = new System.Drawing.Size(23, 25);
			this.toolDelete.Text = "toolStripButton2";
			this.toolDelete.ToolTipText = "Delete";
			this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);
			// 
			// toolMoveDown
			// 
			this.toolMoveDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolMoveDown.Image = global::NClass.DiagramEditor.Properties.Resources.MoveDown;
			this.toolMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolMoveDown.Name = "toolMoveDown";
			this.toolMoveDown.Size = new System.Drawing.Size(23, 25);
			this.toolMoveDown.Text = "toolStripButton3";
			this.toolMoveDown.ToolTipText = "Move Down";
			this.toolMoveDown.Click += new System.EventHandler(this.toolMoveDown_Click);
			// 
			// toolMoveUp
			// 
			this.toolMoveUp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolMoveUp.Image = global::NClass.DiagramEditor.Properties.Resources.MoveUp;
			this.toolMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolMoveUp.Name = "toolMoveUp";
			this.toolMoveUp.Size = new System.Drawing.Size(23, 25);
			this.toolMoveUp.Text = "toolStripButton4";
			this.toolMoveUp.ToolTipText = "Move Up";
			this.toolMoveUp.Click += new System.EventHandler(this.toolMoveUp_Click);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// txtDeclaration
			// 
			this.txtDeclaration.Location = new System.Drawing.Point(4, 4);
			this.txtDeclaration.Name = "txtDeclaration";
			this.txtDeclaration.Padding = new System.Windows.Forms.Padding(1);
			this.txtDeclaration.ReadOnly = false;
			this.txtDeclaration.SelectionStart = 0;
			this.txtDeclaration.Size = new System.Drawing.Size(251, 20);
			this.txtDeclaration.TabIndex = 5;
			this.txtDeclaration.AcceptsTab = true;
			this.txtDeclaration.TextChanged += new System.EventHandler(this.txtDeclaration_TextChanged);
			this.txtDeclaration.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeclaration_Validating);
			this.txtDeclaration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeclaration_KeyDown);
			// 
			// ItemEditor
			// 
			this.Controls.Add(this.txtDeclaration);
			this.Controls.Add(this.toolStrip);
			this.Name = "ItemEditor";
			this.Size = new System.Drawing.Size(330, 28);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton toolDelete;
		protected System.Windows.Forms.ToolStripButton toolMoveDown;
		protected System.Windows.Forms.ToolStripButton toolMoveUp;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private BorderedTextBox txtDeclaration;
	}
}
