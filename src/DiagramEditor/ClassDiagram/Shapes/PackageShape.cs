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
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.DiagramEditor.ClassDiagram.Editors;
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
    internal sealed class PackageShape : Shape
    {
        const float LabelRatio = 0.4f;

        public const int DefaultWidth = 320;
        public const int DefaultHeight = 350;
        const int MarginSize = 8;

        static PackageEditor editor = new PackageEditor();
        static Pen borderPen = new Pen(Color.Black);
        static SolidBrush backgroundBrush = new SolidBrush(Color.White);
        static SolidBrush solidHeaderBrush = new SolidBrush(Color.White);
        static SolidBrush nameBrush = new SolidBrush(Color.Black);
        static SolidBrush identifierBrush = new SolidBrush(Color.Black);
        static StringFormat headerFormat = new StringFormat(StringFormat.GenericTypographic);
        static StringFormat nameFormat = new StringFormat(StringFormat.GenericTypographic);
        Package package;
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
            this.package = package;

            MinimumSize = new Size(DefaultWidth, DefaultHeight);
            package.Modified += delegate { UpdateMinSize(); };
        }

        public override IEntity Entity
        {
            get { return package; }
        }

        public Package Package
        {
            get { return package; }
        }

        public string Name
        {
            get { return package.Name; }
            set { package.Name = value; }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(DefaultWidth, DefaultHeight);
            }
        }

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
            HideEditor();
        }

        void UpdateMinSize()
        {
            MinimumSize = new Size(MinimumSize.Width, GetRequiredHeight());
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

        protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(IDiagram diagram)
        {
            return PackageShapeContextMenu.Default.GetMenuItems(diagram);
        }

        private void DrawSurface(IGraphics g, bool onScreen, Style style)
        {
            float nameHeight = style.PackageFont.GetHeight();
            float identifierHeight = style.IdentifierFont.GetHeight();
            float textHeight = nameHeight + identifierHeight;

            float packageSplitPos = Left + Width * LabelRatio;

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
            g.FillPath(backgroundBrush, path);
            g.DrawPath(borderPen, path);

            // Draw package split line 
            path.Reset();
            path.AddLine(Left, Top + textHeight + 2 * MarginSize, packageSplitPos + MarginSize, Top + textHeight + 2 * MarginSize);
            g.DrawPath(borderPen, path);

            path.Dispose();
        }

        private static StringAlignment GetHorizontalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    return StringAlignment.Near;

                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                default:
                    return StringAlignment.Center;

                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    return StringAlignment.Far;
            }
        }

        private static StringAlignment GetVerticalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    return StringAlignment.Near;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                default:
                    return StringAlignment.Center;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;
            }
        }

        private bool HasIdentifier(Style style)
        {
            return (
                style.ShowSignature ||
                style.ShowStereotype && package.Stereotype != null
            );
        }

        internal Rectangle GetNameRectangle()
        {
            var headerFontHeight = (int)Style.CurrentStyle.IdentifierFont.GetHeight();
            var nameFontHeight = (int)Style.CurrentStyle.PackageFont.GetHeight();

            return new Rectangle(
                Left + MarginSize, Top + MarginSize + headerFontHeight,
                (int)(Width * LabelRatio) - MarginSize, nameFontHeight);
        }

        private void DrawHeaderText(IGraphics g, Style style)
        {
            var name = package.Name;

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
                g.DrawString(package.Stereotype, style.IdentifierFont,
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
            return Strings.Package;
        }
    }
}
