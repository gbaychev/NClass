// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using NClass.DiagramEditor;

namespace NClass.GUI
{
	public class DiagramNavigator : Control
	{
		IDocumentVisualizer visualizer = null;
		Color frameColor = SystemColors.ControlDarkDark;

		public DiagramNavigator()
		{
			SetStyle(ControlStyles.UserPaint, true);
			this.DoubleBuffered = true;
		}

		[Browsable(false)]
		public IDocumentVisualizer DocumentVisualizer
		{
			get
			{
				return visualizer;
			}
			set
			{
				if (visualizer != value)
				{
					if (visualizer != null)
					{
						visualizer.DocumentRedrawed -= visualizer_DocumentRedrawed;
						visualizer.VisibleAreaChanged -= visualizer_VisibleAreaChanged;
					}
					visualizer = value;
					if (visualizer != null)
					{
						visualizer.DocumentRedrawed += visualizer_DocumentRedrawed;
						visualizer.VisibleAreaChanged += visualizer_VisibleAreaChanged;
					}
				}
			}
		}

		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public Color FrameColor
		{
			get { return frameColor; }
			set { frameColor = value; }
		}

		private void visualizer_DocumentRedrawed(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		private void visualizer_VisibleAreaChanged(object sender, EventArgs e)
		{
			this.Invalidate(); //TODO: ezt lehetne tán optimalizálni, hogy ne hívja meg annyiszor...
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				MoveVisibleArea(e.Location);
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				MoveVisibleArea(e.Location);
			base.OnMouseMove(e);
		}

		private void MoveVisibleArea(Point location)
		{
			if (DocumentVisualizer != null && DocumentVisualizer.HasDocument)
			{
				float zoom = this.GetZoom() / DocumentVisualizer.Zoom;
				float frameWidth  = DocumentVisualizer.VisibleArea.Width  * DocumentVisualizer.Zoom;
				float frameHeight = DocumentVisualizer.VisibleArea.Height * DocumentVisualizer.Zoom;

				DocumentVisualizer.Offset = new Point(
					(int) (location.X / zoom - frameWidth / 2),
					(int) (location.Y / zoom - frameHeight / 2)
				);
			}
		}

		private float GetZoom()
		{
			Rectangle borders = this.ClientRectangle;
			float zoom1 = (float) borders.Width / DocumentVisualizer.DocumentSize.Width;
			float zoom2 = (float) borders.Height / DocumentVisualizer.DocumentSize.Height;

			return Math.Min(zoom1, zoom2);
		}

		private void DrawDocument(Graphics g)
		{
			if (DocumentVisualizer != null && DocumentVisualizer.HasDocument)
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.TextRenderingHint = TextRenderingHint.AntiAlias;

				Rectangle borders = this.ClientRectangle;
				float zoom = GetZoom();
				g.ScaleTransform(zoom, zoom);

				DocumentVisualizer.DrawDocument(g);

				g.ResetTransform();

				if (DocumentVisualizer.VisibleArea.Width < DocumentVisualizer.DocumentSize.Width ||
					DocumentVisualizer.VisibleArea.Height < DocumentVisualizer.DocumentSize.Height)
				{
					g.SmoothingMode = SmoothingMode.None;
					Rectangle frame = new Rectangle(
						(int) (DocumentVisualizer.VisibleArea.X * zoom),
						(int) (DocumentVisualizer.VisibleArea.Y * zoom),
						(int) (DocumentVisualizer.VisibleArea.Width * zoom),
						(int) (DocumentVisualizer.VisibleArea.Height * zoom)
					);
					if (frame.Right > ClientRectangle.Right)
						frame.Width = ClientRectangle.Right - frame.Left - 1;
					if (frame.Bottom > ClientRectangle.Bottom)
						frame.Height = ClientRectangle.Bottom - frame.Top - 1;
					DrawFrame(g, frame);
				}
			}
		}

		private void DrawFrame(Graphics g, Rectangle frame)
		{
			FrameColor = Color.FromArgb(80, 100, 150);
			Pen pen = new Pen(FrameColor);

			for (int alpha = 256; alpha >= 4; alpha /= 2)
			{
				pen.Color = Color.FromArgb(alpha - 1, FrameColor);
				g.DrawRectangle(pen, frame);
				frame.Inflate(1, 1);
			}
			pen.Dispose();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawDocument(e.Graphics);
		}
	}
}
