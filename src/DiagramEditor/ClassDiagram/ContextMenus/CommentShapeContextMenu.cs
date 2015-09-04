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
using System.Collections.Generic;
using System.Windows.Forms;
using NClass.DiagramEditor.Properties;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
	internal sealed class CommentShapeContextMenu : DiagramContextMenu
	{
		static CommentShapeContextMenu _default = new CommentShapeContextMenu();

		ToolStripMenuItem mnuEditComment;

		private CommentShapeContextMenu()
		{
			InitMenuItems();
		}

		public static CommentShapeContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuEditComment.Text = Strings.MenuEditComment;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			ShapeContextMenu.Default.ValidateMenuItems(diagram);
			mnuEditComment.Enabled = (diagram.SelectedElementCount == 1);
		}

		private void InitMenuItems()
		{
			mnuEditComment = new ToolStripMenuItem(
				Strings.MenuEditComment,
				Resources.EditComment, mnuEditComment_Click);

			MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				new ToolStripSeparator(),
				mnuEditComment,
			});
		}

		private void mnuEditComment_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				CommentShape commentShape = Diagram.TopSelectedElement as CommentShape;
				if (commentShape != null)
					commentShape.EditText();
			}
		}
	}
}