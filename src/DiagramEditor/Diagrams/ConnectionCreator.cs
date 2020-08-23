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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.Commands;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.Diagrams
{
    internal abstract class ConnectionCreator<T> : IConnectionCreator where T : IDiagram
    {
        protected const int BorderOffset = 8;
        protected const int BorderOffset2 = 12;
        protected const int Radius = 5;
        static readonly float[] dashPattern = new float[] { 3, 3 };
        protected static readonly Pen firstPen;
        protected static readonly Pen secondPen;
        protected static readonly Pen arrowPen;

        protected T diagram;
        protected RelationshipType type;
        protected bool firstSelected = false;
        protected bool created = false;
        protected Shape first = null;
        protected Shape second = null;

        static ConnectionCreator()
        {
            firstPen = new Pen(Color.Blue);
            firstPen.DashPattern = dashPattern;
            firstPen.Width = 1.5F;
            secondPen = new Pen(Color.Red);
            secondPen.DashPattern = dashPattern;
            secondPen.Width = 1.5F;
            arrowPen = new Pen(Color.Black);
            arrowPen.CustomEndCap = new AdjustableArrowCap(6, 7, true);
        }

        public ConnectionCreator(T diagram, RelationshipType type)
        {
            this.diagram = diagram;
            this.type = type;
        }

        public bool Created
        {
            get { return created; }
        }

        public void MouseMove(AbsoluteMouseEventArgs e)
        {
            Point mouseLocation = new Point((int)e.X, (int)e.Y);

            foreach (Shape shape in diagram.GetShapesInDisplayOrder())
            {
                if (shape.BorderRectangle.Contains(mouseLocation))
                {
                    if (!firstSelected)
                    {
                        if (first != shape)
                        {
                            first = shape;
                            diagram.Redraw();
                        }
                    }
                    else
                    {
                        if (second != shape)
                        {
                            second = shape;
                            diagram.Redraw();
                        }
                    }

                    return;
                }
            }

            if (!firstSelected)
            {
                if (first != null)
                {
                    first = null;
                    diagram.Redraw();
                }
            }
            else
            {
                if (second != null)
                {
                    second = null;
                    diagram.Redraw();
                }
            }
        }

        public void MouseDown(AbsoluteMouseEventArgs e)
        {
            if (!firstSelected)
            {
                if (first != null)
                    firstSelected = true;
            }
            else
            {
                if (second != null)
                    CreateConnection();
            }
        }

        public void Draw(Graphics g)
        {
            if (first != null)
            {
                Rectangle border = first.BorderRectangle;
                border.Inflate(BorderOffset, BorderOffset);
                g.DrawRectangle(firstPen, border);
            }

            if (second != null)
            {
                Rectangle border = second.BorderRectangle;
                if (second == first)
                    border.Inflate(BorderOffset2, BorderOffset2);
                else
                    border.Inflate(BorderOffset, BorderOffset);
                g.DrawRectangle(secondPen, border);
            }

            if (first != null && second != null)
            {
                g.DrawLine(arrowPen, first.CenterPoint, second.CenterPoint);
            }
        }

        protected virtual void CreateConnection()
        {
            switch (type)
            {
                case RelationshipType.Comment:
                    CreateCommentRelationship();
                    break;
            }

            created = true;
            diagram.Redraw();
        }

        private void CreateCommentRelationship()
        {
            CommentShape shape1 = first as CommentShape;
            CommentShape shape2 = second as CommentShape;
            Func<Relationship> _connectionFactory; 

            if (shape1 != null)
            {
                _connectionFactory = () => diagram.AddCommentRelationship(shape1.Comment, second.Entity);
            }
            else if (shape2 != null)
            {
                _connectionFactory = () => diagram.AddCommentRelationship(shape2.Comment, first.Entity);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
                return;
            }

            var command = new AddConnectionCommand(diagram, _connectionFactory);
            command.Execute();
            diagram.TrackCommand(command);
        }
    }
}