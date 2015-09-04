namespace PDFExport
{
  partial class PDFExportProgress
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFExportProgress));
      this.lblProgress = new System.Windows.Forms.Label();
      this.timer = new System.Windows.Forms.Timer(this.components);
      this.pictureBoxNClass = new System.Windows.Forms.PictureBox();
      this.pictureBoxPDF = new System.Windows.Forms.PictureBox();
      this.imageListEntities = new System.Windows.Forms.ImageList(this.components);
      this.pictureBoxEntity = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNClass)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPDF)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEntity)).BeginInit();
      this.SuspendLayout();
      // 
      // lblProgress
      // 
      this.lblProgress.Location = new System.Drawing.Point(12, 9);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new System.Drawing.Size(268, 23);
      this.lblProgress.TabIndex = 0;
      this.lblProgress.Text = "Exporting diagram to PDF..";
      this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // timer
      // 
      this.timer.Interval = 10;
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // pictureBoxNClass
      // 
      this.pictureBoxNClass.Image = global::PDFExport.Properties.Resources.NClass;
      this.pictureBoxNClass.Location = new System.Drawing.Point(12, 44);
      this.pictureBoxNClass.Name = "pictureBoxNClass";
      this.pictureBoxNClass.Size = new System.Drawing.Size(48, 48);
      this.pictureBoxNClass.TabIndex = 2;
      this.pictureBoxNClass.TabStop = false;
      // 
      // pictureBoxPDF
      // 
      this.pictureBoxPDF.Image = global::PDFExport.Properties.Resources.Document_pdf_48;
      this.pictureBoxPDF.Location = new System.Drawing.Point(232, 44);
      this.pictureBoxPDF.Name = "pictureBoxPDF";
      this.pictureBoxPDF.Size = new System.Drawing.Size(48, 48);
      this.pictureBoxPDF.TabIndex = 1;
      this.pictureBoxPDF.TabStop = false;
      // 
      // imageListEntities
      // 
      this.imageListEntities.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListEntities.ImageStream")));
      this.imageListEntities.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListEntities.Images.SetKeyName(0, "struct.png");
      this.imageListEntities.Images.SetKeyName(1, "class.png");
      this.imageListEntities.Images.SetKeyName(2, "delegate.png");
      this.imageListEntities.Images.SetKeyName(3, "enum.png");
      this.imageListEntities.Images.SetKeyName(4, "interface_32bits.png");
      this.imageListEntities.Images.SetKeyName(5, "note.png");
      // 
      // pictureBoxEntity
      // 
      this.pictureBoxEntity.Location = new System.Drawing.Point(42, 61);
      this.pictureBoxEntity.Name = "pictureBoxEntity";
      this.pictureBoxEntity.Size = new System.Drawing.Size(16, 16);
      this.pictureBoxEntity.TabIndex = 3;
      this.pictureBoxEntity.TabStop = false;
      // 
      // PDFExportProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 132);
      this.ControlBox = false;
      this.Controls.Add(this.pictureBoxNClass);
      this.Controls.Add(this.pictureBoxPDF);
      this.Controls.Add(this.lblProgress);
      this.Controls.Add(this.pictureBoxEntity);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PDFExportProgress";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "PDF Export...";
      this.Load += new System.EventHandler(this.PDFExportProgress_Load);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNClass)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPDF)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEntity)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblProgress;
    private System.Windows.Forms.PictureBox pictureBoxPDF;
    private System.Windows.Forms.PictureBox pictureBoxNClass;
    private System.Windows.Forms.Timer timer;
    private System.Windows.Forms.ImageList imageListEntities;
    private System.Windows.Forms.PictureBox pictureBoxEntity;
  }
}