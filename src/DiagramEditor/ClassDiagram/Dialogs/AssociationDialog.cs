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
using System.ComponentModel;
using System.Windows.Forms;
using NClass.Core;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{
	public partial class AssociationDialog : Form
	{
		const int ArrowWidth = 18;
		const int ArrowHeight = 10;
		const int DiamondWidth = 20;
		const int DiamondHeight = 10;

		AssociationRelationship association = null;
		Direction modifiedDirection;
		AssociationType modifiedType;

		public AssociationDialog()
		{
			InitializeComponent();
			UpdateTexts();
		}

		public AssociationRelationship Association
		{
			get
			{
				return association;
			}
			set
			{
				if (value != null)
				{
					association = value;
					UpdateFields();
				}
			}
		}

		private void UpdateTexts()
		{
			this.Text = Strings.EditAssociation;
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;
		}

		private void UpdateFields()
		{
			modifiedDirection = association.Direction;
			modifiedType = association.AssociationType;

			txtName.Text = association.Label;
			txtStartRole.Text = association.StartRole;
			txtEndRole.Text = association.EndRole;
			cboStartMultiplicity.Text = association.StartMultiplicity;
			cboEndMultiplicity.Text = association.EndMultiplicity;
		}

		private void ModifyRelationship()
		{
			association.AssociationType = modifiedType;
			association.Direction = modifiedDirection;
			association.Label = txtName.Text;
			association.StartRole = txtStartRole.Text;
			association.EndRole = txtEndRole.Text;
			association.StartMultiplicity = cboStartMultiplicity.Text;
			association.EndMultiplicity = cboEndMultiplicity.Text;
		}

		private void picArrow_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.X <= DiamondWidth)
			{
				ChangeType();
				picArrow.Invalidate();
			}
			else if (e.X >= picArrow.Width - ArrowWidth)
			{
				ChangeHead();
				picArrow.Invalidate();
			}
		}

		private void ChangeType()
		{
			if (modifiedType == AssociationType.Association)
			{
				modifiedType = AssociationType.Aggregation;
			}
			else if (modifiedType == AssociationType.Aggregation)
			{
				modifiedType = AssociationType.Composition;
			}
			else
			{
				modifiedType = AssociationType.Association;
			}
		}

		private void ChangeHead()
		{
			if (modifiedDirection == Direction.Bidirectional)
			{
				modifiedDirection = Direction.Unidirectional;
			}
			else
			{
				modifiedDirection = Direction.Bidirectional;
			}
		}

		private void picArrow_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			int center = picArrow.Height / 2;
			int width = picArrow.Width;

			// Draw line
			g.DrawLine(Pens.Black, 0, center, width, center);

			// Draw arrow head
			if (modifiedDirection == Direction.Unidirectional)
			{
				g.DrawLine(Pens.Black, width - ArrowWidth, center - ArrowHeight / 2, width, center);
				g.DrawLine(Pens.Black, width - ArrowWidth, center + ArrowHeight / 2, width, center);
			}

			// Draw start symbol
			if (modifiedType != AssociationType.Association)
			{
				Point[] diamondPoints =  {
					new Point(0, center),
					new Point(DiamondWidth / 2, center - DiamondHeight / 2),
					new Point(DiamondWidth, center),
					new Point(DiamondWidth / 2, center + DiamondHeight / 2)
				};

				if (modifiedType == AssociationType.Aggregation)
				{
					g.FillPolygon(Brushes.White, diamondPoints);
					g.DrawPolygon(Pens.Black, diamondPoints);
				}
				else if (modifiedType == AssociationType.Composition)
				{
					g.FillPolygon(Brushes.Black, diamondPoints);
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (association != null)
			{
				ModifyRelationship();
			}
		}
	}
}
