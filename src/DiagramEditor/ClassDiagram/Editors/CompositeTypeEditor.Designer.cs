namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	partial class CompositeTypeEditor
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
			this.toolVisibility = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolPublic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolProtint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolInternal = new System.Windows.Forms.ToolStripMenuItem();
			this.toolProtected = new System.Windows.Forms.ToolStripMenuItem();
			this.toolPrivate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.toolModifier = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolNone = new System.Windows.Forms.ToolStripMenuItem();
			this.toolAbstract = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSealed = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStatic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSortByName = new System.Windows.Forms.ToolStripButton();
			this.toolSortByAccess = new System.Windows.Forms.ToolStripButton();
			this.toolSortByKind = new System.Windows.Forms.ToolStripButton();
			this.sepNewMember = new System.Windows.Forms.ToolStripSeparator();
			this.toolImplementList = new System.Windows.Forms.ToolStripButton();
			this.toolOverrideList = new System.Windows.Forms.ToolStripButton();
			this.toolNewMember = new System.Windows.Forms.ToolStripSplitButton();
			this.toolNewField = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewMethod = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewEvent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewConstructor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewDestructor = new System.Windows.Forms.ToolStripMenuItem();
			this.txtName = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolVisibility,
            this.toolModifier,
            this.toolSortByName,
            this.toolSortByAccess,
            this.toolSortByKind,
            this.sepNewMember,
            this.toolImplementList,
            this.toolOverrideList,
            this.toolNewMember});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 25);
			this.toolStrip.TabIndex = 4;
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
			this.toolVisibility.Image = global::NClass.DiagramEditor.Properties.Resources.PublicMethod;
			this.toolVisibility.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolVisibility.Name = "toolVisibility";
			this.toolVisibility.Size = new System.Drawing.Size(30, 22);
			this.toolVisibility.Text = "Protected";
			// 
			// toolPublic
			// 
			this.toolPublic.Image = global::NClass.DiagramEditor.Properties.Resources.PublicMethod;
			this.toolPublic.Name = "toolPublic";
			this.toolPublic.Size = new System.Drawing.Size(173, 22);
			this.toolPublic.Text = "Public";
			this.toolPublic.Click += new System.EventHandler(this.toolPublic_Click);
			// 
			// toolProtint
			// 
			this.toolProtint.Image = global::NClass.DiagramEditor.Properties.Resources.ProtintMethod;
			this.toolProtint.Name = "toolProtint";
			this.toolProtint.Size = new System.Drawing.Size(173, 22);
			this.toolProtint.Text = "Protected Internal";
			this.toolProtint.Click += new System.EventHandler(this.toolProtint_Click);
			// 
			// toolInternal
			// 
			this.toolInternal.Image = global::NClass.DiagramEditor.Properties.Resources.InternalMethod;
			this.toolInternal.Name = "toolInternal";
			this.toolInternal.Size = new System.Drawing.Size(173, 22);
			this.toolInternal.Text = "Internal";
			this.toolInternal.Click += new System.EventHandler(this.toolInternal_Click);
			// 
			// toolProtected
			// 
			this.toolProtected.Image = global::NClass.DiagramEditor.Properties.Resources.ProtectedMethod;
			this.toolProtected.Name = "toolProtected";
			this.toolProtected.Size = new System.Drawing.Size(173, 22);
			this.toolProtected.Text = "Protected";
			this.toolProtected.Click += new System.EventHandler(this.toolProtected_Click);
			// 
			// toolPrivate
			// 
			this.toolPrivate.Image = global::NClass.DiagramEditor.Properties.Resources.PrivateMethod;
			this.toolPrivate.Name = "toolPrivate";
			this.toolPrivate.Size = new System.Drawing.Size(173, 22);
			this.toolPrivate.Text = "Private";
			this.toolPrivate.Click += new System.EventHandler(this.toolPrivate_Click);
			// 
			// toolDefault
			// 
			this.toolDefault.Name = "toolDefault";
			this.toolDefault.Size = new System.Drawing.Size(173, 22);
			this.toolDefault.Text = "Default";
			this.toolDefault.Click += new System.EventHandler(this.toolDefault_Click);
			// 
			// toolModifier
			// 
			this.toolModifier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolModifier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNone,
            this.toolAbstract,
            this.toolSealed,
            this.toolStatic});
			this.toolModifier.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolModifier.Name = "toolModifier";
			this.toolModifier.Size = new System.Drawing.Size(45, 22);
			this.toolModifier.Text = "None";
			// 
			// toolNone
			// 
			this.toolNone.Name = "toolNone";
			this.toolNone.Size = new System.Drawing.Size(126, 22);
			this.toolNone.Text = "None";
			this.toolNone.Click += new System.EventHandler(this.toolNone_Click);
			// 
			// toolAbstract
			// 
			this.toolAbstract.Name = "toolAbstract";
			this.toolAbstract.Size = new System.Drawing.Size(126, 22);
			this.toolAbstract.Text = "Abstract";
			this.toolAbstract.Click += new System.EventHandler(this.toolAbstract_Click);
			// 
			// toolSealed
			// 
			this.toolSealed.Name = "toolSealed";
			this.toolSealed.Size = new System.Drawing.Size(126, 22);
			this.toolSealed.Text = "Sealed";
			this.toolSealed.Click += new System.EventHandler(this.toolSealed_Click);
			// 
			// toolStatic
			// 
			this.toolStatic.Name = "toolStatic";
			this.toolStatic.Size = new System.Drawing.Size(126, 22);
			this.toolStatic.Text = "Static";
			this.toolStatic.Click += new System.EventHandler(this.toolStatic_Click);
			// 
			// toolSortByName
			// 
			this.toolSortByName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSortByName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSortByName.Image = global::NClass.DiagramEditor.Properties.Resources.SortByName;
			this.toolSortByName.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSortByName.Name = "toolSortByName";
			this.toolSortByName.Size = new System.Drawing.Size(23, 22);
			this.toolSortByName.Text = "Sort by Name";
			this.toolSortByName.Click += new System.EventHandler(this.toolSortByName_Click);
			// 
			// toolSortByAccess
			// 
			this.toolSortByAccess.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSortByAccess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSortByAccess.Image = global::NClass.DiagramEditor.Properties.Resources.SortByAccess;
			this.toolSortByAccess.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSortByAccess.Name = "toolSortByAccess";
			this.toolSortByAccess.Size = new System.Drawing.Size(23, 22);
			this.toolSortByAccess.Text = "Sort by Access";
			this.toolSortByAccess.Click += new System.EventHandler(this.toolSortByAccess_Click);
			// 
			// toolSortByKind
			// 
			this.toolSortByKind.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSortByKind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolSortByKind.Image = global::NClass.DiagramEditor.Properties.Resources.SortByKind;
			this.toolSortByKind.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolSortByKind.Name = "toolSortByKind";
			this.toolSortByKind.Size = new System.Drawing.Size(23, 22);
			this.toolSortByKind.Text = "Sort by Kind";
			this.toolSortByKind.Click += new System.EventHandler(this.toolSortByKind_Click);
			// 
			// sepNewMember
			// 
			this.sepNewMember.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.sepNewMember.Name = "sepNewMember";
			this.sepNewMember.Size = new System.Drawing.Size(6, 25);
			// 
			// toolImplementList
			// 
			this.toolImplementList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolImplementList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolImplementList.Image = global::NClass.DiagramEditor.Properties.Resources.Implements;
			this.toolImplementList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolImplementList.Name = "toolImplementList";
			this.toolImplementList.Size = new System.Drawing.Size(23, 22);
			this.toolImplementList.Text = "Interface Implement List";
			this.toolImplementList.Click += new System.EventHandler(this.toolImplementList_Click);
			// 
			// toolOverrideList
			// 
			this.toolOverrideList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOverrideList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOverrideList.Image = global::NClass.DiagramEditor.Properties.Resources.Overrides;
			this.toolOverrideList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOverrideList.Name = "toolOverrideList";
			this.toolOverrideList.Size = new System.Drawing.Size(23, 22);
			this.toolOverrideList.Text = "Override List";
			this.toolOverrideList.Click += new System.EventHandler(this.toolOverrideList_Click);
			// 
			// toolNewMember
			// 
			this.toolNewMember.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolNewMember.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewMember.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewField,
            this.toolNewMethod,
            this.toolNewProperty,
            this.toolNewEvent,
            this.toolNewConstructor,
            this.toolNewDestructor});
			this.toolNewMember.Image = global::NClass.DiagramEditor.Properties.Resources.PublicMethod;
			this.toolNewMember.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewMember.Name = "toolNewMember";
			this.toolNewMember.Size = new System.Drawing.Size(32, 22);
			this.toolNewMember.Text = "New Member";
			this.toolNewMember.ButtonClick += new System.EventHandler(this.toolNewMember_ButtonClick);
			// 
			// toolNewField
			// 
			this.toolNewField.Image = global::NClass.DiagramEditor.Properties.Resources.Field;
			this.toolNewField.Name = "toolNewField";
			this.toolNewField.Size = new System.Drawing.Size(166, 22);
			this.toolNewField.Text = "New Field";
			this.toolNewField.Click += new System.EventHandler(this.toolNewField_Click);
			// 
			// toolNewMethod
			// 
			this.toolNewMethod.Image = global::NClass.DiagramEditor.Properties.Resources.Method;
			this.toolNewMethod.Name = "toolNewMethod";
			this.toolNewMethod.Size = new System.Drawing.Size(166, 22);
			this.toolNewMethod.Text = "New Method";
			this.toolNewMethod.Click += new System.EventHandler(this.toolNewMethod_Click);
			// 
			// toolNewProperty
			// 
			this.toolNewProperty.Image = global::NClass.DiagramEditor.Properties.Resources.Property;
			this.toolNewProperty.Name = "toolNewProperty";
			this.toolNewProperty.Size = new System.Drawing.Size(166, 22);
			this.toolNewProperty.Text = "New Property";
			this.toolNewProperty.Click += new System.EventHandler(this.toolNewProperty_Click);
			// 
			// toolNewEvent
			// 
			this.toolNewEvent.Image = global::NClass.DiagramEditor.Properties.Resources.Event;
			this.toolNewEvent.Name = "toolNewEvent";
			this.toolNewEvent.Size = new System.Drawing.Size(166, 22);
			this.toolNewEvent.Text = "New Event";
			this.toolNewEvent.Click += new System.EventHandler(this.toolNewEvent_Click);
			// 
			// toolNewConstructor
			// 
			this.toolNewConstructor.Image = global::NClass.DiagramEditor.Properties.Resources.Constructor;
			this.toolNewConstructor.Name = "toolNewConstructor";
			this.toolNewConstructor.Size = new System.Drawing.Size(166, 22);
			this.toolNewConstructor.Text = "New Constructor";
			this.toolNewConstructor.Click += new System.EventHandler(this.toolNewConstructor_Click);
			// 
			// toolNewDestructor
			// 
			this.toolNewDestructor.Image = global::NClass.DiagramEditor.Properties.Resources.Destructor;
			this.toolNewDestructor.Name = "toolNewDestructor";
			this.toolNewDestructor.Size = new System.Drawing.Size(166, 22);
			this.toolNewDestructor.Text = "New Destructor";
			this.toolNewDestructor.Click += new System.EventHandler(this.toolNewDestructor_Click);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(4, 28);
			this.txtName.Name = "txtName";
			this.txtName.Padding = new System.Windows.Forms.Padding(1);
			this.txtName.ReadOnly = false;
			this.txtName.SelectionStart = 0;
			this.txtName.Size = new System.Drawing.Size(322, 20);
			this.txtName.TabIndex = 5;
			this.txtName.AcceptsTab = true;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
			this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// CompositeTypeEditor
			// 
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.toolStrip);
			this.Name = "CompositeTypeEditor";
			this.Size = new System.Drawing.Size(330, 52);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton toolVisibility;
		private System.Windows.Forms.ToolStripMenuItem toolPublic;
		private System.Windows.Forms.ToolStripMenuItem toolInternal;
		private System.Windows.Forms.ToolStripMenuItem toolPrivate;
		private System.Windows.Forms.ToolStripDropDownButton toolModifier;
		private System.Windows.Forms.ToolStripMenuItem toolNone;
		private System.Windows.Forms.ToolStripMenuItem toolAbstract;
		private System.Windows.Forms.ToolStripMenuItem toolSealed;
		private System.Windows.Forms.ToolStripMenuItem toolStatic;
		private System.Windows.Forms.ToolStripMenuItem toolDefault;
		private BorderedTextBox txtName;
		private System.Windows.Forms.ToolStripMenuItem toolProtint;
		private System.Windows.Forms.ToolStripMenuItem toolProtected;
		private System.Windows.Forms.ToolStripSplitButton toolNewMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewField;
		private System.Windows.Forms.ToolStripMenuItem toolNewMethod;
		private System.Windows.Forms.ToolStripMenuItem toolNewProperty;
		private System.Windows.Forms.ToolStripMenuItem toolNewEvent;
		private System.Windows.Forms.ToolStripMenuItem toolNewConstructor;
		private System.Windows.Forms.ToolStripMenuItem toolNewDestructor;
		private System.Windows.Forms.ToolStripButton toolSortByName;
		private System.Windows.Forms.ToolStripSeparator sepNewMember;
		private System.Windows.Forms.ToolStripButton toolSortByAccess;
		private System.Windows.Forms.ToolStripButton toolSortByKind;
		private System.Windows.Forms.ToolStripButton toolImplementList;
		private System.Windows.Forms.ToolStripButton toolOverrideList;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}
