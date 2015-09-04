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
using System.Text;

namespace NClass.Core
{
	public abstract class Method : Operation
	{
		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The language of <paramref name="parent"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		protected Method(string name, CompositeType parent) : base(name, parent)
		{
		}

		public override MemberType MemberType
		{
			get { return MemberType.Method; }
		}

		public abstract bool IsOperator
		{
			get;
		}

		public sealed override string GetUmlDescription(bool getType, bool getParameters,
			bool getParameterNames, bool getInitValue)
		{
			StringBuilder builder = new StringBuilder(100);

			builder.AppendFormat("{0}(", Name);

			if (getParameters) {
				for (int i = 0; i < ArgumentList.Count; i++) {
					builder.Append(ArgumentList[i].GetUmlDescription(getParameterNames));
					if (i < ArgumentList.Count - 1)
						builder.Append(", ");
				}
			}

			if (getType && !string.IsNullOrEmpty(Type))
				builder.AppendFormat(") : {0}", Type);
			else
				builder.Append(")");

			return builder.ToString();
		}
	}
}
