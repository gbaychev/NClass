namespace NClass.DiagramEditor.UseCaseDiagram.Editors
{
    partial class UseCaseEditor
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
            this.txtUseCase = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtUseCase
            // 
            this.txtUseCase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUseCase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUseCase.Location = new System.Drawing.Point(0, 0);
            this.txtUseCase.Multiline = true;
            this.txtUseCase.Name = "txtUseCase";
            this.txtUseCase.Size = new System.Drawing.Size(150, 150);
            this.txtUseCase.TabIndex = 0;
            this.txtUseCase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUseCase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComment_KeyDown);
            // 
            // UseCaseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtUseCase);
            this.Name = "UseCaseEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUseCase;
    }
}
