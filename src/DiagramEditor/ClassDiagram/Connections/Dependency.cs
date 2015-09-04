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
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
	internal sealed class Dependency : Connection
	{
		static Pen linePen = new Pen(Color.Black);

		DependencyRelationship dependency;

		static Dependency()
		{
			linePen.MiterLimit = 2.0F;
			linePen.LineJoin = LineJoin.MiterClipped;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="dependency"/> is null.-or-
		/// <paramref name="startShape"/> is null.-or-
		/// <paramref name="endShape"/> is null.
		/// </exception>
		public Dependency(DependencyRelationship dependency, Shape startShape, Shape endShape)
			: base(dependency, startShape, endShape)
		{
			this.dependency = dependency;
		}

		internal DependencyRelationship DependencyRelationship
		{
			get { return dependency; }
		}

		protected internal override Relationship Relationship
		{
			get { return dependency; }
		}

		protected override bool IsDashed
		{
			get { return true; }
		}

		protected override Size EndCapSize
		{
			get { return Arrowhead.OpenArrowSize; }
		}

		protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
		{
			linePen.Color = style.RelationshipColor;
			linePen.Width = style.RelationshipWidth;
			g.DrawLines(linePen, Arrowhead.OpenArrowPoints);
		}

		protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
		{
			TypeBase firstType = first.Entity as TypeBase;
			TypeBase secondType = second.Entity as TypeBase;

			if (firstType != null && secondType != null)
			{
				DependencyRelationship clone = dependency.Clone(firstType, secondType);
				return diagram.InsertDependency(clone);
			}
			else
			{
				return false;
			}
		}
	}
}
