// NClass - Free class diagram editor
// Copyright (C) 2025 Georgi Baychev
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
using NClass.Core;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Editors;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class UseCaseShape : UseCaseShapeBase
    {
        private bool isEditorShown = false;
        private ShapeNameEditor editor;
        private readonly UseCase useCase;
        const int DefaultWidth = 160;
        const int DefaultHeight = 75;
        private readonly StringFormat stringFormat = StringFormat.GenericTypographic;

        public UseCaseShape(UseCase entity) : base(entity)
        {
            this.useCase = entity;
            this.editor = new ShapeNameEditor();
            this.stringFormat.Trimming = StringTrimming.EllipsisWord;
            this.stringFormat.FormatFlags = StringFormatFlags.LineLimit;
            this.stringFormat.Alignment = StringAlignment.Center;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            DrawSurface(g, onScreen, style);
            DrawText(g, onScreen, style);
        }

        private void DrawSurface(IGraphics g, bool onScreen, Style style)
        {
            if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
            {
                shadowBrush.Color = style.ShadowColor;
                g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
                g.FillEllipse(shadowBrush, Location.X + style.ShadowOffset.Width, Location.Y + style.ShadowOffset.Height, Width, Height);
                g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
            }
            
            if(style.UseCaseGradientStyle == GradientStyle.None)
            {
                using (var pen = new Pen(style.UseCaseBorderColor, style.UseCaseBorderWidth))
                using (var brush = new SolidBrush(style.UseCaseBackColor))
                {
                    g.FillEllipse(brush, Location.X, Location.Y, Width, Height);
                    g.DrawEllipse(pen, Location.X, Location.Y, Size.Width, Size.Height);
                }
            }
            else
            {
                LinearGradientMode gradientMode;
                switch (style.UseCaseGradientStyle)
                {
                    case GradientStyle.Vertical:
                        gradientMode = LinearGradientMode.Vertical;
                        break;
                    case GradientStyle.Diagonal:
                        gradientMode = LinearGradientMode.ForwardDiagonal;
                        break;
                    case GradientStyle.Horizontal:
                    default:
                        gradientMode = LinearGradientMode.Horizontal; break;
                }

                var surface = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
                using (var surfaceBrush = new LinearGradientBrush(surface,style.UseCaseGradientColor, style.UseCaseBackColor, gradientMode))
                using (var borderPen = new Pen(style.UseCaseBorderColor, style.UseCaseBorderWidth))
                {
                    g.FillEllipse(surfaceBrush, surface);
                    g.DrawEllipse(borderPen, Location.X, Location.Y, Size.Width, Size.Height);
                }
            }
        }

        private void DrawText(IGraphics g, bool onScreen, Style style)
        {
            var textBounds = GetTextRectangle();
            using (var textBrush = new SolidBrush(Color.FromArgb(128, style.UseCaseTextColor)))
            {
                if (string.IsNullOrEmpty(Text) && onScreen)
                {
                    textBrush.Color = Color.FromArgb(128, style.CommentTextColor);
                    g.DrawString(Strings.DoubleClickToEdit,
                        style.CommentFont, textBrush, textBounds, stringFormat);
                }
                else
                {
                    textBrush.Color = style.CommentTextColor;
                    g.DrawString(Text, style.CommentFont, textBrush, textBounds, stringFormat);
                }
            }
        }

        protected override Size DefaultSize => new Size(DefaultWidth, DefaultHeight);

        public override IEntity Entity => this.useCase;

        public string Text => useCase.Name;
        public UseCase UseCase => useCase;

        protected override int GetBorderWidth(Style style)
        {
            return style.UseCaseBorderWidth;
        }

        protected override float GetRequiredWidth(Graphics g, Style style)
        {
            return Width;
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (!(diagram is UseCaseDiagram useCaseDiagram))
                return false;

            return ((UseCaseDiagram)diagram).InsertUseCase(useCase.Clone());
        }


        /// <summary>
        /// Calculates the text rectangle by finding the two
        /// foci of the ellipse and then calculating where the 
        /// semi latus rectum should intersect
        /// https://en.wikipedia.org/wiki/Ellipse#Equation
        /// </summary>
        /// <returns>The rectangle containing the text bounds</returns>
        internal Rectangle GetTextRectangle()
        {
            var semiAxisA = Width / 2;
            var semiAxisB = Height / 2;

            var focus = (int)Math.Sqrt(semiAxisA * semiAxisA - semiAxisB * semiAxisB);
            var semiLatusRectum = (semiAxisB * semiAxisB / semiAxisA);
            var x = Location.X + semiAxisA - focus;
            var y = Location.Y + semiAxisB - semiLatusRectum;

            var textRectangle =  new Rectangle(x, y, focus * 2, semiLatusRectum * 2);
            return Rectangle.Inflate(textRectangle, -2, -2);
        }

        protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
        {
            ShowEditor();
            base.OnDoubleClick(e);
        }

        protected internal override void ShowEditor()
        {
            if (!isEditorShown)
            {
                var backColor = Style.CurrentStyle.UseCaseBackColor;
                var gradColor = Style.CurrentStyle.UseCaseGradientStyle == GradientStyle.None ?
                                Style.CurrentStyle.UseCaseBackColor : 
                                Style.CurrentStyle.UseCaseGradientColor;
                var textColor = Style.CurrentStyle.UseCaseTextColor;
                editor.Init(this,backColor ,gradColor, textColor, Style.CurrentStyle.UseCaseFont);
                editor.Relocate(this, GetTextRectangle());
                ShowWindow(editor);
                editor.Focus();
                isEditorShown = true;
            }
        }

        protected internal override void HideEditor()
        {
            if (isEditorShown)
            {
                HideWindow(editor);
                isEditorShown = false;
            }
        }

        protected override void OnMove(MoveEventArgs e)
        {
            HideEditor();
            base.OnMove(e);
        }

        protected internal override void MoveWindow()
        {
            HideEditor();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            if (isEditorShown)
            {
                editor.Relocate(this, GetTextRectangle());
                if (!editor.Focused)
                    editor.Focus();
            }
        }

        public static Rectangle GetOutline(Style style)
        {
            return new Rectangle(0, 0, DefaultWidth, DefaultHeight);
        }

        public override IUseCaseEntity UseCaseEntity => this.useCase;

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            if(isEditorShown)
                HideEditor();
            base.OnMouseDown(e);
        }
    }
}