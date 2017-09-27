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

using System;
using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class ActorShape : Shape
    {
        private Actor actor;
        private const int DefaultWidth = 75;
        private const int DefaultHeight = 160;
        private const int PaddingSize = 7;
        private readonly Size defaultSize = new Size(DefaultWidth, DefaultHeight);


        public ActorShape(Actor entity) : base(entity)
        {
            actor = entity;
            this.MinimumSize = defaultSize;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            DrawSurface(g, onScreen, style);
            DrawText(g, onScreen, style);
        }

        private void DrawText(IGraphics graphics, bool onScreen, Style style)
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            graphics.DrawString(
                                Entity.Name,
                                style.StaticMemberFont,
                                new SolidBrush(Color.Black),
                                GetTextRectangle(graphics, style),
                                stringFormat);
        }

        private void DrawSurface(IGraphics graphics, bool onScreen, Style style)
        {
            graphics.FillEllipse(new SolidBrush(Color.BlueViolet), new Rectangle(Left, Top, Width, (Height * 9 / 10) - PaddingSize));
        }

        protected override Size DefaultSize
        {
            get { return defaultSize; }
        }
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

        private Rectangle GetTextRectangle(IGraphics g, Style style)
        {
            float textHeight = g.MeasureString(this.Entity.Name, style.StaticMemberFont).Height;
            int left = this.Left + PaddingSize;
            int top = (int)Math.Ceiling(this.Bottom - textHeight - 2 * PaddingSize);
            int width = this.Width - PaddingSize;
            int height = (int)Math.Ceiling(textHeight + 2 * PaddingSize);
            return new Rectangle(left, top, width, height);
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