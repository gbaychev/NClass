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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Diagrams.Connections
{
    public abstract class SimpleConnection : AbstractConnection
    {
        private Point startPoint, endPoint;

        public SimpleConnection(Relationship relationship, Shape startShape, Shape endShape) 
            : base(relationship, startShape, endShape)
        {
        }

        public void CalculatePoints()
        {
            var startShapeHorizontalMiddle = (startShape.Left + startShape.Width) / 2;
            var endShapeHorizontalMiddle = (endShape.Left + endShape.Width) / 2;

            var startShapeVerticalMiddle = (startShape.Top + startShape.Bottom) / 2;
            var endShapeVerticalMiddle = (endShape.Top + endShape.Bottom) / 2;


        }

        protected override RectangleF CalculateDrawingArea(Style style, bool printing, float zoom)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            throw new System.NotImplementedException();
        }

        protected internal override Rectangle GetLogicalArea()
        {
            throw new System.NotImplementedException();
        }

        protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
        {
            throw new System.NotImplementedException();
        }

        protected internal override bool TrySelect(RectangleF frame)
        {
            throw new System.NotImplementedException();
        }

        protected internal override Size GetMaximumPositionChange(Size offset, int padding)
        {
            throw new System.NotImplementedException();
        }


        [Obsolete]
        protected internal override void Serialize(XmlElement node)
        {
            OnSerializing(new SerializeEventArgs(node));
        }

        [Obsolete]
        protected internal override void Deserialize(XmlElement node)
        {
            OnDeserializing(new SerializeEventArgs(node));
        }

        internal override void MousePressed(AbsoluteMouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        internal override void MouseMoved(AbsoluteMouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        internal override void MouseUpped(AbsoluteMouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        internal override void DoubleClicked(AbsoluteMouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}