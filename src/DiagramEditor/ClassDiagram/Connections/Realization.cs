﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2025 Georgi Baychev
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
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
    internal sealed class Realization : RoutedConnection
    {
        static Pen linePen = new Pen(Color.Black);

        RealizationRelationship realization;

        static Realization()
        {
            linePen.MiterLimit = 2.0F;
            linePen.LineJoin = LineJoin.MiterClipped;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="realization"/> is null.-or-
        /// <paramref name="startShape"/> is null.-or-
        /// <paramref name="endShape"/> is null.
        /// </exception>
        public Realization(RealizationRelationship realization, Shape startShape, Shape endShape)
            : base(realization, startShape, endShape)
        {
            this.realization = realization;
        }

        internal RealizationRelationship RealizationRelationship
        {
            get { return realization; }
        }

        protected internal override Relationship Relationship
        {
            get { return realization; }
        }

        protected override bool IsDashed
        {
            get { return true; }
        }

        protected override Size EndCapSize
        {
            get { return Arrowhead.ClosedArrowSize; }
        }

        protected override int EndSelectionOffset
        {
            get { return Arrowhead.ClosedArrowHeight; }
        }

        protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
        {
            linePen.Color = style.RelationshipColor;
            linePen.Width = style.RelationshipWidth;

            g.FillPath(Brushes.White, Arrowhead.ClosedArrowPath);
            g.DrawPath(linePen, Arrowhead.ClosedArrowPath);
        }

        protected override bool CloneRelationship(IDiagram diagram, Shape first, Shape second)
        {
            if (diagram.DiagramType != DiagramType.ClassDiagram)
                return false;

            TypeBase firstType = first.Entity as TypeBase;
            InterfaceType secondType = second.Entity as InterfaceType;

            if (firstType != null && secondType != null)
            {
                RealizationRelationship clone = realization.Clone(firstType, secondType);
                return ((ClassDiagram)diagram).InsertRealization(clone);
            }
            else
            {
                return false;
            }
        }
    }
}
