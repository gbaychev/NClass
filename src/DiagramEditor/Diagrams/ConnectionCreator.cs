using System.Drawing;
using System.Drawing.Drawing2D;
using NClass.Core;
using NClass.DiagramEditor.Diagrams.Shapes;

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

            foreach (Shape shape in diagram.Shapes)
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

        protected abstract void CreateConnection();
    }
}