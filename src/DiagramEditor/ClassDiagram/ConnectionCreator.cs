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
using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram
{
	internal class ConnectionCreator
	{
		const int BorderOffset = 8;
		const int BorderOffset2 = 12;
		const int Radius = 5;
		static readonly float[] dashPattern = new float[] { 3, 3 };
		static readonly Pen firstPen;
		static readonly Pen secondPen;
		static readonly Pen arrowPen;

		Diagram diagram;
		RelationshipType type;
		bool firstSelected = false;
		bool created = false;
		Shape first = null;
		Shape second = null;

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

		public ConnectionCreator(Diagram diagram, RelationshipType type)
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
			Point mouseLocation = new Point((int) e.X, (int) e.Y);
			
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

		private void CreateConnection()
		{
			switch (type)
			{
				case RelationshipType.Association:
					CreateAssociation();
					break;

				case RelationshipType.Composition:
					CreateComposition();
					break;

				case RelationshipType.Aggregation:
					CreateAggregation();
					break;

				case RelationshipType.Generalization:
					CreateGeneralization();
					break;

				case RelationshipType.Realization:
					CreateRealization();
					break;

				case RelationshipType.Dependency:
					CreateDependency();
					break;

				case RelationshipType.Nesting:
					CreateNesting();
					break;

				case RelationshipType.Comment:
					CreateCommentRelationship();
					break;
			}
			created = true;
			diagram.Redraw();
		}

		private void CreateAssociation()
		{
			TypeShape shape1 = first as TypeShape;
			TypeShape shape2 = second as TypeShape;

			if (shape1 != null && shape2 != null)
			{
				diagram.AddAssociation(shape1.TypeBase, shape2.TypeBase);
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateComposition()
		{
			TypeShape shape1 = first as TypeShape;
			TypeShape shape2 = second as TypeShape;

			if (shape1 != null && shape2 != null)
			{
				diagram.AddComposition(shape1.TypeBase, shape2.TypeBase);
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateAggregation()
		{
			TypeShape shape1 = first as TypeShape;
			TypeShape shape2 = second as TypeShape;

			if (shape1 != null && shape2 != null)
			{
				diagram.AddAggregation(shape1.TypeBase, shape2.TypeBase);
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateGeneralization()
		{
			CompositeTypeShape shape1 = first as CompositeTypeShape;
			CompositeTypeShape shape2 = second as CompositeTypeShape;

			if (shape1 != null && shape2 != null)
			{
				try
				{
					diagram.AddGeneralization(shape1.CompositeType, shape2.CompositeType);
				}
				catch (RelationshipException)
				{
					MessageBox.Show(Strings.ErrorCannotCreateRelationship);
				}
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateRealization()
		{
			TypeShape shape1 = first as TypeShape;
			InterfaceShape shape2 = second as InterfaceShape;

			if (shape1 != null && shape2 != null)
			{
				try
				{
					diagram.AddRealization(shape1.TypeBase, shape2.InterfaceType);
				}
				catch (RelationshipException)
				{
					MessageBox.Show(Strings.ErrorCannotCreateRelationship);
				}
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateDependency()
		{
			TypeShape shape1 = first as TypeShape;
			TypeShape shape2 = second as TypeShape;

			if (shape1 != null && shape2 != null)
			{
				diagram.AddDependency(shape1.TypeBase, shape2.TypeBase);
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateNesting()
		{
			CompositeTypeShape shape1 = first as CompositeTypeShape;
			TypeShape shape2 = second as TypeShape;

			if (shape1 != null && shape2 != null)
			{
				try
				{
					diagram.AddNesting(shape1.CompositeType, shape2.TypeBase);
				}
				catch (RelationshipException)
				{
					MessageBox.Show(Strings.ErrorCannotCreateRelationship);
				}
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
			}
		}

		private void CreateCommentRelationship()
		{
			CommentShape shape1 = first as CommentShape;
			CommentShape shape2 = second as CommentShape;

			if (shape1 != null)
			{
				diagram.AddCommentRelationship(shape1.Comment, second.Entity);
			}
			else if (shape2 != null)
			{
				diagram.AddCommentRelationship(shape2.Comment, first.Entity);
			}
			else
			{
				MessageBox.Show(Strings.ErrorCannotCreateRelationship);
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
	}
}
