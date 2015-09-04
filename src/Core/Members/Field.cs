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
using System.Text;
using System.Text.RegularExpressions;

namespace NClass.Core
{
	public abstract class Field : Member
	{
		FieldModifier modifier = FieldModifier.None;
		string initialValue = null;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The language of <paramref name="parent"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		protected Field(string name, CompositeType parent) : base(name, parent)
		{
		}

		public sealed override MemberType MemberType
		{
			get { return MemberType.Field; }
		}

		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				ValidName = Language.GetValidName(value, false);
			}
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
					Language.ValidateField(this);
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

		public FieldModifier Modifier
		{
			get { return modifier; }
		}

		public sealed override bool IsModifierless
		{
			get
			{
				return (modifier == FieldModifier.None);
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set static modifier.
		/// </exception>
		public override bool IsStatic
		{
			get
			{
				return ((modifier & FieldModifier.Static) != 0);
			}
			set
			{
				if (value == IsStatic)
					return;

				FieldModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= FieldModifier.Static;
					else
						modifier &= ~FieldModifier.Static;
					Language.ValidateField(this);
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
				return ((modifier & FieldModifier.Hider) != 0);
			}
			set
			{
				if (value == IsHider)
					return;

				FieldModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= FieldModifier.Hider;
					else
						modifier &= ~FieldModifier.Hider;
					Language.ValidateField(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set readonly modifier.
		/// </exception>
		public virtual bool IsReadonly
		{
			get
			{
				return ((modifier & FieldModifier.Readonly) != 0);
			}
			set
			{
				if (value == IsReadonly)
					return;

				FieldModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= FieldModifier.Readonly;
					else
						modifier &= ~FieldModifier.Readonly;
					Language.ValidateField(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set constant modifier.
		/// </exception>
		public virtual bool IsConstant
		{
			get
			{
				return ((modifier & FieldModifier.Constant) != 0);
			}
			set
			{
				if (value == IsConstant)
					return;

				FieldModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= FieldModifier.Constant;
					else
						modifier &= ~FieldModifier.Constant;
					Language.ValidateField(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set volatile modifier.
		/// </exception>
		public virtual bool IsVolatile
		{
			get
			{
				return ((modifier & FieldModifier.Volatile) != 0);
			}
			set
			{
				if (value == IsVolatile)
					return;

				FieldModifier previousModifier = modifier;

				try {
					if (value)
						modifier |= FieldModifier.Volatile;
					else
						modifier &= ~FieldModifier.Volatile;
					Language.ValidateField(this);
					Changed();
				}
				catch {
					modifier = previousModifier;
					throw;
				}
			}
		}

		public virtual string InitialValue
		{
			get
			{
				return initialValue;
			}
			set
			{
				if (initialValue != value &&
					(!string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(initialValue)))
				{
					initialValue = value;
					Changed();
				}
			}
		}

		public bool HasInitialValue
		{
			get
			{
				return !string.IsNullOrEmpty(InitialValue);
			}
		}

		public virtual void ClearModifiers()
		{
			if (modifier != FieldModifier.None) {
				modifier = FieldModifier.None;
				Changed();
			}
		}

		public sealed override string GetUmlDescription(bool getType, bool getParameters,
			bool getParameterNames, bool getInitValue)
		{
			StringBuilder builder = new StringBuilder(50);

			builder.Append(Name);
			if (getType)
				builder.AppendFormat(": {0}", Type);
			if (getInitValue && HasInitialValue)
				builder.AppendFormat(" = {0}", InitialValue);

			return builder.ToString();
		}

		protected override void CopyFrom(Member member)
		{
			base.CopyFrom(member);

			Field field = (Field) member;
			modifier = field.modifier;
			initialValue = field.initialValue;
		}

		protected internal abstract Field Clone(CompositeType newParent);
	}
}
