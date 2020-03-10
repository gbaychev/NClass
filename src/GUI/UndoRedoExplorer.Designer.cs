namespace NClass.GUI
{
    partial class UndoRedoExplorer
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
            this.undoStack = new System.Windows.Forms.ListBox();
            this.redoStack = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // undoStack
            // 
            this.undoStack.FormattingEnabled = true;
            this.undoStack.Location = new System.Drawing.Point(12, 12);
            this.undoStack.Name = "undoStack";
            this.undoStack.Size = new System.Drawing.Size(297, 290);
            this.undoStack.TabIndex = 0;
            // 
            // redoStack
            // 
            this.redoStack.FormattingEnabled = true;
            this.redoStack.Location = new System.Drawing.Point(315, 12);
            this.redoStack.Name = "redoStack";
            this.redoStack.Size = new System.Drawing.Size(285, 290);
            this.redoStack.TabIndex = 1;
            // 
            // UndoRedoExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 315);
            this.Controls.Add(this.redoStack);
            this.Controls.Add(this.undoStack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UndoRedoExplorer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Undo Stack";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox undoStack;
        private System.Windows.Forms.ListBox redoStack;
    }
}