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
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.ClassDiagram.Connections;
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

        protected override bool Picked(RectangleF rectangle)
        {
            if (LineIntersectsLine(startPoint, endPoint, new PointF(rectangle.X, rectangle.Y), new PointF(rectangle.X + rectangle.Width, rectangle.Y)))
                return true;
            if (LineIntersectsLine(startPoint, endPoint, new PointF(rectangle.X + rectangle.Width, rectangle.Y), new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)))
                return true;
            if (LineIntersectsLine(startPoint, endPoint, new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), new PointF(rectangle.X, rectangle.Y + rectangle.Height)))
                return true;
            if (LineIntersectsLine(startPoint, endPoint, new PointF(rectangle.X, rectangle.Y + rectangle.Height), new PointF(rectangle.X, rectangle.Y)))
                return true;

            return rectangle.Contains(startPoint) && rectangle.Contains(endPoint);
        }

        private bool LineIntersectsLine(PointF l1p1, PointF l1p2, PointF l2p1, PointF l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        protected override bool Picked(PointF mouseLocation, float zoom)
        {
            float tolerance = PickTolerance / zoom / 2;
            var p1 = startPoint;
            var p2 = endPoint;
            var totalDistance = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            var firstDistance = Math.Sqrt(Math.Pow(p1.X - mouseLocation.X, 2) + Math.Pow(p1.Y - mouseLocation.Y, 2));
            var secondDistance = Math.Sqrt(Math.Pow(p2.X - mouseLocation.X, 2) + Math.Pow(p2.Y - mouseLocation.Y, 2));
            return Math.Abs(firstDistance + secondDistance - totalDistance) < tolerance;
        }

        protected internal override AbstractConnection Paste(IDiagram diagram, Size offset,
            Shape first, Shape second)
        {
            if (CloneRelationship(diagram, first, second))
            {
                var connectionList = (ElementList<AbstractConnection>)diagram.Connections;
                var connection = (SimpleConnection)connectionList.FirstValue;
                connection.IsSelected = true;

                connection.startOrientation = this.startOrientation;
                connection.endOrientation = this.endOrientation;

                return connection;
            }
            else
            {
                return null;
            }
        }
    }
}