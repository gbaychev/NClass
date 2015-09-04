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
using System.Text.RegularExpressions;

namespace NClass.Core
{
	public abstract class EnumValue : LanguageElement
	{
		string name = null;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		protected EnumValue(string declaration)
		{
			Initializing = true;
			InitFromString(declaration);
			Initializing = false;
		}

		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				if (name != value)
				{
					name = value;
					Changed();
				}
			}
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public abstract void InitFromString(string declaration);

		protected internal abstract EnumValue Clone();

		public override string ToString()
		{
			return GetDeclaration();
		}
	}
}