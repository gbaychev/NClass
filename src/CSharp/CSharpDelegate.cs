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
using System.Text;
using System.Xml;
using NClass.Core;

namespace NClass.CSharp
{
	internal sealed class CSharpDelegate : DelegateType
	{
		internal CSharpDelegate() : this("NewDelegate")
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		internal CSharpDelegate(string name) : base(name)
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
					value == AccessModifier.Internal ||
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

		protected override string DefaultReturnType
		{
			get { return "void"; }
		}

		public override Language Language
		{
			get { return CSharpLanguage.Instance; }
		}

		public override string GetDeclaration()
		{
			StringBuilder builder = new StringBuilder();

			if (AccessModifier != AccessModifier.Default) {
				builder.Append(Language.GetAccessString(AccessModifier, true));
				builder.Append(" ");
			}
			builder.AppendFormat("delegate {0} {1}(", ReturnType, Name);

			int parameterIndex = 0;
			foreach (Parameter parameter in Arguments) {
				builder.Append(parameter.ToString());
				if (parameterIndex++ < ArgumentCount - 1)
					builder.Append(", ");
			}
			builder.Append(");");

			return builder.ToString();
		}

		public override DelegateType Clone()
		{
			CSharpDelegate newDelegate = new CSharpDelegate();
			newDelegate.CopyFrom(this);
			return newDelegate;
		}
	}
}
