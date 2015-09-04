namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	partial class MembersDialog
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
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.txtSyntax = new System.Windows.Forms.TextBox();
			this.lblSyntax = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblType = new System.Windows.Forms.Label();
			this.lblAccess = new System.Windows.Forms.Label();
			this.cboAccess = new System.Windows.Forms.ComboBox();
			this.txtInitialValue = new System.Windows.Forms.TextBox();
			this.lblInitValue = new System.Windows.Forms.Label();
			this.lstMembers = new System.Windows.Forms.ListView();
			this.icon = new System.Windows.Forms.ColumnHeader();
			this.name = new System.Windows.Forms.ColumnHeader();
			this.type = new System.Windows.Forms.ColumnHeader();
			this.access = new System.Windows.Forms.ColumnHeader();
			this.modifier = new System.Windows.Forms.ColumnHeader();
			this.cboType = new System.Windows.Forms.ComboBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolNewField = new System.Windows.Forms.ToolStripButton();
			this.toolNewMethod = new System.Windows.Forms.ToolStripButton();
			this.toolNewConstructor = new System.Windows.Forms.ToolStripButton();
			this.toolNewDestructor = new System.Windows.Forms.ToolStripButton();
			this.toolNewProperty = new System.Windows.Forms.ToolStripButton();
			this.toolNewEvent = new System.Windows.Forms.ToolStripButton();
			this.toolDelete = new System.Windows.Forms.ToolStripButton();
			this.toolSepMoving = new System.Windows.Forms.ToolStripSeparator();
			this.toolMoveDown = new System.Windows.Forms.ToolStripButton();
			this.toolMoveUp = new System.Windows.Forms.ToolStripButton();
			this.toolSepSorting = new System.Windows.Forms.ToolStripSeparator();
			this.toolSortByName = new System.Windows.Forms.ToolStripButton();
			this.toolSortByAccess = new System.Windows.Forms.ToolStripButton();
			this.toolSortByKind = new System.Windows.Forms.ToolStripButton();
			this.toolSepAddNew = new System.Windows.Forms.ToolStripSeparator();
			this.toolOverrideList = new System.Windows.Forms.ToolStripButton();
			this.toolImplementList = new System.Windows.Forms.ToolStripButton();
			this.grpFieldModifiers = new System.Windows.Forms.GroupBox();
			this.chkVolatile = new System.Windows.Forms.CheckBox();
			this.chkFieldHider = new System.Windows.Forms.CheckBox();
			this.chkConstant = new System.Windows.Forms.CheckBox();
			this.chkReadonly = new System.Windows.Forms.CheckBox();
			this.chkFieldStatic = new System.Windows.Forms.CheckBox();
			this.chkOperationStatic = new System.Windows.Forms.CheckBox();
			this.chkVirtual = new System.Windows.Forms.CheckBox();
			this.chkAbstract = new System.Windows.Forms.CheckBox();
			this.chkOverride = new System.Windows.Forms.CheckBox();
			this.chkSealed = new System.Windows.Forms.CheckBox();
			this.chkOperationHider = new System.Windows.Forms.CheckBox();
			this.grpOperationModifiers = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
			this.toolStrip.SuspendLayout();
			this.grpFieldModifiers.SuspendLayout();
			this.grpOperationModifiers.SuspendLayout();
			this.SuspendLayout();
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// txtSyntax
			// 
			this.txtSyntax.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSyntax.Location = new System.Drawing.Point(71, 12);
			this.txtSyntax.Name = "txtSyntax";
			this.txtSyntax.Size = new System.Drawing.Size(367, 20);
			this.txtSyntax.TabIndex = 0;
			this.txtSyntax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSyntax_KeyDown);
			this.txtSyntax.Validating += new System.ComponentModel.CancelEventHandler(this.txtSyntax_Validating);
			// 
			// lblSyntax
			// 
			this.lblSyntax.Location = new System.Drawing.Point(0, 15);
			this.lblSyntax.Name = "lblSyntax";
			this.lblSyntax.Size = new System.Drawing.Size(65, 13);
			this.lblSyntax.TabIndex = 10;
			this.lblSyntax.Text = "Syntax";
			this.lblSyntax.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(0, 47);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(65, 13);
			this.lblName.TabIndex = 11;
			this.lblName.Text = "Name";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(71, 44);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(187, 20);
			this.txtName.TabIndex = 1;
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
			// 
			// lblType
			// 
			this.lblType.Location = new System.Drawing.Point(0, 77);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(65, 13);
			this.lblType.TabIndex = 12;
			this.lblType.Text = "Type";
			this.lblType.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblAccess
			// 
			this.lblAccess.Location = new System.Drawing.Point(0, 107);
			this.lblAccess.Name = "lblAccess";
			this.lblAccess.Size = new System.Drawing.Size(65, 13);
			this.lblAccess.TabIndex = 13;
			this.lblAccess.Text = "Access";
			this.lblAccess.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cboAccess
			// 
			this.cboAccess.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAccess.FormattingEnabled = true;
			this.cboAccess.Items.AddRange(new object[] {
            "Default",
            "Public",
            "Protected Internal",
            "Internal",
            "Protected",
            "Private"});
			this.cboAccess.Location = new System.Drawing.Point(71, 104);
			this.cboAccess.Name = "cboAccess";
			this.cboAccess.Size = new System.Drawing.Size(187, 21);
			this.cboAccess.TabIndex = 3;
			this.cboAccess.SelectedIndexChanged += new System.EventHandler(this.cboAccess_SelectedIndexChanged);
			this.cboAccess.Validated += new System.EventHandler(this.cboAccess_Validated);
			// 
			// txtInitialValue
			// 
			this.txtInitialValue.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtInitialValue.Location = new System.Drawing.Point(71, 134);
			this.txtInitialValue.Name = "txtInitialValue";
			this.txtInitialValue.Size = new System.Drawing.Size(367, 20);
			this.txtInitialValue.TabIndex = 4;
			this.txtInitialValue.Validating += new System.ComponentModel.CancelEventHandler(this.txtInitialValue_Validating);
			// 
			// lblInitValue
			// 
			this.lblInitValue.Location = new System.Drawing.Point(0, 137);
			this.lblInitValue.Name = "lblInitValue";
			this.lblInitValue.Size = new System.Drawing.Size(65, 13);
			this.lblInitValue.TabIndex = 14;
			this.lblInitValue.Text = "Initial value";
			this.lblInitValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lstMembers
			// 
			this.lstMembers.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstMembers.AutoArrange = false;
			this.lstMembers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.icon,
            this.name,
            this.type,
            this.access,
            this.modifier});
			this.lstMembers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
			this.lstMembers.FullRowSelect = true;
			this.lstMembers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstMembers.HideSelection = false;
			this.lstMembers.Location = new System.Drawing.Point(12, 200);
			this.lstMembers.MultiSelect = false;
			this.lstMembers.Name = "lstMembers";
			this.lstMembers.ShowGroups = false;
			this.lstMembers.Size = new System.Drawing.Size(445, 255);
			this.lstMembers.TabIndex = 8;
			this.lstMembers.UseCompatibleStateImageBehavior = false;
			this.lstMembers.View = System.Windows.Forms.View.Details;
			this.lstMembers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstMembers_ItemSelectionChanged);
			this.lstMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstMembers_KeyDown);
			// 
			// icon
			// 
			this.icon.Text = "";
			this.icon.Width = 20;
			// 
			// name
			// 
			this.name.Text = "Name";
			this.name.Width = 119;
			// 
			// type
			// 
			this.type.Text = "Type";
			this.type.Width = 101;
			// 
			// access
			// 
			this.access.Text = "Access";
			this.access.Width = 102;
			// 
			// modifier
			// 
			this.modifier.Text = "Modifier";
			this.modifier.Width = 99;
			// 
			// cboType
			// 
			this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboType.FormattingEnabled = true;
			this.cboType.Location = new System.Drawing.Point(71, 74);
			this.cboType.Name = "cboType";
			this.cboType.Size = new System.Drawing.Size(187, 21);
			this.cboType.Sorted = true;
			this.cboType.TabIndex = 2;
			this.cboType.Validating += new System.ComponentModel.CancelEventHandler(this.cboType_Validating);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(382, 461);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 9;
			this.btnClose.Text = "Bezár";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.toolStrip.AutoSize = false;
			this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewField,
            this.toolNewMethod,
            this.toolNewConstructor,
            this.toolNewDestructor,
            this.toolNewProperty,
            this.toolNewEvent,
            this.toolDelete,
            this.toolSepMoving,
            this.toolMoveDown,
            this.toolMoveUp,
            this.toolSepSorting,
            this.toolSortByName,
            this.toolSortByAccess,
            this.toolSortByKind,
            this.toolSepAddNew,
            this.toolOverrideList,
            this.toolImplementList});
			this.toolStrip.Location = new System.Drawing.Point(12, 175);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip.Size = new System.Drawing.Size(445, 25);
			this.toolStrip.TabIndex = 7;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolNewField
			// 
			this.toolNewField.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewField.Image = global::NClass.DiagramEditor.Properties.Resources.Field;
			this.toolNewField.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewField.Name = "toolNewField";
			this.toolNewField.Size = new System.Drawing.Size(23, 22);
			this.toolNewField.Text = "New Field";
			this.toolNewField.Click += new System.EventHandler(this.toolNewField_Click);
			// 
			// toolNewMethod
			// 
			this.toolNewMethod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewMethod.Image = global::NClass.DiagramEditor.Properties.Resources.Method;
			this.toolNewMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewMethod.Name = "toolNewMethod";
			this.toolNewMethod.Size = new System.Drawing.Size(23, 22);
			this.toolNewMethod.Text = "New Method";
			this.toolNewMethod.Click += new System.EventHandler(this.toolNewMethod_Click);
			// 
			// toolNewConstructor
			// 
			this.toolNewConstructor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewConstructor.Image = global::NClass.DiagramEditor.Properties.Resources.Constructor;
			this.toolNewConstructor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewConstructor.Name = "toolNewConstructor";
			this.toolNewConstructor.Size = new System.Drawing.Size(23, 22);
			this.toolNewConstructor.Text = "New Constructor";
			this.toolNewConstructor.Click += new System.EventHandler(this.toolNewConstructor_Click);
			// 
			// toolNewDestructor
			// 
			this.toolNewDestructor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewDestructor.Image = global::NClass.DiagramEditor.Properties.Resources.Destructor;
			this.toolNewDestructor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewDestructor.Name = "toolNewDestructor";
			this.toolNewDestructor.Size = new System.Drawing.Size(23, 22);
			this.toolNewDestructor.Text = "New Destructor";
			this.toolNewDestructor.Click += new System.EventHandler(this.toolNewDestructor_Click);
			// 
			// toolNewProperty
			// 
			this.toolNewProperty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewProperty.Image = global::NClass.DiagramEditor.Properties.Resources.Property;
			this.toolNewProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewProperty.Name = "toolNewProperty";
			this.toolNewProperty.Size = new System.Drawing.Size(23, 22);
			this.toolNewProperty.Text = "New Property";
			this.toolNewProperty.Click += new System.EventHandler(this.toolNewProperty_Click);
			// 
			// toolNewEvent
			// 
			this.toolNewEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolNewEvent.Image = global::NClass.DiagramEditor.Properties.Resources.Event;
			this.toolNewEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolNewEvent.Name = "toolNewEvent";
			this.toolNewEvent.Size = new System.Drawing.Size(23, 22);
			this.toolNewEvent.Text = "New Event";
			this.toolNewEvent.Click += new System.EventHandler(this.toolNewEvent_Click);
			// 
			// toolDelete
			// 
			this.toolDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDelete.Image = global::NClass.DiagramEditor.Properties.Resources.Delete;
			this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDelete.Name = "toolDelete";
			this.toolDelete.Size = new System.Drawing.Size(23, 22);
			this.toolDelete.Text = "Delete";
			this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);
			// 
			// toolSepMoving
			// 
			this.toolSepMoving.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSepMoving.Name = "toolSepMoving";
			this.toolSepMoving.Size = new System.Drawing.Size(6, 25);
			// 
			// toolMoveDown
			// 
			this.toolMoveDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolMoveDown.Image = global::NClass.DiagramEditor.Properties.Resources.MoveDown;
			this.toolMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolMoveDown.Name = "toolMoveDown";
			this.toolMoveDown.Size = new System.Drawing.Size(23, 22);
			this.toolMoveDown.Text = "Move Down";
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
			this.toolMoveUp.Text = "Move Up";
			this.toolMoveUp.Click += new System.EventHandler(this.toolMoveUp_Click);
			// 
			// toolSepSorting
			// 
			this.toolSepSorting.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolSepSorting.Name = "toolSepSorting";
			this.toolSepSorting.Size = new System.Drawing.Size(6, 25);
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
			// toolSepAddNew
			// 
			this.toolSepAddNew.Name = "toolSepAddNew";
			this.toolSepAddNew.Size = new System.Drawing.Size(6, 25);
			// 
			// toolOverrideList
			// 
			this.toolOverrideList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOverrideList.Image = global::NClass.DiagramEditor.Properties.Resources.Overrides;
			this.toolOverrideList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOverrideList.Name = "toolOverrideList";
			this.toolOverrideList.Size = new System.Drawing.Size(23, 22);
			this.toolOverrideList.Text = "Override List";
			this.toolOverrideList.Click += new System.EventHandler(this.toolOverrideList_Click);
			// 
			// toolImplementList
			// 
			this.toolImplementList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolImplementList.Image = global::NClass.DiagramEditor.Properties.Resources.Implements;
			this.toolImplementList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolImplementList.Name = "toolImplementList";
			this.toolImplementList.Size = new System.Drawing.Size(23, 22);
			this.toolImplementList.Text = "Interface Implement List";
			this.toolImplementList.Click += new System.EventHandler(this.toolImplementList_Click);
			// 
			// grpFieldModifiers
			// 
			this.grpFieldModifiers.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.grpFieldModifiers.Controls.Add(this.chkVolatile);
			this.grpFieldModifiers.Controls.Add(this.chkFieldHider);
			this.grpFieldModifiers.Controls.Add(this.chkConstant);
			this.grpFieldModifiers.Controls.Add(this.chkReadonly);
			this.grpFieldModifiers.Controls.Add(this.chkFieldStatic);
			this.grpFieldModifiers.Location = new System.Drawing.Point(280, 37);
			this.grpFieldModifiers.Name = "grpFieldModifiers";
			this.grpFieldModifiers.Size = new System.Drawing.Size(158, 88);
			this.grpFieldModifiers.TabIndex = 6;
			this.grpFieldModifiers.TabStop = false;
			this.grpFieldModifiers.Text = "Modifiers";
			this.grpFieldModifiers.Validated += new System.EventHandler(this.grpFieldModifiers_Validated);
			// 
			// chkVolatile
			// 
			this.chkVolatile.AutoSize = true;
			this.chkVolatile.Location = new System.Drawing.Point(88, 42);
			this.chkVolatile.Name = "chkVolatile";
			this.chkVolatile.Size = new System.Drawing.Size(60, 17);
			this.chkVolatile.TabIndex = 4;
			this.chkVolatile.Text = "Volatile";
			this.chkVolatile.UseVisualStyleBackColor = true;
			this.chkVolatile.CheckedChanged += new System.EventHandler(this.chkVolatile_CheckedChanged);
			// 
			// chkFieldHider
			// 
			this.chkFieldHider.AutoSize = true;
			this.chkFieldHider.Location = new System.Drawing.Point(88, 19);
			this.chkFieldHider.Name = "chkFieldHider";
			this.chkFieldHider.Size = new System.Drawing.Size(48, 17);
			this.chkFieldHider.TabIndex = 3;
			this.chkFieldHider.Text = "New";
			this.chkFieldHider.UseVisualStyleBackColor = true;
			this.chkFieldHider.CheckedChanged += new System.EventHandler(this.chkFieldHider_CheckedChanged);
			// 
			// chkConstant
			// 
			this.chkConstant.AutoSize = true;
			this.chkConstant.Location = new System.Drawing.Point(11, 65);
			this.chkConstant.Name = "chkConstant";
			this.chkConstant.Size = new System.Drawing.Size(53, 17);
			this.chkConstant.TabIndex = 2;
			this.chkConstant.Text = "Const";
			this.chkConstant.UseVisualStyleBackColor = true;
			this.chkConstant.CheckedChanged += new System.EventHandler(this.chkConstant_CheckedChanged);
			// 
			// chkReadonly
			// 
			this.chkReadonly.AutoSize = true;
			this.chkReadonly.Location = new System.Drawing.Point(11, 42);
			this.chkReadonly.Name = "chkReadonly";
			this.chkReadonly.Size = new System.Drawing.Size(71, 17);
			this.chkReadonly.TabIndex = 1;
			this.chkReadonly.Text = "Readonly";
			this.chkReadonly.UseVisualStyleBackColor = true;
			this.chkReadonly.CheckedChanged += new System.EventHandler(this.chkReadonly_CheckedChanged);
			// 
			// chkFieldStatic
			// 
			this.chkFieldStatic.AutoSize = true;
			this.chkFieldStatic.Location = new System.Drawing.Point(11, 19);
			this.chkFieldStatic.Name = "chkFieldStatic";
			this.chkFieldStatic.Size = new System.Drawing.Size(53, 17);
			this.chkFieldStatic.TabIndex = 0;
			this.chkFieldStatic.Text = "Static";
			this.chkFieldStatic.UseVisualStyleBackColor = true;
			this.chkFieldStatic.CheckedChanged += new System.EventHandler(this.chkFieldStatic_CheckedChanged);
			// 
			// chkOperationStatic
			// 
			this.chkOperationStatic.AutoSize = true;
			this.chkOperationStatic.Location = new System.Drawing.Point(11, 19);
			this.chkOperationStatic.Name = "chkOperationStatic";
			this.chkOperationStatic.Size = new System.Drawing.Size(53, 17);
			this.chkOperationStatic.TabIndex = 0;
			this.chkOperationStatic.Text = "Static";
			this.chkOperationStatic.UseVisualStyleBackColor = true;
			this.chkOperationStatic.CheckedChanged += new System.EventHandler(this.chkOperationStatic_CheckedChanged);
			// 
			// chkVirtual
			// 
			this.chkVirtual.AutoSize = true;
			this.chkVirtual.Location = new System.Drawing.Point(11, 42);
			this.chkVirtual.Name = "chkVirtual";
			this.chkVirtual.Size = new System.Drawing.Size(55, 17);
			this.chkVirtual.TabIndex = 1;
			this.chkVirtual.Text = "Virtual";
			this.chkVirtual.UseVisualStyleBackColor = true;
			this.chkVirtual.CheckedChanged += new System.EventHandler(this.chkVirtual_CheckedChanged);
			// 
			// chkAbstract
			// 
			this.chkAbstract.AutoSize = true;
			this.chkAbstract.Location = new System.Drawing.Point(11, 65);
			this.chkAbstract.Name = "chkAbstract";
			this.chkAbstract.Size = new System.Drawing.Size(65, 17);
			this.chkAbstract.TabIndex = 2;
			this.chkAbstract.Text = "Abstract";
			this.chkAbstract.UseVisualStyleBackColor = true;
			this.chkAbstract.CheckedChanged += new System.EventHandler(this.chkAbstract_CheckedChanged);
			// 
			// chkOverride
			// 
			this.chkOverride.AutoSize = true;
			this.chkOverride.Location = new System.Drawing.Point(88, 42);
			this.chkOverride.Name = "chkOverride";
			this.chkOverride.Size = new System.Drawing.Size(66, 17);
			this.chkOverride.TabIndex = 3;
			this.chkOverride.Text = "Override";
			this.chkOverride.UseVisualStyleBackColor = true;
			this.chkOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
			// 
			// chkSealed
			// 
			this.chkSealed.AutoSize = true;
			this.chkSealed.Location = new System.Drawing.Point(88, 65);
			this.chkSealed.Name = "chkSealed";
			this.chkSealed.Size = new System.Drawing.Size(59, 17);
			this.chkSealed.TabIndex = 4;
			this.chkSealed.Text = "Sealed";
			this.chkSealed.UseVisualStyleBackColor = true;
			this.chkSealed.CheckedChanged += new System.EventHandler(this.chkSealed_CheckedChanged);
			// 
			// chkOperationHider
			// 
			this.chkOperationHider.AutoSize = true;
			this.chkOperationHider.Location = new System.Drawing.Point(88, 19);
			this.chkOperationHider.Name = "chkOperationHider";
			this.chkOperationHider.Size = new System.Drawing.Size(48, 17);
			this.chkOperationHider.TabIndex = 6;
			this.chkOperationHider.Text = "New";
			this.chkOperationHider.UseVisualStyleBackColor = true;
			this.chkOperationHider.CheckedChanged += new System.EventHandler(this.chkOperationHider_CheckedChanged);
			// 
			// grpOperationModifiers
			// 
			this.grpOperationModifiers.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.grpOperationModifiers.Controls.Add(this.chkOperationHider);
			this.grpOperationModifiers.Controls.Add(this.chkSealed);
			this.grpOperationModifiers.Controls.Add(this.chkOverride);
			this.grpOperationModifiers.Controls.Add(this.chkAbstract);
			this.grpOperationModifiers.Controls.Add(this.chkVirtual);
			this.grpOperationModifiers.Controls.Add(this.chkOperationStatic);
			this.grpOperationModifiers.Location = new System.Drawing.Point(280, 37);
			this.grpOperationModifiers.Name = "grpOperationModifiers";
			this.grpOperationModifiers.Size = new System.Drawing.Size(158, 88);
			this.grpOperationModifiers.TabIndex = 5;
			this.grpOperationModifiers.TabStop = false;
			this.grpOperationModifiers.Text = "Modifiers";
			this.grpOperationModifiers.Validated += new System.EventHandler(this.grpOperationModifiers_Validated);
			// 
			// MembersDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(469, 496);
			this.Controls.Add(this.lstMembers);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.cboType);
			this.Controls.Add(this.txtInitialValue);
			this.Controls.Add(this.lblInitValue);
			this.Controls.Add(this.cboAccess);
			this.Controls.Add(this.lblAccess);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.lblSyntax);
			this.Controls.Add(this.txtSyntax);
			this.Controls.Add(this.grpOperationModifiers);
			this.Controls.Add(this.grpFieldModifiers);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 450);
			this.Name = "MembersDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Members";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PropertiesDialog_KeyDown);
			((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.grpFieldModifiers.ResumeLayout(false);
			this.grpFieldModifiers.PerformLayout();
			this.grpOperationModifiers.ResumeLayout(false);
			this.grpOperationModifiers.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label lblSyntax;
		private System.Windows.Forms.TextBox txtSyntax;
		private System.Windows.Forms.ComboBox cboAccess;
		private System.Windows.Forms.Label lblAccess;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtInitialValue;
		private System.Windows.Forms.Label lblInitValue;
		private System.Windows.Forms.ListView lstMembers;
		private System.Windows.Forms.ColumnHeader name;
		private System.Windows.Forms.ColumnHeader type;
		private System.Windows.Forms.ColumnHeader access;
		private System.Windows.Forms.ComboBox cboType;
		private System.Windows.Forms.ColumnHeader icon;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ColumnHeader modifier;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton toolNewField;
		private System.Windows.Forms.ToolStripButton toolNewMethod;
		private System.Windows.Forms.ToolStripButton toolNewProperty;
		private System.Windows.Forms.ToolStripButton toolNewConstructor;
		private System.Windows.Forms.ToolStripButton toolNewDestructor;
		private System.Windows.Forms.ToolStripButton toolNewEvent;
		private System.Windows.Forms.ToolStripButton toolDelete;
		private System.Windows.Forms.ToolStripButton toolMoveUp;
		private System.Windows.Forms.ToolStripButton toolMoveDown;
		private System.Windows.Forms.ToolStripButton toolOverrideList;
		private System.Windows.Forms.ToolStripButton toolImplementList;
		private System.Windows.Forms.ToolStripSeparator toolSepMoving;
		private System.Windows.Forms.ToolStripSeparator toolSepSorting;
		private System.Windows.Forms.ToolStripButton toolSortByName;
		private System.Windows.Forms.ToolStripButton toolSortByAccess;
		private System.Windows.Forms.ToolStripButton toolSortByKind;
		private System.Windows.Forms.ToolStripSeparator toolSepAddNew;
		private System.Windows.Forms.GroupBox grpFieldModifiers;
		private System.Windows.Forms.CheckBox chkVolatile;
		private System.Windows.Forms.CheckBox chkFieldHider;
		private System.Windows.Forms.CheckBox chkConstant;
		private System.Windows.Forms.CheckBox chkReadonly;
		private System.Windows.Forms.CheckBox chkFieldStatic;
		private System.Windows.Forms.GroupBox grpOperationModifiers;
		private System.Windows.Forms.CheckBox chkOperationHider;
		private System.Windows.Forms.CheckBox chkSealed;
		private System.Windows.Forms.CheckBox chkOverride;
		private System.Windows.Forms.CheckBox chkAbstract;
		private System.Windows.Forms.CheckBox chkVirtual;
		private System.Windows.Forms.CheckBox chkOperationStatic;
	}
}

