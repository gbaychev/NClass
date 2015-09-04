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
	public sealed class DelegateShape : TypeShape
	{
		static DelegateEditor typeEditor = new DelegateEditor();
		static ParameterEditor parameterEditor = new ParameterEditor();
		static DelegateDialog delegateDialog = new DelegateDialog();
		static SolidBrush parameterBrush = new SolidBrush(Color.Black);

		DelegateType _delegate;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="_delegate"/> is null.
		/// </exception>
		internal DelegateShape(DelegateType _delegate) : base(_delegate)
		{
			this._delegate = _delegate;
			UpdateMinSize();
		}

		public override TypeBase TypeBase
		{
			get { return _delegate; }
		}

		public DelegateType DelegateType
		{
			get { return _delegate; }
		}

		internal Parameter ActiveParameter
		{
			get
			{
				if (ActiveMemberIndex >= 0)
					return DelegateType.GetArgument(ActiveMemberIndex);
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
				Parameter oldParameter = ActiveParameter;

				if (value < DelegateType.ArgumentCount)
					base.ActiveMemberIndex = value;
				else
					base.ActiveMemberIndex = DelegateType.ArgumentCount - 1;

				if (oldParameter != ActiveParameter)
					OnActiveMemberChanged(EventArgs.Empty);
			}
		}

		protected override TypeEditor HeaderEditor
		{
			get { return typeEditor; }
		}

		protected override EditorWindow ContentEditor
		{
			get { return parameterEditor; }
		}

		protected override EditorWindow GetEditorWindow()
		{
			if (ActiveParameter == null)
				return typeEditor;
			else
				return parameterEditor;
		}

		protected internal override bool DeleteSelectedMember(bool showConfirmation)
		{
			if (IsActive && ActiveParameter != null)
			{
				if (!showConfirmation || ConfirmMemberDelete())
					DeleteActiveParameter();
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override bool CloneEntity(Diagram diagram)
		{
			return diagram.InsertDelegate(DelegateType.Clone());
		}

		protected override Color GetBackgroundColor(Style style)
		{
			return style.DelegateBackgroundColor;
		}

		protected override Color GetBorderColor(Style style)
		{
			return style.DelegateBorderColor;
		}

		protected override int GetBorderWidth(Style style)
		{
			return style.DelegateBorderWidth;
		}

		protected override bool IsBorderDashed(Style style)
		{
			return style.IsDelegateBorderDashed;
		}

		protected override Color GetHeaderColor(Style style)
		{
			return style.DelegateHeaderColor;
		}

		protected override int GetRoundingSize(Style style)
		{
			return style.DelegateRoundingSize;
		}

		protected override GradientStyle GetGradientHeaderStyle(Style style)
		{
			return style.DelegateGradientHeaderStyle;
		}

		public override void MoveUp()
		{
			if (ActiveParameter != null && DelegateType.MoveUpItem(ActiveParameter))
			{
				ActiveMemberIndex--;
			}
		}

		public override void MoveDown()
		{
			if (ActiveParameter != null && DelegateType.MoveDownItem(ActiveParameter))
			{
				ActiveMemberIndex++;
			}
		}

		protected internal override void EditMembers()
		{
			delegateDialog.ShowDialog(DelegateType);
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
					if (index < DelegateType.ArgumentCount)
					{
						ActiveMemberIndex = index;
						return;
					}
				}
				ActiveMemberIndex = -1;
			}
		}
		
		internal void DeleteActiveParameter()
		{
			if (ActiveMemberIndex >= 0)
			{
				int newIndex;
				if (ActiveMemberIndex == DelegateType.ArgumentCount - 1) // Last parameter
				{
					newIndex = ActiveMemberIndex - 1;
				}
				else
				{
					newIndex = ActiveMemberIndex;
				}

				DelegateType.RemoveParameter(ActiveParameter);
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

		private void DrawItem(IGraphics g, Parameter parameter, Rectangle record, Style style)
		{
			Font font = GetFont(style);
			string memberString = parameter.ToString();
			parameterBrush.Color = style.EnumItemColor;

			if (style.UseIcons)
			{
				Image icon = Properties.Resources.Parameter;
				g.DrawImage(icon, record.X, record.Y);

				Rectangle textBounds = new Rectangle(
					record.X + IconSpacing, record.Y,
					record.Width - IconSpacing, record.Height);

				g.DrawString(memberString, font, parameterBrush, textBounds, memberFormat);
			}
			else
			{
				g.DrawString(memberString, font, parameterBrush, record, memberFormat);
			}
		}

		protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
		{
			base.DrawSelectionLines(g, zoom, offset);

			// Draw selected parameter rectangle
			if (IsActive && ActiveParameter != null)
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

			foreach (Parameter parameter in DelegateType.Arguments)
			{
				DrawItem(g, parameter, record, style);
				record.Y += MemberHeight;
			}
		}

		protected override float GetRequiredWidth(Graphics g, Style style)
		{
			float requiredWidth = 0;

			Font font = GetFont(style);
			foreach (Parameter parameter in DelegateType.Arguments)
			{
				float itemWidth = g.MeasureString(parameter.ToString(),
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
			return (HeaderHeight + (MarginSize * 2) + (DelegateType.ArgumentCount * MemberHeight));
		}
	}
}
