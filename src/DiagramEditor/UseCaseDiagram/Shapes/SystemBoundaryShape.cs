// NClass - Free class diagram editor
// Copyright (C) 2019 Georgi Baychev
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
using System.Linq;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class SystemBoundaryShape : ShapeContainer
    {
        private static readonly Size defaultSize = new Size(300, 300);

        private SystemBoundary systemBoundary;
        public SystemBoundaryShape(SystemBoundary entity) : base(entity)
        {
            this.systemBoundary = entity;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            using (var pen = new Pen(Color.Black))
            {
                g.DrawRectangle(pen, this.BorderRectangle);
            }
        }

        protected override Size DefaultSize => defaultSize;
        public override IEntity Entity
        {
            get => systemBoundary;
        }

        protected override int GetBorderWidth(Style style)
        {
            return this.size.Width;
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (diagram.DiagramType != DiagramType.UseCaseDiagram)
                return false;

            return ((UseCaseDiagram) diagram).InsertSystemBoundary(this.systemBoundary.Clone());
        }

        protected override bool AcceptsEntity(EntityType type)
        {
            return type == EntityType.UseCase;
        }

        public static Rectangle GetOutline(Style currentStyle)
        {
            return new Rectangle(new Point(0,0), defaultSize);
        }

        protected override void OnMove(MoveEventArgs e)
        {
            base.OnMove(e);

            // drag together only the non-selected shapes
            foreach (var shape in this.ChildrenShapes.Where(s => !s.IsSelected))
            {
                shape.Location += new Size((int)e.Offset.Width, (int)e.Offset.Height);
            }
            HideEditor();
        }
    }
}