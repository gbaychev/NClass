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
using System.Windows.Forms;

namespace NClass.DiagramEditor
{
	public delegate void AbsoluteMouseEventHandler(object sender, AbsoluteMouseEventArgs e);

	public class AbsoluteMouseEventArgs
	{
		float x;
		float y;
		MouseButtons button;
		bool handled = false;
		float zoom;

		public AbsoluteMouseEventArgs(MouseButtons button, float x, float y, float zoom)
		{
			this.button = button;
			this.x = x;
			this.y = y;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseButtons button, PointF location, float zoom)
		{
			this.button = button;
			this.x = location.X;
			this.y = location.Y;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseEventArgs e, Point offset, float zoom)
		{
			this.button = e.Button;
			this.x = (e.X + offset.X) / zoom;
			this.y = (e.Y + offset.Y) / zoom;
			this.zoom = zoom;
		}

		public AbsoluteMouseEventArgs(MouseEventArgs e, IDocument document)
			: this(e, document.Offset, document.Zoom)
		{
		}

		public MouseButtons Button
		{
			get { return button; }
		}

		public float X
		{
			get { return x; }
		}

		public float Y
		{
			get { return y; }
		}

		public PointF Location
		{
			get { return new PointF(x, y); }
		}

		public bool Handled
		{
			get { return handled; }
			set { handled = value; }
		}

		public float Zoom
		{
			get { return zoom; }
			set { zoom = value; }
		}
	}
}
