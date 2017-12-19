// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016 Georgi Baychev
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
using NClass.DiagramEditor.ClassDiagram.ContextMenus;

namespace NClass.DiagramEditor.Diagrams.Shapes
{
	public abstract class Shape : DiagramElement
	{
		protected enum ResizeMode
		{
			None = 0,
			Right = 1,
			Bottom = 2,
            Left = 4,
            Top = 8
        }

		public const int SelectionMargin = 12;
		static readonly Pen selectionSquarePen = new Pen(Color.Black);
		protected static readonly float[] borderDashPattern = new float[] { 3, 3 };
		protected static readonly SolidBrush shadowBrush = new SolidBrush(Color.Gray);
		protected static readonly Size defaultMinSize = new Size(50, 50);

		Point location;
		Size size;
		ResizeMode resizeMode = ResizeMode.None;
		Size minimumSize = defaultMinSize;
		bool mouseLeft = true;
		Cursor cursor = Cursors.Default;

		public event MoveEventHandler Move;
		public event MoveEventHandler Dragging;
		public event ResizeEventHandler Resizing;
		public event ResizeEventHandler Resize;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="entity"/> is null.
		/// </exception>
		protected Shape(IEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			location = Point.Empty;
			size = DefaultSize;

			entity.Modified += delegate { OnModified(EventArgs.Empty); };

			entity.Serializing += delegate(object sender, SerializeEventArgs e) {
				OnSerializing(e);
			};
			entity.Deserializing += delegate(object sender, SerializeEventArgs e) {
				OnDeserializing(e);
			};
		}

		protected abstract Size DefaultSize
		{
			get;
		}

		protected Size MinimumSize
		{
			get
			{
				return minimumSize;
			}
			set
			{
				minimumSize = value;
				if (minimumSize.Width > size.Width)
					Width = value.Width;
				if (minimumSize.Height > size.Height)
					Height = value.Height;
			}
		}

		public abstract IEntity Entity
		{
			get;
		}

		public Point Location
		{
			get
			{
				return location;
			}
			set
			{
				if (location != value)
				{
					Size offset = new Size(value.X - X, value.Y - Y);
					location = value;
					OnMove(new MoveEventArgs(offset));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public int X
		{
			get
			{
				return location.X;
			}
			set
			{
				if (location.X != value)
				{
					Size offset = new Size(value - X, 0);
					location.X = value;
					OnMove(new MoveEventArgs(offset));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public int Y
		{
			get
			{
				return location.Y;
			}
			set
			{
				if (location.Y != value)
				{
					Size offset = new Size(0, value - Y);
					location.Y = value;
					OnMove(new MoveEventArgs(offset));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public virtual Size Size
		{
			get
			{
				return size;
			}
			set
			{
				if (value.Width < MinimumSize.Width)
					value.Width = MinimumSize.Width;
				if (value.Height < MinimumSize.Height)
					value.Height = MinimumSize.Height;

				if (size != value)
				{
					Size change = new Size(value.Width - Width, value.Height - Height);
					size = value;
					OnResize(new ResizeEventArgs(change));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public virtual int Width
		{
			get
			{
				return size.Width;
			}
			set
			{
				if (value < MinimumSize.Width)
					value = MinimumSize.Width;

				if (size.Width != value)
				{
					Size change = new Size(value - Width, 0);
					size.Width = value;
					OnResize(new ResizeEventArgs(change));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public virtual int Height
		{
			get
			{
				return size.Height;
			}
			set
			{
				if (value < MinimumSize.Height)
					value = MinimumSize.Height;

				if (size.Height != value)
				{
					Size change = new Size(0, value - Height);
					size.Height = value;
					OnResize(new ResizeEventArgs(change));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public int Left
		{
			get { return X; }
			set { X = value; }
		}

		public int Right
		{
			get { return X + Width; }
			set { X = value - Width; }
		}

		public int Top
		{
			get { return Y; }
			set { Y = value; }
		}

		public int Bottom
		{
			get { return Y + Height; }
			set { Y = value - Height; }
		}

		public Rectangle BorderRectangle
		{
			get { return new Rectangle(Location, Size); }
		}

		public Point CenterPoint
		{
			get { return new Point(HorizontalCenter, VerticalCenter); }
		}

		public int HorizontalCenter
		{
			get
			{
				return ((Left + Right) / 2);
			}
		}

		public int VerticalCenter
		{
			get
			{
				return ((Top + Bottom) / 2);
			}
		}

		protected abstract int GetBorderWidth(Style style);

		protected override RectangleF CalculateDrawingArea(Style style, bool printing, float zoom)
		{
			RectangleF area = BorderRectangle;

			if (printing || !IsSelected)
			{
				float borderSize = (float) GetBorderWidth(style) / 2;
				area.Inflate(borderSize, borderSize);

				if (style.ShadowOffset.Width > borderSize)
					area.Width += style.ShadowOffset.Width - borderSize;
				if (style.ShadowOffset.Height > borderSize)
					area.Height += style.ShadowOffset.Height - borderSize;

				return area;
			}
			else
			{
				float borderSize = (float) GetBorderWidth(style) / 2;
				float selectionSize = SelectionMargin / zoom;
				float inflation = Math.Max(borderSize, selectionSize);

				area.Inflate(inflation, inflation);
				return area;
			}
		}

		internal bool IsResizing
		{
			get { return (resizeMode != ResizeMode.None); }
		}

		public virtual void Collapse()
		{
		}

		public virtual void Expand()
		{
		}

		protected virtual ResizeMode GetResizeMode(AbsoluteMouseEventArgs e)
		{
			if (e.Zoom <= UndreadableZoom)
				return ResizeMode.None;

			ResizeMode mode = ResizeMode.None;
			float squareSize = SelectionMargin / e.Zoom;
			int horCenter = HorizontalCenter;
			int verCenter = VerticalCenter;

			bool left = (e.X >= Left - squareSize && e.X < Left);
			bool top = (e.Y >= Top - squareSize && e.Y < Top);
			bool right = (e.X > Right && e.X < Right + squareSize);
			bool bottom = (e.Y > Bottom && e.Y < Bottom + squareSize);
			bool center = (e.X >= horCenter - squareSize / 2 &&
				e.X < horCenter + squareSize / 2);
			bool middle = (e.Y >= verCenter - squareSize / 2 &&
				e.Y < verCenter + squareSize / 2);

            if (right && (top || middle || bottom))
                mode |= ResizeMode.Right;
            else if (left && (top || middle || bottom))
                mode |= ResizeMode.Left;

            if (bottom && (left || center || right))
                mode |= ResizeMode.Bottom;
            else if (top && (left || center || right))
                mode |= ResizeMode.Top;

            return mode;
        }

        public Cursor GetCursor(AbsoluteMouseEventArgs e)
        {
            ResizeMode mode = GetResizeMode(e);

            switch (mode)
            {
                case ResizeMode.Bottom:
                case ResizeMode.Top:
                    return Cursors.SizeNS;

                case ResizeMode.Right:
                case ResizeMode.Left:
                    return Cursors.SizeWE;

                case ResizeMode.Bottom | ResizeMode.Right:
                case ResizeMode.Top | ResizeMode.Left:
                    return Cursors.SizeNWSE;

                case ResizeMode.Bottom | ResizeMode.Left:
                case ResizeMode.Top | ResizeMode.Right:
                    return Cursors.SizeNESW;

                default:
                    return Cursors.Default;
            }
        }

        protected internal sealed override Rectangle GetLogicalArea()
		{
			return BorderRectangle;
		}

		protected static Rectangle TransformRelativeToAbsolute(Rectangle rectangle,
			float zoom, Point offset)
		{
			rectangle = Rectangle.FromLTRB(
				(int) (rectangle.Left * zoom),
				(int) (rectangle.Top * zoom),
				(int) Math.Ceiling(rectangle.Right * zoom),
				(int) Math.Ceiling(rectangle.Bottom * zoom));
			rectangle.Offset(-offset.X, -offset.Y);
			
			return rectangle;
		}

		protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
		{
			if (IsSelected)
			{
				if (zoom > UndreadableZoom)
				{
					// Draw selection border and resizing squares
					Rectangle frame = TransformRelativeToAbsolute(BorderRectangle, zoom, offset);
					int borderOffset = SelectionMargin / 2;
					frame.Inflate(borderOffset, borderOffset);

					g.DrawRectangle(DiagramConstants.SelectionPen, frame);
					DrawResizingSquares(g, frame);
				}
				else
				{
					// Draw only selection border
					const int BorderOffset = 2;
					Rectangle frame = TransformRelativeToAbsolute(BorderRectangle, zoom, offset);
					frame.Inflate(BorderOffset, BorderOffset);

					g.DrawRectangle(DiagramConstants.SelectionPen, frame);
				}
			}
		}

		private void DrawResizingSquares(Graphics g, Rectangle frame)
		{
			int squareSize = (SelectionMargin - 4);

			for (int row = 0; row < 3; row++)
			{
				for (int column = 0; column < 3; column++)
				{
					if (row != 1 || column != 1) // It's not the center point
					{
						int x = frame.X + (frame.Width * column / 2) - squareSize / 2;
						int y = frame.Y + (frame.Height * row / 2) - squareSize / 2;

						g.FillRectangle(Brushes.White, x, y, squareSize, squareSize);
						g.DrawRectangle(selectionSquarePen, x, y, squareSize, squareSize);
					}
				}
			}
		}

		protected internal sealed override bool TrySelect(RectangleF frame)
		{
			if (frame.IntersectsWith(BorderRectangle))
			{
				IsSelected = true;
				return true;
			}
			else
			{
				return false;
			}
		}

		protected internal sealed override void Offset(Size offset)
		{
			this.Location += offset;
		}

        /// <summary>
        /// Calculates maximum size change with respect to minimum shape size.
        /// </summary>
        /// <param name="change">Proposed size change</param>
        /// <returns>Maximum size change</returns>
        protected internal Size GetMaximumSizeChange(Size change)
        {
            Size newSize = this.Size;
            newSize += change;

            if (newSize.Width < MinimumSize.Width)
                change.Width += MinimumSize.Width - newSize.Width;
            if (newSize.Height < MinimumSize.Height)
                change.Height += MinimumSize.Height - newSize.Height;

            return change;
        }

        protected internal Size GetMinimumPositionChange(Size change)
        {
            Size newSize = this.Size;
            newSize -= change;

            if (newSize.Width < MinimumSize.Width)
                change.Width -= MinimumSize.Width - newSize.Width;
            if (newSize.Height < MinimumSize.Height)
                change.Height -= MinimumSize.Height - newSize.Height;

            return change;
        }

        protected internal void AdjustPositionChange(ref Size positionChange, ref Size sizeChange, int padding)
        {
            if (IsSelected)
            {
                Rectangle newPosition = this.BorderRectangle;
                newPosition.Offset(positionChange.Width, positionChange.Height);

                if (newPosition.Left < padding)
                {
                    positionChange.Width += (padding - newPosition.Left);
                    sizeChange.Width -= (padding - newPosition.Left);
                }
                if (newPosition.Top < padding)
                {
                    positionChange.Height += (padding - newPosition.Top);
                    sizeChange.Height -= (padding - newPosition.Top);
                }
            }
        }

        protected internal override Size GetMaximumPositionChange(Size change, int padding)
		{
			if (IsSelected)
			{
				Rectangle newPosition = this.BorderRectangle;
				newPosition.Offset(change.Width, change.Height);

				if (newPosition.Left < padding)
					change.Width += (padding - newPosition.Left);
				if (newPosition.Top < padding)
					change.Height += (padding - newPosition.Top);
			}
			return change;
		}

		protected internal bool Contains(PointF point)
		{
			return (
				point.X >= Left && point.X <= Right &&
				point.Y >= Top && point.Y <= Bottom
			);
		}

		internal void AutoWidth()
		{
			if (Graphics != null)
				this.Width = (int) GetRequiredWidth(Graphics, Style.CurrentStyle) + 1;
		}

		protected virtual float GetRequiredWidth(Graphics g, Style style)
		{
			return MinimumSize.Width;
		}

		internal void AutoHeight()
		{
			this.Height = GetRequiredHeight();
		}

		protected virtual int GetRequiredHeight()
		{
			return MinimumSize.Height;
		}

		protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(IDiagram diagram)
		{
			return ShapeContextMenu.Default.GetMenuItems(diagram);
		}

		protected internal Shape Paste(IDiagram diagram, Size offset)
		{
			if (CloneEntity(diagram))
			{
			    var shapeList = (ElementList<Shape>)diagram.Shapes;
				Shape shape = shapeList.FirstValue;
				shape.Location = this.Location + offset;
				shape.Size = this.Size;
				shape.IsSelected = true;
				return shape;
			}
			else
			{
				return null;
			}
		}

		protected abstract bool CloneEntity(IDiagram diagram);

		[Obsolete]
		protected internal sealed override void Serialize(XmlElement node)
		{
			OnSerializing(new SerializeEventArgs(node));
		}

		[Obsolete]
		protected internal sealed override void Deserialize(XmlElement node)
		{
			OnDeserializing(new SerializeEventArgs(node));
		}

		protected virtual void OnSerializing(SerializeEventArgs e)
		{
			XmlElement locationNode = e.Node.OwnerDocument.CreateElement("Location");
			locationNode.SetAttribute("left", Left.ToString());
			locationNode.SetAttribute("top", Top.ToString());
			e.Node.AppendChild(locationNode);

			XmlElement sizeNode = e.Node.OwnerDocument.CreateElement("Size");
			sizeNode.SetAttribute("width", Width.ToString());
			sizeNode.SetAttribute("height", Height.ToString());
			e.Node.AppendChild(sizeNode);
		}

		protected virtual void OnDeserializing(SerializeEventArgs e)
		{
			XmlElement locationNode = e.Node["Location"];
			if (locationNode != null)
			{
				int left, top;

				int.TryParse(locationNode.GetAttribute("left"), out left);
				int.TryParse(locationNode.GetAttribute("top"), out top);
				this.Location = new Point(left, top);
			}

			XmlElement sizeNode = e.Node["Size"];
			if (sizeNode != null)
			{
				int width, height;

				int.TryParse(sizeNode.GetAttribute("width"), out width);
				int.TryParse(sizeNode.GetAttribute("height"), out height);
				this.Size = new Size(width, height);
			}
		}

		internal sealed override void MousePressed(AbsoluteMouseEventArgs e)
		{
			if (!e.Handled)
			{
				bool pressed = Contains(e.Location);

				if (e.Button == MouseButtons.Left)
					pressed |= (IsSelected && GetResizeMode(e) != ResizeMode.None);

				if (pressed)
				{
					e.Handled = true;
					resizeMode = GetResizeMode(e);
					Cursor.Current = cursor;
					OnMouseDown(e);
				}
			}
		}

		internal sealed override void MouseMoved(AbsoluteMouseEventArgs e)
		{
			if (!e.Handled)
			{
				bool contains = IsResizing;

				if (!IsResizing)
				{
					contains = Contains(e.Location);

					if (Contains(e.Location) && mouseLeft)
						OnMouseEnter(EventArgs.Empty);
					else if (!Contains(e.Location) && !mouseLeft)
						OnMouseLeave(EventArgs.Empty);

					contains |= (IsSelected && GetResizeMode(e) != ResizeMode.None);
				}

				if (IsMousePressed || contains)
				{
					e.Handled = true;
					if (IsMousePressed)
					{
						Cursor.Current = cursor;
					}
					else
					{
						cursor = GetCursor(e);
						Cursor.Current = cursor;
					}
					OnMouseMove(e);
				}
			}
			else // Already handled
			{
				if (!mouseLeft)
					OnMouseLeave(EventArgs.Empty);
			}
		}

		internal sealed override void MouseUpped(AbsoluteMouseEventArgs e)
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

		internal sealed override void DoubleClicked(AbsoluteMouseEventArgs e)
		{
			if (!e.Handled)
			{
				bool doubleClicked = Contains(e.Location);

				if (e.Button == MouseButtons.Left)
					doubleClicked |= (IsSelected && GetResizeMode(e) != ResizeMode.None);

				if (doubleClicked)
				{
					e.Handled = true;
					OnDoubleClick(e);
				}
			}
		}

        private void PerformDragging(SizeF mouseOffset)
        {
            OnDragging(new MoveEventArgs(mouseOffset));
        }

        private void PerformResize(SizeF mouseOffset)
		{
			SizeF sizeChange = SizeF.Empty;
            SizeF positionChange = SizeF.Empty;

            if ((resizeMode & ResizeMode.Right) != 0)
            {
                sizeChange.Width += mouseOffset.Width;
            }
            else if ((resizeMode & ResizeMode.Left) != 0)
            {
                positionChange.Width += mouseOffset.Width;
                sizeChange.Width -= mouseOffset.Width;
            }

            if ((resizeMode & ResizeMode.Bottom) != 0)
            {
                sizeChange.Height += mouseOffset.Height;
            }
            else if ((resizeMode & ResizeMode.Top) != 0)
            {
                positionChange.Height += mouseOffset.Height;
                sizeChange.Height -= mouseOffset.Height;
            }

            ResizeEventArgs e = new ResizeEventArgs(positionChange, sizeChange);
            OnResizing(e);
			Size += e.SizeChange.ToSize();
            Location += e.PositionChange.ToSize();
        }

        protected virtual void CopyFrom(Shape shape)
		{
			location = shape.location;
			size = shape.size;
		}

		protected override void OnDeactivating(EventArgs e)
		{
			base.OnDeactivated(e);
			HideEditor();
		}

		protected override void OnMouseDown(AbsoluteMouseEventArgs e)
		{
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(AbsoluteMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (IsResizing)
			{
				PerformResize(e.Offset);
			}
			else if (IsMousePressed && e.Button == MouseButtons.Left)
			{
                PerformDragging(e.Offset);
			}
		}

		protected override void OnMouseUp(AbsoluteMouseEventArgs e)
		{
			base.OnMouseUp(e);
			resizeMode = ResizeMode.None;
		}

		protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
		{
			base.OnDoubleClick(e);
			ResizeMode resizeMode = GetResizeMode(e);

			if ((resizeMode & ResizeMode.Right) != 0)
				AutoWidth();
			if ((resizeMode & ResizeMode.Bottom) != 0)
				AutoHeight();
		}

		protected virtual void OnMove(MoveEventArgs e)
		{
			if (Move != null)
				Move(this, e);
		}

		protected virtual void OnDragging(MoveEventArgs e)
		{
			if (Dragging != null)
				Dragging(this, e);
		}

		protected virtual void OnResizing(ResizeEventArgs e)
		{
			if (Resizing != null)
				Resizing(this, e);
		}

		protected virtual void OnResize(ResizeEventArgs e)
		{
			if (Resize != null)
				Resize(this, e);
		}

		protected virtual void OnMouseEnter(EventArgs e)
		{
			mouseLeft = false;
		}

		protected virtual void OnMouseLeave(EventArgs e)
		{
			mouseLeft = true;
		}

		public override string ToString()
		{
			return Entity.ToString();
		}
	}
}