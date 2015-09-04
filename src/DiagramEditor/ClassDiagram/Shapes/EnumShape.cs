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
using System.Drawing.Drawing2D;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.Editors;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
	internal sealed class EnumShape : TypeShape
	{
		static EnumEditor typeEditor = new EnumEditor();
		static EnumValueEditor valueEditor = new EnumValueEditor();
		static EnumDialog enumDialog = new EnumDialog();
		static SolidBrush itemBrush = new SolidBrush(Color.Black);

		EnumType _enum;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="enumType"/> is null.
		/// </exception>
		internal EnumShape(EnumType _enum) : base(_enum)
		{
			this._enum = _enum;
			UpdateMinSize();
		}

		public override TypeBase TypeBase
		{
			get { return _enum; }
		}

		public EnumType EnumType
		{
			get { return _enum; }
		}

		internal EnumValue ActiveValue
		{
			get
			{
				if (ActiveMemberIndex >= 0)
					return EnumType.GetValue(ActiveMemberIndex);
				else
					return null;
			}
		}

		protected internal override int ActiveMemberIndex
		{
			get
			{
				return base.ActiveMemberIndex;
			}
			set
			{
				EnumValue oldValue = ActiveValue;

				if (value < EnumType.ValueCount)
					base.ActiveMemberIndex = value;
				else
					base.ActiveMemberIndex = EnumType.ValueCount - 1;

				if (oldValue != ActiveValue)
					OnActiveMemberChanged(EventArgs.Empty);
			}
		}

		protected override TypeEditor HeaderEditor
		{
			get { return typeEditor; }
		}

		protected override EditorWindow ContentEditor
		{
			get { return valueEditor; }
		}

		protected override EditorWindow GetEditorWindow()
		{
			if (ActiveValue == null)
				return typeEditor;
			else
				return valueEditor;
		}

		protected internal override bool DeleteSelectedMember(bool showConfirmation)
		{
			if (IsActive && ActiveValue != null)
			{
				if (!showConfirmation || ConfirmMemberDelete())
					DeleteActiveValue();
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override bool CloneEntity(Diagram diagram)
		{
			return diagram.InsertEnum(EnumType.Clone());
		}

		protected override Color GetBackgroundColor(Style style)
		{
			return style.EnumBackgroundColor;
		}

		protected override Color GetBorderColor(Style style)
		{
			return style.EnumBorderColor;
		}

		protected override int GetBorderWidth(Style style)
		{
			return style.EnumBorderWidth;
		}

		protected override bool IsBorderDashed(Style style)
		{
			return style.IsEnumBorderDashed;
		}

		protected override Color GetHeaderColor(Style style)
		{
			return style.EnumHeaderColor;
		}

		protected override int GetRoundingSize(Style style)
		{
			return style.EnumRoundingSize;
		}

		protected override GradientStyle GetGradientHeaderStyle(Style style)
		{
			return style.EnumGradientHeaderStyle;
		}

		public override void MoveUp()
		{
			if (ActiveValue != null && EnumType.MoveUpItem(ActiveValue))
			{
				ActiveMemberIndex--;
			}
		}

		public override void MoveDown()
		{
			if (ActiveValue != null && EnumType.MoveDownItem(ActiveValue))
			{
				ActiveMemberIndex++;
			}
		}

		protected internal override void EditMembers()
		{
			enumDialog.ShowDialog(EnumType);
		}

		protected override void OnMouseDown(AbsoluteMouseEventArgs e)
		{
			base.OnMouseDown(e);
			SelectMember(e.Location);
		}

		private void SelectMember(PointF location)
		{
			if (Contains(location))
			{
				int index;
				int y = (int) location.Y;
				int top = Top + HeaderHeight + MarginSize;

				if (top <= y)
				{
					index = (y - top) / MemberHeight;
					if (index < EnumType.ValueCount)
					{
						ActiveMemberIndex = index;
						return;
					}
				}
				ActiveMemberIndex = -1;
			}
		}

		internal void DeleteActiveValue()
		{
			if (ActiveMemberIndex >= 0)
			{
				int newIndex;
				if (ActiveMemberIndex == EnumType.ValueCount - 1) // Last value
				{
					newIndex = ActiveMemberIndex - 1;
				}
				else
				{
					newIndex = ActiveMemberIndex;
				}
				
				EnumType.RemoveValue(ActiveValue);
				ActiveMemberIndex = newIndex;
				OnActiveMemberChanged(EventArgs.Empty);
			}
		}

		internal Rectangle GetMemberRectangle(int memberIndex)
		{
			return new Rectangle(
				Left + MarginSize,
				Top + HeaderHeight + MarginSize + memberIndex * MemberHeight,
				Width - MarginSize * 2,
				MemberHeight);
		}

		private void DrawItem(IGraphics g, EnumValue value, Rectangle record, Style style)
		{
			Font font = GetFont(style);
			string memberString = value.ToString();
			itemBrush.Color = style.EnumItemColor;

			if (style.UseIcons)
			{
				Image icon = Properties.Resources.EnumItem;
				g.DrawImage(icon, record.X, record.Y);

				Rectangle textBounds = new Rectangle(
					record.X + IconSpacing, record.Y,
					record.Width - IconSpacing, record.Height);

				g.DrawString(memberString, font, itemBrush, textBounds, memberFormat);
			}
			else
			{
				g.DrawString(memberString, font, itemBrush, record, memberFormat);
			}
		}

		protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
		{
			base.DrawSelectionLines(g, zoom, offset);

			// Draw selected parameter rectangle
			if (IsActive && ActiveValue != null)
			{
				Rectangle record = GetMemberRectangle(ActiveMemberIndex);
				record = TransformRelativeToAbsolute(record, zoom, offset);
				record.Inflate(2, 0);
				g.DrawRectangle(Diagram.SelectionPen, record);
			}
		}

		protected override void DrawContent(IGraphics g, Style style)
		{
			Rectangle record = new Rectangle(
				Left + MarginSize, Top + HeaderHeight + MarginSize,
				Width - MarginSize * 2, MemberHeight);

			foreach (EnumValue value in EnumType.Values)
			{
				DrawItem(g, value, record, style);
				record.Y += MemberHeight;
			}
		}

		protected override float GetRequiredWidth(Graphics g, Style style)
		{
			float requiredWidth = 0;

			Font font = GetFont(style);
			foreach (EnumValue value in EnumType.Values)
			{
				float itemWidth = g.MeasureString(value.ToString(),
					font, PointF.Empty, memberFormat).Width;
				requiredWidth = Math.Max(requiredWidth, itemWidth);
			}

			if (style.UseIcons)
				requiredWidth += IconSpacing;
			requiredWidth += MarginSize * 2;

			return Math.Max(requiredWidth, base.GetRequiredWidth(g, style));
		}

		protected override int GetRequiredHeight()
		{
			return (HeaderHeight + (MarginSize * 2) + (EnumType.ValueCount * MemberHeight));
		}
	}
}
