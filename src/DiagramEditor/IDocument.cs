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
using System.Collections.Generic;
using NClass.Core;

namespace NClass.DiagramEditor
{
	public interface IDocument : IProjectItem, IEditable, IPrintable
	{
		event EventHandler OffsetChanged;
		event EventHandler SizeChanged;
		event EventHandler ZoomChanged;
		event EventHandler StatusChanged;
		event EventHandler NeedsRedraw;
		event PopupWindowEventHandler ShowingWindow;
		event PopupWindowEventHandler HidingWindow;


		Point Offset { get; set; }

		Size Size { get; }

		float Zoom { get; set; }

		Color BackColor { get; }

		bool HasSelectedElement { get; }


		void Display(Graphics g);

		void Redraw();

		void CloseWindows();

		DynamicMenu GetDynamicMenu();

		string GetStatus();

		string GetShortDescription();

		string GetSelectedElementName();

		void MouseDown(AbsoluteMouseEventArgs e);

		void MouseMove(AbsoluteMouseEventArgs e);

		void MouseUp(AbsoluteMouseEventArgs e);

		void DoubleClick(AbsoluteMouseEventArgs e);

		void KeyDown(KeyEventArgs e);

		ContextMenuStrip GetContextMenu(AbsoluteMouseEventArgs e);
	}
}
