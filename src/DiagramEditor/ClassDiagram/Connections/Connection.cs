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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.ClassDiagram.Editors;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
	public abstract class Connection : DiagramElement
	{
		private enum LineOrientation
		{
			Horizontal,
			Vertical
		}

		public const int Spacing = 25;
		public const int PrecisionSize = 6;
		const int PickTolerance = 4;
		protected static readonly Size TextMargin = new Size(5, 3);
		static readonly float[] dashPattern = new float[] { 5, 5 };
		static Pen linePen = new Pen(Color.Black);
		static SolidBrush textBrush = new SolidBrush(Color.Black);
		static StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);

		Shape startShape;
		Shape endShape;
		LineOrientation startOrientation;
		LineOrientation endOrientation;
		OrderedList<BendPoint> bendPoints = new OrderedList<BendPoint>();
		BendPoint selectedBendPoint = null;
		bool copied = false;

		List<Point> routeCache = new List<Point>();
		Point[] routeCacheArray = null;

		public event EventHandler RouteChanged;
		public event BendPointEventHandler BendPointMove;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="relationship"/> is null.-or-
		/// <paramref name="startShape"/> is null.-or-
		/// <paramref name="endShape"/> is null.
		/// </exception>
		protected Connection(Relationship relationship, Shape startShape, Shape endShape)
		{
			if (relationship == null)
				throw new ArgumentNullException("relationship");
			if (startShape == null)
				throw new ArgumentNullException("startShape");
			if (endShape == null)
				throw new ArgumentNullException("endShape");

			this.startShape = startShape;
			this.endShape = endShape;
			InitOrientations();
			bendPoints.Add(new BendPoint(startShape, true));
			bendPoints.Add(new BendPoint(endShape, false));

			startShape.Move += ShapeMoving;
			startShape.Resize += StartShapeResizing;
			endShape.Move += ShapeMoving;
			endShape.Resize += EndShapeResizing;

			relationship.Modified += delegate { OnModified(EventArgs.Empty); };

			relationship.Detaching += delegate
			{
				startShape.Move -= ShapeMoving;
				startShape.Resize -= StartShapeResizing;
				endShape.Move -= ShapeMoving;
				endShape.Resize -= EndShapeResizing;
			};
			relationship.Serializing += delegate(object sender, SerializeEventArgs e)
			{
				OnSerializing(e);
			};
			relationship.Deserializing += delegate(object sender, SerializeEventArgs e)
			{
				OnDeserializing(e);
			};
			Reroute();
		}

		protected internal abstract Relationship Relationship
		{
			get;
		}

		protected Shape StartShape
		{
			get { return startShape; }
		}

		protected Shape EndShape
		{
			get { return endShape; }
		}

		public IEnumerable<BendPoint> BendPoints
		{
			get { return bendPoints; }
		}

		protected List<Point> RouteCache
		{
			get { return routeCache; }
		}

		private BendPoint FirstBendPoint
		{
			get { return bendPoints.FirstValue; }
		}

		private BendPoint LastBendPoint
		{
			get { return bendPoints.LastValue; }
		}

		protected virtual Size StartCapSize
		{
			get { return Size.Empty; }
		}

		protected virtual Size EndCapSize
		{
			get { return Size.Empty; }
		}

		protected virtual int StartSelectionOffset
		{
			get { return 0; }
		}

		protected virtual int EndSelectionOffset
		{
			get { return 0; }
		}

		protected virtual bool IsDashed
		{
			get { return false; }
		}

		public void InitOrientations()
		{
			if (startShape == endShape)
			{
				startOrientation = LineOrientation.Horizontal;
				endOrientation = LineOrientation.Vertical;
			}
			else
			{
				int hDiff = Math.Max(startShape.Left - endShape.Right, endShape.Left - startShape.Right);
				int vDiff = Math.Max(startShape.Top - endShape.Bottom, endShape.Top - startShape.Bottom);

				if (vDiff >= Spacing * 2)
				{
					startOrientation = LineOrientation.Vertical;
					endOrientation = LineOrientation.Vertical;
				}
				else if (hDiff >= Spacing * 2)
				{
					startOrientation = LineOrientation.Horizontal;
					endOrientation = LineOrientation.Horizontal;
				}
				else
				{
					startOrientation = LineOrientation.Vertical;
					endOrientation = LineOrientation.Horizontal;
				}
			}
		}

		protected override RectangleF CalculateDrawingArea(Style style, bool printing, float zoom)
		{
			RectangleF area = GetRouteArea();

			float lineSize = (float) style.RelationshipWidth / 2;
			if (IsSelected && !printing)
				lineSize = Math.Max(lineSize, (float) BendPoint.SquareSize / 2 / zoom);
			area.Inflate(lineSize, lineSize);

			if (StartCapSize != Size.Empty)
				area = RectangleF.Union(area, GetStartCapArea());

			if (EndCapSize != Size.Empty)
				area = RectangleF.Union(area, GetEndCapArea());

			if (Relationship.Label != null)
				area = RectangleF.Union(area, GetLabelArea(style));

			return area;
		}

		private RectangleF GetStartCapArea()
		{
			RectangleF area = new RectangleF(routeCache[0], StartCapSize);
			float angle = GetAngle(routeCache[0], routeCache[1]);

			if (angle == 0 || angle == 180) // Vertical direction
			{
				area.X -= (float) StartCapSize.Width / 2;
			}
			if (angle == 90 || angle == 270) // Horizontal direction
			{
				area.Y -= (float) StartCapSize.Width / 2;
				area.Width = StartCapSize.Height;
				area.Height = StartCapSize.Width;
			}

			if (angle == 90) // Left
			{
				area.X -= StartCapSize.Height;
			}
			else if (angle == 180) // Up
			{
				area.Y -= StartCapSize.Height;
			}

			return area;
		}

		private RectangleF GetEndCapArea()
		{
			int lastIndex = routeCache.Count - 1;
			RectangleF area = new RectangleF(routeCache[lastIndex], EndCapSize);
			float angle = GetAngle(routeCache[lastIndex], routeCache[lastIndex - 1]);

			if (angle == 0 || angle == 180) // Up-down direction
			{
				area.X -= (float) EndCapSize.Width / 2;
			}
			if (angle == 90 || angle == 270) // Left-right direction
			{
				area.Y -= (float) EndCapSize.Width / 2;
				area.Width = EndCapSize.Height;
				area.Height = EndCapSize.Width;
			}

			if (angle == 90) // Left
			{
				area.X -= EndCapSize.Height;
			}
			else if (angle == 180) // Up
			{
				area.Y -= EndCapSize.Height;
			}

			return area;
		}

		private RectangleF GetLabelArea(Style style)
		{
			bool horizontal;
			PointF center = GetLineCenter(out horizontal);

			SizeF size = Graphics.MeasureString(Relationship.Label,
				style.RelationshipTextFont, PointF.Empty, stringFormat);

			if (horizontal)
			{
				center.X -= size.Width / 2;
				center.Y -= size.Height + TextMargin.Height;
			}
			else
			{
				center.Y -= size.Height / 2;
				center.X += TextMargin.Width;
			}
			
			return new RectangleF(center.X, center.Y, size.Width, size.Height);
		}

		private Rectangle GetRouteArea()
		{
			Point topLeft = routeCache[0];
			Point bottomRight = routeCache[0];

			for (int i = 1; i < routeCache.Count; i++)
			{
				if (topLeft.X > routeCache[i].X)
					topLeft.X = routeCache[i].X;
				if (topLeft.Y > routeCache[i].Y)
					topLeft.Y = routeCache[i].Y;
				if (bottomRight.X < routeCache[i].X)
					bottomRight.X = routeCache[i].X;
				if (bottomRight.Y < routeCache[i].Y)
					bottomRight.Y = routeCache[i].Y;
			}

			return Rectangle.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}

		private void ShapeMoving(object sender, MoveEventArgs e)
		{
			Reroute();
			OnRouteChanged(EventArgs.Empty);
			OnModified(EventArgs.Empty);
		}

		private void StartShapeResizing(object sender, ResizeEventArgs e)
		{
			foreach (BendPoint bendPoint in bendPoints)
			{
				if (!bendPoint.RelativeToStartShape)
					break;
				bendPoint.ShapeResized(e.Change);
			}
			
			Reroute();
			OnRouteChanged(EventArgs.Empty);
			OnModified(EventArgs.Empty);
		}

		private void EndShapeResizing(object sender, ResizeEventArgs e)
		{
			foreach (BendPoint bendPoint in bendPoints.GetReversedList())
			{
				if (bendPoint.RelativeToStartShape)
					break;
				bendPoint.ShapeResized(e.Change);
			}
			
			Reroute();
			OnRouteChanged(EventArgs.Empty);
			OnModified(EventArgs.Empty);
		}

		internal void AutoRoute()
		{
			if (bendPoints.Count > 0)
			{
				ClearBendPoints();
				Reroute();
				OnRouteChanged(EventArgs.Empty);
				OnModified(EventArgs.Empty);
			}
		}

		private void ClearBendPoints()
		{
			BendPoint startPoint = FirstBendPoint;
			BendPoint endPoint = LastBendPoint;

			bendPoints.Clear();
			bendPoints.Add(startPoint);
			bendPoints.Add(endPoint);
			startPoint.AutoPosition = true;
			endPoint.AutoPosition = true;
		}

		protected void Reverse()
		{
			Shape shape = startShape;
			startShape = endShape;
			endShape = shape;

			LineOrientation orientation = startOrientation;
			startOrientation = endOrientation;
			endOrientation = orientation;

			bendPoints.Reverse();
			RouteCache.Reverse();
			foreach (BendPoint point in BendPoints)
			{
				point.RelativeToStartShape = !point.RelativeToStartShape;
			}

			NeedsRedraw = true;
		}

		internal void ShowPropertiesDialog()
		{
			//UNDONE: Connection.ShowPropertiesDialog()
			throw new NotImplementedException();
		}

		protected internal sealed override Rectangle GetLogicalArea()
		{
			return GetRouteArea();
		}

		public override void Draw(IGraphics g, bool onScreen, Style style)
		{
			DrawLine(g, onScreen, style);
			DrawCaps(g, onScreen, style);
			if (Relationship.SupportsLabel)
				DrawLabel(g, onScreen, style);
		}

		private void DrawLine(IGraphics g, bool onScreen, Style style)
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

				g.DrawLines(linePen, routeCacheArray);
			}
		}

		private void DrawCaps(IGraphics g, bool onScreen, Style style)
		{
			Matrix transformState = g.Transform;
			g.TranslateTransform(routeCache[0].X, routeCache[0].Y);
			g.RotateTransform(GetAngle(routeCache[0], routeCache[1]));
			DrawStartCap(g, onScreen, style);
			g.Transform = transformState;

			int last = routeCache.Count - 1;
			g.TranslateTransform(routeCache[last].X, routeCache[last].Y);
			g.RotateTransform(GetAngle(routeCache[last], routeCache[last - 1]));
			DrawEndCap(g, onScreen, style);
			g.Transform = transformState;
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
				return (float) (
					Math.Atan2(point2.Y - point1.Y, point2.X - point1.X) * (180 / Math.PI)) - 90;
			}
		}

		protected virtual void DrawStartCap(IGraphics g, bool onScreen, Style style)
		{
		}

		protected virtual void DrawEndCap(IGraphics g, bool onScreen, Style style)
		{
		}

		private void DrawLabel(IGraphics g, bool onScreen, Style style)
		{
			if (Relationship.Label != null)
			{
				bool horizontal;
				PointF center = GetLineCenter(out horizontal);

				textBrush.Color = style.RelationshipTextColor;
				if (horizontal)
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Far;
					center.Y -= TextMargin.Height;
				}
				else
				{
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Center;
					center.X += TextMargin.Width;
				}
				g.DrawString(Relationship.Label, style.RelationshipTextFont,
					textBrush, center, stringFormat);
			}
		}

		private PointF GetLineCenter(out bool horizontal)
		{
			int lineLength = 0;
			for (int i = 0; i < routeCache.Count - 1; i++)
			{
				if (routeCache[i].X == routeCache[i + 1].X)
					lineLength += Math.Abs(routeCache[i].Y - routeCache[i + 1].Y);
				else
					lineLength += Math.Abs(routeCache[i].X - routeCache[i + 1].X);
			}

			int distance = lineLength / 2;
			int index = 0;
			horizontal = true;
			while (distance >= 0)
			{
				if (routeCache[index].X == routeCache[index + 1].X)
				{
					distance -= Math.Abs(routeCache[index].Y - routeCache[index + 1].Y);
					horizontal = false;
				}
				else
				{
					distance -= Math.Abs(routeCache[index].X - routeCache[index + 1].X);
					horizontal = true;
				}
				index++;
			}

			return new PointF(
				(float) (routeCache[index - 1].X + routeCache[index].X) / 2,
				(float) (routeCache[index - 1].Y + routeCache[index].Y) / 2
			);
		}

		protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
		{
			if (IsSelected)
			{
				Point[] route = routeCache.ToArray();
				int length = route.Length;
				for (int i = 0; i < route.Length; i++)
				{
					route[i].X = (int) (route[i].X * zoom) - offset.X;
					route[i].Y = (int) (route[i].Y * zoom) - offset.Y;
				}

				// Cut the line's start section
				int startOffset = (int) (StartSelectionOffset * zoom);
				if (route[0].X == route[1].X)
				{
					if (route[0].Y < route[1].Y)
						route[0].Y += startOffset;
					else
						route[0].Y -= startOffset;
				}
				else 
				{
					if (route[0].X < route[1].X)
						route[0].X += startOffset;
					else
						route[0].X -= startOffset;
				}
				// Cut the line's end section
				int endOffset = (int) (EndSelectionOffset * zoom);
				if (route[length - 1].X == route[length - 2].X)
				{
					if (route[length - 1].Y < route[length - 2].Y)
						route[length - 1].Y += endOffset;
					else
						route[length - 1].Y -= endOffset;
				}
				else
				{
					if (route[length - 1].X < route[length - 2].X)
						route[length - 1].X += endOffset;
					else
						route[length - 1].X -= endOffset;
				}

				g.DrawLines(Diagram.SelectionPen, route);
				
				if (zoom > UndreadableZoom)
				{
					foreach (BendPoint point in bendPoints)
						point.Draw(g, true, zoom, offset);
				}
			}
		}

		protected virtual void Reroute()
		{
			RecalculateOrientations();
			RelocateAutoBendPoints();
			RerouteFromBendPoints();
		}

		private void RecalculateOrientations()
		{
			if (!FirstBendPoint.AutoPosition)
			{
				if (FirstBendPoint.X >= startShape.Left && FirstBendPoint.X <= startShape.Right)
				{
					startOrientation = LineOrientation.Vertical;
				}
				else if (FirstBendPoint.Y >= startShape.Top && FirstBendPoint.Y <= startShape.Bottom)
				{
					startOrientation = LineOrientation.Horizontal;
				}
			}
			if (!LastBendPoint.AutoPosition)
			{
				if (LastBendPoint.X >= endShape.Left && LastBendPoint.X <= endShape.Right)
				{
					endOrientation = LineOrientation.Vertical;
				}
				else if (LastBendPoint.Y >= endShape.Top && LastBendPoint.Y <= endShape.Bottom)
				{
					endOrientation = LineOrientation.Horizontal;
				}
			}
		}

		private void RelocateAutoBendPoints()
		{
			if (FirstBendPoint.AutoPosition && LastBendPoint.AutoPosition)
			{
				if (startOrientation == endOrientation && startShape == endShape)
				{
					startOrientation = LineOrientation.Horizontal;
					endOrientation = LineOrientation.Vertical;
				}

				if (startOrientation == LineOrientation.Horizontal &&
					endOrientation == LineOrientation.Horizontal)
				{
					if (startShape.Right <= endShape.Left - 2 * Spacing)
					{
						FirstBendPoint.X = startShape.Right + Spacing;
						LastBendPoint.X = endShape.Left - Spacing;
					}
					else if (startShape.Left >= endShape.Right + 2 * Spacing)
					{
						FirstBendPoint.X = startShape.Left - Spacing;
						LastBendPoint.X = endShape.Right + Spacing;
					}
					else
					{
						if (Math.Abs(startShape.Left - endShape.Left) <
							Math.Abs(startShape.Right - endShape.Right))
						{
							FirstBendPoint.X = startShape.Left - Spacing;
							LastBendPoint.X = endShape.Left - Spacing;
						}
						else
						{
							FirstBendPoint.X = startShape.Right + Spacing;
							LastBendPoint.X = endShape.Right + Spacing;
						}
					}

					Shape smallerShape, biggerShape;
					if (startShape.Height < endShape.Height)
					{
						smallerShape = startShape;
						biggerShape = endShape;
					}
					else
					{
						smallerShape = endShape;
						biggerShape = startShape;
					}

					if (biggerShape.Top <= smallerShape.VerticalCenter &&
						biggerShape.Bottom >= smallerShape.VerticalCenter)
					{
						int center = (
							Math.Max(startShape.Top, endShape.Top) +
							Math.Min(startShape.Bottom, endShape.Bottom)) / 2;

						FirstBendPoint.Y = center;
						LastBendPoint.Y = center;
					}
					else
					{
						FirstBendPoint.Y = startShape.VerticalCenter;
						LastBendPoint.Y = endShape.VerticalCenter;
					}
				}
				else if (startOrientation == LineOrientation.Vertical &&
					endOrientation == LineOrientation.Vertical)
				{
					if (startShape.Bottom <= endShape.Top - 2 * Spacing)
					{
						FirstBendPoint.Y = startShape.Bottom + Spacing;
						LastBendPoint.Y = endShape.Top - Spacing;
					}
					else if (startShape.Top >= endShape.Bottom + 2 * Spacing)
					{
						FirstBendPoint.Y = startShape.Top - Spacing;
						LastBendPoint.Y = endShape.Bottom + Spacing;
					}
					else
					{
						if (Math.Abs(startShape.Top - endShape.Top) <
							Math.Abs(startShape.Bottom - endShape.Bottom))
						{
							FirstBendPoint.Y = startShape.Top - Spacing;
							LastBendPoint.Y = endShape.Top - Spacing;
						}
						else
						{
							FirstBendPoint.Y = startShape.Bottom + Spacing;
							LastBendPoint.Y = endShape.Bottom + Spacing;
						}
					}

					Shape smallerShape, biggerShape;
					if (startShape.Width < endShape.Width)
					{
						smallerShape = startShape;
						biggerShape = endShape;
					}
					else
					{
						smallerShape = endShape;
						biggerShape = startShape;
					}

					if (biggerShape.Left <= smallerShape.HorizontalCenter &&
						biggerShape.Right >= smallerShape.HorizontalCenter)
					{
						int center = (
							Math.Max(startShape.Left, endShape.Left) +
							Math.Min(startShape.Right, endShape.Right)) / 2;

						FirstBendPoint.X = center;
						LastBendPoint.X = center;
					}
					else
					{
						FirstBendPoint.X = startShape.HorizontalCenter;
						LastBendPoint.X = endShape.HorizontalCenter;
					}
				}
				else
				{
					if (startOrientation == LineOrientation.Horizontal)
					{
						FirstBendPoint.Y = startShape.VerticalCenter;
						LastBendPoint.X = endShape.HorizontalCenter;

						if (LastBendPoint.X >= startShape.HorizontalCenter)
							FirstBendPoint.X = startShape.Right + Spacing;
						else
							FirstBendPoint.X = startShape.Left - Spacing;

						if (FirstBendPoint.Y >= endShape.VerticalCenter)
							LastBendPoint.Y = endShape.Bottom + Spacing;
						else
							LastBendPoint.Y = endShape.Top - Spacing;
					}
					else
					{
						FirstBendPoint.X = startShape.HorizontalCenter;
						LastBendPoint.Y = endShape.VerticalCenter;

						if (LastBendPoint.Y >= startShape.VerticalCenter)
							FirstBendPoint.Y = startShape.Bottom + Spacing;
						else
							FirstBendPoint.Y = startShape.Top - Spacing;

						if (FirstBendPoint.X >= endShape.HorizontalCenter)
							LastBendPoint.X = endShape.Right + Spacing;
						else
							LastBendPoint.X = endShape.Left - Spacing;
					}
				}
			}
			else if (FirstBendPoint.AutoPosition)
			{
				if (startOrientation == LineOrientation.Horizontal)
				{
					if (bendPoints.SecondValue.X < startShape.HorizontalCenter)
						FirstBendPoint.X = startShape.Left - Spacing;
					else
						FirstBendPoint.X = startShape.Right + Spacing;

					if (bendPoints.SecondValue.Y >= startShape.Top &&
					    bendPoints.SecondValue.Y <= startShape.Bottom)
					{
						FirstBendPoint.Y = bendPoints.SecondValue.Y;
					}
					else
					{
						FirstBendPoint.Y = startShape.VerticalCenter;
					}
				}
				else
				{
					if (bendPoints.SecondValue.Y < startShape.VerticalCenter)
						FirstBendPoint.Y = startShape.Top - Spacing;
					else
						FirstBendPoint.Y = startShape.Bottom + Spacing;

					if (bendPoints.SecondValue.X >= startShape.Left &&
						bendPoints.SecondValue.X <= startShape.Right)
					{
						FirstBendPoint.X = bendPoints.SecondValue.X;
					}
					else
					{
						FirstBendPoint.X = startShape.HorizontalCenter;
					}
				}
			}
			else if (LastBendPoint.AutoPosition)
			{
				if (endOrientation == LineOrientation.Horizontal)
				{
					if (bendPoints.SecondLastValue.X < endShape.HorizontalCenter)
						LastBendPoint.X = endShape.Left - Spacing;
					else
						LastBendPoint.X = endShape.Right + Spacing;

					if (bendPoints.SecondLastValue.Y >= endShape.Top &&
						bendPoints.SecondLastValue.Y <= endShape.Bottom)
					{
						LastBendPoint.Y = bendPoints.SecondLastValue.Y;
					}
					else
					{
						LastBendPoint.Y = endShape.VerticalCenter;
					}
				}
				else
				{
					if (bendPoints.SecondLastValue.Y < endShape.VerticalCenter)
						LastBendPoint.Y = endShape.Top - Spacing;
					else
						LastBendPoint.Y = endShape.Bottom + Spacing;

					if (bendPoints.SecondLastValue.X >= endShape.Left &&
						bendPoints.SecondLastValue.X <= endShape.Right)
					{
						LastBendPoint.X = bendPoints.SecondLastValue.X;
					}
					else
					{
						LastBendPoint.X = endShape.HorizontalCenter;
					}
				}
			}
		}

		private void RerouteFromBendPoints()
		{
			routeCache.Clear();

			FlowDirection direction = AddStartSegment();

			LinkedListNode<BendPoint> current = bendPoints.First;
			while (current != bendPoints.Last)
			{
				direction = AddInnerSegment(current, direction);
				current = current.Next;
			}

			AddEndSegment();

			routeCacheArray = routeCache.ToArray();
			Array.Reverse(routeCacheArray);
		}

		private FlowDirection AddInnerSegment(LinkedListNode<BendPoint> current, FlowDirection direction)
		{
			BendPoint activePoint = current.Value;
			BendPoint nextPoint = current.Next.Value;

			if (nextPoint.X == activePoint.X)
			{
				routeCache.Add(nextPoint.Location);

				if (nextPoint.Y < activePoint.Y)
					return FlowDirection.BottomUp;
				else
					return FlowDirection.TopDown;
			}
			else if (nextPoint.Y == activePoint.Y)
			{
				routeCache.Add(nextPoint.Location);

				if (nextPoint.X < activePoint.X)
					return FlowDirection.RightToLeft;
				else
					return FlowDirection.LeftToRight;
			}

			else if (direction == FlowDirection.TopDown)
			{
				if (nextPoint.Y < activePoint.Y)
				{
					routeCache.Add(new Point(nextPoint.X, activePoint.Y));
					routeCache.Add(nextPoint.Location);
					return FlowDirection.BottomUp;
				}
				else
				{
					Point nextNextPoint = GetNextNextPoint(current);

					if (current.Next.Next == null &&
						nextNextPoint.X == nextPoint.X &&
						nextNextPoint.Y >= nextPoint.Y)
					{
						int center = (nextPoint.Y + activePoint.Y) / 2;
						routeCache.Add(new Point(activePoint.X, center));
						routeCache.Add(new Point(nextPoint.X, center));
						routeCache.Add(nextPoint.Location);
						return FlowDirection.TopDown;
					}
					else if (nextPoint.X < activePoint.X)
					{
						if (nextNextPoint.X >= activePoint.X ||
							(nextNextPoint.Y >= nextPoint.Y &&
							 nextNextPoint.X > nextPoint.X))
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.TopDown;
						}
						else
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.RightToLeft;
						}
					}
					else
					{
						if (nextNextPoint.X <= activePoint.X ||
							(nextNextPoint.Y >= nextPoint.Y &&
							nextNextPoint.X < nextPoint.X))
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.TopDown;
						}
						else
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.LeftToRight;
						}
					}
				}
			}
			else if (direction == FlowDirection.BottomUp)
			{
				if (nextPoint.Y > activePoint.Y)
				{
					routeCache.Add(new Point(nextPoint.X, activePoint.Y));
					routeCache.Add(nextPoint.Location);
					return FlowDirection.TopDown;
				}
				else
				{
					Point nextNextPoint = GetNextNextPoint(current);

					if (current.Next.Next == null &&
						nextNextPoint.X == nextPoint.X &&
						nextNextPoint.Y <= nextPoint.Y)
					{
						int center = (nextPoint.Y + activePoint.Y) / 2;
						routeCache.Add(new Point(activePoint.X, center));
						routeCache.Add(new Point(nextPoint.X, center));
						routeCache.Add(nextPoint.Location);
						return FlowDirection.BottomUp;
					}
					else if (nextPoint.X > activePoint.X)
					{

						if (nextNextPoint.X <= activePoint.X ||
							(nextNextPoint.Y <= nextPoint.Y &&
							 nextNextPoint.X < nextPoint.X))
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.BottomUp;
						}
						else
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.LeftToRight;
						}
					}
					else
					{
						if (nextNextPoint.X >= activePoint.X ||
							(nextNextPoint.Y <= nextPoint.Y &&
							nextNextPoint.X > nextPoint.X))
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.BottomUp;
						}
						else
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.RightToLeft;
						}
					}
				}
			}
			else if (direction == FlowDirection.LeftToRight)
			{
				if (nextPoint.X < activePoint.X)
				{
					routeCache.Add(new Point(activePoint.X, nextPoint.Y));
					routeCache.Add(nextPoint.Location);
					return FlowDirection.RightToLeft;
				}
				else
				{
					Point nextNextPoint = GetNextNextPoint(current);

					if (current.Next.Next == null &&
						nextNextPoint.Y == nextPoint.Y &&
						nextNextPoint.X >= nextPoint.X)
					{
						int center = (nextPoint.X + activePoint.X) / 2;
						routeCache.Add(new Point(center, activePoint.Y));
						routeCache.Add(new Point(center, nextPoint.Y));
						routeCache.Add(nextPoint.Location);
						return FlowDirection.LeftToRight;
					}
					if (nextPoint.Y > activePoint.Y)
					{
						if (nextNextPoint.Y <= activePoint.Y ||
							(nextNextPoint.X >= nextPoint.X &&
							nextNextPoint.Y < nextPoint.Y))
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.LeftToRight;
						}
						else
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.TopDown;
						}
					}
					else
					{
						if (nextNextPoint.Y >= activePoint.Y ||
							(nextNextPoint.X >= nextPoint.X &&
							nextNextPoint.Y > nextPoint.Y))
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.LeftToRight;
						}
						else
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.BottomUp;
						}
					}
				}
			}
			else if (direction == FlowDirection.RightToLeft)
			{
				if (nextPoint.X > activePoint.X)
				{
					routeCache.Add(new Point(activePoint.X, nextPoint.Y));
					routeCache.Add(nextPoint.Location);
					return FlowDirection.LeftToRight;
				}
				else
				{
					Point nextNextPoint = GetNextNextPoint(current);

					if (current.Next.Next == null &&
						nextNextPoint.Y == nextPoint.Y &&
						nextNextPoint.X <= nextPoint.X)
					{
						int center = (nextPoint.X + activePoint.X) / 2;
						routeCache.Add(new Point(center, activePoint.Y));
						routeCache.Add(new Point(center, nextPoint.Y));
						routeCache.Add(nextPoint.Location);
						return FlowDirection.RightToLeft;
					}
					if (nextPoint.Y < activePoint.Y)
					{
						if (nextNextPoint.Y >= activePoint.Y ||
							(nextNextPoint.X <= nextPoint.X &&
							nextNextPoint.Y > nextPoint.Y))
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.RightToLeft;
						}
						else
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.BottomUp;
						}
					}
					else
					{
						if (nextNextPoint.Y <= activePoint.Y ||
							(nextNextPoint.X <= nextPoint.X &&
							nextNextPoint.Y < nextPoint.Y))
						{
							routeCache.Add(new Point(activePoint.X, nextPoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.RightToLeft;
						}
						else
						{
							routeCache.Add(new Point(nextPoint.X, activePoint.Y));
							routeCache.Add(nextPoint.Location);
							return FlowDirection.TopDown;
						}
					}
				}
			}
			else
			{
				routeCache.Add(nextPoint.Location);
				return direction;
			}
		}

		private Point GetNextNextPoint(LinkedListNode<BendPoint> current)
		{
			LinkedListNode<BendPoint> next = current.Next;
			LinkedListNode<BendPoint> nextNext = next.Next;

			if (nextNext != null)
			{
				return nextNext.Value.Location;
			}
			else
			{
				Point nextNextPoint = next.Value.Location;

				if (nextNextPoint.X < endShape.Left)
					nextNextPoint.X = endShape.Left;
				else if (nextNextPoint.X > endShape.Right)
					nextNextPoint.X = endShape.Right;
				if (nextNextPoint.Y < endShape.Top)
					nextNextPoint.Y = endShape.Top;
				else if (nextNextPoint.Y > endShape.Bottom)
					nextNextPoint.Y = endShape.Bottom;

				return nextNextPoint;
			}
		}

		private FlowDirection AddStartSegment()
		{
			if (startOrientation == LineOrientation.Horizontal)
			{
				int startX, startY;

				if (FirstBendPoint.X < startShape.HorizontalCenter)
					startX = startShape.Left;
				else
					startX = startShape.Right;

				if (FirstBendPoint.Y >= startShape.Top &&
					FirstBendPoint.Y <= startShape.Bottom)
				{
					startY = FirstBendPoint.Y;
					routeCache.Add(new Point(startX, startY));
					routeCache.Add(FirstBendPoint.Location);

					if (startX == startShape.Left)
						return FlowDirection.RightToLeft;
					else
						return FlowDirection.LeftToRight;
				}
				else
				{
					startY = startShape.VerticalCenter;
					routeCache.Add(new Point(startX, startY));
					routeCache.Add(new Point(FirstBendPoint.X, startY));
					routeCache.Add(FirstBendPoint.Location);

					if (FirstBendPoint.Y < startY)
						return FlowDirection.BottomUp;
					else
						return FlowDirection.TopDown;
				}
			}
			else
			{
				int startX, startY;

				if (FirstBendPoint.Y < startShape.VerticalCenter)
					startY = startShape.Top;
				else
					startY = startShape.Bottom;

				if (FirstBendPoint.X >= startShape.Left &&
					FirstBendPoint.X <= startShape.Right)
				{
					startX = FirstBendPoint.X;
					routeCache.Add(new Point(startX, startY));
					routeCache.Add(FirstBendPoint.Location);

					if (startY == startShape.Top)
						return FlowDirection.BottomUp;
					else
						return FlowDirection.TopDown;
				}
				else
				{
					startX = startShape.HorizontalCenter;
					routeCache.Add(new Point(startX, startY));
					routeCache.Add(new Point(startX, FirstBendPoint.Y));
					routeCache.Add(FirstBendPoint.Location);

					if (FirstBendPoint.X < startX)
						return FlowDirection.RightToLeft;
					else
						return FlowDirection.LeftToRight;
				}
			}
		}

		private void AddEndSegment()
		{
			if (endOrientation == LineOrientation.Horizontal)
			{
				int endX, endY;

				if (LastBendPoint.X < endShape.HorizontalCenter)
					endX = endShape.Left;
				else
					endX = endShape.Right;

				if (LastBendPoint.Y >= endShape.Top &&
					LastBendPoint.Y <= endShape.Bottom)
				{
					endY = LastBendPoint.Y;
				}
				else
				{
					endY = endShape.VerticalCenter;
					routeCache.Add(new Point(LastBendPoint.X, endY));
				}
				routeCache.Add(new Point(endX, endY));
			}
			else
			{
				int endX, endY;

				if (LastBendPoint.Y < endShape.VerticalCenter)
					endY = endShape.Top;
				else
					endY = endShape.Bottom;

				if (LastBendPoint.X >= endShape.Left &&
					LastBendPoint.X <= endShape.Right)
				{
					endX = LastBendPoint.X;
				}
				else
				{
					endX = endShape.HorizontalCenter;
					routeCache.Add(new Point(endX, LastBendPoint.Y));
				}
				routeCache.Add(new Point(endX, endY));
			}
		}

		protected internal sealed override bool TrySelect(RectangleF frame)
		{
			if (Picked(frame))
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
			// Do nothing
		}

		protected internal override Size GetMaximalOffset(Size offset, int padding)
		{
			if (!IsSelected && !startShape.IsSelected && !endShape.IsSelected)
				return offset;

			foreach (BendPoint bendPoint in bendPoints)
			{
				if (IsSelected || (bendPoint.RelativeToStartShape && startShape.IsSelected) ||
					(!bendPoint.RelativeToStartShape && endShape.IsSelected))
				{
					Point newLocation = bendPoint.Location + offset;

					if (newLocation.X < padding)
						offset.Width += (padding - newLocation.X);
					if (newLocation.Y < padding)
						offset.Height += (padding - newLocation.Y);
				}
			}
			return offset;
		}

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

		private BendPoint GetBendPoint(AbsoluteMouseEventArgs e)
		{
			if (e.Zoom <= UndreadableZoom)
				return null;

			foreach (BendPoint point in bendPoints)
			{
				if (point.Contains(e.Location, e.Zoom))
					return point;
			}
			return null;
		}

		private bool BendPointPressed(AbsoluteMouseEventArgs e)
		{
			BendPoint point = GetBendPoint(e);

			selectedBendPoint = point;
			if (point != null)
			{
				if (point.AutoPosition)
				{
					point.AutoPosition = false;
					Reroute();
					OnRouteChanged(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool BendPointDoubleClicked(AbsoluteMouseEventArgs e)
		{
			BendPoint point = GetBendPoint(e);

			if (point != null)
			{
				if (!point.AutoPosition)
				{
					if (point == FirstBendPoint && !bendPoints.SecondValue.RelativeToStartShape ||
						point == LastBendPoint && bendPoints.SecondLastValue.RelativeToStartShape)
					{
						point.AutoPosition = true;
					}
					else
					{
						bendPoints.Remove(point);
					}
					Reroute();
					OnRouteChanged(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
				e.Handled = true;
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool Picked(PointF mouseLocation, float zoom)
		{
			float tolerance = PickTolerance / zoom;

			for (int i = 0; i < routeCache.Count - 1; i++)
			{
				float x = mouseLocation.X;
				float y = mouseLocation.Y;
				float x1 = routeCache[i].X;
				float y1 = routeCache[i].Y;
				float x2 = routeCache[i + 1].X;
				float y2 = routeCache[i + 1].Y;

				if (x1 == x2)
				{
					if ((x >= x1 - tolerance) && (x <= x1 + tolerance) &&
						(y >= y1 && y <= y2 || y >= y2 && y <= y1))
					{
						return true;
					}
				}
				else // y1 == y2
				{
					if ((y >= y1 - tolerance) && (y <= y1 + tolerance) &&
						(x >= x1 && x <= x2 || x >= x2 && x <= x1))
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool Picked(RectangleF rectangle)
		{
			for (int i = 0; i < routeCache.Count - 1; i++)
			{
				if (rectangle.Contains(routeCache[i]) || rectangle.Contains(routeCache[i + 1]))
					return true;

				float x1 = routeCache[i].X;
				float y1 = routeCache[i].Y;
				float x2 = routeCache[i + 1].X;
				float y2 = routeCache[i + 1].Y;

				if (x1 == x2)
				{
					if (x1 >= rectangle.Left && x1 <= rectangle.Right && (
						y1 < rectangle.Top && y2 > rectangle.Bottom ||
						y2 < rectangle.Top && y1 > rectangle.Bottom))
					{
						return true;
					}
				}
				else // y1 == y2
				{
					if (y1 >= rectangle.Top && y1 <= rectangle.Bottom && (
						x1 < rectangle.Left && x2 > rectangle.Right ||
						x2 < rectangle.Left && x1 > rectangle.Right))
					{
						return true;
					}
				}
			}

			return false;
		}

		internal override void MousePressed(AbsoluteMouseEventArgs e)
		{
			if (!e.Handled)
			{
				bool pressed = Picked(e.Location, e.Zoom);

				if (e.Button == MouseButtons.Left)
					pressed |= (IsSelected && BendPointPressed(e));

				if (pressed)
				{
					e.Handled = true;
					OnMouseDown(e);
				}
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

		internal override void DoubleClicked(AbsoluteMouseEventArgs e)
		{
			bool doubleClicked = Picked(e.Location, e.Zoom);

			if (e.Button == MouseButtons.Left)
				doubleClicked |= (IsSelected && BendPointDoubleClicked(e));

			if (doubleClicked)
			{
				OnDoubleClick(e);
				e.Handled = true;
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

		protected override void OnMouseMove(AbsoluteMouseEventArgs e)
		{
			base.OnMouseMove(e);

			//TODO: szebb lenne külön eljárásba tenni
			if (e.Button == MouseButtons.Left && selectedBendPoint != null)
			{
				Point newLocation = Point.Truncate(e.Location);

				if (selectedBendPoint.Location != newLocation)
				{
					if (!copied && Control.ModifierKeys == Keys.Control)
					{
						BendPoint newPoint = (BendPoint) selectedBendPoint.Clone();
						newPoint.Location = newLocation;
						if (selectedBendPoint.RelativeToStartShape)
							bendPoints.AddAfter(bendPoints.Find(selectedBendPoint), newPoint);
						else
							bendPoints.AddBefore(bendPoints.Find(selectedBendPoint), newPoint);
						selectedBendPoint = newPoint;
						copied = true;
						OnBendPointMove(new BendPointEventArgs(selectedBendPoint));
					}
					else
					{
						selectedBendPoint.Location = newLocation;
						OnBendPointMove(new BendPointEventArgs(selectedBendPoint));
					}

					Reroute();
					OnRouteChanged(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
			}
		}

		public void ValidatePosition(int padding)
		{
			LinkedListNode<BendPoint> bendPoint = bendPoints.First;
			int index = 0;

			while (bendPoint != null && index < routeCache.Count - 1)
			{
				BendPoint point = bendPoint.Value;
				while (point.Location != routeCache[index])
					index++;

				if (point.X < padding)
				{
					if (point.RelativeToStartShape)
						startShape.X += (padding - point.X);
					else
						endShape.X += (padding - point.X);
					return;
				}
				if (point.Y < padding)
				{
					if (point.RelativeToStartShape)
						startShape.Y += (padding - point.Y);
					else
						endShape.Y += (padding - point.Y);
					return;
				}

				bendPoint = bendPoint.Next;
			}
		}

		protected internal Connection Paste(Diagram diagram, Size offset,
			Shape first, Shape second)
		{
			if (CloneRelationship(diagram, first, second))
			{
				Connection connection = diagram.ConnectionList.FirstValue;
				connection.IsSelected = true;

				connection.startOrientation = this.startOrientation;
				connection.endOrientation = this.endOrientation;
				connection.bendPoints.Clear();
				foreach (BendPoint point in this.bendPoints)
				{
					Shape relativeShape = point.RelativeToStartShape ? first : second;
					BendPoint newPoint = new BendPoint(relativeShape,
						point.RelativeToStartShape, point.AutoPosition);
					newPoint.Location = point.Location + offset;
					connection.bendPoints.Add(newPoint);
				}
				connection.Reroute();

				return connection;
			}
			else
			{
				return null;
			}
		}

		protected abstract bool CloneRelationship(Diagram diagram, Shape first, Shape second);

		protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(Diagram diagram)
		{
			return ConnectionContextMenu.Default.GetMenuItems(diagram);
		}

		protected override void OnMouseUp(AbsoluteMouseEventArgs e)
		{
			base.OnMouseUp(e);
			selectedBendPoint = null;
		}

		protected virtual void OnRouteChanged(EventArgs e)
		{
			if (RouteChanged != null)
				RouteChanged(this, e);
		}

		protected virtual void OnBendPointMove(BendPointEventArgs e)
		{
			if (BendPointMove != null)
				BendPointMove(this, e);
		}

		protected virtual void OnSerializing(SerializeEventArgs e)
		{
			XmlDocument document = e.Node.OwnerDocument;

			XmlElement startNode = document.CreateElement("StartOrientation");
			startNode.InnerText = startOrientation.ToString();
			e.Node.AppendChild(startNode);

			XmlElement endNode = document.CreateElement("EndOrientation");
			endNode.InnerText = endOrientation.ToString();
			e.Node.AppendChild(endNode);

			foreach (BendPoint point in bendPoints)
			{
				if (!point.AutoPosition)
				{
					XmlElement node = document.CreateElement("BendPoint");
					node.SetAttribute("relativeToStartShape", point.RelativeToStartShape.ToString());
					point.Serialize(node);
					e.Node.AppendChild(node);
				}
			}
		}

		protected virtual void OnDeserializing(SerializeEventArgs e)
		{
			// Old file format
			XmlElement oldStartNode = e.Node["StartNode"];
			XmlElement oldEndNode = e.Node["EndNode"];
			if (oldStartNode != null && oldEndNode != null)
			{
				bool isHorizontal;
				bool.TryParse(oldStartNode.GetAttribute("isHorizontal"), out isHorizontal);
				startOrientation = (isHorizontal) ? LineOrientation.Horizontal : LineOrientation.Vertical;
				bool.TryParse(oldEndNode.GetAttribute("isHorizontal"), out isHorizontal);
				endOrientation = (isHorizontal) ? LineOrientation.Horizontal : LineOrientation.Vertical;

				int startLocation, endLocation;
				int.TryParse(oldStartNode.GetAttribute("location"), out startLocation);
				int.TryParse(oldEndNode.GetAttribute("location"), out endLocation);

				Reroute();
				if (startOrientation == LineOrientation.Vertical)
					FirstBendPoint.X = startShape.Left + startLocation;
				else
					FirstBendPoint.Y = startShape.Top + startLocation;

				if (endOrientation == LineOrientation.Vertical)
					LastBendPoint.X = endShape.Left + endLocation;
				else
					LastBendPoint.Y = endShape.Top + endLocation;

				FirstBendPoint.AutoPosition = false;
				LastBendPoint.AutoPosition = false;
				Reroute();
			}
			else
			{
				// New file format
				XmlElement startNode = e.Node["StartOrientation"];
				if (startNode != null)
				{
					if (startNode.InnerText == "Horizontal")
						startOrientation = LineOrientation.Horizontal;
					else
						startOrientation = LineOrientation.Vertical;
				}
				XmlElement endNode = e.Node["EndOrientation"];
				if (endNode != null)
				{
					if (endNode.InnerText == "Horizontal")
						endOrientation = LineOrientation.Horizontal;
					else
						endOrientation = LineOrientation.Vertical;
				}

				if (startNode != null && endNode != null) // To be sure it's the new file format
				{
					bendPoints.Clear();

					XmlNodeList nodes = e.Node.SelectNodes("child::BendPoint");
					foreach (XmlElement node in nodes)
					{
						bool relativeToStartShape;
						bool.TryParse(node.GetAttribute("relativeToStartShape"), out relativeToStartShape);
						Shape relativeShape = relativeToStartShape ? startShape : endShape;

						BendPoint point = new BendPoint(relativeShape, relativeToStartShape, false);
						point.Deserialize(node);
						bendPoints.Add(point);
					}
					if (bendPoints.Count == 0 || !FirstBendPoint.RelativeToStartShape)
						bendPoints.AddFirst(new BendPoint(startShape, true));
					if (LastBendPoint.RelativeToStartShape)
						bendPoints.Add(new BendPoint(endShape, false));
				}
				Reroute();
			}
		}

		public override string ToString()
		{
			return Relationship.ToString();
		}
	}
}
