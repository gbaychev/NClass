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
using System.Collections.Generic;
using NClass.Core;

namespace NClass.Java
{
	internal sealed class JavaInterface : InterfaceType
	{
		internal JavaInterface() : this("NewInterface")
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		internal JavaInterface(string name) : base(name)
		{
		}

		public override AccessModifier AccessModifier
		{
			get
			{
				return base.AccessModifier;
			}
			set
			{
				if (IsNested ||
					value == AccessModifier.Default ||
					value == AccessModifier.Public)
				{
					base.AccessModifier = value;
				}
			}
		}

		public override AccessModifier DefaultAccess
		{
			get { return AccessModifier.Internal; }
		}

		public override AccessModifier DefaultMemberAccess
		{
			get { return AccessModifier.Public; }
		}

		public override bool SupportsFields
		{
			get { return true; }
		}

		public override bool SupportsProperties
		{
			get { return false; }
		}

		public override bool SupportsEvents
		{
			get { return false; }
		}

		/// <exception cref="ArgumentException">
		/// The <paramref name="value"/> is already a child member of the type.
		/// </exception>
		public override CompositeType NestingParent
		{
			get
			{
				return base.NestingParent;
			}
			protected set
			{
				try {
					RaiseChangedEvent = false;

					base.NestingParent = value;
					if (NestingParent == null && Access != AccessModifier.Public)
						AccessModifier = AccessModifier.Default;
				}
				finally {
					RaiseChangedEvent = true;
				}
			}
		}

		public override Language Language
		{
			get { return JavaLanguage.Instance; }
		}

		public override Field AddField()
		{
			JavaField field = new JavaField(this);

			field.IsStatic = true;
			field.IsReadonly = true;
			AddField(field);

			return field;
		}

		public override Method AddMethod()
		{
			Method method = new JavaMethod(this);

			AddOperation(method);
			return method;
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support properties.
		/// </exception>
		public override Property AddProperty()
		{
			throw new InvalidOperationException("Java language does not support properties.");
		}

		/// <exception cref="InvalidOperationException">
		/// The type does not support events.
		/// </exception>
		public override Event AddEvent()
		{
			throw new InvalidOperationException("Java language does not support events.");
		}

		public override string GetDeclaration()
		{
			StringBuilder builder = new StringBuilder(30);

			if (AccessModifier != AccessModifier.Default) {
				builder.Append(Language.GetAccessString(AccessModifier, true));
				builder.Append(" ");
			}
			if (IsNested)
				builder.Append("static ");

			builder.AppendFormat("interface {0}", Name);

			if (HasExplicitBase) {
				builder.Append(" extends ");
				for (int i = 0; i < BaseList.Count; i++) {
					builder.Append(BaseList[i].Name);
					if (i < BaseList.Count - 1)
						builder.Append(", ");
				}
			}

			return builder.ToString();
		}

		public override InterfaceType Clone()
		{
			JavaInterface newInterface = new JavaInterface();
			newInterface.CopyFrom(this);
			return newInterface;
		}
	}
}
