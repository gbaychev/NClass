// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
// 
// This program is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free Software
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with
// this program; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using NClass.GUI.Properties;
using NClass.Translations;

namespace NClass.GUI.Dialogs
{
    public class DetailsErrorDialog : Form
    {
            private TableLayoutPanel overarchingTableLayoutPanel;
            private TableLayoutPanel buttonTableLayoutPanel;
            private PictureBox pictureBox;
            private Label lblMessage;
            private Button detailsBtn;
            private Button cancelBtn;
            private Button okBtn;
            private TableLayoutPanel pictureLabelTableLayoutPanel;
            private TextBox details;

            private Bitmap expandImage = null;
            private Bitmap collapseImage = null;

            private bool detailsButtonExpanded = false;

            public bool DetailsButtonExpanded => detailsButtonExpanded;

            public static void Show(string title, string message, string details, MessageBoxIcon icon, bool isCenteredOnParent = false)
            {
                if (!string.IsNullOrEmpty(details))
                {
                    using (var dlg = new DetailsErrorDialog(icon))
                    {
                        if (isCenteredOnParent)
                        {
                            dlg.StartPosition = FormStartPosition.CenterParent;
                        }
                        dlg.Message = message;
                        dlg.Text = title;
                        dlg.Details = details;
                        dlg.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
                }
            }

            public string Details
            {
                set => this.details.Text = value;
            }


            public string Message
            {
                set => this.lblMessage.Text = value;
            }

            public DetailsErrorDialog(MessageBoxIcon icon = MessageBoxIcon.Warning)
            {
                expandImage = Resources.Down;
                expandImage.MakeTransparent();
                collapseImage = Resources.Up;
                collapseImage.MakeTransparent();

                InitializeComponent();

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        pictureBox.Image = SystemIcons.Error.ToBitmap();
                        break;
                    case MessageBoxIcon.Information:
                        pictureBox.Image = SystemIcons.Information.ToBitmap();
                        break;
                    default:
                        pictureBox.Image = SystemIcons.Warning.ToBitmap();
                        break;
                }

                detailsBtn.Text = Strings.ButtonDetails;

                okBtn.Text = Strings.ButtonOK;
                cancelBtn.Text = Strings.ButtonCancel;
                detailsBtn.Image = expandImage;
            }

            private void DetailsClick(object sender, EventArgs devent)
            {
                int delta = details.Height + 8;

                if (details.Visible)
                {
                    detailsBtn.Image = expandImage;
                    detailsButtonExpanded = false;
                    Height -= delta;
                }
                else
                {
                    detailsBtn.Image = collapseImage;
                    detailsButtonExpanded = true;
                    details.Width = overarchingTableLayoutPanel.Width - details.Margin.Horizontal;
                    Height += delta;
                }

                details.Visible = !details.Visible;
            }

            private void InitializeComponent()
            {
                this.detailsBtn = new Button();
                this.overarchingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.buttonTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.okBtn = new System.Windows.Forms.Button();
                this.cancelBtn = new System.Windows.Forms.Button();
                this.pictureLabelTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.lblMessage = new System.Windows.Forms.Label();
                this.pictureBox = new System.Windows.Forms.PictureBox();
                this.details = new System.Windows.Forms.TextBox();
                this.overarchingTableLayoutPanel.SuspendLayout();
                this.buttonTableLayoutPanel.SuspendLayout();
                this.pictureLabelTableLayoutPanel.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
                this.SuspendLayout();
                // 
                // lblMessage
                // 
                this.lblMessage.AutoSize = true;
                this.lblMessage.Location = new System.Drawing.Point(73, 15);
                this.lblMessage.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
                this.lblMessage.Name = "lblMessage";
                this.lblMessage.Size = new System.Drawing.Size(208, 43);
                this.lblMessage.TabIndex = 0;
                // 
                // pictureBox
                // 
                this.pictureBox.Location = new System.Drawing.Point(3, 3);
                this.pictureBox.Name = "pictureBox";
                this.pictureBox.Size = new System.Drawing.Size(64, 64);
                this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                this.pictureBox.TabIndex = 5;
                this.pictureBox.TabStop = false;
                this.pictureBox.AutoSize = true;
                // 
                // detailsBtn
                // 
                this.detailsBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                this.detailsBtn.Location = new System.Drawing.Point(3, 3);
                this.detailsBtn.Margin = new System.Windows.Forms.Padding(12, 3, 29, 3);
                this.detailsBtn.Name = "detailsBtn";
                this.detailsBtn.Size = new System.Drawing.Size(100, 23);
                this.detailsBtn.TabIndex = 4;
                this.detailsBtn.Click += new System.EventHandler(this.DetailsClick);
                // 
                // overarchingTableLayoutPanel
                // 
                this.overarchingTableLayoutPanel.AutoSize = true;
                this.overarchingTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.overarchingTableLayoutPanel.ColumnCount = 1;
                this.overarchingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
                this.overarchingTableLayoutPanel.Controls.Add(this.buttonTableLayoutPanel, 0, 1);
                this.overarchingTableLayoutPanel.Controls.Add(this.pictureLabelTableLayoutPanel, 0, 0);
                this.overarchingTableLayoutPanel.Location = new System.Drawing.Point(1, 0);
                this.overarchingTableLayoutPanel.MinimumSize = new System.Drawing.Size(279, 50);
                this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
                this.overarchingTableLayoutPanel.RowCount = 2;
                this.overarchingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
                this.overarchingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
                this.overarchingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                this.overarchingTableLayoutPanel.Size = new System.Drawing.Size(290, 108);
                this.overarchingTableLayoutPanel.TabIndex = 6;
                // 
                // buttonTableLayoutPanel
                // 
                this.buttonTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.buttonTableLayoutPanel.AutoSize = true;
                this.buttonTableLayoutPanel.ColumnCount = 3;
                this.overarchingTableLayoutPanel.SetColumnSpan(this.buttonTableLayoutPanel, 2);
                this.buttonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.buttonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
                this.buttonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
                this.buttonTableLayoutPanel.Controls.Add(this.cancelBtn, 2, 0);
                this.buttonTableLayoutPanel.Controls.Add(this.okBtn, 1, 0);
                this.buttonTableLayoutPanel.Controls.Add(this.detailsBtn, 0, 0);
                this.buttonTableLayoutPanel.Location = new System.Drawing.Point(0, 79);
                this.buttonTableLayoutPanel.Name = "buttonTableLayoutPanel";
                this.buttonTableLayoutPanel.RowCount = 1;
                this.buttonTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
                this.buttonTableLayoutPanel.Size = new System.Drawing.Size(290, 29);
                this.buttonTableLayoutPanel.TabIndex = 8;
                // 
                // okBtn
                // 
                this.okBtn.AutoSize = true;
                this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.okBtn.Location = new System.Drawing.Point(131, 3);
                this.okBtn.Name = "okBtn";
                this.okBtn.Size = new System.Drawing.Size(75, 23);
                this.okBtn.TabIndex = 1;
                this.okBtn.Click += new System.EventHandler(this.OnButtonClick);
                // 
                // cancelBtn
                // 
                this.cancelBtn.AutoSize = true;
                this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.cancelBtn.Location = new System.Drawing.Point(212, 3);
                this.cancelBtn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
                this.cancelBtn.Name = "cancelBtn";
                this.cancelBtn.Size = new System.Drawing.Size(75, 23);
                this.cancelBtn.TabIndex = 2;
                this.cancelBtn.Click += new System.EventHandler(this.OnButtonClick);
                // 
                // pictureLabelTableLayoutPanel
                // 
                this.pictureLabelTableLayoutPanel.AutoSize = true;
                this.pictureLabelTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                this.pictureLabelTableLayoutPanel.ColumnCount = 2;
                this.pictureLabelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
                this.pictureLabelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.pictureLabelTableLayoutPanel.Controls.Add(this.lblMessage, 1, 0);
                this.pictureLabelTableLayoutPanel.Controls.Add(this.pictureBox, 0, 0);
                this.pictureLabelTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.pictureLabelTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
                this.pictureLabelTableLayoutPanel.Name = "pictureLabelTableLayoutPanel";
                this.pictureLabelTableLayoutPanel.RowCount = 1;
                this.pictureLabelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.pictureLabelTableLayoutPanel.Size = new System.Drawing.Size(284, 73);
                this.pictureLabelTableLayoutPanel.TabIndex = 4;
                // 
                // details
                // 
                this.details.Location = new System.Drawing.Point(4, 114);
                this.details.Multiline = true;
                this.details.Name = "details";
                this.details.ReadOnly = true;
                this.details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                this.details.Size = new System.Drawing.Size(273, 100);
                this.details.TabIndex = 3;
                this.details.TabStop = false;
                this.details.Visible = false;
                // 
                // Form1
                // 
                this.AcceptButton = this.okBtn;
                this.AutoSize = true;
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.CancelButton = this.cancelBtn;
                this.ClientSize = new System.Drawing.Size(299, 113);
                this.Controls.Add(this.details);
                this.Controls.Add(this.overarchingTableLayoutPanel);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "Form1";
                this.ShowInTaskbar = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.overarchingTableLayoutPanel.ResumeLayout(false);
                this.overarchingTableLayoutPanel.PerformLayout();
                this.buttonTableLayoutPanel.ResumeLayout(false);
                this.buttonTableLayoutPanel.PerformLayout();
                this.pictureLabelTableLayoutPanel.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();
            }

            private void OnButtonClick(object s, EventArgs e)
            {
                DialogResult = ((Button)s).DialogResult;
                Close();
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                if (this.Visible)
                {
                    // make sure the details button is sized properly
                    //
                    using (Graphics g = CreateGraphics())
                    {
                        SizeF sizef = TextRenderer.MeasureText(g, detailsBtn.Text, detailsBtn.Font, new Size(0, 0), TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform);
                        int detailsWidth = (int)Math.Ceiling(sizef.Width);
                        detailsWidth += detailsBtn.Image.Width;
                        detailsBtn.Width = (int)Math.Ceiling(detailsWidth * 1.4f);
                        detailsBtn.Height = okBtn.Height;
                    }

                    // Update the location of the TextBox details
                    int x = details.Location.X;
                    int y = detailsBtn.Location.Y + detailsBtn.Height + detailsBtn.Margin.Bottom;

                    // Location is relative to its parent,
                    // therefore, we need to take its parent into consideration
                    Control parent = detailsBtn.Parent;
                    while (parent != null && !(parent is Form))
                    {
                        y += parent.Location.Y;
                        parent = parent.Parent;
                    }

                    details.Location = new System.Drawing.Point(x, y);

                    if (details.Visible)
                    {
                        DetailsClick(details, EventArgs.Empty);
                    }
                }
                okBtn.Focus();
            }
    }
}