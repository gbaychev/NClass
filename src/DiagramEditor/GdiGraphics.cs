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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NClass.DiagramEditor
{
	public sealed class GdiGraphics : IGraphics
	{
		readonly Graphics graphics;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="graphics"/> is null.
		/// </exception>
		public GdiGraphics(Graphics graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.graphics = graphics;
		}

		public Region Clip
		{
			get { return graphics.Clip; }
			set { graphics.Clip = value; }
		}

		public RectangleF ClipBounds
		{
			get { return graphics.ClipBounds; }
		}

		public Matrix Transform
		{
			get { return graphics.Transform; }
			set { graphics.Transform = value; }
		}

		public void DrawEllipse(Pen pen, int x, int y, int width, int height)
		{
			graphics.DrawEllipse(pen, x, y, width, height);
		}

		public void DrawImage(Image image, int x, int y)
		{
			graphics.DrawImage(image, x, y, image.Width, image.Height);
		}

		public void DrawImage(Image image, Point point)
		{
			graphics.DrawImage(image, point.X, point.Y, image.Width, image.Height);
		}

		public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
		{
			graphics.DrawLine(pen, x1, y1, x2, y2);
		}

		public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			graphics.DrawLine(pen, pt1, pt2);
		}

		public void DrawLines(Pen pen, Point[] points)
		{
			graphics.DrawLines(pen, points);
		}

		public void DrawPath(Pen pen, GraphicsPath path)
		{
			graphics.DrawPath(pen, path);
		}

		public void DrawPolygon(Pen pen, Point[] points)
		{
			graphics.DrawPolygon(pen, points);
		}

		public void DrawRectangle(Pen pen, Rectangle rect)
		{
			graphics.DrawRectangle(pen, rect);
		}

		public void DrawString(string s, Font font, Brush brush, PointF point)
		{
			graphics.DrawString(s, font, brush, point);
		}

		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			graphics.DrawString(s, font, brush, layoutRectangle);
		}

		public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
		{
			graphics.DrawString(s, font, brush, point, format);
		}

		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle,
			StringFormat format)
		{
			graphics.DrawString(s, font, brush, layoutRectangle, format);
		}

		public void FillEllipse(Brush brush, Rectangle rect)
		{
			graphics.FillEllipse(brush, rect);
		}

		public void FillEllipse(Brush brush, int x, int y, int width, int height)
		{
			graphics.FillEllipse(brush, x, y, width, height);
		}

		public void FillPath(Brush brush, GraphicsPath path)
		{
			graphics.FillPath(brush, path);
		}

		public void FillPolygon(Brush brush, Point[] points)
		{
			graphics.FillPolygon(brush, points);
		}

		public void FillRectangle(Brush brush, Rectangle rect)
		{
			graphics.FillRectangle(brush, rect);
		}

		public void SetClip(GraphicsPath path, CombineMode combineMode)
		{
			graphics.SetClip(path, combineMode);
		}

		public void SetClip(Rectangle rect, CombineMode combineMode)
		{
			graphics.SetClip(rect, combineMode);
		}

		public void SetClip(RectangleF rect, CombineMode combineMode)
		{
			graphics.SetClip(rect, combineMode);
		}

		public void SetClip(Region region, CombineMode combineMode)
		{
			graphics.SetClip(region, combineMode);
		}

		public void ResetTransform()
		{
			graphics.ResetTransform();
		}

		public void RotateTransform(float angle)
		{
			graphics.RotateTransform(angle);
		}

		public void ScaleTransform(float sx, float sy)
		{
			graphics.TranslateTransform(sx, sy);
		}

		public void TranslateTransform(float dx, float dy)
		{
			graphics.TranslateTransform(dx, dy);
		}
	}
}
