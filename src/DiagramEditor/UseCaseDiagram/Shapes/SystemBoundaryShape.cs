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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Editors;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class SystemBoundaryShape : ShapeContainer
    {
        private static readonly Size defaultSize = new Size(300, 300);
        private const int MarginSize = 8;
        private readonly Size marginSize = new Size(MarginSize, MarginSize);
        private const int HeaderHeight = 45;

        private readonly SystemBoundary systemBoundary;

        private ShapeNameEditor editor;
        private bool editorShown = false;

        public SystemBoundaryShape(SystemBoundary entity) : base(entity)
        {
            this.systemBoundary = entity;
            this.systemBoundary.Modified += delegate { UpdateMinSize(); };
            this.editor = new ShapeNameEditor();
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            DrawSurface(g, onScreen, style);
            DrawText(g, style);
        }

        private void DrawSurface(IGraphics g, bool onScreen, Style style)
        {
            var backgroundRectangle = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            var backGroundPath = RoundedRectangle.Create(backgroundRectangle, 5);
            if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
            {
                shadowBrush.Color = style.ShadowColor;
                g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
                g.FillPath(shadowBrush, backGroundPath);
                g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
            }

            Brush backgroundBrush;

            switch (style.SystemBoundaryGradientStyle)
            {
                case GradientStyle.Vertical:
                    backgroundBrush = new LinearGradientBrush(backgroundRectangle, style.SystemBoundaryGradientColor, style.SystemBoundaryBackColor, LinearGradientMode.Vertical);
                    break;
                case GradientStyle.Diagonal:
                    backgroundBrush = new LinearGradientBrush(backgroundRectangle, style.SystemBoundaryGradientColor, style.SystemBoundaryBackColor, LinearGradientMode.BackwardDiagonal);
                    break;
                case GradientStyle.Horizontal:
                    backgroundBrush = new LinearGradientBrush(backgroundRectangle, style.SystemBoundaryGradientColor, style.SystemBoundaryBackColor, LinearGradientMode.Horizontal);
                    break;
                default:
                    backgroundBrush = new SolidBrush(style.SystemBoundaryBackColor);
                    break;
            }

            using (backGroundPath)
            using (backgroundBrush)
            using (var borderPen = new Pen(style.SystemBoundaryBorderColor, style.SystemBoundaryBorderWidth))
            {
                g.FillPath(backgroundBrush, backGroundPath);
                g.DrawPath(borderPen, backGroundPath);
            }
        }

        private void DrawText(IGraphics g, Style style)
        {
            var name = this.systemBoundary.Name;
            var textRegion = GetTextRectangle();
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };

            using (var nameBrush = new SolidBrush(style.SystemBoundaryTextColor))
            {
                g.DrawString(name, style.SystemBoundaryFont, nameBrush, textRegion, stringFormat);
            }
        }

        private Rectangle GetTextRectangle()
        {
            return new Rectangle(Left + MarginSize, Top + MarginSize,
                Width - MarginSize * 2, HeaderHeight - MarginSize * 2);
        }

        protected override Size DefaultSize => defaultSize;
        public override IEntity Entity => systemBoundary;

        protected override int GetBorderWidth(Style style)
        {
            return this.size.Width;
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (diagram.DiagramType != DiagramType.UseCaseDiagram)
                return false;

            return ((UseCaseDiagram) diagram).InsertSystemBoundary(this.systemBoundary.Clone());
        }

        protected override bool AcceptsEntity(EntityType type)
        {
            return type == EntityType.UseCase;
        }

        public static Rectangle GetOutline(Style currentStyle)
        {
            return new Rectangle(new Point(0,0), defaultSize);
        }

        protected override void OnMove(MoveEventArgs e)
        {
            base.OnMove(e);

            // drag together only the non-selected shapes
            foreach (var shape in this.ChildrenShapes.Where(s => !s.IsSelected))
            {
                shape.Location += new Size((int)e.Offset.Width, (int)e.Offset.Height);
            }
            HideEditor();
        }

        protected override void UpdateMinSize()
        {
            var defaultRectangle = new Rectangle(this.Location.X, this.Location.Y, defaultSize.Width, defaultSize.Height);
            var minRectangle = defaultRectangle;
            var shouldAddMargin = false;
            foreach (var childrenShape in ChildrenShapes)
            {
                if (!defaultRectangle.Contains(childrenShape.BorderRectangle))
                    shouldAddMargin = true;
                minRectangle = Rectangle.Union(minRectangle, childrenShape.BorderRectangle);
            }

            if (shouldAddMargin)
                MinimumSize = minRectangle.Size + marginSize;
            else
                MinimumSize = minRectangle.Size;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            if (editorShown)
            {
                editor.Relocate(this, GetTextRectangle());
                if (!editor.Focused)
                    editor.Focus();
            }
        }

        protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
        {
            if (Contains(e.Location) && e.Button == MouseButtons.Left)
                ShowEditor();
        }

        protected internal override void ShowEditor()
        {
            if (!editorShown)
            {
                editor.Init(this, Style.CurrentStyle.SystemBoundaryBackColor, Style.CurrentStyle.SystemBoundaryTextColor, Style.CurrentStyle.SystemBoundaryFont);
                editor.Relocate(this, GetTextRectangle());
                ShowWindow(editor);
                editor.Focus();
                editorShown = true;
            }
        }

        protected internal override void HideEditor()
        {
            if (editorShown)
            {
                HideWindow(editor);
                editorShown = false;
            }
        }

        protected internal override void MoveWindow()
        {
            HideEditor();
        }

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsActive = true;
            }
            base.OnMouseDown(e);
        }
    }
}