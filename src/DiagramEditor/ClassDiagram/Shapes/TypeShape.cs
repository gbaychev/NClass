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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.Editors;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
	public abstract class TypeShape : Shape
	{
		public const int DefaultWidth = 162;
		public const int DefaultHeight = 216;
		protected const int MarginSize = 8;
		protected const int IconSpacing = 21;
		protected const int HeaderHeight = 45;
		protected const int MemberHeight = 17;
		protected static readonly StringFormat memberFormat;

		static Pen borderPen = new Pen(Color.Black);
		static SolidBrush backgroundBrush = new SolidBrush(Color.White);
		static SolidBrush solidHeaderBrush = new SolidBrush(Color.White);
		static SolidBrush nameBrush = new SolidBrush(Color.Black);
		static SolidBrush identifierBrush = new SolidBrush(Color.Black);
		static StringFormat headerFormat = new StringFormat(StringFormat.GenericTypographic);
		static readonly Size chevronSize = new Size(13, 13);

		public event EventHandler ActiveMemberChanged;

		int activeMemberIndex = -1;
		bool collapsed = false;
		bool showChevron = false;
		EditorWindow showedEditor = null;

		static TypeShape()
		{
			memberFormat = new StringFormat(StringFormat.GenericTypographic);
			memberFormat.FormatFlags = StringFormatFlags.NoWrap;
			memberFormat.LineAlignment = StringAlignment.Center;
			memberFormat.Trimming = StringTrimming.EllipsisCharacter;

			headerFormat.FormatFlags = StringFormatFlags.NoWrap;
			headerFormat.Trimming = StringTrimming.EllipsisCharacter;
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="typeBase"/> is null.
		/// </exception>
		protected TypeShape(TypeBase typeBase) : base(typeBase)
		{
			MinimumSize = new Size(DefaultWidth, MinimumSize.Height);
			typeBase.Modified += delegate { UpdateMinSize(); };
		}

		protected internal virtual int ActiveMemberIndex
		{
			get
			{
				return activeMemberIndex;
			}
			set
			{
				if (value >= -1)
					activeMemberIndex = value;
			}
		}

		protected bool Collapsed
		{
			get
			{
				return collapsed;
			}
			set
			{
				if (collapsed != value)
				{
					Size oldSize = Size;
					collapsed = value;
					OnResize(new ResizeEventArgs(Size - oldSize));
					OnModified(EventArgs.Empty);
				}
			}
		}

		public override Size Size
		{
			get
			{
				if (Collapsed)
					return new Size(Width, HeaderHeight);
				else
					return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		public override int Height
		{
			get
			{
				if (Collapsed)
					return HeaderHeight;
				else
					return base.Height;
			}
			set
			{
				base.Height = value;
			}
		}

		private bool CanDrawChevron
		{
			get
			{
				return (
					Settings.Default.ShowChevron == ChevronMode.Always ||
					Settings.Default.ShowChevron == ChevronMode.AsNeeded && showChevron
				);
			}
		}

		private RectangleF CaptionRegion
		{
			get
			{
				return new RectangleF(
					Left + MarginSize, Top + MarginSize,
					Width - MarginSize * 2, HeaderHeight - MarginSize * 2
				);
			}
		}

		public sealed override IEntity Entity
		{
			get { return TypeBase; }
		}

		public abstract TypeBase TypeBase
		{
			get;
		}

		protected abstract TypeEditor HeaderEditor
		{
			get;
		}

		protected abstract EditorWindow ContentEditor
		{
			get;
		}

		public static Rectangle GetOutline(Style style)
		{
			return new Rectangle(0, 0, DefaultWidth, DefaultHeight);
		}

		protected abstract Color GetBackgroundColor(Style style);

		protected abstract Color GetHeaderColor(Style style);

		protected abstract GradientStyle GetGradientHeaderStyle(Style style);

		protected abstract Color GetBorderColor(Style style);

		protected abstract bool IsBorderDashed(Style style);

		protected abstract int GetRoundingSize(Style style);

		protected virtual Font GetFont(Style style)
		{
			return style.MemberFont;
		}

		protected virtual Font GetNameFont(Style style)
		{
			return style.NameFont;
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(DefaultWidth, DefaultHeight);
			}
		}

		private bool HasIdentifier(Style style)
		{
			return (
				style.ShowSignature ||
				style.ShowStereotype && TypeBase.Stereotype != null
			);
		}

		public override void Collapse()
		{
			Collapsed = true;
		}

		public override void Expand()
		{
			Collapsed = false;
		}

		protected internal override void ShowEditor()
		{
			EditorWindow editor = GetEditorWindow();
			if (editor != null)
			{
				ShowEditor(editor);
			}
		}

		protected internal override void HideEditor()
		{
			if (showedEditor != null)
			{
				HideWindow(showedEditor);
				showedEditor = null;
			}
		}

		private void ShowEditor(EditorWindow editor)
		{
			editor.Relocate(this);
			ShowWindow(editor);
			editor.Init(this);
			editor.Focus();
			showedEditor = editor;
		}

		protected internal abstract void EditMembers();

		protected internal abstract override bool DeleteSelectedMember(bool showConfirmation);

		protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(Diagram diagram)
		{
			return TypeShapeContextMenu.Default.GetMenuItems(diagram);
		}

		private bool IsChevronPressed(PointF mouseLocation)
		{
			return (
				Settings.Default.ShowChevron != ChevronMode.Never &&
				mouseLocation.X >= Right - MarginSize - chevronSize.Width &&
				mouseLocation.X < Right - MarginSize &&
				mouseLocation.Y >= Top + MarginSize &&
				mouseLocation.Y < Top + MarginSize + chevronSize.Height
			);
		}

		protected abstract EditorWindow GetEditorWindow();

		protected override void OnMove(MoveEventArgs e)
		{
			base.OnMove(e);
			EditorWindow window = GetEditorWindow();
			if (window != null)
				window.Relocate(this);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			
			EditorWindow window = GetEditorWindow();
			if (window != null)
				window.Relocate(this);
		}

		protected override void OnMouseDown(AbsoluteMouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (IsChevronPressed(e.Location))
			{
				Collapsed = !Collapsed;
			}
			else
			{
				if (e.Button == MouseButtons.Left)
				{
					IsActive = true;
				}
			}
		}

		protected override void OnMouseUp(AbsoluteMouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (showedEditor != null)
			{
				showedEditor.Focus();
			}
		}

		protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
		{
			base.OnDoubleClick(e);

			if (!IsChevronPressed(e.Location) && Contains(e.Location) &&
				e.Button == MouseButtons.Left)
			{
				ShowEditor();
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			showChevron = true;
			if (Settings.Default.ShowChevron == ChevronMode.AsNeeded)
				NeedsRedraw = true;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			showChevron = false;
			if (Settings.Default.ShowChevron == ChevronMode.AsNeeded)
				NeedsRedraw = true;
		}

		protected void UpdateMinSize()
		{
			MinimumSize = new Size(MinimumSize.Width, GetRequiredHeight());
		}

		protected override Shape.ResizeMode GetResizeMode(AbsoluteMouseEventArgs e)
		{
			ResizeMode resizeMode = base.GetResizeMode(e);

			if (Collapsed)
			{
				return (resizeMode & ~ResizeMode.Bottom);
			}
			else
			{
				return resizeMode;
			}
		}

		protected void DrawSeparatorLine(IGraphics g, int height)
		{
			g.DrawLine(borderPen, Left, height, Right, height);
		}

		private void DrawRectangleSurface(IGraphics g, bool onScreen, Style style)
		{
			// Draw shadow
			if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
			{
				shadowBrush.Color = style.ShadowColor;
				g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
				g.FillRectangle(shadowBrush, BorderRectangle);
				g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
			}

			// Draw background
			backgroundBrush.Color = GetBackgroundColor(style);
			g.FillRectangle(backgroundBrush, BorderRectangle);

			// Draw header background
			DrawHeaderBackground(g, style);

			// Draw border
			g.DrawRectangle(borderPen, BorderRectangle);
		}

		private void DrawHeaderBackground(IGraphics g, Style style)
		{
			Color backColor = GetBackgroundColor(style);
			Color headerColor = GetHeaderColor(style);

			Rectangle headerRectangle = new Rectangle(Left, Top, Width, HeaderHeight);

			if (GetGradientHeaderStyle(style) != GradientStyle.None)
			{
				LinearGradientMode gradientMode;

				switch (GetGradientHeaderStyle(style))
				{
					case GradientStyle.Vertical:
						gradientMode = LinearGradientMode.Vertical; break;

					case GradientStyle.Diagonal:
						gradientMode = LinearGradientMode.ForwardDiagonal; break;

					case GradientStyle.Horizontal:
					default:
						gradientMode = LinearGradientMode.Horizontal; break;
				}

				Brush headerBrush = new LinearGradientBrush(headerRectangle,
					headerColor, backColor, gradientMode);
				g.FillRectangle(headerBrush, headerRectangle);
				headerBrush.Dispose();
			}
			else
			{
				if (headerColor != backColor || headerColor.A < 255)
				{
					solidHeaderBrush.Color = GetHeaderColor(style);
					g.FillRectangle(solidHeaderBrush, headerRectangle);
				}
			}
		}

		private void DrawRoundedSurface(IGraphics g, bool onScreen, Style style)
		{
			int diameter = GetRoundingSize(style) * 2;

			GraphicsPath borderPath = new GraphicsPath();
			borderPath.AddArc(Left, Top, diameter, diameter, 180, 90);
			borderPath.AddArc(Right - diameter, Top, diameter, diameter, 270, 90);
			borderPath.AddArc(Right - diameter, Bottom - diameter, diameter, diameter, 0, 90);
			borderPath.AddArc(Left, Bottom - diameter, diameter, diameter, 90, 90);
			borderPath.CloseFigure();

			// Draw shadow
			if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
			{
				shadowBrush.Color = style.ShadowColor;
				g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
				g.FillPath(shadowBrush, borderPath);
				g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
			}

			// Draw background
			g.FillPath(backgroundBrush, borderPath);

			// Draw header background
			Region oldClip = g.Clip;
			g.SetClip(borderPath, CombineMode.Intersect);
			DrawHeaderBackground(g, style);
			g.Clip.Dispose();
			g.Clip = oldClip;

			// Draw border
			g.DrawPath(borderPen, borderPath);

			borderPath.Dispose();
		}

		private void DrawSurface(IGraphics g, bool onScreen, Style style)
		{
			// Update styles
			backgroundBrush.Color = GetBackgroundColor(style);
			borderPen.Color = GetBorderColor(style);
			borderPen.Width = GetBorderWidth(style);
			if (IsBorderDashed(style))
				borderPen.DashPattern = borderDashPattern;
			else
				borderPen.DashStyle = DashStyle.Solid;

			if (GetRoundingSize(style) == 0)
				DrawRectangleSurface(g, onScreen, style);
			else
				DrawRoundedSurface(g, onScreen, style);
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

		private static float GetHeaderTextTop(RectangleF textRegion, float textHeight,
			ContentAlignment alignment)
		{
			float top = textRegion.Top;

			switch (alignment)
			{
				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomRight:
					top += textRegion.Height - textHeight;
					break;

				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleRight:
					top += (textRegion.Height - textHeight) / 2;
					break;
			}

			return top;
		}

		private void DrawHeaderText(IGraphics g, Style style)
		{
			string name = TypeBase.Name;
			RectangleF textRegion = CaptionRegion;

			// Update styles
			nameBrush.Color = style.NameColor;
			identifierBrush.Color = style.IdentifierColor;
			headerFormat.Alignment = GetHorizontalAlignment(style.HeaderAlignment);

			if (HasIdentifier(style))
			{
				float nameHeight = GetNameFont(style).GetHeight();
				float identifierHeight = style.IdentifierFont.GetHeight();
				float textHeight = nameHeight + identifierHeight;

				textRegion.Y = GetHeaderTextTop(textRegion, textHeight, style.HeaderAlignment);
				textRegion.Height = textHeight;

				// Draw name and signature
				if (style.ShowSignature)
				{
					// Draw name to the top
					headerFormat.LineAlignment = StringAlignment.Near;
					g.DrawString(name, GetNameFont(style), nameBrush, textRegion, headerFormat);

					// Draw signature to the bottom
					headerFormat.LineAlignment = StringAlignment.Far;
					g.DrawString(TypeBase.Signature, style.IdentifierFont, identifierBrush,
						textRegion, headerFormat);
				}
				// Draw name and stereotype
				else
				{
					// Draw stereotype to the top
					headerFormat.LineAlignment = StringAlignment.Near;
					g.DrawString(TypeBase.Stereotype, style.IdentifierFont,
						identifierBrush, textRegion, headerFormat);

					// Draw name to the bottom
					headerFormat.LineAlignment = StringAlignment.Far;
					g.DrawString(name, GetNameFont(style), nameBrush, textRegion, headerFormat);
				}
			}
			else
			{
				// Draw name only
				headerFormat.LineAlignment = GetVerticalAlignment(style.HeaderAlignment);
				g.DrawString(name, GetNameFont(style), nameBrush, textRegion, headerFormat);
			}

			if (!Collapsed)
				DrawSeparatorLine(g, Top + HeaderHeight);
		}

		private void DrawChevron(IGraphics g)
		{
			Bitmap chevron = (Collapsed) ? Properties.Resources.Expand : Properties.Resources.Collapse;
			Point location = new Point(Right - MarginSize - chevronSize.Width, Top + MarginSize);

			g.DrawImage(chevron, location);
		}

		protected abstract void DrawContent(IGraphics g, Style style);

		public override void Draw(IGraphics g, bool onScreen, Style style)
		{
			DrawSurface(g, onScreen, style);
			DrawHeaderText(g, style);
			if (onScreen && CanDrawChevron)
				DrawChevron(g);
			if (!Collapsed)
				DrawContent(g, style);
		}

		protected override float GetRequiredWidth(Graphics g, Style style)
		{
			float nameWidth;
			float identifierWidth = 0;

			nameWidth = g.MeasureString(TypeBase.Name, GetNameFont(style),
				PointF.Empty, headerFormat).Width;

			if (HasIdentifier(style))
			{
				string identifier =
					(style.ShowSignature) ? TypeBase.Signature : TypeBase.Stereotype;
				identifierWidth = g.MeasureString(identifier, style.IdentifierFont,
					PointF.Empty, headerFormat).Width;
			}

			float requiredWidth = Math.Max(nameWidth, identifierWidth) + MarginSize * 2;
			return Math.Max(requiredWidth, base.GetRequiredWidth(g, style));
		}

		protected abstract override int GetRequiredHeight();

		protected internal override void MoveWindow()
		{
			EditorWindow editor = GetEditorWindow();
			if (editor != null)
				editor.Relocate(this);
		}

		public sealed override void SelectPrevious()
		{
			ActiveMemberIndex--;
		}

		public sealed override void SelectNext()
		{
			ActiveMemberIndex++;
		}

		protected virtual void OnActiveMemberChanged(EventArgs e)
		{
			if (ActiveMemberChanged != null)
				ActiveMemberChanged(this, e);

			if (showedEditor != null)
			{
				EditorWindow editor = GetEditorWindow();

				if (editor != showedEditor)
				{
					HideWindow(showedEditor);
				}
				ShowEditor(editor);
			}
			NeedsRedraw = true;
		}

		protected override void OnSerializing(SerializeEventArgs e)
		{
			if (collapsed)
			{
				collapsed = false;
				base.OnSerializing(e);
				collapsed = true;
			}
			else
			{
				base.OnSerializing(e);
			}

			XmlElement collapsedNode = e.Node.OwnerDocument.CreateElement("Collapsed");
			collapsedNode.InnerText = Collapsed.ToString();
			e.Node.AppendChild(collapsedNode);
		}

		protected override void OnDeserializing(SerializeEventArgs e)
		{
			base.OnDeserializing(e);

			XmlElement collapsedNode = e.Node["Collapsed"];
			if (collapsedNode != null)
			{
				bool collapsed;
				if (bool.TryParse(collapsedNode.InnerText, out collapsed))
					this.Collapsed = collapsed;
			}
			UpdateMinSize();
		}

		public override string ToString()
		{
			return TypeBase.ToString();
		}
	}
}
