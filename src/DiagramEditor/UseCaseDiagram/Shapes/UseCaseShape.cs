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
    public class UseCaseShape : Shape
    {
        private UseCase useCase;
        const int PaddingSize = 10;
        const int DefaultWidth = 160;
        const int DefaultHeight = 75;

        public UseCaseShape(UseCase entity) : base(entity)
        {
            this.useCase = entity;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            g.DrawString(Entity.Name, style.UseCaseFont, new SolidBrush(style.UseCaseTextColor), new PointF(Location.X, Location.Y));
        }

        protected override Size DefaultSize { get; }
        public override IEntity Entity {
            get { return this.useCase; }
        }
        protected override int GetBorderWidth(Style style)
        {
            return style.UseCaseBorderWidth;
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

            return useCaseDiagram.InsertUseCase(useCase.Clone());
        }
    }
}