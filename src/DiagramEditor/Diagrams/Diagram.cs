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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.Diagrams
{
	public abstract class Diagram<T> : IDiagram where T : Model, new()
	{
		protected enum State
		{
			Normal,
			Multiselecting,
			CreatingShape,
			CreatingConnection,
			Dragging
		}

		protected ElementList<Shape> shapes = new ElementList<Shape>();
		protected ElementList<Connection> connections = new ElementList<Connection>();
		protected DiagramElement activeElement = null;
		protected Point offset = Point.Empty;
		protected float zoom = 1.0F;
		protected Size size = DiagramConstants.MinSize;

		protected State state = State.Normal;
		protected bool selectioning = false;
		protected RectangleF selectionFrame = RectangleF.Empty;
		protected PointF mouseLocation = PointF.Empty;
		protected bool redrawSuspended = false;
		protected int selectedShapeCount = 0;
		protected int selectedConnectionCount = 0;
		protected Rectangle shapeOutline = Rectangle.Empty;
		protected EntityType shapeType;
        protected IConnectionCreator connectionCreator = null;
	    protected EntityType newShapeType;
	    protected ContextMenu diagramContextMenu;
	    protected DynamicMenu diagramDynamicMenu;

        public event EventHandler Modified;
        public event EventHandler OffsetChanged;
		public event EventHandler SizeChanged;
		public event EventHandler ZoomChanged;
		public event EventHandler StatusChanged;
		public event EventHandler SelectionChanged;
		public event EventHandler NeedsRedraw;
		public event EventHandler ClipboardAvailabilityChanged;
		public event PopupWindowEventHandler ShowingWindow;
		public event PopupWindowEventHandler HidingWindow;
        public event EventHandler Renamed;
        bool isDirty = false;
        bool loading = false;

        protected T model;
	    protected string name;

	    public DiagramType DiagramType { get; protected set; }

        // ReSharper disable once UnusedMember.Global
        // hide the public ctor
		protected Diagram()
		{
		}

		/// <exception cref="ArgumentException">
		/// <paramref name="name"/> cannot be empty string.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="language"/> is null.
		/// </exception>
		public Diagram(string name)
		{
		    this.name = name;
        }

        #region Abstract Methods and Properties
        public abstract void KeyDown(KeyEventArgs e);
        public abstract void CreateShape(EntityType type);
        public abstract Shape AddShape(EntityType type);
        protected abstract void OnEntityAdded(object sender, EntityEventArgs e);
        protected abstract void OnRelationAdded(object sender, RelationshipEventArgs e);
        #endregion

        public Model Model
        {
            get { return this.model; }
        }

        public string Name
	    {
            get
            {
                if (name == null)
                    return Strings.Untitled;
                else
                    return name;
            }
            set
            {
                if (name != value && value != null)
                {
                    name = value;
                    model.Name = value;
                    OnRenamed(EventArgs.Empty);
                    OnModified(EventArgs.Empty);
                }
            }
        }

	    protected Project project;
	    public Project Project
        {
	        get { return project; }
	        set
	        {
	            if (this.project != value)
	            {
	                this.model.Project = value;
	                this.project = value;
	            }
	        }
        }

	    public IEnumerable<Shape> Shapes
		{
			get { return shapes; }
		}

		protected internal ElementList<Shape> ShapeList
		{
			get { return shapes; }
		}

		public IEnumerable<Connection> Connections
		{
			get { return connections; }
		}

		protected internal ElementList<Connection> ConnectionList
		{
			get { return connections; }
		}

		public Point Offset
		{
			get
			{
				return offset;
			}
			set
			{
				if (value.X < 0) value.X = 0;
				if (value.Y < 0) value.Y = 0;

				if (offset != value)
				{
					offset = value;
					OnOffsetChanged(EventArgs.Empty);
				}
			}
		}

		public Size Size
		{
			get
			{
				return size;
			}
			protected set
			{
				if (value.Width < DiagramConstants.MinSize.Width)
					value.Width = DiagramConstants.MinSize.Width;
				if (value.Height < DiagramConstants.MinSize.Height)
					value.Height = DiagramConstants.MinSize.Height;

				if (size != value)
				{
					size = value;
					OnSizeChanged(EventArgs.Empty);
				}
			}
		}

		public float Zoom
		{
			get
			{
				return zoom;
			}
			set
			{
				if (value < Canvas.MinZoom) value = Canvas.MinZoom;
				if (value > Canvas.MaxZoom) value = Canvas.MaxZoom;

				if (zoom != value)
				{
					zoom = value;
					OnZoomChanged(EventArgs.Empty);
				}
			}
		}

		public Color BackColor
		{
			get { return Style.CurrentStyle.BackgroundColor; }
		}

		public bool RedrawSuspended
		{
			get
			{
				return redrawSuspended;
			}
			set
			{
				if (redrawSuspended != value)
				{
					redrawSuspended = value;
					if (!redrawSuspended)
					{
						RecalculateSize();
						RequestRedrawIfNeeded();
					}
				}
			}
		}

		public bool CanCutToClipboard
		{
			get { return SelectedShapeCount > 0; }
		}

		public bool CanCopyToClipboard
		{
			get { return SelectedShapeCount > 0; }
		}

		public bool CanPasteFromClipboard
		{
		    get
		    {
		        var container = Clipboard.Item as ElementContainer;
		        if (container == null)
		            return false;

		        return container.SourceDiagramType == this.DiagramType;
		    }
		}

		public int ShapeCount
		{
			get { return shapes.Count; }
		}

		public int ConnectionCount
		{
			get { return connections.Count; }
		}

		public DiagramElement ActiveElement
		{
			get
			{
				return activeElement;
			}
			private set
			{
				if (activeElement != null)
				{
					activeElement.IsActive = false;
				}
				activeElement = value;
			}
		}

		public DiagramElement TopSelectedElement
		{
			get
			{
				if (SelectedConnectionCount > 0)
					return connections.FirstValue;
				else if (SelectedShapeCount > 0)
					return shapes.FirstValue;
				else
					return null;
			}
		}

		public bool HasSelectedElement
		{
			get
			{
				return (SelectedElementCount > 0);
			}
		}

		public int SelectedElementCount
		{
			get { return selectedShapeCount + selectedConnectionCount; }
		}

		public int SelectedShapeCount
		{
			get { return selectedShapeCount; }
		}

		public int SelectedConnectionCount
		{
			get { return selectedConnectionCount; }
		}

		public string GetSelectedElementName()
		{
			if (HasSelectedElement && SelectedElementCount == 1)
			{
				foreach (Shape shape in shapes)
				{
					if (shape.IsSelected)
						return shape.Entity.Name;
				}
			}

			return null;
		}

		public IEnumerable<Shape> GetSelectedShapes()
		{
			return shapes.GetSelectedElements();
		}

		public IEnumerable<Connection> GetSelectedConnections()
		{
			return connections.GetSelectedElements();
		}

		public IEnumerable<DiagramElement> GetSelectedElements()
		{
			foreach (Shape shape in shapes)
			{
				if (shape.IsSelected)
					yield return shape;
			}
			foreach (Connection connection in connections)
			{
				if (connection.IsSelected)
					yield return connection;
			}
		}

		private IEnumerable<DiagramElement> GetElementsInDisplayOrder()
		{
			foreach (Shape shape in shapes.GetSelectedElements())
				yield return shape;

			foreach (Connection connection in connections.GetSelectedElements())
				yield return connection;
			
			foreach (Connection connection in connections.GetUnselectedElements())
				yield return connection;
			
			foreach (Shape shape in shapes.GetUnselectedElements())
				yield return shape;
		}

		private IEnumerable<DiagramElement> GetElementsInReversedDisplayOrder()
		{
			foreach (Shape shape in shapes.GetUnselectedElementsReversed())
				yield return shape;
			
			foreach (Connection connection in connections.GetUnselectedElementsReversed())
				yield return connection;
			
			foreach (Connection connection in connections.GetSelectedElementsReversed())
				yield return connection;

			foreach (Shape shape in shapes.GetSelectedElementsReversed())
				yield return shape;
		}

		public void CloseWindows()
		{
			if (ActiveElement != null)
				ActiveElement.HideEditor();
		}

		public void Cut()
		{
			if (CanCutToClipboard)
			{
				Copy();
				DeleteSelectedElements(false);
			}
		}

		public void Copy()
		{
			if (CanCopyToClipboard)
			{
				ElementContainer elements = new ElementContainer(this.DiagramType);
				foreach (Shape shape in GetSelectedShapes())
				{
					elements.AddShape(shape);
				}
				foreach (Connection connection in GetSelectedConnections())
				{
					elements.AddConnection(connection);
				}
				Clipboard.Item = elements;
			}
		}

		public void Paste()
		{
			if (CanPasteFromClipboard)
			{
				DeselectAll();
				RedrawSuspended = true;
				Clipboard.Paste(this);
				RedrawSuspended = false;
				OnClipboardAvailabilityChanged(EventArgs.Empty);
			}
		}

		public void Display(Graphics g)
		{
			RectangleF clip = g.ClipBounds;

			// Draw diagram elements
			IGraphics graphics = new GdiGraphics(g);
			foreach (DiagramElement element in GetElementsInReversedDisplayOrder())
			{
				if (clip.IntersectsWith(element.GetVisibleArea(Zoom)))
					element.Draw(graphics, true);
				element.NeedsRedraw = false;
			}
			if (state == State.CreatingShape)
			{
				g.DrawRectangle(DiagramConstants.SelectionPen,
					shapeOutline.X, shapeOutline.Y, shapeOutline.Width, shapeOutline.Height);
			}
			else if (state == State.CreatingConnection)
			{
				connectionCreator.Draw(g);
			}

			// Draw selection lines
			GraphicsState savedState = g.Save();
			g.ResetTransform();
			g.SmoothingMode = SmoothingMode.None;
			foreach (Shape shape in shapes.GetSelectedElementsReversed())
			{
				if (clip.IntersectsWith(shape.GetVisibleArea(Zoom)))
					shape.DrawSelectionLines(g, Zoom, Offset);
			}
			foreach (Connection connection in connections.GetSelectedElementsReversed())
			{
				if (clip.IntersectsWith(connection.GetVisibleArea(Zoom)))
					connection.DrawSelectionLines(g, Zoom, Offset);
			}
			
			if (state == State.Multiselecting)
			{
				RectangleF frame = RectangleF.FromLTRB(
					Math.Min(selectionFrame.Left, selectionFrame.Right),
					Math.Min(selectionFrame.Top, selectionFrame.Bottom),
					Math.Max(selectionFrame.Left, selectionFrame.Right),
					Math.Max(selectionFrame.Top, selectionFrame.Bottom));
				g.DrawRectangle(DiagramConstants.SelectionPen,
					frame.X * Zoom - Offset.X,
					frame.Y * Zoom - Offset.Y,
					frame.Width * Zoom,
					frame.Height * Zoom);
			}

			// Draw diagram border
			clip = g.ClipBounds;
			float borderWidth = Size.Width * Zoom;
			float borderHeight = Size.Height * Zoom;
			if (clip.Right > borderWidth || clip.Bottom > borderHeight)
			{
                DiagramConstants.SelectionPen.DashOffset = Offset.Y - Offset.X;
				g.DrawLines(DiagramConstants.SelectionPen, new PointF[] {
					new PointF(borderWidth, 0),
					new PointF(borderWidth, borderHeight),
					new PointF(0, borderHeight)
				});
                DiagramConstants.SelectionPen.DashOffset = 0;
			}

			// Restore original state
			g.Restore(savedState);
		}

		public void CopyAsImage()
		{
			ImageCreator.CopyAsImage(this);
		}

		public void CopyAsImage(bool selectedOnly)
		{
			ImageCreator.CopyAsImage(this, selectedOnly);
		}

		public void SaveAsImage()
		{
			ImageCreator.SaveAsImage(this);
		}

		public void SaveAsImage(bool selectedOnly)
		{
			ImageCreator.SaveAsImage(this, selectedOnly);
		}

		public void ShowPrintDialog()
		{
			DiagramPrintDialog dialog = new DiagramPrintDialog();
			dialog.Document = this;
			dialog.ShowDialog();
		}

		public void Print(IGraphics g)
		{
			Print(g, false, Style.CurrentStyle);
		}

		public void Print(IGraphics g, bool selectedOnly, Style style)
		{
			foreach (Shape shape in shapes.GetReversedList())
			{
				if (!selectedOnly || shape.IsSelected)
					shape.Draw(g, false, style);
			}
			foreach (Connection connection in connections.GetReversedList())
			{
				if (!selectedOnly || connection.IsSelected)
					connection.Draw(g, false, style);
			}
		}

		protected void RecalculateSize()
		{
			const int Padding = 500;
			int rightMax = DiagramConstants.MinSize.Width, bottomMax = DiagramConstants.MinSize.Height;

			foreach (Shape shape in shapes)
			{
				Rectangle area = shape.GetLogicalArea();
				if (area.Right + Padding > rightMax)
					rightMax = area.Right + Padding;
				if (area.Bottom + Padding > bottomMax)
					bottomMax = area.Bottom + Padding;
			}
			foreach (Connection connection in connections)
			{
				Rectangle area = connection.GetLogicalArea();
				if (area.Right + Padding > rightMax)
					rightMax = area.Right + Padding;
				if (area.Bottom + Padding > bottomMax)
					bottomMax = area.Bottom + Padding;
			}

			this.Size = new Size(rightMax, bottomMax);
		}

		public void AlignLeft()
		{
			if (SelectedShapeCount >= 2)
			{
				int left = Size.Width;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					left = Math.Min(left, shape.Left);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Left = left;
				}

				RedrawSuspended = false;
			}
		}

		public void AlignRight()
		{
			if (SelectedShapeCount >= 2)
			{
				int right = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					right = Math.Max(right, shape.Right);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Right = right;
				}

				RedrawSuspended = false;
			}
		}

		public void AlignTop()
		{
			if (SelectedShapeCount >= 2)
			{
				int top = Size.Height;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					top = Math.Min(top, shape.Top);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Top = top;
				}

				RedrawSuspended = false;
			}
		}

		public void AlignBottom()
		{
			if (SelectedShapeCount >= 2)
			{
				int bottom = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					bottom = Math.Max(bottom, shape.Bottom);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Bottom = bottom;
				}

				RedrawSuspended = false;
			}
		}

		public void AlignHorizontal()
		{
			if (SelectedShapeCount >= 2)
			{
				int center = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					center += (shape.Top + shape.Bottom) / 2;
				}
				center /= SelectedShapeCount;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Top = center - shape.Height / 2;
				}

				RedrawSuspended = false;
			}
		}

		public void AlignVertical()
		{
			if (SelectedShapeCount >= 2)
			{
				int center = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					center += (shape.Left + shape.Right) / 2;
				}
				center /= SelectedShapeCount;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Left = center - shape.Width / 2;
				}

				RedrawSuspended = false;
			}
		}

		public void AdjustToSameWidth()
		{
			if (SelectedShapeCount >= 2)
			{
				int maxWidth = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					maxWidth = Math.Max(maxWidth, shape.Width);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Width = maxWidth;
				}
				RedrawSuspended = false;
			}
		}

		public void AdjustToSameHeight()
		{
			if (SelectedShapeCount >= 2)
			{
				int maxHeight = 0;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					maxHeight = Math.Max(maxHeight, shape.Height);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Height = maxHeight;
				}

				RedrawSuspended = false;
			}
		}

		public void AdjustToSameSize()
		{
			if (SelectedShapeCount >= 2)
			{
				Size maxSize = Size.Empty;
				RedrawSuspended = true;

				foreach (Shape shape in shapes.GetSelectedElements())
				{
					maxSize.Width = Math.Max(maxSize.Width, shape.Width);
					maxSize.Height = Math.Max(maxSize.Height, shape.Height);
				}
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Size = maxSize;
				}

				RedrawSuspended = false;
			}
		}

		public void AutoSizeOfSelectedShapes()
		{
			RedrawSuspended = true;
			foreach (Shape shape in shapes.GetSelectedElements())
			{
				shape.AutoWidth();
				shape.AutoHeight();
			}
			RedrawSuspended = false;
		}

		public void AutoWidthOfSelectedShapes()
		{
			RedrawSuspended = true;
			foreach (Shape shape in shapes.GetSelectedElements())
			{
				shape.AutoWidth();
			}
			RedrawSuspended = false;
		}

		public void AutoHeightOfSelectedShapes()
		{
			RedrawSuspended = true;
			foreach (Shape shape in shapes.GetSelectedElements())
			{
				shape.AutoHeight();
			}
			RedrawSuspended = false;
		}

		public void CollapseAll()
		{
			bool selectedOnly = HasSelectedElement;
			CollapseAll(selectedOnly);
		}

		public void CollapseAll(bool selectedOnly)
		{
			RedrawSuspended = true;

			foreach (Shape shape in shapes)
			{
				if (shape.IsSelected || !selectedOnly)
					shape.Collapse();
			}

			RedrawSuspended = false;
		}

		public void ExpandAll()
		{
			bool selectedOnly = HasSelectedElement;
			ExpandAll(selectedOnly);
		}

		public void ExpandAll(bool selectedOnly)
		{
			RedrawSuspended = true;

			foreach (Shape shape in shapes)
			{
				if (shape.IsSelected || !selectedOnly)
					shape.Expand();
			}

			RedrawSuspended = false;
		}

		public void SelectAll()
		{
			RedrawSuspended = true;
			selectioning = true;

			foreach (Shape shape in shapes)
			{
				shape.IsSelected = true;
			}
			foreach (Connection connection in connections)
			{
				connection.IsSelected = true;
			}

			selectedShapeCount = shapes.Count;
			selectedConnectionCount = connections.Count;

			OnSelectionChanged(EventArgs.Empty);
			OnClipboardAvailabilityChanged(EventArgs.Empty);
			OnSatusChanged(EventArgs.Empty);

			selectioning = false;
			RedrawSuspended = false;
		}

		private bool ConfirmDelete()
		{
			DialogResult result = MessageBox.Show(
				Strings.DeleteElementsConfirmation, Strings.Confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			return (result == DialogResult.Yes);
		}

		public void DeleteSelectedElements()
		{
			DeleteSelectedElements(true);
		}

		private void DeleteSelectedElements(bool showConfirmation)
		{
			if (HasSelectedElement && (!showConfirmation || ConfirmDelete()))
			{
				if (selectedShapeCount > 0)
				{
					foreach (Shape shape in shapes.GetModifiableList())
					{
						if (shape.IsSelected)
							RemoveEntity(shape.Entity);
					}
				}
				if (selectedConnectionCount > 0)
				{
					foreach (Connection connection in connections.GetModifiableList())
					{
						if (connection.IsSelected)
							RemoveRelationship(connection.Relationship);
					}
				}
				Redraw();
			}
		}

		public void Redraw()
		{
			OnNeedsRedraw(EventArgs.Empty);
		}

		private void RequestRedrawIfNeeded()
		{
			if (loading)
				return;

			foreach (Shape shape in shapes)
			{
				if (shape.NeedsRedraw)
				{
					OnNeedsRedraw(EventArgs.Empty);
					return;
				}					
			}
			foreach (Connection connection in connections)
			{
				if (connection.NeedsRedraw)
				{
					OnNeedsRedraw(EventArgs.Empty);
					return;
				}
			}
		}
        
		public DynamicMenu GetDynamicMenu()
		{
			diagramDynamicMenu.SetReference(this);
			return diagramDynamicMenu;
		}
		public ContextMenuStrip GetContextMenu(AbsoluteMouseEventArgs e)
		{
			if (HasSelectedElement)
			{
				Intersector<ToolStripItem> intersector = new Intersector<ToolStripItem>();
				ContextMenu.MenuStrip.Items.Clear();

				foreach (Shape shape in GetSelectedShapes())
					intersector.AddSet(shape.GetContextMenuItems(this));
				foreach (Connection connection in GetSelectedConnections())
					intersector.AddSet(connection.GetContextMenuItems(this));

				foreach (ToolStripItem menuItem in intersector.GetIntersection())
					ContextMenu.MenuStrip.Items.Add(menuItem);
				return ContextMenu.MenuStrip;
			}
			else
			{
				ContextMenu.MenuStrip.Items.Clear();
				foreach (ToolStripItem menuItem in diagramContextMenu.GetMenuItems(this))
					ContextMenu.MenuStrip.Items.Add(menuItem);

				return ContextMenu.MenuStrip;
			}
		}

		public string GetStatus()
		{
			if (SelectedElementCount == 1)
			{
				return TopSelectedElement.ToString();
			}
			else if (SelectedElementCount > 1)
			{
				return string.Format(Strings.ItemsSelected, SelectedElementCount);
			}
			else
			{
				return Strings.Ready;
			}
		}

		public virtual string GetShortDescription()
		{
		    return string.Empty;
		}

		public void DeselectAll()
		{
			foreach (Shape shape in shapes)
			{
				shape.IsSelected = false;
				shape.IsActive = false;
			}
			foreach (Connection connection in connections)
			{
				connection.IsSelected = false;
				connection.IsActive = false;
			}
			ActiveElement = null;
		}

		private void DeselectAllOthers(DiagramElement onlySelected)
		{
			foreach (Shape shape in shapes)
			{
				if (shape != onlySelected)
				{
					shape.IsSelected = false;
					shape.IsActive = false;
				}
			}
			foreach (Connection connection in connections)
			{
				if (connection != onlySelected)
				{
					connection.IsSelected = false;
					connection.IsActive = false;
				}
			}
		}

		public void MouseDown(AbsoluteMouseEventArgs e)
		{
			RedrawSuspended = true;
			if (state == State.CreatingShape)
			{
				AddCreatedShape();
			}
			else if (state == State.CreatingConnection)
			{
				connectionCreator.MouseDown(e);
				if (connectionCreator.Created)
					state = State.Normal;
			}
			else
			{
				SelectElements(e);
			}

			if (e.Button == MouseButtons.Right)
			{
				ActiveElement = null;
			}

			RedrawSuspended = false;
		}

		private void AddCreatedShape()
		{
			DeselectAll();
			Shape shape = AddShape(shapeType);
			shape.Location = shapeOutline.Location;
			RecalculateSize();
			state = State.Normal;

			shape.IsSelected = true;
			shape.IsActive = true;
			if (shape is TypeShape) //TODO: nem szép
				shape.ShowEditor();
		}

		private void SelectElements(AbsoluteMouseEventArgs e)
		{
			DiagramElement firstElement = null;
			bool multiSelection = (Control.ModifierKeys == Keys.Control);
			
			foreach (DiagramElement element in GetElementsInDisplayOrder())
			{
				bool isSelected = element.IsSelected;
				element.MousePressed(e);
				if (e.Handled && firstElement == null)
				{
					firstElement = element;
					if (isSelected)
						multiSelection = true;
				}
			}

			if (firstElement != null && !multiSelection)
			{
				DeselectAllOthers(firstElement);
			}

			if (!e.Handled)
			{
				if (!multiSelection)
					DeselectAll();

				if (e.Button == MouseButtons.Left)
				{
					state = State.Multiselecting;
					selectionFrame.Location = e.Location;
					selectionFrame.Size = Size.Empty;
				}
			}
		}

		public void MouseMove(AbsoluteMouseEventArgs e)
		{
			RedrawSuspended = true;

			mouseLocation = e.Location;
			if (state == State.Multiselecting)
			{
				selectionFrame = RectangleF.FromLTRB(
					selectionFrame.Left, selectionFrame.Top, e.X, e.Y);
				Redraw();
			}
			else if (state == State.CreatingShape)
			{
				shapeOutline.Location = new Point((int) e.X, (int) e.Y);
				Redraw();
			}
			else if (state == State.CreatingConnection)
			{
				connectionCreator.MouseMove(e);
			}
			else
			{
				foreach (DiagramElement element in GetElementsInDisplayOrder())
				{
					element.MouseMoved(e);
				}
			}

			RedrawSuspended = false;
		}

		public void MouseUp(AbsoluteMouseEventArgs e)
		{
			RedrawSuspended = true;

			if (state == State.Multiselecting)
			{
				TrySelectElements();
				state = State.Normal;
			}
			else
			{
				foreach (DiagramElement element in GetElementsInDisplayOrder())
				{
					element.MouseUpped(e);
				}
			}

			RedrawSuspended = false;
		}

		private void TrySelectElements()
		{
			selectionFrame = RectangleF.FromLTRB(
				Math.Min(selectionFrame.Left, selectionFrame.Right),
				Math.Min(selectionFrame.Top, selectionFrame.Bottom),
				Math.Max(selectionFrame.Left, selectionFrame.Right),
				Math.Max(selectionFrame.Top, selectionFrame.Bottom));
			selectioning = true;

			foreach (Shape shape in shapes)
			{
				if (shape.TrySelect(selectionFrame))
					selectedShapeCount++;
			}
			foreach (Connection connection in connections)
			{
				if (connection.TrySelect(selectionFrame))
					selectedConnectionCount++;
			}

			OnSelectionChanged(EventArgs.Empty);
			OnClipboardAvailabilityChanged(EventArgs.Empty);
			OnSatusChanged(EventArgs.Empty);
			Redraw();

			selectioning = false;
		}

		public void DoubleClick(AbsoluteMouseEventArgs e)
		{
			foreach (DiagramElement element in GetElementsInDisplayOrder())
			{
				element.DoubleClicked(e);
			}
		}
        
		public RectangleF GetPrintingArea(bool selectedOnly)
		{
			RectangleF area = Rectangle.Empty;
			bool first = true;

			foreach (Shape shape in shapes)
			{
				if (!selectedOnly || shape.IsSelected)
				{
					if (first)
					{
						area = shape.GetPrintingClip(Zoom);
						first = false;
					}
					else
					{
						area = RectangleF.Union(area, shape.GetPrintingClip(Zoom));
					}
				}
			}
			foreach (Connection connection in connections)
			{
				if (!selectedOnly || connection.IsSelected)
				{
					if (first)
					{
						area = connection.GetPrintingClip(Zoom);
						first = false;
					}
					else
					{
						area = RectangleF.Union(area, connection.GetPrintingClip(Zoom));
					}
				}
			}

			return area;
		}

		private void UpdateWindowPosition()
		{
			if (ActiveElement != null)
				ActiveElement.MoveWindow();
		}

		public void ShowWindow(PopupWindow window)
		{
			Redraw();
			OnShowingWindow(new PopupWindowEventArgs(window));
		}

		public void HideWindow(PopupWindow window)
		{
			window.Closing();
			OnHidingWindow(new PopupWindowEventArgs(window));
		}

		protected void AddShape(Shape shape)
		{
			shape.Diagram = this;
			shape.Modified += element_Modified;
			shape.Activating += element_Activating;
			shape.Dragging += shape_Dragging;
			shape.Resizing += shape_Resizing;
			shape.SelectionChanged += shape_SelectionChanged;
			shapes.AddFirst(shape);
			RecalculateSize();
		}

		protected void element_Modified(object sender, EventArgs e)
		{
			if (!RedrawSuspended)
				RequestRedrawIfNeeded();
			OnModified(EventArgs.Empty);
		}

		private void element_Activating(object sender, EventArgs e)
		{
			foreach (Shape shape in shapes)
			{
				if (shape != sender)
					shape.IsActive = false;
			}
			foreach (Connection connection in connections)
			{
				if (connection != sender)
					connection.IsActive = false;
			}
			ActiveElement = (DiagramElement) sender;
		}

		private void shape_Dragging(object sender, MoveEventArgs e)
		{
			Size offset = e.Offset;

			// Align to other shapes
			if (Settings.Default.UsePrecisionSnapping && Control.ModifierKeys != Keys.Shift)
			{
				Shape shape = (Shape) sender;

				foreach (Shape otherShape in shapes.GetUnselectedElements())
				{
					int xDist = otherShape.X - (shape.X + offset.Width);
					int yDist = otherShape.Y - (shape.Y + offset.Height);

					if (Math.Abs(xDist) <= DiagramConstants.PrecisionSize)
					{
						int distance1 = Math.Abs(shape.Top - otherShape.Bottom);
						int distance2 = Math.Abs(otherShape.Top - shape.Bottom);
						int distance = Math.Min(distance1, distance2);

						if (distance <= DiagramConstants.MaximalPrecisionDistance)
							offset.Width += xDist;
					}
					if (Math.Abs(yDist) <= DiagramConstants.PrecisionSize)
					{
						int distance1 = Math.Abs(shape.Left - otherShape.Right);
						int distance2 = Math.Abs(otherShape.Left - shape.Right);
						int distance = Math.Min(distance1, distance2);

						if (distance <= DiagramConstants.MaximalPrecisionDistance)
							offset.Height += yDist;
					}
				}
			}
			
			// Get maxmimal avaiable offset for the selected elements
			foreach (Shape shape in shapes)
			{
				offset = shape.GetMaximalOffset(offset, DiagramConstants.DiagramPadding);
			}
			foreach (Connection connection in connections)
			{
				offset = connection.GetMaximalOffset(offset, DiagramConstants.DiagramPadding);
			}
			if (!offset.IsEmpty)
			{
				foreach (Shape shape in shapes.GetSelectedElements())
				{
					shape.Offset(offset);
				}
				foreach (Connection connection in connections.GetSelectedElements())
				{
					connection.Offset(offset);
				}
			}
			RecalculateSize();
		}

		private void shape_Resizing(object sender, ResizeEventArgs e)
		{
			if (Settings.Default.UsePrecisionSnapping && Control.ModifierKeys != Keys.Shift)
			{
				Shape shape = (Shape) sender;
				Size change = e.Change;

				// Horizontal resizing
				if (change.Width != 0)
				{
					foreach (Shape otherShape in shapes.GetUnselectedElements())
					{
						if (otherShape != shape)
						{
							int xDist = otherShape.Right - (shape.Right + change.Width);
							if (Math.Abs(xDist) <= DiagramConstants.PrecisionSize)
							{
								int distance1 = Math.Abs(shape.Top - otherShape.Bottom);
								int distance2 = Math.Abs(otherShape.Top - shape.Bottom);
								int distance = Math.Min(distance1, distance2);

								if (distance <= DiagramConstants.MaximalPrecisionDistance)
								{
									change.Width += xDist;
									break;
								}
							}
						}
					}
				}

				// Vertical resizing
				if (change.Height != 0)
				{
					foreach (Shape otherShape in shapes.GetUnselectedElements())
					{
						if (otherShape != shape)
						{
							int yDist = otherShape.Bottom - (shape.Bottom + change.Height);
							if (Math.Abs(yDist) <= DiagramConstants.PrecisionSize)
							{
								int distance1 = Math.Abs(shape.Left - otherShape.Right);
								int distance2 = Math.Abs(otherShape.Left - shape.Right);
								int distance = Math.Min(distance1, distance2);

								if (distance <= DiagramConstants.MaximalPrecisionDistance)
								{
									change.Height += yDist;
									break;
								}
							}
						}
					}
				}

				e.Change = change;
			}
		}

		private void RemoveShape(Shape shape)
		{
			if (shape.IsActive)
			{
				ActiveElement = null;
			}
			if (shape.IsSelected)
			{
				selectedShapeCount--;
				OnSelectionChanged(EventArgs.Empty);
				OnClipboardAvailabilityChanged(EventArgs.Empty);
				OnSatusChanged(EventArgs.Empty);
			}
			shape.Diagram = null;
			shape.Modified -= element_Modified;
			shape.Activating -= element_Activating;
			shape.Dragging -= shape_Dragging;
			shape.Resizing -= shape_Resizing;
			shape.SelectionChanged -= shape_SelectionChanged;
			shapes.Remove(shape);
			RecalculateSize();
		}

		//TODO: legyenek inkább hivatkozások a shape-ekhez
		protected Shape GetShape(IEntity entity)
		{
			foreach (Shape shape in shapes)
			{
				if (shape.Entity == entity)
					return shape;
			}
			return null;
		}

		private Connection GetConnection(Relationship relationship)
		{
			foreach (Connection connection in connections)
			{
				if (connection.Relationship == relationship)
					return connection;
			}
			return null;
		}

		protected void AddConnection(Connection connection)
		{
			connection.Diagram = this;
			connection.Modified += element_Modified;
			connection.Activating += element_Activating;
			connection.SelectionChanged += connection_SelectionChanged;
			connection.RouteChanged += connection_RouteChanged;
			connection.BendPointMove += connection_BendPointMove;
			connections.AddFirst(connection);
			RecalculateSize();
		}

		private void RemoveConnection(Connection connection)
		{
			if (connection.IsSelected)
			{
				selectedConnectionCount--;
				OnSelectionChanged(EventArgs.Empty);
				OnClipboardAvailabilityChanged(EventArgs.Empty);
				OnSatusChanged(EventArgs.Empty);
			}
			connection.Diagram = null;
			connection.Modified -= element_Modified;
			connection.Activating -= element_Activating;
			connection.SelectionChanged -= connection_SelectionChanged;
			connection.RouteChanged -= connection_RouteChanged;
			connection.BendPointMove -= connection_BendPointMove;
			connections.Remove(connection);			
			RecalculateSize();
		}

		private void shape_SelectionChanged(object sender, EventArgs e)
		{
			if (!selectioning)
			{
				Shape shape = (Shape) sender;

				if (shape.IsSelected)
				{
					selectedShapeCount++;
					shapes.ShiftToFirstPlace(shape);
				}
				else
				{
					selectedShapeCount--;
				}

				OnSelectionChanged(EventArgs.Empty);
				OnClipboardAvailabilityChanged(EventArgs.Empty);
				OnSatusChanged(EventArgs.Empty);
			}
		}

		private void connection_SelectionChanged(object sender, EventArgs e)
		{
			if (!selectioning)
			{
				Connection connection = (Connection) sender;

				if (connection.IsSelected)
				{
					selectedConnectionCount++;
					connections.ShiftToFirstPlace(connection);
				}
				else
				{
					selectedConnectionCount--;
				}

				OnSelectionChanged(EventArgs.Empty);
				OnClipboardAvailabilityChanged(EventArgs.Empty);
				OnSatusChanged(EventArgs.Empty);
			}
		}

		private void connection_RouteChanged(object sender, EventArgs e)
		{
			Connection connection = (Connection) sender;
			connection.ValidatePosition(DiagramConstants.DiagramPadding);

			RecalculateSize();
		}

		private void connection_BendPointMove(object sender, BendPointEventArgs e)
		{
			if (e.BendPoint.X < DiagramConstants.DiagramPadding)
				e.BendPoint.X = DiagramConstants.DiagramPadding;
			if (e.BendPoint.Y < DiagramConstants.DiagramPadding)
				e.BendPoint.Y = DiagramConstants.DiagramPadding;

			// Snap bend points to others if possible
			if (Settings.Default.UsePrecisionSnapping && Control.ModifierKeys != Keys.Shift)
			{
				foreach (Connection connection in connections.GetSelectedElements())
				{
					foreach (BendPoint point in connection.BendPoints)
					{
						if (point != e.BendPoint && !point.AutoPosition)
						{
							int xDist = Math.Abs(e.BendPoint.X - point.X);
							int yDist = Math.Abs(e.BendPoint.Y - point.Y);

							if (xDist <= Connection.PrecisionSize)
							{
								e.BendPoint.X = point.X;
							}
							if (yDist <= Connection.PrecisionSize)
							{
								e.BendPoint.Y = point.Y;
							}
						}
					}
				}
			}
		}

        public void CreateShape()
        {
            CreateShape(newShapeType);
        }

        public virtual void CreateConnection(RelationshipType type)
		{
			state = State.CreatingConnection;
		}
        
		protected virtual void OnEntityRemoved(object sender, EntityEventArgs e)
		{
			Shape shape = GetShape(e.Entity);
			RemoveShape(shape);
		}

		protected void OnRelationRemoved(object sender,RelationshipEventArgs e)
		{
			Connection connection = GetConnection(e.Relationship);
			RemoveConnection(connection);
		}

		protected void OnDeserializing(object sender, SerializeEventArgs e)
		{
            // Old file format
			{
				XmlElement positionsNode = e.Node["Positions"];
				if (positionsNode != null)
				{
					LinkedListNode<Shape> currentShapeNode = shapes.Last;
					foreach (XmlElement shapeNode in positionsNode.SelectNodes("Shape"))
					{
						if (currentShapeNode == null)
							break;
						currentShapeNode.Value.Deserialize(shapeNode);
						currentShapeNode = currentShapeNode.Previous;
					}

					LinkedListNode<Connection> currentConnecitonNode = connections.Last;
					foreach (XmlElement connectionNode in positionsNode.SelectNodes("Connection"))
					{
						if (currentConnecitonNode == null)
							break;
						currentConnecitonNode.Value.Deserialize(connectionNode);
						currentConnecitonNode = currentConnecitonNode.Previous;
					}
				}
			}
		}

		protected virtual void OnOffsetChanged(EventArgs e)
		{
			if (OffsetChanged != null)
				OffsetChanged(this, e);
			UpdateWindowPosition();
		}

		protected virtual void OnSizeChanged(EventArgs e)
		{
			if (SizeChanged != null)
				SizeChanged(this, e);
		}

		protected virtual void OnZoomChanged(EventArgs e)
		{
			if (ZoomChanged != null)
				ZoomChanged(this, e);
			CloseWindows();
		}

		protected virtual void OnSatusChanged(EventArgs e)
		{
			if (StatusChanged != null)
				StatusChanged(this, e);
		}

		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, e);
		}

		protected virtual void OnNeedsRedraw(EventArgs e)
		{
			if (NeedsRedraw != null)
				NeedsRedraw(this, e);
		}

		protected virtual void OnClipboardAvailabilityChanged(EventArgs e)
		{
			if (ClipboardAvailabilityChanged != null)
				ClipboardAvailabilityChanged(this, e);
		}

		protected virtual void OnShowingWindow(PopupWindowEventArgs e)
		{
			if (ShowingWindow != null)
				ShowingWindow(this, e);
		}

		protected virtual void OnHidingWindow(PopupWindowEventArgs e)
		{
			if (HidingWindow != null)
				HidingWindow(this, e);
		}

        public void RemoveEntity(IEntity entity)
        {
            model.RemoveEntity(entity);
        }

        public void RemoveRelationship(Relationship relationship)
        {
            model.RemoveRelationship(relationship);
        }

        protected virtual void OnModified(EventArgs e)
        {
            isDirty = true;
            if (Modified != null)
                Modified(this, e);
        }

	    public bool IsEmpty { get { return model.IsEmpty; } }

	    public bool IsDirty
        {
	        get { return isDirty; }
	    }

        // review the conistency of the model (e.g. Model.IsDirty, etc.).
	    public void Clean()
	    {
            isDirty = false;
	    }

        public bool IsUntitled
        {
            get
            {
                return (name == null);
            }
        }

        public event EventHandler Closing;
        
        public void Close()
	    {
            if (Closing != null)
                Closing(this, EventArgs.Empty);
        }

	    public void Serialize(XmlElement node)
	    {
            if (node == null)
                throw new ArgumentNullException("root");

            XmlElement nameElement = node.OwnerDocument.CreateElement("Name");
            nameElement.InnerText = this.Name;
            node.AppendChild(nameElement);

            model.Serialize(node);
	    }

	    public virtual void Deserialize(XmlElement node)
	    {
            XmlElement nameElement = node["Name"];
            if (nameElement == null || nameElement.InnerText == "")
                this.name = null;
            else
                this.name = nameElement.InnerText;
	    }
        
	    protected virtual void OnRenamed(EventArgs e)
        {
            if (Renamed != null)
                Renamed(this, e);
        }

        public void AddComment()
        {
            model.AddComment();
        }

        protected void AddComment(Comment comment)
        {
            AddShape(new CommentShape(comment));
        }

        public bool InsertComment(Comment comment)
        {
            if (comment == null || model.Entities.Contains(comment)) return false;

            AddComment(comment);
            return true;
        }

        public bool InsertCommentRelationship(CommentRelationship commentRelationship)
        {
            if (commentRelationship != null && !model.Relationships.Contains(commentRelationship) &&
                model.Entities.Contains(commentRelationship.First) && model.Entities.Contains(commentRelationship.Second))
            {
                AddCommentRelationship(commentRelationship);
                return true;
            }

            return false;
        }

        public CommentRelationship AddCommentRelationship(Comment comment, IEntity entity)
        {
            return model.AddCommentRelationship(comment, entity);
        }

        protected CommentRelationship AddCommentRelationship(CommentRelationship commentRelationship)
        {
            Shape startShape = GetShape(commentRelationship.First);
            Shape endShape = GetShape(commentRelationship.Second);
            AddConnection(new CommentConnection(commentRelationship, startShape, endShape));
            return commentRelationship;
        }
    }
}