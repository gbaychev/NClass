namespace NClass.GUI.Dialogs
{
    partial class AboutDialog
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lnkHomepage = new System.Windows.Forms.LinkLabel();
            this.lnkEmail = new System.Windows.Forms.LinkLabel();
            this.picEmail = new System.Windows.Forms.PictureBox();
            this.picHomepage = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTranslator = new System.Windows.Forms.Label();
            this.lblBuildInfo = new System.Windows.Forms.Label();
            this.lblGitCommit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTitle.Location = new System.Drawing.Point(65, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 44);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "NClass vX.X";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(284, 216);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyright.Location = new System.Drawing.Point(63, 66);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(245, 46);
            this.lblCopyright.TabIndex = 9;
            this.lblCopyright.Text = "Copyright (C) 2016-2018 Georgi Baychev";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lnkHomepage
            // 
            this.lnkHomepage.ActiveLinkColor = System.Drawing.Color.Black;
            this.lnkHomepage.AutoSize = true;
            this.lnkHomepage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkHomepage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHomepage.LinkColor = System.Drawing.Color.Black;
            this.lnkHomepage.Location = new System.Drawing.Point(57, 181);
            this.lnkHomepage.Name = "lnkHomepage";
            this.lnkHomepage.Size = new System.Drawing.Size(127, 13);
            this.lnkHomepage.TabIndex = 7;
            this.lnkHomepage.TabStop = true;
            this.lnkHomepage.Text = "Visit program\'s homepage";
            this.lnkHomepage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkHomepage.VisitedLinkColor = System.Drawing.Color.Black;
            this.lnkHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHomepage_LinkClicked);
            // 
            // lnkEmail
            // 
            this.lnkEmail.AccessibleName = "";
            this.lnkEmail.ActiveLinkColor = System.Drawing.Color.Black;
            this.lnkEmail.AutoSize = true;
            this.lnkEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkEmail.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkEmail.LinkColor = System.Drawing.Color.Black;
            this.lnkEmail.Location = new System.Drawing.Point(57, 140);
            this.lnkEmail.Name = "lnkEmail";
            this.lnkEmail.Size = new System.Drawing.Size(173, 13);
            this.lnkEmail.TabIndex = 6;
            this.lnkEmail.TabStop = true;
            this.lnkEmail.Text = "Send e-mail to the program\'s author";
            this.lnkEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkEmail.VisitedLinkColor = System.Drawing.Color.Black;
            this.lnkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEmail_LinkClicked);
            // 
            // picEmail
            // 
            this.picEmail.Image = global::NClass.GUI.Properties.Resources.Mail;
            this.picEmail.Location = new System.Drawing.Point(14, 131);
            this.picEmail.Name = "picEmail";
            this.picEmail.Size = new System.Drawing.Size(32, 32);
            this.picEmail.TabIndex = 10;
            this.picEmail.TabStop = false;
            // 
            // picHomepage
            // 
            this.picHomepage.Image = global::NClass.GUI.Properties.Resources.Web;
            this.picHomepage.Location = new System.Drawing.Point(15, 173);
            this.picHomepage.Name = "picHomepage";
            this.picHomepage.Size = new System.Drawing.Size(30, 30);
            this.picHomepage.TabIndex = 11;
            this.picHomepage.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblStatus.Location = new System.Drawing.Point(95, 112);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(181, 13);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Beta version";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblStatus.Visible = false;
            // 
            // lblTranslator
            // 
            this.lblTranslator.AutoSize = true;
            this.lblTranslator.Location = new System.Drawing.Point(12, 221);
            this.lblTranslator.Name = "lblTranslator";
            this.lblTranslator.Size = new System.Drawing.Size(54, 13);
            this.lblTranslator.TabIndex = 13;
            this.lblTranslator.Text = "Translator";
            // 
            // lblBuildInfo
            // 
            this.lblBuildInfo.AutoSize = true;
            this.lblBuildInfo.Location = new System.Drawing.Point(151, 53);
            this.lblBuildInfo.Name = "lblBuildInfo";
            this.lblBuildInfo.Size = new System.Drawing.Size(80, 13);
            this.lblBuildInfo.TabIndex = 14;
            this.lblBuildInfo.Text = "branch-git-hash";
            this.lblBuildInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGitCommit
            // 
            this.lblGitCommit.AutoSize = true;
            this.lblGitCommit.Location = new System.Drawing.Point(63, 53);
            this.lblGitCommit.Name = "lblGitCommit";
            this.lblGitCommit.Size = new System.Drawing.Size(85, 13);
            this.lblGitCommit.TabIndex = 15;
            this.lblGitCommit.Text = "Git commit hash:";
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(371, 244);
            this.Controls.Add(this.lblGitCommit);
            this.Controls.Add(this.lblBuildInfo);
            this.Controls.Add(this.lblTranslator);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.picHomepage);
            this.Controls.Add(this.picEmail);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lnkHomepage);
            this.Controls.Add(this.lnkEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About NClass";
            ((System.ComponentModel.ISupportInitialize)(this.picEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHomepage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.LinkLabel lnkHomepage;
        private System.Windows.Forms.LinkLabel lnkEmail;
        private System.Windows.Forms.PictureBox picEmail;
        private System.Windows.Forms.PictureBox picHomepage;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTranslator;
        private System.Windows.Forms.Label lblBuildInfo;
        private System.Windows.Forms.Label lblGitCommit;
    }
}
