namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	partial class MemberEditor
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
			this.toolStatic = new System.Windows.Forms.ToolStripButton();
			this.toolDelete = new System.Windows.Forms.ToolStripButton();
			this.toolMoveDown = new System.Windows.Forms.ToolStripButton();
			this.toolMoveUp = new System.Windows.Forms.ToolStripButton();
			this.toolHider = new System.Windows.Forms.ToolStripButton();
			this.toolFieldModifiers = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolFieldNone = new System.Windows.Forms.ToolStripMenuItem();
			this.toolReadonlyField = new System.Windows.Forms.ToolStripMenuItem();
			this.toolConst = new System.Windows.Forms.ToolStripMenuItem();
			this.toolVolatile = new System.Windows.Forms.ToolStripMenuItem();
			this.toolOperationModifiers = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolOperationNone = new System.Windows.Forms.ToolStripMenuItem();
			this.toolAbstract = new System.Windows.Forms.ToolStripMenuItem();
			this.toolVirtual = new System.Windows.Forms.ToolStripMenuItem();
			this.toolOverride = new System.Windows.Forms.ToolStripMenuItem();
			this.toolSealed = new System.Windows.Forms.ToolStripMenuItem();
			this.toolAbstractOverride = new System.Windows.Forms.ToolStripMenuItem();
			this.toolAccessor = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolReadWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.toolReadOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.toolWriteOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.sepNewMember = new System.Windows.Forms.ToolStripSeparator();
			this.toolNewMember = new System.Windows.Forms.ToolStripSplitButton();
			this.toolNewField = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewMethod = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewEvent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewConstructor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolNewDestructor = new System.Windows.Forms.ToolStripMenuItem();
			this.txtDeclaration = new NClass.DiagramEditor.ClassDiagram.Editors.BorderedTextBox();
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
            this.toolStatic,
            this.toolDelete,
            this.toolMoveDown,
            this.toolMoveUp,
            this.toolHider,
            this.toolFieldModifiers,
            this.toolOperationModifiers,
            this.toolAccessor,
            this.sepNewMember,
            this.toolNewMember});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
			this.toolStrip.Size = new System.Drawing.Size(330, 25);
			this.toolStrip.TabIndex = 3;
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
			this.toolDefault.Image = global::NClass.DiagramEditor.Properties.Resources.DefaultMethod;
			this.toolDefault.Name = "toolDefault";
			this.toolDefault.Size = new System.Drawing.Size(173, 22);
			this.toolDefault.Text = "Default";
			this.toolDefault.Click += new System.EventHandler(this.toolDefault_Click);
			// 
			// toolStatic
			// 
			this.toolStatic.CheckOnClick = true;
			this.toolStatic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStatic.Image = global::NClass.DiagramEditor.Properties.Resources.Static;
			this.toolStatic.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStatic.Name = "toolStatic";
			this.toolStatic.Size = new System.Drawing.Size(23, 22);
			this.toolStatic.Text = "Static";
			this.toolStatic.ToolTipText = "Static";
			this.toolStatic.CheckedChanged += new System.EventHandler(this.toolStatic_CheckedChanged);
			// 
			// toolDelete
			// 
			this.toolDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDelete.Image = global::NClass.DiagramEditor.Properties.Resources.Delete;
			this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDelete.Name = "toolDelete";
			this.toolDelete.Size = new System.Drawing.Size(23, 22);
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
			this.toolMoveDown.Size = new System.Drawing.Size(23, 22);
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
			this.toolMoveUp.Size = new System.Drawing.Size(23, 22);
			this.toolMoveUp.Text = "toolStripButton4";
			this.toolMoveUp.ToolTipText = "Move Up";
			this.toolMoveUp.Click += new System.EventHandler(this.toolMoveUp_Click);
			// 
			// toolHider
			// 
			this.toolHider.CheckOnClick = true;
			this.toolHider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolHider.Image = global::NClass.DiagramEditor.Properties.Resources.NewModifier;
			this.toolHider.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolHider.Name = "toolHider";
			this.toolHider.Size = new System.Drawing.Size(23, 22);
			this.toolHider.Text = "toolStripButton5";
			this.toolHider.ToolTipText = "New";
			this.toolHider.CheckedChanged += new System.EventHandler(this.toolHider_CheckedChanged);
			// 
			// toolFieldModifiers
			// 
			this.toolFieldModifiers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolFieldModifiers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFieldNone,
            this.toolReadonlyField,
            this.toolConst,
            this.toolVolatile});
			this.toolFieldModifiers.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFieldModifiers.Name = "toolFieldModifiers";
			this.toolFieldModifiers.Size = new System.Drawing.Size(45, 22);
			this.toolFieldModifiers.Text = "None";
			// 
			// toolFieldNone
			// 
			this.toolFieldNone.Name = "toolFieldNone";
			this.toolFieldNone.Size = new System.Drawing.Size(130, 22);
			this.toolFieldNone.Text = "None";
			this.toolFieldNone.Click += new System.EventHandler(this.toolFieldNone_Click);
			// 
			// toolReadonlyField
			// 
			this.toolReadonlyField.Name = "toolReadonlyField";
			this.toolReadonlyField.Size = new System.Drawing.Size(130, 22);
			this.toolReadonlyField.Text = "Readonly";
			this.toolReadonlyField.Click += new System.EventHandler(this.toolReadonlyField_Click);
			// 
			// toolConst
			// 
			this.toolConst.Name = "toolConst";
			this.toolConst.Size = new System.Drawing.Size(130, 22);
			this.toolConst.Text = "Const";
			this.toolConst.Click += new System.EventHandler(this.toolConst_Click);
			// 
			// toolVolatile
			// 
			this.toolVolatile.Name = "toolVolatile";
			this.toolVolatile.Size = new System.Drawing.Size(130, 22);
			this.toolVolatile.Text = "Volatile";
			this.toolVolatile.Click += new System.EventHandler(this.toolVolatile_Click);
			// 
			// toolOperationModifiers
			// 
			this.toolOperationModifiers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolOperationModifiers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolOperationNone,
            this.toolAbstract,
            this.toolVirtual,
            this.toolOverride,
            this.toolSealed,
            this.toolAbstractOverride});
			this.toolOperationModifiers.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOperationModifiers.Name = "toolOperationModifiers";
			this.toolOperationModifiers.Size = new System.Drawing.Size(45, 22);
			this.toolOperationModifiers.Text = "None";
			// 
			// toolOperationNone
			// 
			this.toolOperationNone.Name = "toolOperationNone";
			this.toolOperationNone.Size = new System.Drawing.Size(171, 22);
			this.toolOperationNone.Text = "None";
			this.toolOperationNone.Click += new System.EventHandler(this.toolOperationNone_Click);
			// 
			// toolAbstract
			// 
			this.toolAbstract.Name = "toolAbstract";
			this.toolAbstract.Size = new System.Drawing.Size(171, 22);
			this.toolAbstract.Text = "Abstract";
			this.toolAbstract.Click += new System.EventHandler(this.toolAbstract_Click);
			// 
			// toolVirtual
			// 
			this.toolVirtual.Name = "toolVirtual";
			this.toolVirtual.Size = new System.Drawing.Size(171, 22);
			this.toolVirtual.Text = "Virtual";
			this.toolVirtual.Click += new System.EventHandler(this.toolVirtual_Click);
			// 
			// toolOverride
			// 
			this.toolOverride.Name = "toolOverride";
			this.toolOverride.Size = new System.Drawing.Size(171, 22);
			this.toolOverride.Text = "Override";
			this.toolOverride.Click += new System.EventHandler(this.toolOverride_Click);
			// 
			// toolSealed
			// 
			this.toolSealed.Name = "toolSealed";
			this.toolSealed.Size = new System.Drawing.Size(171, 22);
			this.toolSealed.Text = "Sealed";
			this.toolSealed.Click += new System.EventHandler(this.toolSealed_Click);
			// 
			// toolAbstractOverride
			// 
			this.toolAbstractOverride.Name = "toolAbstractOverride";
			this.toolAbstractOverride.Size = new System.Drawing.Size(171, 22);
			this.toolAbstractOverride.Text = "Abstract Override";
			this.toolAbstractOverride.Click += new System.EventHandler(this.toolAbstractOverride_Click);
			// 
			// toolAccessor
			// 
			this.toolAccessor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAccessor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolReadWrite,
            this.toolReadOnly,
            this.toolWriteOnly});
			this.toolAccessor.Image = global::NClass.DiagramEditor.Properties.Resources.PublicProperty;
			this.toolAccessor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAccessor.Name = "toolAccessor";
			this.toolAccessor.Size = new System.Drawing.Size(29, 22);
			this.toolAccessor.Text = "toolStripDropDownButton3";
			this.toolAccessor.ToolTipText = "Read-Write";
			// 
			// toolReadWrite
			// 
			this.toolReadWrite.Image = global::NClass.DiagramEditor.Properties.Resources.PublicProperty;
			this.toolReadWrite.Name = "toolReadWrite";
			this.toolReadWrite.Size = new System.Drawing.Size(140, 22);
			this.toolReadWrite.Text = "Read-Write";
			this.toolReadWrite.Click += new System.EventHandler(this.toolReadWrite_Click);
			// 
			// toolReadOnly
			// 
			this.toolReadOnly.Image = global::NClass.DiagramEditor.Properties.Resources.PublicReadonly;
			this.toolReadOnly.Name = "toolReadOnly";
			this.toolReadOnly.Size = new System.Drawing.Size(140, 22);
			this.toolReadOnly.Text = "Read-only";
			this.toolReadOnly.Click += new System.EventHandler(this.toolReadOnly_Click);
			// 
			// toolWriteOnly
			// 
			this.toolWriteOnly.Image = global::NClass.DiagramEditor.Properties.Resources.PublicWriteonly;
			this.toolWriteOnly.Name = "toolWriteOnly";
			this.toolWriteOnly.Size = new System.Drawing.Size(140, 22);
			this.toolWriteOnly.Text = "Write-only";
			this.toolWriteOnly.Click += new System.EventHandler(this.toolWriteOnly_Click);
			// 
			// sepNewMember
			// 
			this.sepNewMember.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.sepNewMember.Name = "sepNewMember";
			this.sepNewMember.Size = new System.Drawing.Size(6, 25);
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
			this.toolNewMember.Text = "toolStripSplitButton1";
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
			// txtDeclaration
			// 
			this.txtDeclaration.AcceptsTab = true;
			this.txtDeclaration.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDeclaration.Location = new System.Drawing.Point(4, 28);
			this.txtDeclaration.Name = "txtDeclaration";
			this.txtDeclaration.Padding = new System.Windows.Forms.Padding(1);
			this.txtDeclaration.ReadOnly = false;
			this.txtDeclaration.SelectionStart = 0;
			this.txtDeclaration.Size = new System.Drawing.Size(322, 20);
			this.txtDeclaration.TabIndex = 4;
			this.txtDeclaration.TextChanged += new System.EventHandler(this.txtDeclaration_TextChanged);
			this.txtDeclaration.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeclaration_Validating);
			this.txtDeclaration.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeclaration_KeyDown);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// MemberEditor
			// 
			this.Controls.Add(this.txtDeclaration);
			this.Controls.Add(this.toolStrip);
			this.Name = "MemberEditor";
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
		private System.Windows.Forms.ToolStripMenuItem toolProtint;
		private System.Windows.Forms.ToolStripMenuItem toolInternal;
		private System.Windows.Forms.ToolStripMenuItem toolProtected;
		private System.Windows.Forms.ToolStripMenuItem toolPrivate;
		private System.Windows.Forms.ToolStripButton toolStatic;
		private System.Windows.Forms.ToolStripButton toolDelete;
		private System.Windows.Forms.ToolStripButton toolMoveDown;
		private System.Windows.Forms.ToolStripButton toolMoveUp;
		private System.Windows.Forms.ToolStripButton toolHider;
		private System.Windows.Forms.ToolStripDropDownButton toolOperationModifiers;
		private System.Windows.Forms.ToolStripMenuItem toolOperationNone;
		private System.Windows.Forms.ToolStripMenuItem toolAbstract;
		private System.Windows.Forms.ToolStripMenuItem toolVirtual;
		private System.Windows.Forms.ToolStripMenuItem toolOverride;
		private System.Windows.Forms.ToolStripMenuItem toolSealed;
		private System.Windows.Forms.ToolStripMenuItem toolAbstractOverride;
		private System.Windows.Forms.ToolStripDropDownButton toolAccessor;
		private System.Windows.Forms.ToolStripMenuItem toolReadWrite;
		private System.Windows.Forms.ToolStripMenuItem toolReadOnly;
		private System.Windows.Forms.ToolStripMenuItem toolWriteOnly;
		private System.Windows.Forms.ToolStripSeparator sepNewMember;
		private System.Windows.Forms.ToolStripSplitButton toolNewMember;
		private System.Windows.Forms.ToolStripMenuItem toolNewMethod;
		private System.Windows.Forms.ToolStripMenuItem toolNewField;
		private System.Windows.Forms.ToolStripMenuItem toolNewProperty;
		private BorderedTextBox txtDeclaration;
		private System.Windows.Forms.ToolStripMenuItem toolDefault;
		private System.Windows.Forms.ToolStripDropDownButton toolFieldModifiers;
		private System.Windows.Forms.ToolStripMenuItem toolFieldNone;
		private System.Windows.Forms.ToolStripMenuItem toolReadonlyField;
		private System.Windows.Forms.ToolStripMenuItem toolConst;
		private System.Windows.Forms.ToolStripMenuItem toolVolatile;
		private System.Windows.Forms.ToolStripMenuItem toolNewConstructor;
		private System.Windows.Forms.ToolStripMenuItem toolNewDestructor;
		private System.Windows.Forms.ToolStripMenuItem toolNewEvent;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}
