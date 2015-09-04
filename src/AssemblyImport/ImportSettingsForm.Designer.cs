using NClass.AssemblyImport.Controls;

namespace NClass.AssemblyImport
{
  partial class ImportSettingsForm
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.cmdOK = new System.Windows.Forms.Button();
      this.grpTemplate = new System.Windows.Forms.GroupBox();
      this.cmdLoadTemplate = new System.Windows.Forms.Button();
      this.cmdDeleteTemplate = new System.Windows.Forms.Button();
      this.cmdStoreTemplate = new System.Windows.Forms.Button();
      this.cboTemplate = new System.Windows.Forms.ComboBox();
      this.cmdCancel = new System.Windows.Forms.Button();
      this.grpAggregations = new System.Windows.Forms.GroupBox();
      this.chkCreateGeneralizations = new System.Windows.Forms.CheckBox();
      this.chkCreateRealizations = new System.Windows.Forms.CheckBox();
      this.chkCreateNesting = new System.Windows.Forms.CheckBox();
      this.chkCreateAssociations = new System.Windows.Forms.CheckBox();
      this.chkRemoveFields = new System.Windows.Forms.CheckBox();
      this.chkLabelAssociations = new System.Windows.Forms.CheckBox();
      this.chkCreateRelationships = new System.Windows.Forms.CheckBox();
      this.rdoWhiteList = new System.Windows.Forms.RadioButton();
      this.rdoBlackList = new System.Windows.Forms.RadioButton();
      this.grpFilter = new System.Windows.Forms.GroupBox();
      this.dgvFilter = new System.Windows.Forms.DataGridView();
      this.colFilterModifier = new DataGridViewImageComboBoxColumn();
      this.colFilterElement = new DataGridViewImageComboBoxColumn();
      this.grpTemplate.SuspendLayout();
      this.grpAggregations.SuspendLayout();
      this.grpFilter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
      this.SuspendLayout();
      // 
      // cmdOK
      // 
      this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.cmdOK.Location = new System.Drawing.Point(260, 476);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(75, 23);
      this.cmdOK.TabIndex = 3;
      this.cmdOK.Text = "OK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
      // 
      // grpTemplate
      // 
      this.grpTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpTemplate.Controls.Add(this.cmdLoadTemplate);
      this.grpTemplate.Controls.Add(this.cmdDeleteTemplate);
      this.grpTemplate.Controls.Add(this.cmdStoreTemplate);
      this.grpTemplate.Controls.Add(this.cboTemplate);
      this.grpTemplate.Location = new System.Drawing.Point(12, 12);
      this.grpTemplate.Name = "grpTemplate";
      this.grpTemplate.Size = new System.Drawing.Size(404, 50);
      this.grpTemplate.TabIndex = 8;
      this.grpTemplate.TabStop = false;
      this.grpTemplate.Text = "Template";
      // 
      // cmdLoadTemplate
      // 
      this.cmdLoadTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdLoadTemplate.Location = new System.Drawing.Point(149, 17);
      this.cmdLoadTemplate.Name = "cmdLoadTemplate";
      this.cmdLoadTemplate.Size = new System.Drawing.Size(75, 23);
      this.cmdLoadTemplate.TabIndex = 2;
      this.cmdLoadTemplate.Text = "Load";
      this.cmdLoadTemplate.UseVisualStyleBackColor = true;
      this.cmdLoadTemplate.Click += new System.EventHandler(this.cmdLoadTemplate_Click);
      // 
      // cmdDeleteTemplate
      // 
      this.cmdDeleteTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdDeleteTemplate.Location = new System.Drawing.Point(311, 17);
      this.cmdDeleteTemplate.Name = "cmdDeleteTemplate";
      this.cmdDeleteTemplate.Size = new System.Drawing.Size(87, 23);
      this.cmdDeleteTemplate.TabIndex = 1;
      this.cmdDeleteTemplate.Text = "Delete";
      this.cmdDeleteTemplate.UseVisualStyleBackColor = true;
      this.cmdDeleteTemplate.Click += new System.EventHandler(this.cmdDeleteTemplate_Click);
      // 
      // cmdStoreTemplate
      // 
      this.cmdStoreTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdStoreTemplate.Location = new System.Drawing.Point(230, 17);
      this.cmdStoreTemplate.Name = "cmdStoreTemplate";
      this.cmdStoreTemplate.Size = new System.Drawing.Size(75, 23);
      this.cmdStoreTemplate.TabIndex = 1;
      this.cmdStoreTemplate.Text = "Store";
      this.cmdStoreTemplate.UseVisualStyleBackColor = true;
      this.cmdStoreTemplate.Click += new System.EventHandler(this.cmdStoreTemplate_Click);
      // 
      // cboTemplate
      // 
      this.cboTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cboTemplate.FormattingEnabled = true;
      this.cboTemplate.Location = new System.Drawing.Point(6, 19);
      this.cboTemplate.Name = "cboTemplate";
      this.cboTemplate.Size = new System.Drawing.Size(137, 21);
      this.cboTemplate.TabIndex = 0;
      // 
      // cmdCancel
      // 
      this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(341, 476);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(75, 23);
      this.cmdCancel.TabIndex = 9;
      this.cmdCancel.Text = "Cancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      // 
      // grpAggregations
      // 
      this.grpAggregations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpAggregations.Controls.Add(this.chkCreateGeneralizations);
      this.grpAggregations.Controls.Add(this.chkCreateRealizations);
      this.grpAggregations.Controls.Add(this.chkCreateNesting);
      this.grpAggregations.Controls.Add(this.chkCreateAssociations);
      this.grpAggregations.Controls.Add(this.chkRemoveFields);
      this.grpAggregations.Controls.Add(this.chkLabelAssociations);
      this.grpAggregations.Controls.Add(this.chkCreateRelationships);
      this.grpAggregations.Location = new System.Drawing.Point(12, 294);
      this.grpAggregations.Name = "grpAggregations";
      this.grpAggregations.Size = new System.Drawing.Size(404, 176);
      this.grpAggregations.TabIndex = 10;
      this.grpAggregations.TabStop = false;
      // 
      // chkCreateGeneralizations
      // 
      this.chkCreateGeneralizations.AutoSize = true;
      this.chkCreateGeneralizations.Checked = true;
      this.chkCreateGeneralizations.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCreateGeneralizations.Location = new System.Drawing.Point(6, 151);
      this.chkCreateGeneralizations.Name = "chkCreateGeneralizations";
      this.chkCreateGeneralizations.Size = new System.Drawing.Size(132, 17);
      this.chkCreateGeneralizations.TabIndex = 2;
      this.chkCreateGeneralizations.Text = "Create Generalizations";
      this.chkCreateGeneralizations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.chkCreateGeneralizations.UseVisualStyleBackColor = true;
      // 
      // chkCreateRealizations
      // 
      this.chkCreateRealizations.AutoSize = true;
      this.chkCreateRealizations.Checked = true;
      this.chkCreateRealizations.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCreateRealizations.Location = new System.Drawing.Point(6, 128);
      this.chkCreateRealizations.Name = "chkCreateRealizations";
      this.chkCreateRealizations.Size = new System.Drawing.Size(117, 17);
      this.chkCreateRealizations.TabIndex = 2;
      this.chkCreateRealizations.Text = "Create Realizations";
      this.chkCreateRealizations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.chkCreateRealizations.UseVisualStyleBackColor = true;
      // 
      // chkCreateNesting
      // 
      this.chkCreateNesting.AutoSize = true;
      this.chkCreateNesting.Checked = true;
      this.chkCreateNesting.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCreateNesting.Location = new System.Drawing.Point(6, 105);
      this.chkCreateNesting.Name = "chkCreateNesting";
      this.chkCreateNesting.Size = new System.Drawing.Size(160, 17);
      this.chkCreateNesting.TabIndex = 2;
      this.chkCreateNesting.Text = "Create nesting Relationships";
      this.chkCreateNesting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.chkCreateNesting.UseVisualStyleBackColor = true;
      // 
      // chkCreateAssociations
      // 
      this.chkCreateAssociations.AutoSize = true;
      this.chkCreateAssociations.Checked = true;
      this.chkCreateAssociations.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCreateAssociations.Location = new System.Drawing.Point(6, 23);
      this.chkCreateAssociations.Name = "chkCreateAssociations";
      this.chkCreateAssociations.Size = new System.Drawing.Size(122, 17);
      this.chkCreateAssociations.TabIndex = 2;
      this.chkCreateAssociations.Text = "Create Aggregations";
      this.chkCreateAssociations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.chkCreateAssociations.UseVisualStyleBackColor = true;
      this.chkCreateAssociations.CheckedChanged += new System.EventHandler(this.chkCreateAssociations_CheckedChanged);
      // 
      // chkRemoveFields
      // 
      this.chkRemoveFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.chkRemoveFields.Location = new System.Drawing.Point(24, 69);
      this.chkRemoveFields.Name = "chkRemoveFields";
      this.chkRemoveFields.Size = new System.Drawing.Size(374, 30);
      this.chkRemoveFields.TabIndex = 1;
      this.chkRemoveFields.Text = "Remove fields which constructs the aggregation (code generation won\'t produce any" +
          " code for that aggregation)";
      this.chkRemoveFields.UseVisualStyleBackColor = true;
      // 
      // chkLabelAssociations
      // 
      this.chkLabelAssociations.Checked = true;
      this.chkLabelAssociations.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkLabelAssociations.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.chkLabelAssociations.Location = new System.Drawing.Point(24, 46);
      this.chkLabelAssociations.Name = "chkLabelAssociations";
      this.chkLabelAssociations.Size = new System.Drawing.Size(374, 17);
      this.chkLabelAssociations.TabIndex = 1;
      this.chkLabelAssociations.Text = "Create a label on the side of the aggregation";
      this.chkLabelAssociations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.chkLabelAssociations.UseVisualStyleBackColor = true;
      // 
      // chkCreateRelationships
      // 
      this.chkCreateRelationships.AutoSize = true;
      this.chkCreateRelationships.Checked = true;
      this.chkCreateRelationships.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCreateRelationships.Location = new System.Drawing.Point(6, 0);
      this.chkCreateRelationships.Name = "chkCreateRelationships";
      this.chkCreateRelationships.Size = new System.Drawing.Size(123, 17);
      this.chkCreateRelationships.TabIndex = 0;
      this.chkCreateRelationships.Text = "Create Relationships";
      this.chkCreateRelationships.UseVisualStyleBackColor = true;
      this.chkCreateRelationships.CheckedChanged += new System.EventHandler(this.chkCreateRelationships_CheckedChanged);
      // 
      // rdoWhiteList
      // 
      this.rdoWhiteList.AutoSize = true;
      this.rdoWhiteList.Checked = true;
      this.rdoWhiteList.Location = new System.Drawing.Point(6, 19);
      this.rdoWhiteList.Name = "rdoWhiteList";
      this.rdoWhiteList.Size = new System.Drawing.Size(136, 17);
      this.rdoWhiteList.TabIndex = 11;
      this.rdoWhiteList.TabStop = true;
      this.rdoWhiteList.Text = "Import only the folowing";
      this.rdoWhiteList.UseVisualStyleBackColor = true;
      // 
      // rdoBlackList
      // 
      this.rdoBlackList.AutoSize = true;
      this.rdoBlackList.Location = new System.Drawing.Point(148, 19);
      this.rdoBlackList.Name = "rdoBlackList";
      this.rdoBlackList.Size = new System.Drawing.Size(141, 17);
      this.rdoBlackList.TabIndex = 11;
      this.rdoBlackList.Text = "Import everything except";
      this.rdoBlackList.UseVisualStyleBackColor = true;
      // 
      // grpFilter
      // 
      this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpFilter.Controls.Add(this.rdoWhiteList);
      this.grpFilter.Controls.Add(this.rdoBlackList);
      this.grpFilter.Controls.Add(this.dgvFilter);
      this.grpFilter.Location = new System.Drawing.Point(12, 68);
      this.grpFilter.Name = "grpFilter";
      this.grpFilter.Size = new System.Drawing.Size(404, 220);
      this.grpFilter.TabIndex = 12;
      this.grpFilter.TabStop = false;
      this.grpFilter.Text = "Filter";
      // 
      // dgvFilter
      // 
      this.dgvFilter.AllowUserToResizeColumns = false;
      this.dgvFilter.AllowUserToResizeRows = false;
      this.dgvFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvFilter.BackgroundColor = System.Drawing.SystemColors.Control;
      this.dgvFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvFilter.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFilterModifier,
            this.colFilterElement});
      this.dgvFilter.GridColor = System.Drawing.SystemColors.Control;
      this.dgvFilter.Location = new System.Drawing.Point(7, 42);
      this.dgvFilter.MultiSelect = false;
      this.dgvFilter.Name = "dgvFilter";
      this.dgvFilter.RowHeadersVisible = false;
      this.dgvFilter.RowTemplate.Height = 26;
      this.dgvFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvFilter.Size = new System.Drawing.Size(391, 172);
      this.dgvFilter.TabIndex = 4;
      this.dgvFilter.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilter_CellValueChanged);
      this.dgvFilter.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvFilter_CurrentCellDirtyStateChanged);
      // 
      // colFilterModifier
      // 
      this.colFilterModifier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.colFilterModifier.HeaderText = "Modifier";
      this.colFilterModifier.ImageList = null;
      this.colFilterModifier.ImageSize = new System.Drawing.Size(0, 0);
      this.colFilterModifier.Name = "colFilterModifier";
      this.colFilterModifier.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      // 
      // colFilterElement
      // 
      this.colFilterElement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.colFilterElement.HeaderText = "Element";
      this.colFilterElement.ImageList = null;
      this.colFilterElement.ImageSize = new System.Drawing.Size(0, 0);
      this.colFilterElement.Name = "colFilterElement";
      this.colFilterElement.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      // 
      // ImportSettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(428, 511);
      this.Controls.Add(this.grpFilter);
      this.Controls.Add(this.grpAggregations);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this.grpTemplate);
      this.Controls.Add(this.cmdOK);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(444, 500);
      this.Name = "ImportSettingsForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import Settings";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportSettingsForm_FormClosed);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImportSettingsForm_KeyDown);
      this.grpTemplate.ResumeLayout(false);
      this.grpAggregations.ResumeLayout(false);
      this.grpAggregations.PerformLayout();
      this.grpFilter.ResumeLayout(false);
      this.grpFilter.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cmdOK;
    private System.Windows.Forms.DataGridView dgvFilter;
    private System.Windows.Forms.GroupBox grpTemplate;
    private System.Windows.Forms.Button cmdLoadTemplate;
    private System.Windows.Forms.Button cmdDeleteTemplate;
    private System.Windows.Forms.Button cmdStoreTemplate;
    private System.Windows.Forms.ComboBox cboTemplate;
    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.GroupBox grpAggregations;
    private System.Windows.Forms.CheckBox chkRemoveFields;
    private System.Windows.Forms.CheckBox chkLabelAssociations;
    private System.Windows.Forms.CheckBox chkCreateRelationships;
    private System.Windows.Forms.CheckBox chkCreateGeneralizations;
    private System.Windows.Forms.CheckBox chkCreateRealizations;
    private System.Windows.Forms.CheckBox chkCreateNesting;
    private System.Windows.Forms.CheckBox chkCreateAssociations;
    private System.Windows.Forms.RadioButton rdoWhiteList;
    private System.Windows.Forms.RadioButton rdoBlackList;
    private System.Windows.Forms.GroupBox grpFilter;
    private DataGridViewImageComboBoxColumn colFilterModifier;
    private DataGridViewImageComboBoxColumn colFilterElement;
  }
}