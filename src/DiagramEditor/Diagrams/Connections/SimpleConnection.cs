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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Diagrams.Connections
{
    public abstract class SimpleConnection : AbstractConnection
    {
        private Point startPoint, endPoint;

        public SimpleConnection(Relationship relationship, Shape startShape, Shape endShape) 
            : base(relationship, startShape, endShape)
        {
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            var startShapeHorizontalMiddle = startShape.Left + startShape.Width / 2;
            var endShapeHorizontalMiddle = endShape.Left + endShape.Width / 2;

            var startShapeVerticalMiddle = startShape.Top + startShape.Height / 2;
            var endShapeVerticalMiddle = endShape.Top + endShape.Height / 2;

            if (startShape.Right <= endShape.Left)
            {
                startPoint = new Point(startShape.Right, startShapeVerticalMiddle);
                endPoint = new Point(endShape.Left, endShapeVerticalMiddle);
            }
            else if (endShape.Right <= startShape.Left)
            {
                startPoint = new Point(startShape.Left, startShapeVerticalMiddle);
                endPoint = new Point(endShape.Right, endShapeVerticalMiddle);
            }
            else
            {
                if (startShape.Top < endShape.Top)
                {
                    startPoint = new Point(startShapeHorizontalMiddle, startShape.Bottom);
                    endPoint = new Point(endShapeHorizontalMiddle, endShape.Top);
                }
                else
                {
                    startPoint = new Point(startShapeHorizontalMiddle, startShape.Top);
                    endPoint = new Point(endShapeHorizontalMiddle, endShape.Bottom);
                }
            }
        }

        protected override RectangleF CalculateDrawingArea(Style style, bool printing, float zoom)
        {
            var angle = GetAngle(startPoint, endPoint);
            RectangleF area = GetLogicalArea();
            var lineSize = (float)style.RelationshipWidth / 2;
            area.Inflate(lineSize, lineSize);

            if (Relationship.Label != null)
               area = RectangleF.Union(area, GetLabelArea(style));

            if (StartCapSize != Size.Empty)
            {
                var startCapArea = GetStartCapArea(angle);
                if(startCapArea != RectangleF.Empty)
                    area = RectangleF.Union(area, startCapArea);
            }

            if (EndCapSize != Size.Empty)
            {
                var endCapArea = GetEndCapArea(angle);
                if (endCapArea != RectangleF.Empty)
                    area = RectangleF.Union(area, endCapArea);
            }

            return area;
        }

        private RectangleF GetLabelArea(Style style)
        {
            PointF center = GetLineCenter();

            SizeF size = Graphics.MeasureString(Relationship.Label,
                style.RelationshipTextFont, center, stringFormat);

            center.X -= size.Width / 2;
            center.Y -= size.Height + TextMargin.Height;

            return new RectangleF(center.X, center.Y, size.Width, size.Height);
        }

        private RectangleF GetEndCapArea(float angle)
        {
            
            if (angle == 90) // right
            {
                var area = new RectangleF(endPoint.X, endPoint.Y, EndCapSize.Height, EndCapSize.Width);
                area.Y -= (float)area.Height / 2;
                return area;
            }

            if (angle == 270) // Left
            {
                var area = new RectangleF(endPoint.X, endPoint.Y, EndCapSize.Height, EndCapSize.Width);
                area.Y -= (float)area.Height / 2;
                area.X -= (float)area.Width;
                return area;
            }

            if (angle == 0) // down
            {
                var area = new RectangleF(endPoint.X, endPoint.Y, EndCapSize.Width, EndCapSize.Height);
                area.Y -= (float)area.Height;
                area.X -= (float)area.Width / 2;
                return area;
            }

            if (angle == 180) // up
            {
                var area = new RectangleF(endPoint.X, endPoint.Y, EndCapSize.Width, EndCapSize.Height);
                area.X -= (float)area.Width / 2;
                return area;
            }

            return RectangleF.Empty;
        }

        private RectangleF GetStartCapArea(float angle)
        {

            if (angle == 90) // right
            {
                var area = new RectangleF(startPoint.X, startPoint.Y, StartCapSize.Height, StartCapSize.Width);
                area.Y -= (float)area.Height / 2;
                area.X -= (float)area.Width;
                return area;
            }

            if (angle == 270) // Left
            {
                var area = new RectangleF(startPoint.X, startPoint.Y, StartCapSize.Height, StartCapSize.Width);
                area.Y -= (float)area.Height / 2;
                return area;
            }

            if (angle == 0) // down
            {
                var area = new RectangleF(startPoint.X, startPoint.Y, StartCapSize.Width, StartCapSize.Height);
                area.X -= (float)area.Width / 2;
                return area;
            }

            if (angle == 180) // up
            {
                var area = new RectangleF(startPoint.X, startPoint.Y, StartCapSize.Width, StartCapSize.Height);
                area.Y -= (float)area.Height;
                area.X -= (float)area.Width / 2;
                return area;
            }

            return RectangleF.Empty;
        }


        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            var linePoints = new[] { startPoint, endPoint};
            DrawLine(g, onScreen, style, linePoints);
            DrawCaps(g, onScreen, style);
            if(Relationship.SupportsLabel)
                DrawLabel(g, onScreen, style);
        }

        protected void DrawCaps(IGraphics g, bool onScreen, Style style)
        {
            Matrix transformState = g.Transform;
            g.TranslateTransform(startPoint.X, startPoint.Y);
            g.RotateTransform(GetAngle(startPoint, endPoint));
            DrawStartCap(g, onScreen, style);
            g.Transform = transformState;

            g.TranslateTransform(endPoint.X, endPoint.Y);
            g.RotateTransform(GetAngle(endPoint, startPoint));
            DrawEndCap(g, onScreen, style);
            g.Transform = transformState;
        }

        private PointF GetLineCenter()
        {
            return new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
        }

        private void DrawLabel(IGraphics g, bool onScreen, Style style)
        {
            if (Relationship.Label == null) return;

            PointF center = GetLineCenter();

            textBrush.Color = style.RelationshipTextColor;
                
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Far;
            center.Y -= TextMargin.Height;
                
            g.DrawString(Relationship.Label, style.RelationshipTextFont, textBrush, center, stringFormat);
        }

        protected internal override Rectangle GetLogicalArea()
        {
            var topLeft = startPoint;
            var bottomRight = endPoint;

            if (topLeft.X > endPoint.X)
                topLeft.X = endPoint.X;
            if (topLeft.Y > endPoint.Y)
                topLeft.Y = endPoint.Y;
            if (bottomRight.X < startPoint.X)
                bottomRight.X = startPoint.X;
            if (bottomRight.Y < startPoint.Y)
                bottomRight.Y = startPoint.Y;

            return Rectangle.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
        {
            if (!IsSelected)
                return;

            var oldSmoothingMode = g.SmoothingMode;
            var oldTransform = g.Transform;
            g.Transform = new Matrix();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLine(DiagramConstants.SelectionPen, startPoint, endPoint);
            g.SmoothingMode = oldSmoothingMode;
            g.Transform = oldTransform;
        }

        protected internal override bool TrySelect(RectangleF frame)
        {
            this.IsSelected = Picked(frame);
            return this.IsSelected;
        }

        protected internal override Size GetMaximumPositionChange(Size offset, int padding)
        {
            if (!IsSelected && !startShape.IsSelected && !endShape.IsSelected)
                return offset;

            Point newLocation = startPoint + offset;

            if (newLocation.X < padding)
                offset.Width += (padding - newLocation.X);
            if (newLocation.Y < padding)
                offset.Height += (padding - newLocation.Y);

            newLocation = endPoint + offset;

            if (newLocation.X < padding)
                offset.Width += (padding - newLocation.X);
            if (newLocation.Y < padding)
                offset.Height += (padding - newLocation.Y);

            return offset;
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
            if (e.Handled) return;

            if (Picked(e.Location, e.Zoom))
            {
                e.Handled = true;
                OnMouseDown(e);
            }
        }

        internal override void MouseMoved(AbsoluteMouseEventArgs e)
        {
            base.MouseMoved(e);
            if (e.Button == MouseButtons.Left)
            {
                CalculatePoints();
            }
        }

        internal override void MouseUpped(AbsoluteMouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        internal override void DoubleClicked(AbsoluteMouseEventArgs e)
        {
            bool doubleClicked = Picked(e.Location, e.Zoom);

            if (e.Button == MouseButtons.Left)
                doubleClicked |= (IsSelected);

            if (doubleClicked)
            {
                OnDoubleClick(e);
                e.Handled = true;
            }
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

                connection.startPoint = this.startPoint;
                connection.endPoint = this.endPoint;

                return connection;
            }
            else
            {
                return null;
            }
        }

        protected override void OnSerializing(SerializeEventArgs e)
        {
            var document = e.Node.OwnerDocument;

            var startNode = document.CreateElement("StartPoint");
            startNode.SetAttribute("x", startPoint.X.ToString());
            startNode.SetAttribute("y", startPoint.Y.ToString());
            e.Node.AppendChild(startNode);

            var endNode = document.CreateElement("EndPoint");
            endNode.SetAttribute("x", endPoint.X.ToString());
            endNode.SetAttribute("y", endPoint.Y.ToString());
            e.Node.AppendChild(endNode);
        }

        protected override void OnDeserializing(SerializeEventArgs e)
        {
            var startNode = e.Node["StartPoint"];
            if (startNode != null)
            {
                int.TryParse(startNode.GetAttribute("x"), out var x);
                int.TryParse(startNode.GetAttribute("y"), out var y);

                startPoint = new Point(x, y);
            }
            var endNode = e.Node["EndPoint"];
            if (endNode != null)
            {
                int.TryParse(endNode.GetAttribute("x"), out var x);
                int.TryParse(endNode.GetAttribute("y"), out var y);

                endPoint = new Point(x, y);
            }
        }
    }
}