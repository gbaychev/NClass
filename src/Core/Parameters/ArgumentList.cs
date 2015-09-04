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
using System.Collections;
using System.Collections.Generic;

namespace NClass.Core
{
	public abstract class ArgumentList : CollectionBase, IEnumerable<Parameter>
	{
		protected ArgumentList()
		{
		}

		protected ArgumentList(int capacity) : base(capacity)
		{
		}

		public Parameter this[int index]
		{
			get { return (InnerList[index] as Parameter); }
		}

		IEnumerator<Parameter> IEnumerable<Parameter>.GetEnumerator()
		{
			for (int i = 0; i < InnerList.Count; i++)
				yield return (Parameter) InnerList[i];
		}

		public void Remove(Parameter parameter)
		{
			InnerList.Remove(parameter);
		}

		protected bool IsReservedName(string name)
		{
			return IsReservedName(name, -1);
		}

		protected bool IsReservedName(string name, int index)
		{
			for (int i = 0; i < Count; i++) {
				if (((Parameter) InnerList[i]).Name == name && i != index)
					return true;
			}
			return false;
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The parameter name is already exists.
		/// </exception>
		public abstract Parameter Add(string declaration);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The parameter name is already exists.
		/// </exception>
		public abstract Parameter ModifyParameter(Parameter parmeter, string declaration);

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public abstract void InitFromString(string declaration);

		public abstract ArgumentList Clone();
	}
}
