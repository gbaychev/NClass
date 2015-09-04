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
using System.ComponentModel;
using System.Windows.Forms;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public partial class MemberEditor : FloatingEditor
	{
		CompositeTypeShape shape = null;
		bool needValidation = false;

		public MemberEditor()
		{
			InitializeComponent();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
			UpdateTexts();
			if (MonoHelper.IsRunningOnMono)
				toolNewMember.Alignment = ToolStripItemAlignment.Left;
		}

		private void UpdateTexts()
		{
			toolFieldNone.Text = Strings.None;
			toolOperationNone.Text = Strings.None;
			toolReadWrite.Text = Strings.ReadWrite;
			toolReadOnly.Text = Strings.ReadOnly;
			toolWriteOnly.Text = Strings.WriteOnly;
			toolNewField.Text = Strings.NewField;
			toolNewMethod.Text = Strings.NewMethod;
			toolNewConstructor.Text = Strings.NewConstructor;
			toolNewDestructor.Text = Strings.NewDestructor;
			toolNewProperty.Text = Strings.NewProperty;
			toolNewEvent.Text = Strings.NewEvent;
			toolMoveUp.ToolTipText = Strings.MoveUp + " (Ctrl+Up)";
			toolMoveDown.ToolTipText = Strings.MoveDown + " (Ctrl+Down)";
			toolDelete.ToolTipText = Strings.Delete;
		}

		internal override void Init(DiagramElement element)
		{
			shape = (CompositeTypeShape) element;
			RefreshValues();
		}

		private void RefreshValues()
		{
			if (shape.ActiveMember != null)
			{
				Member member = shape.ActiveMember;
				SuspendLayout();
				RefreshModifiers();
				Language language = shape.CompositeType.Language;

				int cursorPosition = txtDeclaration.SelectionStart;
				txtDeclaration.Text = member.ToString();
				txtDeclaration.SelectionStart = cursorPosition;
				txtDeclaration.ReadOnly = (member.MemberType == MemberType.Destructor);
				
				SetError(null);
				needValidation = false;

				// Visibility
				RefreshVisibility();

				// Static and New modifiers
				toolStatic.Checked = member.IsStatic;
				toolStatic.Enabled = (member.MemberType != MemberType.Destructor);
				toolHider.Checked = member.IsHider;
				toolHider.Enabled = (member.MemberType != MemberType.Destructor);
				
				// Field modifiers
				Field field = member as Field;
				if (field != null)
				{
					FieldModifier modifier = field.Modifier &
						(FieldModifier.Constant | FieldModifier.Readonly | FieldModifier.Volatile);

					if (modifier == FieldModifier.None)
						toolFieldModifiers.Text = Strings.None;
					else
						toolFieldModifiers.Text = language.ValidFieldModifiers[modifier];
				}

				// Operation modifiers
				Operation operation = member as Operation;
				if (operation != null)
				{
					OperationModifier modifier = operation.Modifier &
						(OperationModifier.Abstract | OperationModifier.Override |
						OperationModifier.Sealed | OperationModifier.Virtual);

					if (modifier == OperationModifier.None)
						toolOperationModifiers.Text = Strings.None;
					else
						toolOperationModifiers.Text = language.ValidOperationModifiers[modifier];

					toolOperationModifiers.Enabled = (member.MemberType != MemberType.Destructor);
				}

				// Property accessor
				Property property = member as Property;
				if (property != null)
				{
					if (property.IsReadonly)
					{
						toolAccessor.Image = Properties.Resources.PublicReadonly;
						toolAccessor.ToolTipText = Strings.ReadOnly;
					}
					else if (property.IsWriteonly)
					{
						toolAccessor.Image = Properties.Resources.PublicWriteonly;
						toolAccessor.ToolTipText = Strings.WriteOnly;
					}
					else
					{
						toolAccessor.Image = Properties.Resources.PublicProperty;
						toolAccessor.ToolTipText = Strings.ReadWrite;
					}
				}
				
				RefreshNewMembers();
				RefreshMoveUpDownTools();
				ResumeLayout();
			}
		}

		private void RefreshVisibility()
		{
			Member member = shape.ActiveMember;
			Language language = shape.ActiveMember.Language;

			toolVisibility.Image = Icons.GetImage(member.MemberType, member.AccessModifier);
			toolVisibility.Text = language.ValidAccessModifiers[member.AccessModifier];
			toolVisibility.Enabled = (member.MemberType != MemberType.Destructor);

			// Public
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Public))
			{
				toolPublic.Visible = true;
				toolPublic.Text = language.ValidAccessModifiers[AccessModifier.Public];
				toolPublic.Image = Icons.GetImage(member.MemberType, AccessModifier.Public);
			}
			else
			{
				toolPublic.Visible = false;
			}
			// Protected Internal
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.ProtectedInternal))
			{
				toolProtint.Visible = true;
				toolProtint.Text = language.ValidAccessModifiers[AccessModifier.ProtectedInternal];
				toolProtint.Image = Icons.GetImage(member.MemberType, AccessModifier.ProtectedInternal);
			}
			else
			{
				toolProtint.Visible = false;
			}
			// Internal
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Internal))
			{
				toolInternal.Visible = true;
				toolInternal.Text = language.ValidAccessModifiers[AccessModifier.Internal];
				toolInternal.Image = Icons.GetImage(member.MemberType, AccessModifier.Internal);
			}
			else
			{
				toolInternal.Visible = false;
			}
			// Protected
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Protected))
			{
				toolProtected.Visible = true;
				toolProtected.Text = language.ValidAccessModifiers[AccessModifier.Protected];
				toolProtected.Image = Icons.GetImage(member.MemberType, AccessModifier.Protected);
			}
			else
			{
				toolProtected.Visible = false;
			}
			// Private
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Private))
			{
				toolPrivate.Visible = true;
				toolPrivate.Text = language.ValidAccessModifiers[AccessModifier.Private];
				toolPrivate.Image = Icons.GetImage(member.MemberType, AccessModifier.Private);
			}
			else
			{
				toolPrivate.Visible = false;
			}
			// Default
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Default))
			{
				toolDefault.Visible = true;
				toolDefault.Text = language.ValidAccessModifiers[AccessModifier.Default];
				toolDefault.Image = Icons.GetImage(member.MemberType, AccessModifier.Default);
			}
			else
			{
				toolDefault.Visible = false;
			}
		}

		private void RefreshModifiers()
		{
			Language language = shape.CompositeType.Language;

			if (shape.ActiveMember is Field)
			{
				toolFieldModifiers.Visible = true;
				toolOperationModifiers.Visible = false;
				toolAccessor.Visible = false;

				// Static modifier
				if (language.ValidFieldModifiers.ContainsKey(FieldModifier.Static))
				{
					toolStatic.Visible = true;
					toolStatic.Text = language.ValidFieldModifiers[FieldModifier.Static];
				}
				else
				{
					toolStatic.Visible = false;
				}
				// New modifier
				if (language.ValidFieldModifiers.ContainsKey(FieldModifier.Hider))
				{
					toolHider.Visible = true;
					toolNewField.Text = language.ValidFieldModifiers[FieldModifier.Hider];
				}
				else
				{
					toolHider.Visible = false;
				}
				// Readonly modifier
				if (language.ValidFieldModifiers.ContainsKey(FieldModifier.Readonly))
				{
					toolReadonlyField.Visible = true;
					toolReadonlyField.Text = language.ValidFieldModifiers[FieldModifier.Readonly];
				}
				else
				{
					toolReadonlyField.Visible = false;
				}
				// Const modifier
				if (language.ValidFieldModifiers.ContainsKey(FieldModifier.Constant))
				{
					toolConst.Visible = true;
					toolConst.Text = language.ValidFieldModifiers[FieldModifier.Constant];
				}
				else
				{
					toolConst.Visible = false;
				}
				// Volatile modifier
				if (language.ValidFieldModifiers.ContainsKey(FieldModifier.Volatile))
				{
					toolVolatile.Visible = true;
					toolVolatile.Text = language.ValidFieldModifiers[FieldModifier.Volatile];
				}
				else
				{
					toolVolatile.Visible = false;
				}
			}
			else
			{
				toolAccessor.Visible = (shape.ActiveMember is Property);
				toolFieldModifiers.Visible = false;
				toolOperationModifiers.Visible = true;

				// Static modifier
				if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Static))
				{
					toolStatic.Visible = true;
					toolStatic.Text = language.ValidOperationModifiers[OperationModifier.Static];
				}
				else
				{
					toolStatic.Visible = false;
				}
				// New modifier
				if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Hider))
				{
					toolHider.Visible = true;
					toolHider.Text = language.ValidOperationModifiers[OperationModifier.Hider];
				}
				else
				{
					toolHider.Visible = false;
				}
				// Abstract modifier
				if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Abstract))
				{
					toolAbstract.Visible = true;
					toolAbstract.Text = language.ValidOperationModifiers[OperationModifier.Abstract];
				}
				else
				{
					toolAbstract.Visible = false;
				}
				// Virtual modifier
				if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Virtual))
				{
					toolVirtual.Visible = true;
					toolVirtual.Text = language.ValidOperationModifiers[OperationModifier.Virtual];
				}
				else
				{
					toolVirtual.Visible = false;
				}
				// Override modifier
				if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Override))
				{
					toolOverride.Visible = true;
					toolOverride.Text = language.ValidOperationModifiers[OperationModifier.Override];
				}
				else
				{
					toolOverride.Visible = false;
				}
				// Sealed modifier
				if (language.ValidOperationModifiers.ContainsKey(
					OperationModifier.Sealed | OperationModifier.Override))
				{
					toolSealed.Visible = true;
					toolSealed.Text = language.ValidOperationModifiers[
						OperationModifier.Sealed | OperationModifier.Override];
				}
				else if (language.ValidOperationModifiers.ContainsKey(OperationModifier.Sealed))
				{
					toolSealed.Visible = true;
					toolSealed.Text = language.ValidOperationModifiers[OperationModifier.Sealed];
				}
				else
				{
					toolSealed.Visible = false;
				}
				// Abstract-Override modifier
				if (language.ValidOperationModifiers.ContainsKey(
					OperationModifier.Abstract | OperationModifier.Override))
				{
					toolAbstractOverride.Visible = true;
					toolAbstractOverride.Text = language.ValidOperationModifiers[
						OperationModifier.Abstract | OperationModifier.Override];
				}
				else
				{
					toolAbstractOverride.Visible = false;
				}
			}
		}

		private void RefreshNewMembers()
		{
			bool valid = false;
			switch (NewMemberType)
			{
				case MemberType.Field:
					if (shape.CompositeType.SupportsFields)
					{
						toolNewMember.Image = Properties.Resources.NewField;
						toolNewMember.Text = Strings.NewField;
						valid = true;
					}
					break;

				case MemberType.Method:
					if (shape.CompositeType.SupportsMethods)
					{
						toolNewMember.Image = Properties.Resources.NewMethod;
						toolNewMember.Text = Strings.NewMethod;
						valid = true;
					}
					break;

				case MemberType.Constructor:
					if (shape.CompositeType.SupportsConstuctors)
					{
						toolNewMember.Image = Properties.Resources.NewConstructor;
						toolNewMember.Text = Strings.NewConstructor;
						valid = true;
					}
					break;

				case MemberType.Destructor:
					if (shape.CompositeType.SupportsDestructors)
					{
						toolNewMember.Image = Properties.Resources.NewDestructor;
						toolNewMember.Text = Strings.NewDestructor;
						valid = true;
					}
					break;

				case MemberType.Property:
					if (shape.CompositeType.SupportsProperties)
					{
						toolNewMember.Image = Properties.Resources.NewProperty;
						toolNewMember.Text = Strings.NewProperty;
						valid = true;
					}
					break;

				case MemberType.Event:
					if (shape.CompositeType.SupportsEvents)
					{
						toolNewMember.Image = Properties.Resources.NewEvent;
						toolNewMember.Text = Strings.NewEvent;
						valid = true;
					}
					break;
			}

			if (!valid)
			{
				NewMemberType = MemberType.Method;
				toolNewMember.Image = Properties.Resources.NewMethod;
				toolNewMember.Text = Strings.NewMethod;
			}

			toolNewField.Visible = shape.CompositeType.SupportsFields;
			toolNewMethod.Visible = shape.CompositeType.SupportsMethods;
			toolNewConstructor.Visible = shape.CompositeType.SupportsConstuctors;
			toolNewDestructor.Visible = shape.CompositeType.SupportsDestructors;
			toolNewProperty.Visible = shape.CompositeType.SupportsProperties;
			toolNewEvent.Visible = shape.CompositeType.SupportsEvents;
		}

		private void RefreshMoveUpDownTools()
		{
			int index = shape.ActiveMemberIndex;
			int fieldCount = shape.CompositeType.FieldCount;
			int memberCount = shape.CompositeType.MemberCount;

			toolMoveUp.Enabled = ((index < fieldCount && index > 0) || index > fieldCount);
			toolMoveDown.Enabled = (index < fieldCount - 1 ||
				(index >= fieldCount && index < memberCount - 1));
		}

		internal override void Relocate(DiagramElement element)
		{
			Relocate((CompositeTypeShape) element);
		}

		internal void Relocate(CompositeTypeShape shape)
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

		public override void ValidateData()
		{
			ValidateDeclarationLine();
			SetError(null);
		}

		private bool ValidateDeclarationLine()
		{
			if (needValidation && shape.ActiveMember != null)
			{
				try
				{
					shape.ActiveMember.InitFromString(txtDeclaration.Text);
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

		private void SetError(string message)
		{
			if (MonoHelper.IsRunningOnMono && MonoHelper.IsOlderVersionThan("2.4"))
				return;

			errorProvider.SetError(this, message);
		}

		private void ChangeAccess(AccessModifier access)
		{
			if (shape.ActiveMember != null && ValidateDeclarationLine())
			{
				try
				{
					shape.ActiveMember.AccessModifier = access;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					RefreshValues();
					SetError(ex.Message);
				}
			}
		}

		private void ChangeModifier(FieldModifier modifier)
		{
			if (ValidateDeclarationLine())
			{
				Field field = shape.ActiveMember as Field;
				if (field != null)
				{
					try
					{
						// Clear other modifiers
						field.ClearModifiers();

						// Set new values
						field.IsReadonly = ((modifier & FieldModifier.Readonly) != 0);
						field.IsConstant = ((modifier & FieldModifier.Constant) != 0);
						field.IsVolatile = ((modifier & FieldModifier.Volatile) != 0);

						RefreshValues();
					}
					catch (BadSyntaxException ex)
					{
						RefreshValues();
						SetError(ex.Message);
					}
				}
			}
		}

		private void ChangeModifier(OperationModifier modifier)
		{
			if (shape.ActiveMember != null && ValidateDeclarationLine())
			{
				Operation operation = shape.ActiveMember as Operation;
				if (operation != null)
				{
					// Class changing to abstract
					if ((modifier & OperationModifier.Abstract) != 0)
					{
						ClassType classType = shape.CompositeType as ClassType;
						if (classType != null && classType.Modifier != ClassModifier.Abstract)
						{
							DialogResult result = MessageBox.Show(
								Strings.ChangingToAbstractConfirmation, Strings.Confirmation,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

							if (result == DialogResult.No)
								return;
						}
					}
					
					try
					{
						// Clear other modifiers
						operation.ClearModifiers();

						// Set new values
						operation.IsAbstract = ((modifier & OperationModifier.Abstract) != 0);
						operation.IsVirtual = ((modifier & OperationModifier.Virtual) != 0);
						operation.IsOverride = ((modifier & OperationModifier.Override) != 0);
						operation.IsSealed = ((modifier & OperationModifier.Sealed) != 0);

						RefreshValues();
					}
					catch (BadSyntaxException ex)
					{
						RefreshValues();
						SetError(ex.Message);
					}
				}
			}
		}

		private void SelectPrevious()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectPrevious();
			}
		}

		private void SelectNext()
		{
			if (ValidateDeclarationLine())
			{
				shape.SelectNext();
			}
		}

		private void MoveUp()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveUp();
			}
		}

		private void MoveDown()
		{
			if (ValidateDeclarationLine())
			{
				shape.MoveDown();
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			txtDeclaration.SelectionStart = 0;
		}

		private void txtDeclaration_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					if (e.Modifiers == Keys.Control || e.Modifiers == Keys.Shift)
						OpenNewMemberDropDown();
					else
						ValidateDeclarationLine();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					shape.HideEditor();
					e.Handled = true;
					break;

				case Keys.Up:
					if (e.Shift || e.Control)
						MoveUp();
					else
						SelectPrevious();
					e.Handled = true;
					break;

				case Keys.Down:
					if (e.Shift || e.Control)
						MoveDown();
					else
						SelectNext();
					e.Handled = true;
					break;
			}

			if (e.Modifiers == (Keys.Control | Keys.Shift))
			{
				switch (e.KeyCode)
				{
					case Keys.A:
						AddNewMember();
						break;

					case Keys.F:
						AddNewMember(MemberType.Field);
						break;

					case Keys.M:
						AddNewMember(MemberType.Method);
						break;

					case Keys.C:
						AddNewMember(MemberType.Constructor);
						break;

					case Keys.D:
						AddNewMember(MemberType.Destructor);
						break;

					case Keys.P:
						AddNewMember(MemberType.Property);
						break;

					case Keys.E:
						AddNewMember(MemberType.Event);
						break;
				}
			}
		}

		private void OpenNewMemberDropDown()
		{
			toolNewMember.ShowDropDown();
			
			switch (NewMemberType)
			{
				case MemberType.Field:
					toolNewField.Select();
					break;

				case MemberType.Method:
					toolNewMethod.Select();
					break;

				case MemberType.Constructor:
					toolNewConstructor.Select();
					break;

				case MemberType.Destructor:
					toolNewDestructor.Select();
					break;

				case MemberType.Property:
					toolNewProperty.Select();
					break;

				case MemberType.Event:
					toolNewEvent.Select();
					break;
			}
		}

		private void txtDeclaration_TextChanged(object sender, EventArgs e)
		{
			needValidation = true;
		}

		private void txtDeclaration_Validating(object sender, CancelEventArgs e)
		{
			ValidateDeclarationLine();
		}

		private void toolPublic_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.Public);
		}

		private void toolProtint_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.ProtectedInternal);
		}

		private void toolInternal_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.Internal);
		}

		private void toolProtected_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.Protected);
		}

		private void toolPrivate_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.Private);
		}

		private void toolDefault_Click(object sender, EventArgs e)
		{
			ChangeAccess(AccessModifier.Default);
		}

		private void toolStatic_CheckedChanged(object sender, EventArgs e)
		{
			bool checkedState = toolStatic.Checked;

			if (shape.ActiveMember != null && ValidateDeclarationLine())
			{
				try
				{
					shape.ActiveMember.IsStatic = checkedState;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					RefreshValues();
					SetError(ex.Message);
				}
			}
		}

		private void toolHider_CheckedChanged(object sender, EventArgs e)
		{
			bool checkedState = toolHider.Checked;

			if (shape.ActiveMember != null && ValidateDeclarationLine())
			{
				try
				{
					shape.ActiveMember.IsHider = checkedState;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					RefreshValues();
					SetError(ex.Message);
				}
			}
		}

		private void toolFieldNone_Click(object sender, EventArgs e)
		{
			ChangeModifier(FieldModifier.None);
		}

		private void toolReadonlyField_Click(object sender, EventArgs e)
		{
			ChangeModifier(FieldModifier.Readonly);
		}

		private void toolConst_Click(object sender, EventArgs e)
		{
			ChangeModifier(FieldModifier.Constant);
		}

		private void toolVolatile_Click(object sender, EventArgs e)
		{
			ChangeModifier(FieldModifier.Volatile);
		}

		private void toolOperationNone_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.None);
		}

		private void toolAbstract_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.Abstract);
		}

		private void toolVirtual_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.Virtual);
		}

		private void toolOverride_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.Override);
		}

		private void toolSealed_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.Sealed);
		}

		private void toolAbstractOverride_Click(object sender, EventArgs e)
		{
			ChangeModifier(OperationModifier.Abstract | OperationModifier.Override);
		}

		private void toolReadWrite_Click(object sender, EventArgs e)
		{
			if (ValidateDeclarationLine())
			{
				Property property = shape.ActiveMember as Property;
				if (property != null)
				{
					try
					{
						property.IsReadonly = false;
						property.IsWriteonly = false;
						RefreshValues();
					}
					catch (BadSyntaxException ex)
					{
						RefreshValues();
						SetError(ex.Message);
					}
				}
			}
		}

		private void toolReadOnly_Click(object sender, EventArgs e)
		{
			if (ValidateDeclarationLine())
			{
				Property property = shape.ActiveMember as Property;
				if (property != null)
				{
					try
					{
						property.IsReadonly = true;
						RefreshValues();
					}
					catch (BadSyntaxException ex)
					{
						RefreshValues();
						SetError(ex.Message);
					}
				}
			}
		}

		private void toolWriteOnly_Click(object sender, EventArgs e)
		{
			if (ValidateDeclarationLine())
			{
				Property property = shape.ActiveMember as Property;
				if (property != null)
				{
					try
					{
						property.IsWriteonly = true;
						RefreshValues();
					}
					catch (BadSyntaxException ex)
					{
						RefreshValues();
						SetError(ex.Message);
					}
				}
			}
		}

		private void AddNewMember()
		{
			AddNewMember(NewMemberType);
		}

		private void AddNewMember(MemberType type)
		{
			if (!ValidateDeclarationLine())
				return;

			NewMemberType = type;
			shape.InsertNewMember(type);
			txtDeclaration.SelectionStart = 0;
		}

		private void toolNewMember_ButtonClick(object sender, EventArgs e)
		{
			AddNewMember();
		}

		private void toolNewField_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Field);
		}

		private void toolNewMethod_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Method);
		}

		private void toolNewConstructor_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Constructor);
		}

		private void toolNewDestructor_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Destructor);
		}

		private void toolNewProperty_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Property);
		}

		private void toolNewEvent_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Event);
		}

		private void toolMoveUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void toolMoveDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}

		private void toolDelete_Click(object sender, EventArgs e)
		{
			shape.DeleteActiveMember();
		}
	}
}
