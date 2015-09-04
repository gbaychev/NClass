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
using System.Windows.Forms;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Core;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public partial class ParameterEditor : ItemEditor
	{
		DelegateShape shape = null;

		internal override void Init(DiagramElement element)
		{
			shape = (DelegateShape) element;
			base.Init(element);
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((DelegateShape) element);
		}

		internal void Relocate(DelegateShape shape)
		{
			Diagram diagram = shape.Diagram;
			if (diagram != null)
			{
				Rectangle record = shape.GetMemberRectangle(shape.ActiveMemberIndex);

				Point absolute = new Point(shape.Right, record.Top);
				Size relative = new Size(
					(int) (absolute.X * diagram.Zoom) - diagram.Offset.X + MarginSize,
					(int) (absolute.Y * diagram.Zoom) - diagram.Offset.Y);
				relative.Height -= (Height - (int) (record.Height * diagram.Zoom)) / 2;

				this.Location = ParentLocation + relative;
			}
		}

		protected override void RefreshValues()
		{
			if (shape.ActiveParameter != null)
			{
				int cursorPosition = SelectionStart;
				DeclarationText = shape.ActiveParameter.ToString();
				SelectionStart = cursorPosition;

				SetError(null);
				NeedValidation = false;
				RefreshMoveUpDownTools();
			}
		}

		private void RefreshMoveUpDownTools()
		{
			int index = shape.ActiveMemberIndex;
			int parameterCount = shape.DelegateType.ArgumentCount;

			toolMoveUp.Enabled = (index > 0);
			toolMoveDown.Enabled = (index < parameterCount - 1);
		}

		protected override bool ValidateDeclarationLine()
		{
			if (NeedValidation && shape.ActiveParameter != null)
			{
				try
				{
					shape.DelegateType.ModifyParameter(shape.ActiveParameter, DeclarationText);
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					SetError(ex.Message);
					return false;
				}
			}
			return true;
		}

		protected override void HideEditor()
		{
			NeedValidation = false;
			shape.HideEditor();
		}

		protected override void SelectPrevious()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectPrevious();
			}
		}

		protected override void SelectNext()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectNext();
			}
		}

		protected override void MoveUp()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveUp();
			}
		}

		protected override void MoveDown()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveDown();
			}
		}

		protected override void Delete()
		{
			shape.DeleteActiveParameter();
		}
	}
}
