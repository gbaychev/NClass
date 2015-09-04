// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
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
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace NClass.DiagramEditor
{
	public interface IGraphics
	{
		Region Clip { get; set; }
		RectangleF ClipBounds { get; }
		Matrix Transform { get; set; }

		void DrawEllipse(Pen pen, int x, int y, int width, int height);
		void DrawImage(Image image, int x, int y);
		void DrawImage(Image image, Point point);
		void DrawLine(Pen pen, int x1, int y1, int x2, int y2);
		void DrawLine(Pen pen, Point pt1, Point pt2);
		void DrawLines(Pen pen, Point[] points);
		void DrawPath(Pen pen, GraphicsPath path);
		void DrawPolygon(Pen pen, Point[] points);
		void DrawRectangle(Pen pen, Rectangle rect);
		void DrawString(string s, Font font, Brush brush, PointF point);
		void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle);
		void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format);
		void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format);
		void FillEllipse(Brush brush, Rectangle rect);
		void FillEllipse(Brush brush, int x, int y, int width, int height);
		void FillPath(Brush brush, GraphicsPath path);
		void FillPolygon(Brush brush, Point[] points);
		void FillRectangle(Brush brush, Rectangle rect);

		void RotateTransform(float angle);
		void ScaleTransform(float sx, float sy);
		void TranslateTransform(float dx, float dy);
		void ResetTransform();

		void SetClip(GraphicsPath path, CombineMode combineMode);
		void SetClip(Rectangle rect, CombineMode combineMode);
		void SetClip(RectangleF rect, CombineMode combineMode);
		void SetClip(Region region, CombineMode combineMode);
	}
}
