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
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
	internal sealed class ConnectionContextMenu : DiagramContextMenu
	{
		static ConnectionContextMenu _default = new ConnectionContextMenu();

		ToolStripMenuItem mnuAutoRouting;

		private ConnectionContextMenu()
		{
			InitMenuItems();
		}

		public static ConnectionContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuAutoRouting.Text = Strings.MenuAutoRouting;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			GeneralContextMenu.Default.ValidateMenuItems(diagram);
		}

		private void InitMenuItems()
		{
			mnuAutoRouting = new ToolStripMenuItem(Strings.MenuAutoRouting,
				null, mnuAutoRouting_Click);

			MenuList.AddRange(GeneralContextMenu.Default.MenuItems);
			MenuList.AddRange(new ToolStripItem[] {
				new ToolStripSeparator(),
				mnuAutoRouting,
			});
		}

		private void mnuAutoRouting_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Connection connection in Diagram.GetSelectedConnections())
					connection.AutoRoute();
			}
		}
	}
}