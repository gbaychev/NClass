// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Diagrams.Connections
{
    public abstract class AbstractConnection : DiagramElement
    { 
        protected Shape startShape;
        protected Shape endShape;

        private static Pen linePen = new Pen(Color.Black);
        protected static SolidBrush textBrush = new SolidBrush(Color.Black);
        protected static StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
        protected static readonly Size TextMargin = new Size(5, 3);

        protected bool copied = false;

        protected const int PickTolerance = 4;

        protected static readonly float[] dashPattern = new float[] { 5, 5 };

        protected AbstractConnection(Relationship relationship, Shape startShape, Shape endShape)
        {
            if (relationship == null)
                throw new ArgumentNullException("relationship");

            this.startShape = startShape ?? throw new ArgumentNullException("startShape");
            this.endShape = endShape ?? throw new ArgumentNullException("endShape");

            startShape.Move += ShapeMoving;
            startShape.Resize += StartShapeResizing;
            endShape.Move += ShapeMoving;
            endShape.Resize += EndShapeResizing;

            relationship.Modified += delegate { OnModified(ModificationEventArgs.Empty); };

            relationship.Detaching += delegate
            {
                startShape.Move -= ShapeMoving;
                startShape.Resize -= StartShapeResizing;
                endShape.Move -= ShapeMoving;
                endShape.Resize -= EndShapeResizing;
            };
            relationship.Serializing += delegate (object sender, SerializeEventArgs e)
            {
                OnSerializing(e);
            };
            relationship.Deserializing += delegate (object sender, SerializeEventArgs e)
            {
                OnDeserializing(e);
            };
        }

        /// <summary>
        /// Called when reinserting shapes via undo/redo
        /// </summary>
        internal void Reattach()
        {
            Debug.Assert(startShape != null);
            Debug.Assert(endShape != null);

            startShape.Move += ShapeMoving;
            startShape.Resize += StartShapeResizing;
            endShape.Move += ShapeMoving;
            endShape.Resize += EndShapeResizing;
        }

        protected virtual Size StartCapSize => Size.Empty;

        protected virtual Size EndCapSize => Size.Empty;

        protected virtual int StartSelectionOffset => 0;

        protected virtual int EndSelectionOffset => 0;

        protected virtual bool IsDashed => false;

        protected abstract bool CloneRelationship(IDiagram diagram, Shape first, Shape second);

        protected internal abstract Relationship Relationship
        {
            get;
        }

        public Shape StartShape => startShape;

        public Shape EndShape => endShape;

        protected virtual void ShapeMoving(object sender, MoveEventArgs e)
        {
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void StartShapeResizing(object sender, ResizeEventArgs e)
        {
            OnModified(ModificationEventArgs.Empty);
        }

        protected virtual void EndShapeResizing(object sender, ResizeEventArgs e)
        {
            OnModified(ModificationEventArgs.Empty);
        }

        protected abstract void OnSerializing(SerializeEventArgs e);

        protected abstract void OnDeserializing(SerializeEventArgs e);

        protected internal override void Offset(Size offset)
        {
            // nothing
        }

        protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(IDiagram diagram, PointF? openedAt = null)
        {
            return ConnectionContextMenu.Default.GetMenuItems(diagram);
        }

        public override string ToString()
        {
            return Relationship.ToString();
        }

        protected float GetAngle(Point point1, Point point2)
        {
            if (point1.X == point2.X)
            {
                return (point1.Y < point2.Y) ? 0 : 180;
            }
            else if (point1.Y == point2.Y)
            {
                return (point1.X < point2.X) ? 270 : 90;
            }
            else
            {
                return (float)(
                           Math.Atan2(point2.Y - point1.Y, point2.X - point1.X) * (180 / Math.PI)) - 90;
            }
        }

        internal override void MouseMoved(AbsoluteMouseEventArgs e)
        {
            if (!e.Handled)
            {
                bool moved = IsMousePressed;

                if (moved)
                {
                    e.Handled = true;
                    OnMouseMove(e);
                }
            }
        }

        internal override void MouseUpped(AbsoluteMouseEventArgs e)
        {
            if (!e.Handled)
            {
                bool upped = IsMousePressed;

                if (upped)
                {
                    e.Handled = true;
                    OnMouseUp(e);
                }
            }
        }

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsActive = true;
            }
            base.OnMouseDown(e);
            copied = false;
        }

        protected internal abstract AbstractConnection Paste(IDiagram diagram, Size offset, Shape first, Shape second);
        protected abstract bool Picked(PointF mouseLocation, float zoom);
        protected abstract bool Picked(RectangleF rectangle);

        protected virtual void DrawStartCap(IGraphics g, bool onScreen, Style style)
        {
        }

        protected virtual void DrawEndCap(IGraphics g, bool onScreen, Style style)
        {
        }

        protected void DrawLine(IGraphics g, bool onScreen, Style style, Point[] linePoints)
        {
            if (!IsSelected || !onScreen)
            {
                linePen.Width = style.RelationshipWidth;
                linePen.Color = style.RelationshipColor;
                if (IsDashed)
                {
                    dashPattern[0] = style.RelationshipDashSize;
                    dashPattern[1] = style.RelationshipDashSize;
                    linePen.DashPattern = dashPattern;
                }
                else
                {
                    linePen.DashStyle = DashStyle.Solid;
                }

                g.DrawLines(linePen, linePoints);
            }
        }
    }
}