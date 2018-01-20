// NClass - Free class diagram editor
// Copyright (C) 2017 Georgi Baychev
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

using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Connection
{
    public class ExtendsConnection : Diagrams.Connections.Connection
    {
        private ExtendsRelationship extendsRelationship;
        private Pen linePen = new Pen(Color.Black);

        public ExtendsConnection(Relationship relationship, Shape firstShape, Shape secondShape) 
            : base(relationship, firstShape, secondShape)
        {
            this.extendsRelationship = (ExtendsRelationship)relationship;
        }

        protected internal override Relationship Relationship
        {
            get { return extendsRelationship; }
        }
        protected override bool CloneRelationship(IDiagram diagram, Shape first, Shape second)
        {
            if (diagram.DiagramType != DiagramType.UseCaseDiagram)
                return false;

            UseCase firstType = first.Entity as UseCase;
            UseCase secondType = second.Entity as UseCase;

            if (firstType != null && secondType != null)
            {
                var clone = extendsRelationship.Clone(firstType, secondType);
                return ((UseCaseDiagram)diagram).InsertExtends(clone);
            }
            else
            {
                return false;
            }
        }

        protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
        {
            this.linePen.Color = style.RelationshipColor;
            this.linePen.Width = style.RelationshipWidth;

            g.DrawLines(linePen, Arrowhead.OpenArrowPoints);
        }
    }
}