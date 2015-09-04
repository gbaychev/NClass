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
using NClass.DiagramEditor.ClassDiagram.Dialogs;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
	public partial class CompositeTypeEditor : TypeEditor
	{
		CompositeTypeShape shape = null;
		bool needValidation = false;

		public CompositeTypeEditor()
		{
			InitializeComponent();
			toolStrip.Renderer = ToolStripSimplifiedRenderer.Default;
			UpdateTexts();

			if (MonoHelper.IsRunningOnMono)
				toolNewMember.Alignment = ToolStripItemAlignment.Left;
		}

		internal override void Init(DiagramElement element)
		{
			shape = (CompositeTypeShape) element;
			RefreshToolAvailability();
			RefreshValues();
		}

		private void RefreshToolAvailability()
		{
			toolOverrideList.Visible = shape.CompositeType is SingleInharitanceType;
			
			IInterfaceImplementer implementer = shape.CompositeType as IInterfaceImplementer;
			if (implementer != null)
			{
				toolImplementList.Visible = true;
				toolImplementList.Enabled = implementer.ImplementsInterface;
			}
			else
			{
				toolImplementList.Visible = false;
			}
		}

		private void UpdateTexts()
		{
			toolNewField.Text = Strings.NewField;
			toolNewMethod.Text = Strings.NewMethod;
			toolNewConstructor.Text = Strings.NewConstructor;
			toolNewDestructor.Text = Strings.NewDestructor;
			toolNewProperty.Text = Strings.NewProperty;
			toolNewEvent.Text = Strings.NewEvent;
			toolOverrideList.Text = Strings.OverrideMembers;
			toolImplementList.Text = Strings.Implementing;
			toolSortByKind.Text = Strings.SortByKind;
			toolSortByAccess.Text = Strings.SortByAccess;
			toolSortByName.Text = Strings.SortByName;
		}

		private void RefreshValues()
		{
			CompositeType type = shape.CompositeType;
			Language language = type.Language;
			SuspendLayout();

			int cursorPosition = txtName.SelectionStart;
			txtName.Text = type.Name;
			txtName.SelectionStart = cursorPosition;

			SetError(null);
			needValidation = false;

			bool hasMember = (type.MemberCount > 0);
			toolSortByAccess.Enabled = hasMember;
			toolSortByKind.Enabled = hasMember;
			toolSortByName.Enabled = hasMember;

			RefreshVisibility();
			RefreshModifiers();
			RefreshNewMembers();

			ResumeLayout();
		}

		private void RefreshVisibility()
		{
			Language language = shape.CompositeType.Language;
			CompositeType type = shape.CompositeType;

			toolVisibility.Image = Icons.GetImage(type);
			toolVisibility.Text = language.ValidAccessModifiers[type.AccessModifier];

			// Public
			if (language.ValidAccessModifiers.ContainsKey(AccessModifier.Public))
			{
				toolPublic.Visible = true;
				toolPublic.Text = language.ValidAccessModifiers[AccessModifier.Public];
				toolPublic.Image = Icons.GetImage(type.EntityType, AccessModifier.Public);
			}
			else
			{
				toolPublic.Visible = false;
			}
			// Protected Internal
			if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.ProtectedInternal))
			{
				toolProtint.Visible = true;
				toolProtint.Text = language.ValidAccessModifiers[AccessModifier.ProtectedInternal];
				toolProtint.Image = Icons.GetImage(type.EntityType, AccessModifier.ProtectedInternal);
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
				toolInternal.Image = Icons.GetImage(type.EntityType, AccessModifier.Internal);
			}
			else
			{
				toolInternal.Visible = false;
			}
			// Protected
			if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.Protected))
			{
				toolProtected.Visible = true;
				toolProtected.Text = language.ValidAccessModifiers[AccessModifier.Protected];
				toolProtected.Image = Icons.GetImage(type.EntityType, AccessModifier.Protected);
			}
			else
			{
				toolProtected.Visible = false;
			}
			// Private
			if (type.IsNested && language.ValidAccessModifiers.ContainsKey(AccessModifier.Private))
			{
				toolPrivate.Visible = true;
				toolPrivate.Text = language.ValidAccessModifiers[AccessModifier.Private];
				toolPrivate.Image = Icons.GetImage(type.EntityType, AccessModifier.Private);
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
				toolDefault.Image = Icons.GetImage(type.EntityType, AccessModifier.Default);
			}
			else
			{
				toolDefault.Visible = false;
			}
		}

		private void RefreshModifiers()
		{
			Language language = shape.CompositeType.Language;

			ClassType classType = shape.CompositeType as ClassType;
			if (classType != null)
			{
				toolModifier.Visible = true;
				if (classType.Modifier == ClassModifier.None)
					toolModifier.Text = Strings.None;
				else
					toolModifier.Text = language.ValidClassModifiers[classType.Modifier];

				// Abstract modifier
				if (language.ValidClassModifiers.ContainsKey(ClassModifier.Abstract))
				{
					toolAbstract.Visible = true;
					toolAbstract.Text = language.ValidClassModifiers[ClassModifier.Abstract];
				}
				else
				{
					toolAbstract.Visible = false;
				}
				// Sealed modifier
				if (language.ValidClassModifiers.ContainsKey(ClassModifier.Sealed))
				{
					toolSealed.Visible = true;
					toolSealed.Text = language.ValidClassModifiers[ClassModifier.Sealed];
				}
				else
				{
					toolSealed.Visible = false;
				}
				// Static modifier
				if (language.ValidClassModifiers.ContainsKey(ClassModifier.Static))
				{
					toolStatic.Visible = true;
					toolStatic.Text = language.ValidClassModifiers[ClassModifier.Static];
				}
				else
				{
					toolStatic.Visible = false;
				}
			}
			else
			{
				toolModifier.Visible = false;
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

		public override void ValidateData()
		{
			ValidateName();
			SetError(null);
		}

		private bool ValidateName()
		{
			if (needValidation)
			{
				try
				{
					shape.CompositeType.Name = txtName.Text;
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
			if (ValidateName())
			{
				try
				{
					shape.CompositeType.AccessModifier = access;
					RefreshValues();
				}
				catch (BadSyntaxException ex)
				{
					RefreshValues();
					SetError(ex.Message);
				}
			}
		}

		private void ChangeModifier(ClassModifier modifier)
		{
			if (ValidateName())
			{
				ClassType classType = shape.CompositeType as ClassType;
				if (classType != null)
				{
					try
					{
						classType.Modifier = modifier;
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

		private void txtName_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					if (e.Modifiers == Keys.Control || e.Modifiers == Keys.Shift)
						OpenNewMemberDropDown();
					else
						ValidateName();
					e.Handled = true;
					break;

				case Keys.Escape:
					needValidation = false;
					shape.HideEditor();
					e.Handled = true;
					break;

				case Keys.Down:
					shape.ActiveMemberIndex = 0;
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

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			needValidation = true;
		}

		private void txtName_Validating(object sender, CancelEventArgs e)
		{
			ValidateName();
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

		private void toolNone_Click(object sender, EventArgs e)
		{
			ChangeModifier(ClassModifier.None);
		}

		private void toolAbstract_Click(object sender, EventArgs e)
		{
			ChangeModifier(ClassModifier.Abstract);
		}

		private void toolSealed_Click(object sender, EventArgs e)
		{
			ChangeModifier(ClassModifier.Sealed);
		}

		private void toolStatic_Click(object sender, EventArgs e)
		{
			ChangeModifier(ClassModifier.Static);
		}

		private void AddNewMember()
		{
			AddNewMember(NewMemberType);
		}

		private void AddNewMember(MemberType type)
		{
			if (!ValidateName())
				return;

			NewMemberType = type;
			switch (type)
			{
				case MemberType.Field:
					if (shape.CompositeType.SupportsFields)
					{
						shape.CompositeType.AddField();
						shape.ActiveMemberIndex = shape.CompositeType.FieldCount - 1;
					}
					break;

				case MemberType.Method:
					if (shape.CompositeType.SupportsMethods)
					{
						shape.CompositeType.AddMethod();
						shape.ActiveMemberIndex = shape.CompositeType.MemberCount - 1;
					}
					break;

				case MemberType.Constructor:
					if (shape.CompositeType.SupportsConstuctors)
					{
						shape.CompositeType.AddConstructor();
						shape.ActiveMemberIndex = shape.CompositeType.MemberCount - 1;
					}
					break;

				case MemberType.Destructor:
					if (shape.CompositeType.SupportsDestructors)
					{
						shape.CompositeType.AddDestructor();
						shape.ActiveMemberIndex = shape.CompositeType.MemberCount - 1;
					}
					break;

				case MemberType.Property:
					if (shape.CompositeType.SupportsProperties)
					{
						shape.CompositeType.AddProperty();
						shape.ActiveMemberIndex = shape.CompositeType.MemberCount - 1;
					}
					break;

				case MemberType.Event:
					if (shape.CompositeType.SupportsEvents)
					{
						shape.CompositeType.AddEvent();
						shape.ActiveMemberIndex = shape.CompositeType.MemberCount - 1;
					}
					break;
			}
			txtName.SelectionStart = 0;
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

		private void toolNewProperty_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Property);
		}

		private void toolNewEvent_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Event);
		}

		private void toolNewConstructor_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Constructor);			
		}

		private void toolNewDestructor_Click(object sender, EventArgs e)
		{
			AddNewMember(MemberType.Destructor);
		}

		private void toolOverrideList_Click(object sender, EventArgs e)
		{
			SingleInharitanceType type = shape.CompositeType as SingleInharitanceType;
			if (type != null)
			{
				using (OverrideDialog dialog = new OverrideDialog())
				{
					if (dialog.ShowDialog(type) == DialogResult.OK)
					{
						foreach (Operation operation in dialog.GetSelectedOperations())
						{
							type.Override(operation);
						}
					}
				}
			}
		}

		private void toolImplementList_Click(object sender, EventArgs e)
		{
			IInterfaceImplementer type = shape.CompositeType as IInterfaceImplementer;
			if (type != null)
			{
				using (ImplementDialog dialog = new ImplementDialog())
				{
					if (dialog.ShowDialog(type) == DialogResult.OK)
					{
						foreach (Operation operation in dialog.GetSelectedOperations())
						{
							Operation defined = type.GetDefinedOperation(operation);
							bool implementExplicitly = dialog.ImplementExplicitly &&
								type.Language.SupportsExplicitImplementation;

							if (defined == null)
							{
								type.Implement(operation, implementExplicitly);
							}
							else if (defined.Type != operation.Type)
							{
								type.Implement(operation, true);
							}
						}
					}
				}
			}
		}

		private void toolSortByKind_Click(object sender, EventArgs e)
		{
			shape.CompositeType.SortMembers(SortingMode.ByKind);
		}

		private void toolSortByAccess_Click(object sender, EventArgs e)
		{
			shape.CompositeType.SortMembers(SortingMode.ByAccess);
		}

		private void toolSortByName_Click(object sender, EventArgs e)
		{
			shape.CompositeType.SortMembers(SortingMode.ByName);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			txtName.SelectionStart = 0;
		}
	}
}
