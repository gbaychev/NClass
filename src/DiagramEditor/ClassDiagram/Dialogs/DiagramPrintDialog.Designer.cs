namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	partial class DiagramPrintDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
			this.printDocument = new System.Drawing.Printing.PrintDocument();
			this.chkSelectedOnly = new System.Windows.Forms.CheckBox();
			this.lblPages = new System.Windows.Forms.Label();
			this.lblStyle = new System.Windows.Forms.Label();
			this.selectPrinterDialog = new System.Windows.Forms.PrintDialog();
			this.cboStyle = new System.Windows.Forms.ComboBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnPrinter = new System.Windows.Forms.Button();
			this.lblX = new System.Windows.Forms.Label();
			this.numColumns = new System.Windows.Forms.NumericUpDown();
			this.numRows = new System.Windows.Forms.NumericUpDown();
			this.btnPageSetup = new System.Windows.Forms.Button();
			this.printPreview = new System.Windows.Forms.PrintPreviewControl();
			((System.ComponentModel.ISupportInitialize) (this.numColumns)).BeginInit();
			((System.ComponentModel.ISupportInitialize) (this.numRows)).BeginInit();
			this.SuspendLayout();
			// 
			// pageSetupDialog
			// 
			this.pageSetupDialog.Document = this.printDocument;
			// 
			// printDocument
			// 
			this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
			this.printDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_EndPrint);
			this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
			// 
			// chkSelectedOnly
			// 
			this.chkSelectedOnly.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkSelectedOnly.AutoEllipsis = true;
			this.chkSelectedOnly.Location = new System.Drawing.Point(288, 536);
			this.chkSelectedOnly.Name = "chkSelectedOnly";
			this.chkSelectedOnly.Size = new System.Drawing.Size(230, 18);
			this.chkSelectedOnly.TabIndex = 9;
			this.chkSelectedOnly.Text = "Print only the selected elements";
			this.chkSelectedOnly.UseVisualStyleBackColor = true;
			this.chkSelectedOnly.CheckedChanged += new System.EventHandler(this.chkSelectedOnly_CheckedChanged);
			// 
			// lblPages
			// 
			this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPages.AutoSize = true;
			this.lblPages.Location = new System.Drawing.Point(146, 536);
			this.lblPages.Name = "lblPages";
			this.lblPages.Size = new System.Drawing.Size(40, 13);
			this.lblPages.TabIndex = 5;
			this.lblPages.Text = "Pages:";
			// 
			// lblStyle
			// 
			this.lblStyle.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStyle.AutoSize = true;
			this.lblStyle.Location = new System.Drawing.Point(146, 507);
			this.lblStyle.Name = "lblStyle";
			this.lblStyle.Size = new System.Drawing.Size(33, 13);
			this.lblStyle.TabIndex = 3;
			this.lblStyle.Text = "Style:";
			// 
			// selectPrinterDialog
			// 
			this.selectPrinterDialog.AllowPrintToFile = false;
			this.selectPrinterDialog.Document = this.printDocument;
			this.selectPrinterDialog.UseEXDialog = true;
			// 
			// cboStyle
			// 
			this.cboStyle.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cboStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStyle.FormattingEnabled = true;
			this.cboStyle.Location = new System.Drawing.Point(192, 504);
			this.cboStyle.Name = "cboStyle";
			this.cboStyle.Size = new System.Drawing.Size(217, 21);
			this.cboStyle.TabIndex = 4;
			this.cboStyle.SelectedIndexChanged += new System.EventHandler(this.cboStyle_SelectedIndexChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(605, 531);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnPrint.Location = new System.Drawing.Point(524, 531);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 10;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnPrinter
			// 
			this.btnPrinter.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPrinter.AutoSize = true;
			this.btnPrinter.Location = new System.Drawing.Point(12, 502);
			this.btnPrinter.Name = "btnPrinter";
			this.btnPrinter.Size = new System.Drawing.Size(128, 23);
			this.btnPrinter.TabIndex = 1;
			this.btnPrinter.Text = "Select printer...";
			this.btnPrinter.UseVisualStyleBackColor = true;
			this.btnPrinter.Click += new System.EventHandler(this.btnPrinter_Click);
			// 
			// lblX
			// 
			this.lblX.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblX.AutoSize = true;
			this.lblX.Location = new System.Drawing.Point(227, 536);
			this.lblX.Name = "lblX";
			this.lblX.Size = new System.Drawing.Size(12, 13);
			this.lblX.TabIndex = 7;
			this.lblX.Text = "x";
			// 
			// numColumns
			// 
			this.numColumns.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numColumns.Location = new System.Drawing.Point(192, 534);
			this.numColumns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numColumns.Name = "numColumns";
			this.numColumns.Size = new System.Drawing.Size(34, 20);
			this.numColumns.TabIndex = 8;
			this.numColumns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numColumns.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numColumns.ValueChanged += new System.EventHandler(this.numColumns_ValueChanged);
			// 
			// numRows
			// 
			this.numRows.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numRows.Location = new System.Drawing.Point(240, 534);
			this.numRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRows.Name = "numRows";
			this.numRows.Size = new System.Drawing.Size(34, 20);
			this.numRows.TabIndex = 6;
			this.numRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRows.ValueChanged += new System.EventHandler(this.numRows_ValueChanged);
			// 
			// btnPageSetup
			// 
			this.btnPageSetup.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPageSetup.AutoSize = true;
			this.btnPageSetup.Location = new System.Drawing.Point(12, 531);
			this.btnPageSetup.Name = "btnPageSetup";
			this.btnPageSetup.Size = new System.Drawing.Size(128, 23);
			this.btnPageSetup.TabIndex = 2;
			this.btnPageSetup.Text = "Page setup...";
			this.btnPageSetup.UseVisualStyleBackColor = true;
			this.btnPageSetup.Click += new System.EventHandler(this.btnPageSetup_Click);
			// 
			// printPreview
			// 
			this.printPreview.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.printPreview.AutoZoom = false;
			this.printPreview.Document = this.printDocument;
			this.printPreview.Location = new System.Drawing.Point(12, 12);
			this.printPreview.Name = "printPreview";
			this.printPreview.Size = new System.Drawing.Size(668, 477);
			this.printPreview.TabIndex = 0;
			this.printPreview.UseAntiAlias = true;
			this.printPreview.Zoom = 0.40718562874251496;
			this.printPreview.Click += new System.EventHandler(this.printPreview_Click);
			// 
			// DiagramPrintDialog
			// 
			this.AcceptButton = this.btnPrint;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(692, 566);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.chkSelectedOnly);
			this.Controls.Add(this.lblPages);
			this.Controls.Add(this.lblStyle);
			this.Controls.Add(this.cboStyle);
			this.Controls.Add(this.btnPrinter);
			this.Controls.Add(this.lblX);
			this.Controls.Add(this.numColumns);
			this.Controls.Add(this.numRows);
			this.Controls.Add(this.btnPageSetup);
			this.Controls.Add(this.printPreview);
			this.Name = "DiagramPrintDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Print";
			((System.ComponentModel.ISupportInitialize) (this.numColumns)).EndInit();
			((System.ComponentModel.ISupportInitialize) (this.numRows)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PageSetupDialog pageSetupDialog;
		private System.Drawing.Printing.PrintDocument printDocument;
		private System.Windows.Forms.CheckBox chkSelectedOnly;
		private System.Windows.Forms.Label lblPages;
		private System.Windows.Forms.Label lblStyle;
		private System.Windows.Forms.PrintDialog selectPrinterDialog;
		private System.Windows.Forms.ComboBox cboStyle;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnPrinter;
		private System.Windows.Forms.Label lblX;
		private System.Windows.Forms.NumericUpDown numColumns;
		private System.Windows.Forms.NumericUpDown numRows;
		private System.Windows.Forms.Button btnPageSetup;
		private System.Windows.Forms.PrintPreviewControl printPreview;
	}
}