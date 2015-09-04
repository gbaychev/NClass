namespace NClass.GUI
{
	partial class TabbedWindow
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
			this.canvas = new NClass.DiagramEditor.Canvas();
			this.tabBar = new NClass.GUI.TabBar();
			this.SuspendLayout();
			// 
			// canvas
			// 
			this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.canvas.Document = null;
			this.canvas.Location = new System.Drawing.Point(0, 25);
			this.canvas.Margin = new System.Windows.Forms.Padding(0);
			this.canvas.Name = "canvas";
			this.canvas.Offset = new System.Drawing.Point(0, 0);
			this.canvas.Size = new System.Drawing.Size(150, 125);
			this.canvas.TabIndex = 3;
			this.canvas.Zoom = 1F;
			// 
			// tabBar
			// 
			this.tabBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabBar.DocumentManager = null;
			this.tabBar.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
			this.tabBar.Location = new System.Drawing.Point(0, 0);
			this.tabBar.Margin = new System.Windows.Forms.Padding(0);
			this.tabBar.Name = "tabBar";
			this.tabBar.Size = new System.Drawing.Size(150, 25);
			this.tabBar.TabIndex = 2;
			// 
			// TabbedWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Controls.Add(this.canvas);
			this.Controls.Add(this.tabBar);
			this.Name = "TabbedWindow";
			this.ResumeLayout(false);

		}

		#endregion

		private TabBar tabBar;
		private NClass.DiagramEditor.Canvas canvas;

	}
}
