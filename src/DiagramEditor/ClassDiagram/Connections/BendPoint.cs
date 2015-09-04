// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
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
using System.Xml;
using System.Drawing;
using NClass.DiagramEditor.ClassDiagram.Shapes;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
	public sealed class BendPoint
	{
		const int Spacing = Connection.Spacing;
		internal const int SquareSize = 8;

		static Color darkStartColor = Color.Blue;
		static Color darkEndColor = Color.Red;
		static Color lightStartColor = Color.FromArgb(178, 178, 255);
		static Color lightEndColor = Color.FromArgb(255, 178, 178);
		static Pen squarePen = new Pen(Color.Black);
		static SolidBrush squareBrush = new SolidBrush(Color.Black);

		Shape relativeShape;
		bool relativeToStartShape;
		bool autoPosition = true;
		Size relativePosition = Size.Empty;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="relativeShape"/> is null.
		/// </exception>
		public BendPoint(Shape relativeShape, bool relativeToStartShape)
		{
			if (relativeShape == null)
				throw new ArgumentNullException("relativeShape");

			this.relativeShape = relativeShape;
			this.relativeToStartShape = relativeToStartShape;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="relativeShape"/> is null.
		/// </exception>
		public BendPoint(Shape relativeShape, bool relativeToStartShape, bool autoPosition)
			: this(relativeShape, relativeToStartShape)
		{
			this.autoPosition = autoPosition;
		}

		public bool RelativeToStartShape
		{
			get { return relativeToStartShape; }
			set { relativeToStartShape = value; }
		}

		internal bool AutoPosition
		{
			get { return autoPosition; }
			set { autoPosition = value; }
		}

		public int X
		{
			get
			{
				return (relativeShape.X + relativePosition.Width);
			}
			internal set
			{
				relativePosition.Width = value - relativeShape.X;
			}
		}

		public int Y
		{
			get
			{
				return (relativeShape.Y + relativePosition.Height);
			}
			internal set
			{
				relativePosition.Height = value - relativeShape.Y;
			}
		}

		public Point Location
		{
			get
			{
				return (relativeShape.Location + relativePosition);
			}
			set
			{
				if (value.X > relativeShape.Left - Spacing &&
					value.X < relativeShape.Right + Spacing &&
					value.Y > relativeShape.Top - Spacing &&
					value.Y < relativeShape.Bottom + Spacing)
				{
					if (X <= relativeShape.Left - Spacing)
					{
						X = relativeShape.Left - Spacing;
						Y = value.Y;
					}
					else if (X >= relativeShape.Right + Spacing)
					{
						X = relativeShape.Right + Spacing;
						Y = value.Y;
					}
					else if (Y <= relativeShape.Top - Spacing)
					{
						X = value.X;
						Y = relativeShape.Top - Spacing;
					}
					else
					{
						X = value.X;
						Y = relativeShape.Bottom + Spacing;
					}
				}
				else
				{
					X = value.X;
					Y = value.Y;
				}
			}
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		internal void Draw(Graphics g, bool onScreen, float zoom, Point offset)
		{
			int x = (int) (X * zoom) - SquareSize / 2 - offset.X;
			int y = (int) (Y * zoom) - SquareSize / 2 - offset.Y;
			Rectangle square = new Rectangle(x, y, SquareSize, SquareSize);

			if (AutoPosition)
			{
				squarePen.Color = RelativeToStartShape ? lightStartColor : lightEndColor;
				g.DrawRectangle(squarePen, square.X, square.Y, square.Width, square.Height);
			}
			else
			{
				squarePen.Color = RelativeToStartShape ? darkStartColor : darkEndColor;
				squareBrush.Color = RelativeToStartShape ? lightStartColor : lightEndColor;

				g.FillRectangle(squareBrush, square);
				g.DrawRectangle(squarePen, square);
			}
		}

		internal bool Contains(PointF point, float zoom)
		{
			float halfSize = SquareSize / zoom / 2;

			return (
				point.X >= X - halfSize && point.X <= X + halfSize &&
				point.Y >= Y - halfSize && point.Y <= Y + halfSize
			);
		}

		internal void ShapeResized(Size size)
		{
			if (X >= relativeShape.Left && X <= relativeShape.Right && Y > relativeShape.Top)
				Y += size.Height;

			if (Y >= relativeShape.Top && Y <= relativeShape.Bottom && X > relativeShape.Left)
				X += size.Width;
		}

		internal void Serialize(XmlElement node)
		{
			XmlDocument document = node.OwnerDocument;

			XmlElement xNode = document.CreateElement("X");
			xNode.InnerText = X.ToString();
			node.AppendChild(xNode);

			XmlElement yNode = document.CreateElement("Y");
			yNode.InnerText = Y.ToString();
			node.AppendChild(yNode);
		}

		internal void Deserialize(XmlElement node)
		{
			XmlElement xNode = node["X"];
			if (xNode != null)
			{
				int x;
				int.TryParse(xNode.InnerText, out x);
				X = x;
			}
			XmlElement yNode = node["Y"];
			if (yNode != null)
			{
				int y;
				int.TryParse(yNode.InnerText, out y);
				Y = y;
			}
		}
	}
}
