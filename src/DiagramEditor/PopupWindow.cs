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
using System.ComponentModel;
using System.Windows.Forms;

namespace NClass.DiagramEditor
{
	public class PopupWindow : UserControl
	{
		const int ClientMargin = 20;
		
		Point parentLocation;

		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if (Parent != null)
				{
					Rectangle client = Parent.ClientRectangle;

					if (value.X < ClientMargin)
						value.X = ClientMargin;
					if (value.Y < ClientMargin)
						value.Y = ClientMargin;
					if (value.X + Width > client.Width - ClientMargin)
						value.X = client.Width - Width - ClientMargin;
					if (value.Y + Height > client.Height - ClientMargin)
						value.Y = client.Height - Height - ClientMargin;
				}
				base.Location = value;
			}
		}

		internal Point ParentLocation
		{
			get
			{
				return parentLocation;
			}
			set
			{
				Size offset = new Size(value.X - parentLocation.X, value.Y - parentLocation.Y);
				parentLocation = value;
				this.Location += offset;
			}
		}

		public virtual void Closing()
		{
		}
	}
}
