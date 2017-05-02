// // NClass - Free class diagram editor
// // Copyright (C) 2016 Georgi Baychev
// // 
// // This program is free software; you can redistribute it and/or modify it under 
// // the terms of the GNU General Public License as published by the Free Software 
// // Foundation; either version 3 of the License, or (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful, but WITHOUT 
// // ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// // FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with 
// // this program; if not, write to the Free Software Foundation, Inc., 
// // 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class ActorShape : Shape
    {
        private Actor actor;

        public ActorShape(Actor entity) : base(entity)
        {
            actor = entity;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            g.DrawString("Actor", style.StaticMemberFont, new SolidBrush(Color.Black), new PointF(Location.X, Location.Y));
        }

        protected override Size DefaultSize { get; }
        public override IEntity Entity {
            get
            {
                return this.actor;
            }
        }
        protected override int GetBorderWidth(Style style)
        {
            //FIXME
            return style.CommentBorderWidth;
        }

        protected override float GetRequiredWidth(Graphics g, Style style)
        {
            return Width;
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            var useCaseDiagram = diagram as UseCaseDiagram;
            if (useCaseDiagram == null)
                return false;

            return useCaseDiagram.InsertActor(actor.Clone());
        }
    }
}