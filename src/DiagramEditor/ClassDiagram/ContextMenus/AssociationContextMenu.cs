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
using NClass.Core;
using NClass.DiagramEditor.Properties;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
	internal sealed class AssociationContextMenu : DiagramContextMenu
	{
		static AssociationContextMenu _default = new AssociationContextMenu();

		ToolStripMenuItem mnuDirection, mnuUnidirectional, mnuBidirectional;
		ToolStripMenuItem mnuType, mnuAssociation, mnuComposition, mnuAggregation;
		ToolStripMenuItem mnuReverse;
		ToolStripMenuItem mnuEdit;

		private AssociationContextMenu()
		{
			InitMenuItems();
		}

		public static AssociationContextMenu Default
		{
			get { return _default; }
		}

		private void UpdateTexts()
		{
			mnuDirection.Text = Strings.MenuDirection;
			mnuUnidirectional.Text = Strings.MenuUnidirectional;
			mnuBidirectional.Text = Strings.MenuBidirectional;
			mnuType.Text = Strings.MenuType;
			mnuAssociation.Text = Strings.MenuAssociation;
			mnuComposition.Text = Strings.MenuComposition;
			mnuAggregation.Text = Strings.MenuAggregation;
			mnuReverse.Text = Strings.MenuReverse;
			mnuEdit.Text = Strings.MenuProperties;
		}

		public override void ValidateMenuItems(Diagram diagram)
		{
			base.ValidateMenuItems(diagram);
			ConnectionContextMenu.Default.ValidateMenuItems(diagram);
			mnuEdit.Enabled = (diagram.SelectedElementCount == 1);
		}

		private void InitMenuItems()
		{
			mnuUnidirectional = new ToolStripMenuItem(Strings.MenuUnidirectional,
				Resources.Unidirectional, mnuUnidirectional_Click);
			mnuBidirectional = new ToolStripMenuItem(Strings.MenuBidirectional,
				Resources.Bidirectional, mnuBidirectional_Click);
			mnuDirection = new ToolStripMenuItem(Strings.MenuDirection, null,
				mnuUnidirectional,
				mnuBidirectional
			);

			mnuAssociation = new ToolStripMenuItem(Strings.MenuAssociation,
				Resources.Association, mnuAssociation_Click);
			mnuComposition = new ToolStripMenuItem(Strings.MenuComposition,
				Resources.Composition, mnuComposition_Click);
			mnuAggregation = new ToolStripMenuItem(Strings.MenuAggregation,
				Resources.Aggregation, mnuAggregation_Click);
			mnuType = new ToolStripMenuItem(Strings.MenuType, null,
				mnuAssociation,
				mnuComposition,
				mnuAggregation
			);

			mnuReverse = new ToolStripMenuItem(Strings.MenuReverse, null, mnuReverse_Click);
			mnuEdit = new ToolStripMenuItem(Strings.MenuEditAssociation,
				Resources.Property, mnuEdit_Click);

			MenuList.AddRange(ConnectionContextMenu.Default.MenuItems);
			MenuList.InsertRange(7, new ToolStripItem[] {
				mnuDirection,
				mnuType,
				mnuReverse,
				new ToolStripSeparator(),
			});
			MenuList.Add(mnuEdit);
		}

		private void mnuUnidirectional_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Association association in Diagram.GetSelectedConnections())
					association.AssociationRelationship.Direction = Direction.Unidirectional;
			}
		}

		private void mnuBidirectional_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Association association in Diagram.GetSelectedConnections())
					association.AssociationRelationship.Direction = Direction.Bidirectional;
			}
		}

		private void mnuAssociation_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Association association in Diagram.GetSelectedConnections())
				{
					association.AssociationRelationship.AssociationType = AssociationType.Association;
				}
			}
		}

		private void mnuComposition_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Association association in Diagram.GetSelectedConnections())
					association.AssociationRelationship.AssociationType = AssociationType.Composition;
			}
		}

		private void mnuAggregation_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				foreach (Association association in Diagram.GetSelectedConnections())
					association.AssociationRelationship.AssociationType = AssociationType.Aggregation;
			}
		}

		private void mnuReverse_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				Association association = Diagram.TopSelectedElement as Association;
				if (association != null)
					association.AssociationRelationship.Reverse();
			}
		}

		private void mnuEdit_Click(object sender, EventArgs e)
		{
			if (Diagram != null)
			{
				Association association = Diagram.TopSelectedElement as Association;
				if (association != null)
					association.ShowEditDialog();
			}
		}
	}
}