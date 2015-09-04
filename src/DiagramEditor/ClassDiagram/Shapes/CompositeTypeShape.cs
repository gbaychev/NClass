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
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Dialogs;
using NClass.DiagramEditor.ClassDiagram.Editors;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
	public abstract class CompositeTypeShape : TypeShape
	{
		const int AccessSpacing = 12;

		static CompositeTypeEditor typeEditor = new CompositeTypeEditor();
		static MemberEditor memberEditor = new MemberEditor();
		static MembersDialog membersDialog = new MembersDialog();
		static SolidBrush memberBrush = new SolidBrush(Color.Black);
		static StringFormat accessFormat = new StringFormat(StringFormat.GenericTypographic);
		static Pen selectionPen = new Pen(Color.Black);

		static CompositeTypeShape()
		{
			accessFormat.Alignment = StringAlignment.Center;
			accessFormat.LineAlignment = StringAlignment.Center;
			selectionPen.DashPattern = new float[] { 2, 4 };
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="compositeType"/> is null.
		/// </exception>
		protected CompositeTypeShape(CompositeType compositeType)
			: base(compositeType)
		{
		}

		public sealed override TypeBase TypeBase
		{
			get { return CompositeType; }
		}

		public abstract CompositeType CompositeType { get; }

		protected override TypeEditor HeaderEditor
		{
			get { return typeEditor; }
		}

		protected override EditorWindow ContentEditor
		{
			get { return memberEditor; }
		}

		protected internal Member ActiveMember
		{
			get
			{
				if (ActiveMemberIndex >= 0 && ActiveMemberIndex < CompositeType.FieldCount)
				{
					return CompositeType.GetField(ActiveMemberIndex);
				}
				else if (ActiveMemberIndex >= CompositeType.FieldCount)
				{
					return CompositeType.GetOperation(ActiveMemberIndex - CompositeType.FieldCount);
				}
				else
				{
					return null;
				}
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
				Member oldMember = ActiveMember;

				if (value < CompositeType.MemberCount)
					base.ActiveMemberIndex = value;
				else
					base.ActiveMemberIndex = CompositeType.MemberCount - 1;

				if (oldMember != ActiveMember)
					OnActiveMemberChanged(EventArgs.Empty);
			}
		}

		public override void MoveUp()
		{
			if (ActiveMember != null && CompositeType.MoveUpItem(ActiveMember))
			{
				ActiveMemberIndex--;
			}
		}

		public override void MoveDown()
		{
			if (ActiveMember != null && CompositeType.MoveDownItem(ActiveMember))
			{
				ActiveMemberIndex++;
			}
		}

		protected internal override void EditMembers()
		{
			membersDialog.ShowDialog(CompositeType);
		}

		protected override EditorWindow GetEditorWindow()
		{
			if (IsActive)
			{
				if (ActiveMember == null)
					return HeaderEditor;
				else
					return memberEditor;
			}
			else
			{
				return null;
			}
		}

		protected internal sealed override bool DeleteSelectedMember(bool showConfirmation)
		{
			if (IsActive && ActiveMember != null)
			{
				if (!showConfirmation || ConfirmMemberDelete())
					DeleteActiveMember();
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void OnMouseDown(AbsoluteMouseEventArgs e)
		{
			base.OnMouseDown(e);
			SelectMember(e.Location);
		}

		internal Rectangle GetMemberRectangle(int memberIndex)
		{
			Rectangle record = new Rectangle(
				Left + MarginSize, Top + HeaderHeight + MarginSize,
				Width - MarginSize * 2, MemberHeight);

			record.Y += memberIndex * MemberHeight;
			if (CompositeType.SupportsFields && memberIndex >= CompositeType.FieldCount)
			{
				record.Y += MarginSize * 2;
			}
			return record;
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
					if (CompositeType.SupportsFields)
					{
						index = (y - top) / MemberHeight;
						if (index < CompositeType.FieldCount)
						{
							ActiveMemberIndex = index;
							return;
						}
						top += MarginSize * 2;
					}

					int operationTop = top + CompositeType.FieldCount * MemberHeight;
					if (operationTop <= y)
					{
						index = (y - top) / MemberHeight;
						if (index < CompositeType.MemberCount)
						{
							ActiveMemberIndex = index;
							return;
						}
					}
				}
				ActiveMemberIndex = -1;
			}
		}

		internal void DeleteActiveMember()
		{
			if (ActiveMemberIndex >= 0)
			{
				int newIndex;
				int fieldCount = CompositeType.FieldCount;
				int memberCount = CompositeType.MemberCount;

				if (ActiveMemberIndex == fieldCount - 1 && fieldCount >= 2) // Last field
				{
					newIndex = fieldCount - 2;
				}
				else if (ActiveMemberIndex == memberCount - 1) // Last member
				{
					newIndex = ActiveMemberIndex - 1;
				}
				else
				{
					newIndex = ActiveMemberIndex;
				}
				
				CompositeType.RemoveMember(ActiveMember);
				ActiveMemberIndex = newIndex;
				OnActiveMemberChanged(EventArgs.Empty);
			}
		}

		internal void InsertNewMember(MemberType type)
		{
			int fieldCount = CompositeType.FieldCount;
			switch (type)
			{
				case MemberType.Field:
					if (CompositeType.SupportsFields)
					{
						int index = Math.Min(ActiveMemberIndex + 1, fieldCount);
						bool changing = (index == fieldCount &&
							ActiveMember.MemberType != MemberType.Field);

						CompositeType.InsertMember(MemberType.Field, index);
						ActiveMemberIndex = index;
					}
					break;

				case MemberType.Method:
					if (CompositeType.SupportsMethods)
					{
						int index = Math.Max(ActiveMemberIndex + 1, fieldCount);
						CompositeType.InsertMember(MemberType.Method, index);
						ActiveMemberIndex = index;
					}
					break;

				case MemberType.Constructor:
					if (CompositeType.SupportsConstuctors)
					{
						int index = Math.Max(ActiveMemberIndex + 1, fieldCount);
						CompositeType.InsertMember(MemberType.Constructor, index);
						ActiveMemberIndex = index;
					}
					break;

				case MemberType.Destructor:
					if (CompositeType.SupportsDestructors)
					{
						int index = Math.Max(ActiveMemberIndex + 1, fieldCount);
						CompositeType.InsertMember(MemberType.Destructor, index);
						ActiveMemberIndex = index;
					}
					break;

				case MemberType.Property:
					if (CompositeType.SupportsProperties)
					{
						int index = Math.Max(ActiveMemberIndex + 1, fieldCount);
						CompositeType.InsertMember(MemberType.Property, index);
						ActiveMemberIndex = index;
					}
					break;

				case MemberType.Event:
					if (CompositeType.SupportsEvents)
					{
						int index = Math.Max(ActiveMemberIndex + 1, fieldCount);
						CompositeType.InsertMember(MemberType.Event, index);
						ActiveMemberIndex = index;
					}
					break;
			}
		}

		private static string GetAccessString(Member member)
		{
			switch (member.Access)
			{
				case AccessModifier.Public:
					return "+";

				case AccessModifier.Internal:
					return "~";

				case AccessModifier.ProtectedInternal:
				case AccessModifier.Protected:
					return "#";

				case AccessModifier.Private:
				default:
					return "-";
			}
		}

		private static string GetMemberString(Member member)
		{
			return member.GetUmlDescription(
				Settings.Default.ShowType,
				Settings.Default.ShowParameters,
				Settings.Default.ShowParameterNames,
				Settings.Default.ShowInitialValue);
		}

		private Font GetMemberFont(Member member, Style style)
		{
			Font memberFont;
			if (member.IsStatic)
			{
				memberFont = style.StaticMemberFont;
			}
			else if (member is Operation &&
				(((Operation) member).IsAbstract || member.Parent is InterfaceType))
			{
				memberFont = style.AbstractMemberFont;
			}
			else
			{
				memberFont = GetFont(style);
			}

			return memberFont;
		}

		private void DrawMember(IGraphics g, Member member, Rectangle record, Style style)
		{
			Font memberFont = GetMemberFont(member, style);

			if (member is Field)
				memberBrush.Color = style.AttributeColor;
			else
				memberBrush.Color = style.OperationColor;

			if (style.UseIcons)
			{
				Image icon = Icons.GetImage(member);
				g.DrawImage(icon, record.X, record.Y);

				Rectangle textBounds = new Rectangle(
					record.X + IconSpacing, record.Y,
					record.Width - IconSpacing, record.Height);

				string memberString = GetMemberString(member);
				g.DrawString(memberString, memberFont, memberBrush, textBounds, memberFormat);
			}
			else
			{
				Rectangle accessBounds = new Rectangle(
					record.X, record.Y, AccessSpacing, record.Height);
				Rectangle textBounds = new Rectangle(
					record.X + AccessSpacing, record.Y,
					record.Width - AccessSpacing, record.Height);

				g.DrawString(GetAccessString(member), GetFont(style),
					memberBrush, accessBounds, accessFormat);
				g.DrawString(GetMemberString(member), memberFont,
					memberBrush, textBounds, memberFormat);
			}
		}

		protected internal override void DrawSelectionLines(Graphics g, float zoom, Point offset)
		{
			base.DrawSelectionLines(g, zoom, offset);
			
			// Draw selected member rectangle
			if (IsActive && ActiveMember != null)
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

			// Draw fields
			foreach (Field field in CompositeType.Fields)
			{
				DrawMember(g, field, record, style);
				record.Y += MemberHeight;
			}

			//Draw separator line 
			if (CompositeType.SupportsFields)
			{
				DrawSeparatorLine(g, record.Top + MarginSize);
				record.Y += MarginSize * 2;
			}

			// Draw operations
			foreach (Operation operation in CompositeType.Operations)
			{
				DrawMember(g, operation, record, style);
				record.Y += MemberHeight;
			}
		}

		protected override float GetRequiredWidth(Graphics g, Style style)
		{
			float requiredWidth = 0;

			foreach (Field field in CompositeType.Fields)
			{
				float fieldWidth = g.MeasureString(GetMemberString(field),
					GetMemberFont(field, style), PointF.Empty, memberFormat).Width;
				requiredWidth = Math.Max(requiredWidth, fieldWidth);
			}
			foreach (Operation operation in CompositeType.Operations)
			{
				float operationWidth = g.MeasureString(GetMemberString(operation),
					GetMemberFont(operation, style), PointF.Empty, memberFormat).Width;
				requiredWidth = Math.Max(requiredWidth, operationWidth);
			}
			requiredWidth += (style.UseIcons) ? IconSpacing : AccessSpacing;
			requiredWidth += MarginSize * 2;

			return Math.Max(requiredWidth, base.GetRequiredWidth(g, style));
		}

		protected override int GetRequiredHeight()
		{
			int memberCount = 0;
			int spacingHeight = 0;

			if (CompositeType.SupportsFields)
			{
				memberCount += CompositeType.FieldCount;
				spacingHeight += MarginSize * 2;
			}
			if (CompositeType.SupportsOperations)
			{
				memberCount += CompositeType.OperationCount;
				spacingHeight += MarginSize * 2;
			}

			return (HeaderHeight + spacingHeight + (memberCount * MemberHeight));
		}

		private int GetRowIndex(int height)
		{
			height -= HeaderHeight + MarginSize;

			if (CompositeType.SupportsFields && (height > CompositeType.FieldCount * MemberHeight))
				height -= MarginSize * 2;

			return (height / MemberHeight);
		}
	}
}
