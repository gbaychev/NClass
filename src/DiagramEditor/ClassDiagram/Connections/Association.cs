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
using System.Collections.Generic;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.ClassDiagram.Dialogs;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
	internal sealed class Association : Connection
	{
		const int DiamondWidth = 10;
		const int DiamondHeight = 18;
		static readonly Point[] diamondPoints =  {
			new Point(0, 0),
			new Point(DiamondWidth / 2, DiamondHeight / 2),
			new Point(0, DiamondHeight),
			new Point(-DiamondWidth / 2, DiamondHeight / 2)
		};
		static Pen linePen = new Pen(Color.Black);
		static SolidBrush lineBrush = new SolidBrush(Color.Black);
		static SolidBrush textBrush = new SolidBrush(Color.Black);
		static StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);

		AssociationRelationship association;

		static Association()
		{
			linePen.MiterLimit = 2.0F;
			linePen.LineJoin = LineJoin.MiterClipped;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="association"/> is null.-or-
		/// <paramref name="startShape"/> is null.-or-
		/// <paramref name="endShape"/> is null.
		/// </exception>
		public Association(AssociationRelationship association, Shape startShape, Shape endShape)
			: base(association, startShape, endShape)
		{
			this.association = association;
			association.Reversed += new EventHandler(association_Reversed);
		}

		internal AssociationRelationship AssociationRelationship
		{
			get { return association; }
		}

		protected internal override Relationship Relationship
		{
			get { return association; }
		}

		protected override Size StartCapSize
		{
			get
			{
				if (AssociationRelationship.AssociationType != AssociationType.Association)
					return new Size(DiamondWidth, DiamondHeight);
				else
					return Size.Empty;
			}
		}

		protected override Size EndCapSize
		{
			get
			{
				if (AssociationRelationship.Direction == Direction.Unidirectional)
					return Arrowhead.OpenArrowSize;
				else
					return Size.Empty;
			}
		}

		protected override int StartSelectionOffset
		{
			get
			{
				if (AssociationRelationship.AssociationType != AssociationType.Association)
					return DiamondHeight;
				else
					return 0;
			}
		}

		protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(Diagram diagram)
		{
			return AssociationContextMenu.Default.GetMenuItems(diagram);
		}

		protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
		{
			base.OnDoubleClick(e);

			if (!e.Handled)
			{
				ShowEditDialog();
			}
		}

		protected internal override void ShowEditor()
		{
			ShowEditDialog();
		}

		public void ShowEditDialog()
		{
			using (AssociationDialog dialog = new AssociationDialog())
			{
				dialog.Association = AssociationRelationship;
				dialog.ShowDialog();
			}
		}

		private void association_Reversed(object sender, EventArgs e)
		{
			Reverse();
		}

		protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
		{
			TypeBase firstType = first.Entity as TypeBase;
			TypeBase secondType = second.Entity as TypeBase;

			if (firstType != null && secondType != null)
			{
				AssociationRelationship clone = association.Clone(firstType, secondType);
				return diagram.InsertAssociation(clone);
			}
			else
			{
				return false;
			}
		}

		public override void Draw(IGraphics g, bool onScreen, Style style)
		{
			base.Draw(g, onScreen, style);

			DrawStartRole(g, style);
			DrawEndRole(g, style);
			DrawStartMultiplicity(g, style);
			DrawEndMultiplicity(g, style);
		}

		private void DrawStartRole(IGraphics g, Style style)
		{
			string startRole = AssociationRelationship.StartRole;
			if (startRole != null)
			{
				DrawRole(g, style, startRole, RouteCache[0], RouteCache[1], StartCapSize);
			}
		}

		private void DrawEndRole(IGraphics g, Style style)
		{
			string endRole = AssociationRelationship.EndRole;
			if (endRole != null)
			{
				int last = RouteCache.Count - 1;
				DrawRole(g, style, endRole, RouteCache[last], RouteCache[last - 1], EndCapSize);
			}
		}

		private void DrawRole(IGraphics g, Style style, string text, Point firstPoint,
			Point secondPoint, Size capSize)
		{
			float angle = GetAngle(firstPoint, secondPoint);
			Point point = firstPoint;

			if (angle == 0) // Down
			{
				point.X -= capSize.Width / 2 + TextMargin.Width;
				point.Y += style.ShadowOffset.Height + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Far;
				stringFormat.LineAlignment = StringAlignment.Near;
			}
			else if (angle == 90) // Left
			{
				point.X -= TextMargin.Width;
				point.Y += capSize.Width / 2 + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Far;
				stringFormat.LineAlignment = StringAlignment.Near;
			}
			else if (angle == 180) // Up
			{
				point.X -= capSize.Width / 2 + TextMargin.Width;
				point.Y -= TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Far;
				stringFormat.LineAlignment = StringAlignment.Far;
			}
			else // Right
			{
				point.X += style.ShadowOffset.Width + TextMargin.Width;
				point.Y += capSize.Width / 2 + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Near;
			}

			textBrush.Color = style.RelationshipTextColor;
			g.DrawString(text, style.RelationshipTextFont, textBrush, point, stringFormat);
		}

		private void DrawStartMultiplicity(IGraphics g, Style style)
		{
			string startMultiplicity = AssociationRelationship.StartMultiplicity;
			if (startMultiplicity != null)
			{
				DrawMultiplicity(g, style, startMultiplicity, RouteCache[0],
					RouteCache[1], StartCapSize);
			}
		}

		private void DrawEndMultiplicity(IGraphics g, Style style)
		{
			string endMultiplicity = AssociationRelationship.EndMultiplicity;
			if (endMultiplicity != null)
			{
				int last = RouteCache.Count - 1;
				DrawMultiplicity(g, style, endMultiplicity, RouteCache[last],
					RouteCache[last - 1], EndCapSize);
			}
		}

		private void DrawMultiplicity(IGraphics g, Style style, string text, Point firstPoint,
			Point secondPoint, Size capSize)
		{
			float angle = GetAngle(firstPoint, secondPoint);
			Point point = firstPoint;

			if (angle == 0) // Down
			{
				point.X += capSize.Width / 2 + TextMargin.Width;
				point.Y += style.ShadowOffset.Height + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Near;
			}
			else if (angle == 90) // Left
			{
				point.X -= TextMargin.Width;
				point.Y -= capSize.Width / 2 + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Far;
				stringFormat.LineAlignment = StringAlignment.Far;
			}
			else if (angle == 180) // Up
			{
				point.X += capSize.Width / 2 + TextMargin.Width;
				point.Y -= TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Far;
			}
			else // Right
			{
				point.X += style.ShadowOffset.Width + TextMargin.Width;
				point.Y -= capSize.Width / 2 + TextMargin.Height;
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Far;
			}

			textBrush.Color = style.RelationshipTextColor;
			g.DrawString(text, style.RelationshipTextFont, textBrush, point, stringFormat);
		}

		protected override void DrawStartCap(IGraphics g, bool onScreen, Style style)
		{
			linePen.Color = style.RelationshipColor;
			linePen.Width = style.RelationshipWidth;

			if (association.IsAggregation)
			{
				g.FillPolygon(Brushes.White, diamondPoints);
				g.DrawPolygon(linePen, diamondPoints);
			}
			else if (association.IsComposition)
			{
				lineBrush.Color = style.RelationshipColor;

				g.FillPolygon(lineBrush, diamondPoints);
				g.DrawPolygon(linePen, diamondPoints);
			}
		}

		protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
		{
			if (association.Direction == Direction.Unidirectional)
			{
				linePen.Color = style.RelationshipColor;
				linePen.Width = style.RelationshipWidth;
				g.DrawLines(linePen, Arrowhead.OpenArrowPoints);
			}
		}

		protected override RectangleF CalculateDrawingArea(Style style, bool printing, float zoom)
		{
			RectangleF area = base.CalculateDrawingArea(style, printing, zoom);

			if (AssociationRelationship.StartRole != null)
				area = RectangleF.Union(area, GetStartRoleArea(style));

			if (AssociationRelationship.EndRole != null)
				area = RectangleF.Union(area, GetEndRoleArea(style));

			if (AssociationRelationship.StartMultiplicity != null)
				area = RectangleF.Union(area, GetStartMultiplicityArea(style));

			if (AssociationRelationship.EndMultiplicity != null)
				area = RectangleF.Union(area, GetEndMultiplicityArea(style));

			return area;
		}

		private RectangleF GetStartRoleArea(Style style)
		{
			return GetRoleArea(style, AssociationRelationship.StartRole,
				RouteCache[0], RouteCache[1], StartCapSize);
		}

		private RectangleF GetEndRoleArea(Style style)
		{
			int last = RouteCache.Count - 1;
			return GetRoleArea(style, AssociationRelationship.EndRole,
				RouteCache[last], RouteCache[last - 1], EndCapSize);
		}

		private RectangleF GetRoleArea(Style style, string text, Point firstPoint,
			Point secondPoint, Size capSize)
		{
			float angle = GetAngle(firstPoint, secondPoint);

			SizeF textSize = Graphics.MeasureString(text, style.RelationshipTextFont,
				PointF.Empty, stringFormat);
			RectangleF area = new RectangleF(firstPoint, textSize);

			if (angle == 0) // Down
			{
				area.X -= textSize.Width + capSize.Width / 2 + TextMargin.Width;
				area.Y += style.ShadowOffset.Height + TextMargin.Height;
			}
			else if (angle == 90) // Left
			{
				area.X -= textSize.Width + TextMargin.Width;
				area.Y += capSize.Width / 2 + TextMargin.Height;
			}
			else if (angle == 180) // Up
			{
				area.X -= textSize.Width + capSize.Width / 2 + TextMargin.Width;
				area.Y -= textSize.Height + TextMargin.Height;
			}
			else // Right
			{
				area.X += style.ShadowOffset.Width + TextMargin.Width;
				area.Y += capSize.Width / 2 + TextMargin.Height;
			}

			return area;
		}

		private RectangleF GetStartMultiplicityArea(Style style)
		{
			return MultiplicityArea(style, AssociationRelationship.StartMultiplicity,
				RouteCache[0], RouteCache[1], StartCapSize);
		}

		private RectangleF GetEndMultiplicityArea(Style style)
		{
			int last = RouteCache.Count - 1;
			return MultiplicityArea(style, AssociationRelationship.EndMultiplicity,
				RouteCache[last], RouteCache[last - 1], EndCapSize);
		}

		private RectangleF MultiplicityArea(Style style, string text, Point firstPoint,
			Point secondPoint, Size capSize)
		{
			float angle = GetAngle(firstPoint, secondPoint);

			SizeF textSize = Graphics.MeasureString(text, style.RelationshipTextFont,
				PointF.Empty, stringFormat);
			RectangleF area = new RectangleF(firstPoint, textSize);

			if (angle == 0) // Down
			{
				area.X += capSize.Width / 2 + TextMargin.Width;
				area.Y += style.ShadowOffset.Height + TextMargin.Height;
			}
			else if (angle == 90) // Left
			{
				area.X -= textSize.Width + TextMargin.Width;
				area.Y -= textSize.Height + capSize.Width / 2 + TextMargin.Height;
			}
			else if (angle == 180) // Up
			{
				area.X += capSize.Width / 2 + TextMargin.Width;
				area.Y -= textSize.Height + TextMargin.Height;
			}
			else // Right
			{
				area.X += style.ShadowOffset.Width + TextMargin.Width;
				area.Y -= textSize.Height + capSize.Width / 2 + TextMargin.Height;
			}

			return area;
		}
	}
}
