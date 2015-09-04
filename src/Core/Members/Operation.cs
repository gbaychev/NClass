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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NClass.Core
{
	public abstract class Operation : Member
	{
		OperationModifier modifier = OperationModifier.None;
		ArgumentList argumentList;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The language of <paramref name="parent"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		protected Operation(string name, CompositeType parent) : base(name, parent)
		{
			argumentList = Language.CreateParameterCollection();
		}

		public bool HasParameter
		{
			get
			{
				return (ArgumentList != null && ArgumentList.Count > 0);
			}
		}

		protected ArgumentList ArgumentList
		{
			get { return argumentList; }
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set access visibility.
		/// </exception>
		public override AccessModifier AccessModifier
		{
			get
			{
				return base.AccessModifier;
			}
			set
			{
				if (value == AccessModifier)
					return;

				AccessModifier previousAccess = base.AccessModifier;

				try {
					RaiseChangedEvent = false;

					base.AccessModifier = value;
					Language.ValidateOperation(this);
				}
				catch {
					base.AccessModifier = previousAccess;
					throw;
				}
				finally {
					RaiseChangedEvent = true;
				}
			}
		}

		public OperationModifier Modifier
		{
			get { return modifier; }
		}

		public sealed override bool IsModifierless
		{
			get
			{
				return (modifier == OperationModifier.None);
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set static modifier.
		/// </exception>
		public override bool IsStatic
		{
			get
			{
				return ((modifier & OperationModifier.Static) != 0);
			}
			set
			{
				if (value == IsStatic)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Static;
					else
						modifier &= ~OperationModifier.Static;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set hider modifier.
		/// </exception>
		public override bool IsHider
		{
			get
			{
				return ((modifier & OperationModifier.Hider) != 0);
			}
			set
			{
				if (value == IsHider)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Hider;
					else
						modifier &= ~OperationModifier.Hider;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set virtual modifier.
		/// </exception>
		public virtual bool IsVirtual
		{
			get
			{
				return ((modifier & OperationModifier.Virtual) != 0);
			}
			set
			{
				if (value == IsVirtual)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Virtual;
					else
						modifier &= ~OperationModifier.Virtual;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set abstract modifier.
		/// </exception>
		public virtual bool IsAbstract
		{
			get
			{
				return ((modifier & OperationModifier.Abstract) != 0);
			}
			set
			{
				if (value == IsAbstract)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Abstract;
					else
						modifier &= ~OperationModifier.Abstract;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set override modifier.
		/// </exception>
		public virtual bool IsOverride
		{
			get
			{
				return ((modifier & OperationModifier.Override) != 0);
			}
			set
			{
				if (value == IsOverride)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Override;
					else
						modifier &= ~OperationModifier.Override;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set sealed modifier.
		/// </exception>
		public virtual bool IsSealed
		{
			get
			{
				return ((modifier & OperationModifier.Sealed) != 0);
			}
			set
			{
				if (value == IsSealed)
					return;

				OperationModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= OperationModifier.Sealed;
					else
						modifier &= ~OperationModifier.Sealed;
					Language.ValidateOperation(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		public virtual bool HasBody
		{
			get
			{
				return (!IsAbstract && !(Parent is InterfaceType));
			}
		}

		public virtual bool Overridable
		{
			get
			{
				if (Language.ExplicitVirtualMethods) {
					return (IsVirtual || IsAbstract || (IsOverride && !IsSealed));
				}
				else {
					return (
						Access != AccessModifier.Private &&
						(IsModifierless || IsAbstract || IsHider)
					);
				}
			}
		}

		public virtual void ClearModifiers()
		{
			if (modifier != OperationModifier.None) {
				modifier = OperationModifier.None;
				Changed();
			}
		}

		protected override void CopyFrom(Member member)
		{
			base.CopyFrom(member);

			Operation operation = (Operation) member;
			modifier = operation.modifier;
			argumentList = operation.argumentList.Clone();
		}

		public abstract Operation Clone(CompositeType newParent);

		public virtual bool HasSameSignatureAs(Operation operation)
		{
			if (operation == null || Name != operation.Name)
				return false;

			// Names and types are the same and the parameter counts do not equal
			if (ArgumentList.Count != operation.ArgumentList.Count)
				return false;

			for (int i = 0; i < ArgumentList.Count; i++) {
				if (ArgumentList[i].Type != operation.ArgumentList[i].Type ||
					ArgumentList[i].Modifier != operation.ArgumentList[i].Modifier)
				{
					return false;
				}
			}

			return true;
		}
	}
}
