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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.DiagramEditor.ClassDiagram.Editors;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
    internal sealed class PackageShape : ShapeContainer
    {
        const float LabelRatio = 0.4f;

        public const int DefaultWidth = 320;
        public const int DefaultHeight = 350;
        const int MarginSize = 8;
        private readonly Size marginSize = new Size(MarginSize, MarginSize);

        static PackageEditor editor = new PackageEditor();
        static Pen borderPen = new Pen(Color.Black);
        static SolidBrush backgroundBrush = new SolidBrush(Color.White);
        static SolidBrush solidHeaderBrush = new SolidBrush(Color.White);
        static SolidBrush nameBrush = new SolidBrush(Color.Black);
        static SolidBrush identifierBrush = new SolidBrush(Color.Black);
        static StringFormat headerFormat = new StringFormat(StringFormat.GenericTypographic);
        static StringFormat nameFormat = new StringFormat(StringFormat.GenericTypographic);
        bool editorShowed = false;

        static PackageShape()
        {
            float identifierHeight = Style.CurrentStyle.IdentifierFont.GetHeight();

            nameFormat.FormatFlags = StringFormatFlags.NoWrap;
            nameFormat.Trimming = StringTrimming.EllipsisCharacter;
            headerFormat.FormatFlags = StringFormatFlags.NoWrap;
            headerFormat.Trimming = StringTrimming.EllipsisCharacter;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="package"/> is null.
        /// </exception>
        internal PackageShape(Package package) : base(package)
        {
            this.containerEntity = package;

            MinimumSize = new Size(DefaultWidth, DefaultHeight);
            areShapesHovering = false;
            package.Modified += delegate { UpdateMinSize(); };
        }

        public override IEntity Entity => containerEntity;

        public Package Package => (Package)containerEntity;

        public string Name
        {
            get => containerEntity.Name;
            set
            {
                if (containerEntity.Name == value) return;

                Package.Name = value;
                OnRenamed(EventArgs.Empty);
            }
        }

        public string FullName
        {
            get
            {
                if (this.ParentShape is PackageShape parentPackage)
                    return parentPackage.FullName + "." + Name;
                else
                    return Name;
            }
        }
        protected override Size DefaultSize => new Size(DefaultWidth, DefaultHeight);

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (diagram.DiagramType != DiagramType.ClassDiagram)
                return false;

            return ((ClassDiagram)diagram).InsertPackage(Package.Clone());
        }

        public static Rectangle GetOutline(Style style)
        {
            return new Rectangle(0, 0, DefaultWidth, DefaultHeight);
        }

        protected override int GetBorderWidth(Style style)
        {
            return style.CommentBorderWidth;
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
            var defaultRectangle = new Rectangle(this.Location.X, this.Location.Y, DefaultWidth, DefaultHeight);
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

        protected override bool AcceptsEntity(EntityType type)
        {
            return true;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            if (editorShowed)
            {
                editor.Relocate(this);
                if (!editor.Focused)
                    editor.Focus();
            }
        }

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsActive = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
        {
            if (Contains(e.Location) && e.Button == MouseButtons.Left)
                ShowEditor();
        }

        protected internal override void ShowEditor()
        {
            if (!editorShowed)
            {
                editor.Relocate(this);
                editor.Init(this);
                ShowWindow(editor);
                editor.Focus();
                editorShowed = true;
            }
        }

        protected internal override void HideEditor()
        {
            if (editorShowed)
            {
                HideWindow(editor);
                editorShowed = false;
            }
        }

        protected internal override void MoveWindow()
        {
            HideEditor();
        }

        internal void EditText()
        {
            using (EditCommentDialog dialog = new EditCommentDialog(Name))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    Name = dialog.InputText;
            }
        }

        protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(IDiagram diagram, PointF? openedAt = null)
        {
            return PackageShapeContextMenu.Default.GetMenuItems(diagram, openedAt);
        }

        private void DrawSurface(IGraphics g, bool onScreen, Style style)
        {
            float nameHeight = style.PackageFont.GetHeight();
            float identifierHeight = style.IdentifierFont.GetHeight();
            float textHeight = nameHeight + identifierHeight;

            float packageSplitPos = Left + Width * LabelRatio;

            var gradientBrush = new LinearGradientBrush(this.BorderRectangle, style.PackageBackColor, style.PackageGradientColor, LinearGradientMode.Horizontal);
            // Update graphical objects
            backgroundBrush.Color = style.PackageBackColor;
            borderPen.Color = style.PackageBorderColor;
            borderPen.Width = style.PackageBorderWidth;
            if (style.IsPackageBorderDashed)
                borderPen.DashPattern = borderDashPattern;
            else
                borderPen.DashStyle = DashStyle.Solid;

            // Create shape pattern
            GraphicsPath path = new GraphicsPath();
            path.AddLine(Left, Top, Left + Width * LabelRatio, Top);
            path.AddLine(packageSplitPos, Top, packageSplitPos + MarginSize, Top + textHeight + 2 * MarginSize);
            path.AddLine(packageSplitPos + MarginSize, Top + textHeight + 2* MarginSize, Right, Top + textHeight + 2 * MarginSize);
            path.AddLine(Right, Top + textHeight + 2 * MarginSize, Right, Bottom);
            path.AddLine(Right, Bottom, Left, Bottom);
            path.CloseFigure();

            // Draw shadow first
            if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
            {
                shadowBrush.Color = style.ShadowColor;
                g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
                g.FillPath(shadowBrush, path);
                g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
            }

            // Draw borders & background
            g.FillPath(gradientBrush, path);
            g.DrawPath(borderPen, path);
            gradientBrush.Dispose();
            
            // Draw package split line 
            path.Reset();
            path.AddLine(Left, Top + textHeight + 2 * MarginSize, packageSplitPos + MarginSize, Top + textHeight + 2 * MarginSize);
            g.DrawPath(borderPen, path);
            //g.DrawLine(new Pen(Color.Chartreuse), Left, Top, (int)packageSplitPos, Top);
            //g.DrawLine(new Pen(Color.Red), (int)packageSplitPos, Top, (int)packageSplitPos + MarginSize, Top + (int)textHeight + 2 * MarginSize);
            //g.DrawLine(new Pen(Color.Green), (int)packageSplitPos + MarginSize, Top + (int)textHeight + 2 * MarginSize, Right, Top + (int)textHeight + 2 * MarginSize);
            //g.DrawLine(new Pen(Color.Cyan), Right, Top + (int)textHeight + 2 * MarginSize, Right, Bottom);
            //g.DrawLine(new Pen(Color.BlueViolet), Right, Bottom, Left, Bottom);

            path.Dispose();
        }

        private void DrawHoveringRectangle(IGraphics g, bool onScreen, Style style)
        {

            using (var pen = new Pen(style.HoveringRectangleColor, 1))
            using (var brush = new HatchBrush(style.NonAcceptedShapesStyle, style.NonAcceptedShapesColor, Color.Transparent))
            {
                pen.DashPattern = new[] { 3f, 3f, 1f, 3f, 3f };
                var mouseOverRect = new Rectangle(this.Left, this.Top, this.Width, this.Height);
                mouseOverRect.Inflate(marginSize);
                g.DrawRectangle(pen, mouseOverRect);
                if (!AcceptsEntity(hoveringShape.Entity.EntityType))
                {
                    g.FillRectangle(brush, mouseOverRect);
                }
            }
        }

        private bool HasIdentifier(Style style)
        {
            return (
                style.ShowSignature ||
                style.ShowStereotype && Package.Stereotype != null
            );
        }

        internal Rectangle GetNameRectangle()
        {
            var headerFontHeight = (int)Style.CurrentStyle.IdentifierFont.GetHeight();
            var nameFontHeight = (int)(Style.CurrentStyle.PackageFont.GetHeight() * 1.05f);

            return new Rectangle(
                Left + MarginSize, Top + MarginSize + headerFontHeight,
                (int)(Width * LabelRatio) - MarginSize, nameFontHeight);
        }

        private void DrawHeaderText(IGraphics g, Style style)
        {
            var name = Package.Name;

            RectangleF headerRegion = new Rectangle(Left + MarginSize, Top + MarginSize,
                                                    (int)(Width * LabelRatio) - MarginSize, (int)style.IdentifierFont.GetHeight());

            RectangleF nameRegion = GetNameRectangle();

            // Update styles
            nameBrush.Color = style.NameColor;
            identifierBrush.Color = style.IdentifierColor;
            headerFormat.Alignment = StringAlignment.Near;
            nameFormat.Alignment = StringAlignment.Near;
            headerFormat.LineAlignment = StringAlignment.Far;

            if (HasIdentifier(style))
            {
                // Draw stereotype to the top
                g.DrawString(Package.Stereotype, style.IdentifierFont,
                    identifierBrush, headerRegion, headerFormat);

                // Draw name to the bottom
                g.DrawString(name, style.PackageFont, nameBrush, nameRegion, headerFormat);
            }
            else
            {
                // Draw name only
                g.DrawString(name, style.PackageFont, nameBrush, nameRegion, nameFormat);
            }
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            DrawSurface(g, onScreen, style);
            DrawHeaderText(g, style);
            // draw the hovering rectangle
            if (areShapesHovering)
            {
                DrawHoveringRectangle(g, onScreen, style);
            }
        }

        protected override float GetRequiredWidth(Graphics g, Style style)
        {
            return Width;
        }

        protected override int GetRequiredHeight()
        {
            return Height;
        }

        public override string ToString()
        {
            return $"{this.FullName} : {Strings.Package}";
        }
        
        protected override void UpdateSize()
        {
            var border = this.BorderRectangle;
            var shouldAddMargin = false;
            foreach (var shape in ChildrenShapes)
            {
                if (!this.BorderRectangle.Contains(shape.BorderRectangle))
                    shouldAddMargin = true;
                border = Rectangle.Union(border, shape.BorderRectangle);
            }

            if (shouldAddMargin)
                this.Size = border.Size + marginSize;
            else
                this.Size = border.Size;
        }
    }
}
