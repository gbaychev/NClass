namespace PDFExport
{
  partial class PDFExportFinished
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
      if(disposing && (components != null))
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
      this.lblFinished = new System.Windows.Forms.Label();
      this.cmdOpen = new System.Windows.Forms.Button();
      this.pictureBoxPDF = new System.Windows.Forms.PictureBox();
      this.cmdClose = new System.Windows.Forms.Button();
      this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPDF)).BeginInit();
      this.tableLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblFinished
      // 
      this.lblFinished.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lblFinished.Location = new System.Drawing.Point(66, 12);
      this.lblFinished.Name = "lblFinished";
      this.lblFinished.Size = new System.Drawing.Size(314, 48);
      this.lblFinished.TabIndex = 0;
      this.lblFinished.Text = "PDF Export is finished. Do you want to open the exported PDF?";
      this.lblFinished.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cmdOpen
      // 
      this.cmdOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.cmdOpen.Location = new System.Drawing.Point(112, 3);
      this.cmdOpen.Name = "cmdOpen";
      this.cmdOpen.Size = new System.Drawing.Size(69, 23);
      this.cmdOpen.TabIndex = 1;
      this.cmdOpen.Text = "Open";
      this.cmdOpen.UseVisualStyleBackColor = true;
      // 
      // pictureBoxPDF
      // 
      this.pictureBoxPDF.Image = global::PDFExport.Properties.Resources.Document_pdf_48;
      this.pictureBoxPDF.Location = new System.Drawing.Point(12, 12);
      this.pictureBoxPDF.Name = "pictureBoxPDF";
      this.pictureBoxPDF.Size = new System.Drawing.Size(48, 48);
      this.pictureBoxPDF.TabIndex = 2;
      this.pictureBoxPDF.TabStop = false;
      // 
      // cmdClose
      // 
      this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdClose.Location = new System.Drawing.Point(187, 3);
      this.cmdClose.Name = "cmdClose";
      this.cmdClose.Size = new System.Drawing.Size(69, 23);
      this.cmdClose.TabIndex = 1;
      this.cmdClose.Text = "Close";
      this.cmdClose.UseVisualStyleBackColor = true;
      // 
      // tableLayoutPanel
      // 
      this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel.ColumnCount = 4;
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel.Controls.Add(this.cmdOpen, 1, 0);
      this.tableLayoutPanel.Controls.Add(this.cmdClose, 2, 0);
      this.tableLayoutPanel.Location = new System.Drawing.Point(12, 66);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 1;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel.Size = new System.Drawing.Size(368, 30);
      this.tableLayoutPanel.TabIndex = 3;
      // 
      // PDFExportFinished
      // 
      this.AcceptButton = this.cmdOpen;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cmdClose;
      this.ClientSize = new System.Drawing.Size(392, 100);
      this.ControlBox = false;
      this.Controls.Add(this.tableLayoutPanel);
      this.Controls.Add(this.pictureBoxPDF);
      this.Controls.Add(this.lblFinished);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PDFExportFinished";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "PDF Exporter";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPDF)).EndInit();
      this.tableLayoutPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblFinished;
    private System.Windows.Forms.Button cmdOpen;
    private System.Windows.Forms.PictureBox pictureBoxPDF;
    private System.Windows.Forms.Button cmdClose;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
  }
}