namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	partial class EnumEditor
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
			this.txtName = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolVisibility = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolPublic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolProtint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolInternal = new System.Windows.Forms.ToolStripMenuItem();
			this.toolProtected = new System.Windows.Forms.ToolStripMenuItem();
			this.toolPrivate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewValue = new System.Windows.Forms.ToolStripButton();
			this.txtNewValue = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(4, 27);
			this.txtName.Name = "txtName";
			this.txtName.Padding = new System.Windows.Forms.Padding(1);
			this.txtName.ReadOnly = false;
			this.txtName.SelectionStart = 0;
			this.txtName.Size = new System.Drawing.Size(322, 20);
			this.txtName.TabIndex = 0;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
			this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolVisibility,
            this.toolNewValue});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 25);
			this.toolStrip.TabIndex = 2;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolVisibility
			// 
			this.toolVisibility.AutoSize = false;
			this.toolVisibility.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolVisibility.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPublic,
            this.toolProtint,
            this.toolInternal,
            this.toolProtected,
            this.toolPrivate,
            this.toolDefault});
			this.toolVisibility.Image = global::NClass.DiagramEditor.Properties.Resources.PublicEnum;
			this.toolVisibility.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolVisibility.Name = "toolVisibility";
			this.toolVisibility.Size = new System.Drawing.Size(30, 22);
			this.toolVisibility.Text = "Public";
			// 
			// toolPublic
			// 
			this.toolPublic.Image = global::NClass.DiagramEditor.Properties.Resources.PublicEnum;
			this.toolPublic.Name = "toolPublic";
			this.toolPublic.Size = new System.Drawing.Size(173, 22);
			this.toolPublic.Text = "Public";
			this.toolPublic.Click += new System.EventHandler(this.toolPublic_Click);
			// 
			// toolProtint
			// 
			this.toolProtint.Image = global::NClass.DiagramEditor.Properties.Resources.ProtintEnum;
			this.toolProtint.Name = "toolProtint";
			this.toolProtint.Size = new System.Drawing.Size(173, 22);
			this.toolProtint.Text = "Protected Internal";
			this.toolProtint.Click += new System.EventHandler(this.toolProtint_Click);
			// 
			// toolInternal
			// 
			this.toolInternal.Image = global::NClass.DiagramEditor.Properties.Resources.InternalEnum;
			this.toolInternal.Name = "toolInternal";
			this.toolInternal.Size = new System.Drawing.Size(173, 22);
			this.toolInternal.Text = "Internal";
			this.toolInternal.Click += new System.EventHandler(this.toolInternal_Click);
			// 
			// toolProtected
			// 
			this.toolProtected.Image = global::NClass.DiagramEditor.Properties.Resources.ProtectedEnum;
			this.toolProtected.Name = "toolProtected";
			this.toolProtected.Size = new System.Drawing.Size(173, 22);
			this.toolProtected.Text = "Protected";
			this.toolProtected.Click += new System.EventHandler(this.toolProtected_Click);
			// 
			// toolPrivate
			// 
			this.toolPrivate.Image = global::NClass.DiagramEditor.Properties.Resources.PrivateEnum;
			this.toolPrivate.Name = "toolPrivate";
			this.toolPrivate.Size = new System.Drawing.Size(173, 22);
			this.toolPrivate.Text = "Private";
			this.toolPrivate.Click += new System.EventHandler(this.toolPrivate_Click);
			// 
			// toolDefault
			// 
			this.toolDefault.Image = global::NClass.DiagramEditor.Properties.Resources.DefaultEnum;
			this.toolDefault.Name = "toolDefault";
			this.toolDefault.Size = new System.Drawing.Size(173, 22);
			this.toolDefault.Text = "Default";
			this.toolDefault.Click += new System.EventHandler(this.toolDefault_Click);
			// 
			// toolNewValue
			// 
			this.toolNewValue.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolNewValue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewValue.Image = global::NClass.DiagramEditor.Properties.Resources.NewEnumItem;
			this.toolNewValue.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewValue.Name = "toolNewValue";
			this.toolNewValue.Size = new System.Drawing.Size(23, 22);
			this.toolNewValue.Text = "New Value";
			this.toolNewValue.Click += new System.EventHandler(this.toolNewValue_Click);
			// 
			// txtNewValue
			// 
			this.txtNewValue.AcceptsTab = true;
			this.txtNewValue.Location = new System.Drawing.Point(110, 4);
			this.txtNewValue.Name = "txtNewValue";
			this.txtNewValue.Padding = new System.Windows.Forms.Padding(1);
			this.txtNewValue.ReadOnly = false;
			this.txtNewValue.SelectionStart = 0;
			this.txtNewValue.Size = new System.Drawing.Size(191, 20);
			this.txtNewValue.TabIndex = 1;
			this.txtNewValue.TextChanged += new System.EventHandler(this.txtNewValue_TextChanged);
			this.txtNewValue.GotFocus += new System.EventHandler(this.txtNewValue_GotFocus);
			this.txtNewValue.LostFocus += new System.EventHandler(this.txtNewValue_LostFocus);
			this.txtNewValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewValue_KeyDown);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// EnumEditor
			// 
			this.Controls.Add(this.txtNewValue);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.toolStrip);
			this.Name = "EnumEditor";
			this.Size = new System.Drawing.Size(330, 51);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BorderedTextBox txtName;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton toolVisibility;
		private System.Windows.Forms.ToolStripMenuItem toolPublic;
		private System.Windows.Forms.ToolStripMenuItem toolProtint;
		private System.Windows.Forms.ToolStripMenuItem toolInternal;
		private System.Windows.Forms.ToolStripMenuItem toolProtected;
		private System.Windows.Forms.ToolStripMenuItem toolPrivate;
		private System.Windows.Forms.ToolStripMenuItem toolDefault;
		private System.Windows.Forms.ToolStripButton toolNewValue;
		private BorderedTextBox txtNewValue;
		private System.Windows.Forms.ErrorProvider errorProvider;

	}
}
