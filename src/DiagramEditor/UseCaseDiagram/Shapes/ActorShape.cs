// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
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

namespace NClass.DiagramEditor.UseCaseDiagram.Shapes
{
    public class ActorShape : UseCaseShapeBase
    {
        private bool isEditorShown = false;
        private ShapeNameEditor editor;
        private Actor actor;
        private const int DefaultWidth = 75;
        private const int DefaultHeight = 150;
        private const int PaddingSize = 5;
        private readonly Size defaultSize = new Size(DefaultWidth, DefaultHeight);
        private const int Proportion = 2; // Height / Width
        private readonly StringFormat stringFormat;


        public ActorShape(Actor entity) : base(entity)
        {
            actor = entity;
            this.MinimumSize = defaultSize;
            editor = new ShapeNameEditor();
            this.stringFormat = StringFormat.GenericTypographic;
            this.stringFormat.Alignment = StringAlignment.Center;
            this.stringFormat.Trimming = StringTrimming.EllipsisWord;
            this.stringFormat.FormatFlags = StringFormatFlags.LineLimit;
        }

        public override void Draw(IGraphics g, bool onScreen, Style style)
        {
            DrawSurface(g, onScreen, style);
            DrawText(g, onScreen, style);
        }

        private void DrawText(IGraphics graphics, bool onScreen, Style style)
        {
            //graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(this.Left, this.Top, this.Width, this.Height) );
            //graphics.DrawRectangle(new Pen(Color.Black), GetTextRectangle(graphics, style) );
            graphics.DrawString(
                                Entity.Name,
                                style.StaticMemberFont,
                                new SolidBrush(style.ActorTextColor),
                                GetTextRectangle(),
                                this.stringFormat);
        }

        private void DrawSurface(IGraphics graphics, bool onScreen, Style style)
        {
            var actorPen = new Pen(style.ActorColor);
            var headSize = this.Width / 2 - PaddingSize;
            var headRectangle = new Rectangle(this.Left + PaddingSize + headSize / 2, this.Top + PaddingSize, headSize, headSize);
            // directly beyond the head
            var bodyStart = new Point(headRectangle.X + headRectangle.Width / 2, headRectangle.Bottom);
            // 2/3 of the length;
            var bodyEnd = new Point(headRectangle.X + headRectangle.Width / 2, headRectangle.Bottom + (this.Height + PaddingSize * 2) / 3);
            var leftLeg = new Point(bodyEnd.X - headRectangle.Width / 3, bodyEnd.Y + ((bodyEnd.Y - bodyStart.Y) / 2));
            var rightLeg = new Point(bodyEnd.X + headRectangle.Width / 3, bodyEnd.Y + ((bodyEnd.Y - bodyStart.Y) / 2));

            var handsStart = new Point(headRectangle.X - headRectangle.Width / 6, headRectangle.Bottom + ((bodyEnd.Y - bodyStart.Y) / 10));
            var handsEnd = new Point(headRectangle.X + headRectangle.Width + headRectangle.Width / 6, headRectangle.Bottom + ((bodyEnd.Y - bodyStart.Y) / 10));

            Brush headBrush;

            switch (style.ActorGradientStyle)
            {
                case GradientStyle.Vertical:
                    headBrush = new LinearGradientBrush(headRectangle, style.ActorBackColor, style.ActorGradientColor, LinearGradientMode.Vertical);
                    break;
                case GradientStyle.Diagonal:
                    headBrush = new LinearGradientBrush(headRectangle, style.ActorBackColor, style.ActorGradientColor, LinearGradientMode.BackwardDiagonal);
                    break;
                case GradientStyle.Horizontal:
                    headBrush = new LinearGradientBrush(headRectangle, style.ActorBackColor, style.ActorGradientColor, LinearGradientMode.Horizontal);
                    break;
                default:
                    headBrush = new SolidBrush(style.ActorBackColor);
                    break;
            }

            // head
            graphics.FillEllipse(headBrush, headRectangle);
            graphics.DrawEllipse(actorPen,
                                 headRectangle.X,
                                 headRectangle.Y,
                                 headRectangle.Width,
                                 headRectangle.Height);
            headBrush.Dispose();

            // body
            graphics.DrawLine(actorPen, bodyStart, bodyEnd);
            // legs
            graphics.DrawLine(actorPen, bodyEnd, leftLeg);
            graphics.DrawLine(actorPen, bodyEnd, rightLeg);
            // hands
            graphics.DrawLine(actorPen, handsStart, handsEnd);
            actorPen.Dispose();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            if (Math.Abs(e.SizeChange.Width) > Math.Abs(e.SizeChange.Height))
            {
                this.size.Height = Proportion * this.Width;
            }
            else
            {
                this.size.Width = this.Height / Proportion;
            }
            if (isEditorShown)
            {
                editor.Relocate(this, GetTextRectangle());
                if (!editor.Focused)
                    editor.Focus();
            }
            base.OnResize(e);
        }
        protected override Size DefaultSize => defaultSize;

        public override IEntity Entity => this.actor;

        protected override int GetBorderWidth(Style style)
        {
            return 0;
        }

        protected override float GetRequiredWidth(Graphics g, Style style)
        {
            return Width;
        }

        public Rectangle GetTextRectangle()
        {
            int left = this.Left + PaddingSize;
            int width = this.Width - 2 * PaddingSize;
            int height = this.Height / 10;
            int top = this.Bottom - height - PaddingSize;
            return new Rectangle(left, top, width, height);
        }

        protected override bool CloneEntity(IDiagram diagram)
        {
            if (!(diagram is UseCaseDiagram useCaseDiagram))
                return false;

            return ((UseCaseDiagram)diagram).InsertActor(actor.Clone());
        }

        public string Name
        {
            get { return actor.Name; }
            set
            {
                if (String.Compare(actor.Name, value, StringComparison.Ordinal) != 0)
                {
                    actor.Name = value;
                }
            }
        }

        public static Rectangle GetOutline(Style style)
        {
            return new Rectangle(0, 0, DefaultWidth, DefaultHeight);
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
                editor.Init(this, Style.CurrentStyle.ActorBackColor, Style.CurrentStyle.ActorBackColor, Style.CurrentStyle.ActorTextColor, Style.CurrentStyle.ActorFont);
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

        public override IUseCaseEntity UseCaseEntity => this.actor;

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            if (isEditorShown)
                HideEditor();
            base.OnMouseDown(e);
        }
    }
}