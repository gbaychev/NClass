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
using NClass.Core.UndoRedo;
using NClass.Translations;

namespace NClass.DiagramEditor.Diagrams
{
    public abstract class DiagramElement : IModifiable
    {
        protected const float UndreadableZoom = 0.25F;
        internal static Graphics Graphics = null; // Graphics object for text measuring

        IDiagram diagram;
        bool isSelected = false;
        bool isActive = false;
        bool isDirty = true;
        bool isMousePressed = false;
        bool needsRedraw = true;

        public event ModifiedEventHandler Modified;
        public event EventHandler SelectionChanged;
        public event EventHandler Activating;
        public event EventHandler Activated;
        public event EventHandler Deactivating;
        public event EventHandler Deactivated;
        public event AbsoluteMouseEventHandler MouseDown;
        public event AbsoluteMouseEventHandler MouseMove;
        public event AbsoluteMouseEventHandler MouseUp;
        public event AbsoluteMouseEventHandler DoubleClick;

        private int dontRaiseRequestCount = 0;
        public bool RaiseChangedEvent
        {
            get => (dontRaiseRequestCount == 0);
            set
            {
                if (!value)
                    dontRaiseRequestCount++;
                else if (dontRaiseRequestCount > 0)
                    dontRaiseRequestCount--;
            }
        }

        public IDiagram Diagram
        {
            get { return diagram; }
            set { diagram = value; }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    NeedsRedraw = true;
                    OnSelectionChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (isActive != value)
                {
                    if (value)
                        OnActivating(EventArgs.Empty);
                    else
                        OnDeactivating(EventArgs.Empty);

                    isActive = value;
                    NeedsRedraw = true;

                    if (isActive)
                        OnActivated(EventArgs.Empty);
                    else
                        OnDeactivated(EventArgs.Empty);
                }
            }
        }

        public bool IsDirty
        {
            get { return isDirty; }
        }

        public bool NeedsRedraw
        {
            get { return needsRedraw; }
            protected internal set { needsRedraw = value; }
        }

        protected bool IsMousePressed
        {
            get { return isMousePressed; }
        }

        public virtual void Clean()
        {
            isDirty = false;
        }

        public virtual void SelectPrevious()
        {
        }

        public virtual void SelectNext()
        {
        }

        public virtual void MoveUp()
        {
        }

        public virtual void MoveDown()
        {
        }

        protected void ShowWindow(PopupWindow window)
        {
            if (Diagram != null)
                Diagram.ShowWindow(window);
        }

        protected void HideWindow(PopupWindow window)
        {
            if (Diagram != null)
                Diagram.HideWindow(window);
        }

        protected internal virtual void ShowEditor()
        {
        }

        protected internal virtual void HideEditor()
        {
        }

        internal RectangleF GetVisibleArea(float zoom)
        {
            return GetVisibleArea(Style.CurrentStyle, zoom);
        }

        internal RectangleF GetVisibleArea(Style style, float zoom)
        {
            return CalculateDrawingArea(style, false, zoom);
        }

        internal RectangleF GetPrintingClip(float zoom)
        {
            return GetPrintingClip(Style.CurrentStyle, zoom);
        }

        internal RectangleF GetPrintingClip(Style style, float zoom)
        {
            return CalculateDrawingArea(style, true, zoom);
        }

        protected abstract RectangleF CalculateDrawingArea(Style style, bool printing, float zoom);

        protected internal virtual void MoveWindow()
        {
        }

        internal bool DeleteSelectedMember()
        {
            return DeleteSelectedMember(true);
        }

        protected internal virtual bool DeleteSelectedMember(bool showConfirmation)
        {
            return false;
        }

        protected bool ConfirmMemberDelete()
        {
            DialogResult result = MessageBox.Show(
                Strings.DeleteMemberConfirmation, Strings.Confirmation,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            return (result == DialogResult.Yes);
        }

        public void Draw(IGraphics g)
        {
            Draw(g, false, Style.CurrentStyle);
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="style"/> is null.
        /// </exception>
        public void Draw(IGraphics g, Style style)
        {
            if (style == null)
                throw new ArgumentNullException("style");

            Draw(g, false, style);
        }

        public void Draw(IGraphics g, bool onScreen)
        {
            Draw(g, onScreen, Style.CurrentStyle);
        }

        public abstract void Draw(IGraphics g, bool onScreen, Style style);

        protected internal abstract Rectangle GetLogicalArea();

        protected internal abstract void DrawSelectionLines(Graphics g, float zoom, Point offset);

        protected internal abstract bool TrySelect(RectangleF frame);

        protected internal abstract void Offset(Size offset);

        protected internal abstract Size GetMaximumPositionChange(Size offset, int padding);

        protected internal abstract IEnumerable<ToolStripItem> GetContextMenuItems(IDiagram diagram, PointF? openedAt = null);

        [Obsolete]
        protected internal abstract void Serialize(XmlElement node);

        [Obsolete]
        protected internal abstract void Deserialize(XmlElement node);

        protected virtual void OnModified(ModificationEventArgs e)
        {
            isDirty = true;
            NeedsRedraw = true;
            Modified?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnActivating(EventArgs e)
        {
            Activating?.Invoke(this, e);
        }

        protected virtual void OnActivated(EventArgs e)
        {
            Activated?.Invoke(this, e);
        }

        protected virtual void OnDeactivating(EventArgs e)
        {
            Deactivating?.Invoke(this, e);
        }

        protected virtual void OnDeactivated(EventArgs e)
        {
            Deactivated?.Invoke(this, e);
        }

        protected virtual void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            isMousePressed = true;
            IsSelected = true;

            MouseDown?.Invoke(this, e);
        }

        protected virtual void OnMouseMove(AbsoluteMouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        protected virtual void OnMouseUp(AbsoluteMouseEventArgs e)
        {
            isMousePressed = false;
            MouseUp?.Invoke(this, e);
        }

        protected virtual void OnDoubleClick(AbsoluteMouseEventArgs e)
        {
            DoubleClick?.Invoke(this, e);
        }

        internal abstract void MousePressed(AbsoluteMouseEventArgs e);

        internal abstract void MouseMoved(AbsoluteMouseEventArgs e);

        internal abstract void MouseUpped(AbsoluteMouseEventArgs e);

        internal abstract void DoubleClicked(AbsoluteMouseEventArgs e);
    }
}
