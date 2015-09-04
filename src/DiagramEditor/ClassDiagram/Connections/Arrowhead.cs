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

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
	internal static class Arrowhead
	{
		public const int ClosedArrowWidth = 12;
		public const int ClosedArrowHeight = 17;
		public static readonly Size ClosedArrowSize = new Size(ClosedArrowWidth, ClosedArrowHeight);
		static readonly GraphicsPath closedArrowPath = new GraphicsPath();

		public const int OpenArrowWidth = 10;
		public const int OpenArrowHeight = 16;
		public static readonly Size OpenArrowSize = new Size(OpenArrowWidth, OpenArrowHeight);
		static readonly Point[] openArrowPoints;

		static Arrowhead()
		{
			openArrowPoints = new Point[] {
				new Point(-OpenArrowWidth / 2, OpenArrowHeight),
				new Point(0, 0),
				new Point(OpenArrowWidth / 2, OpenArrowHeight)
			};

			closedArrowPath.AddLines(new Point[] {
				new Point(0, 0),
				new Point(ClosedArrowWidth / 2, ClosedArrowHeight),
				new Point(-ClosedArrowWidth / 2, ClosedArrowHeight)
			});
			closedArrowPath.CloseFigure();
		}

		public static GraphicsPath ClosedArrowPath
		{
			get { return closedArrowPath; }
		}

		public static Point[] OpenArrowPoints
		{
			get { return openArrowPoints; }
		}
	}
}
