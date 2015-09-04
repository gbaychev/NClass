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

namespace NClass.CSharp
{
	internal sealed class CSharpParameter : Parameter
	{
		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> or <paramref name="type"/> 
		/// does not fit to the syntax.
		/// </exception>
		internal CSharpParameter(string name, string type, ParameterModifier modifier, string defaultValue)
			: base(name, type, modifier, defaultValue)
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
				if (value == "void")
				{
					throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
				}
				base.Type = value;
			}
		}

		public override ParameterModifier Modifier
		{
			get
			{
				return base.Modifier;
			}
			protected set
			{
				if (value != ParameterModifier.In && DefaultValue != null)
				{
					throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
				}
				base.Modifier = value;
			}
		}

		public override string DefaultValue
		{
			get
			{
				return base.DefaultValue;
			}
			protected set
			{
				if (!string.IsNullOrWhiteSpace(value) && Modifier != ParameterModifier.In)
				{
					throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
				}
				base.DefaultValue = value;
			}
		}

		public override Language Language
		{
			get { return CSharpLanguage.Instance; }
		}

		public override string GetDeclaration()
		{
			if (DefaultValue != null)
			{
				return Type + " " + Name + " = " + DefaultValue;
			}
			else if (Modifier == ParameterModifier.In)
			{
				return Type + " " + Name;
			}
			else if (Modifier == ParameterModifier.Inout)
			{
				return string.Format("ref {0} {1}", Type, Name);
			}
			else
			{
				return string.Format("{0} {1} {2}",
					Modifier.ToString().ToLower(), Type, Name);
			}
		}

		public override Parameter Clone()
		{
			return new CSharpParameter(Name, Type, Modifier, DefaultValue);
		}
	}
}
