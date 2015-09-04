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
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
	internal sealed class JavaParameter : Parameter
	{
		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> or <paramref name="type"/> 
		/// does not fit to the syntax.
		/// </exception>
		internal JavaParameter(string name, string type) : base(name, type, ParameterModifier.In, null)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="value"/> does not fit to the syntax.
		/// </exception>
		public override string Type
		{
			get
			{
				return base.Type;
			}
			protected set
			{
				if (value == "void") {
					throw new BadSyntaxException(
						Strings.ErrorInvalidParameterDeclaration);
				}
				base.Type = value;
			}
		}

		public override string DefaultValue
		{
			get
			{
				return null;
			}
			protected set
			{
				if (value != null)
					throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
			}
		}

		public override Language Language
		{
			get { return JavaLanguage.Instance; }
		}

		public override string GetDeclaration()
		{
			return Type + " " + Name;
		}

		public override Parameter Clone()
		{
			return new JavaParameter(Name, Type);
		}
	}
}
