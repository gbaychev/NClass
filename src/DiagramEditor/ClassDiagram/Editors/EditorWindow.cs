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
using System.Windows.Forms;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Core;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public abstract class EditorWindow : PopupWindow
	{
		internal abstract void Init(DiagramElement element);

		internal abstract void Relocate(DiagramElement element);

		public abstract void ValidateData();

		public override void Closing()
		{
			ValidateData();
		}
	}
}
