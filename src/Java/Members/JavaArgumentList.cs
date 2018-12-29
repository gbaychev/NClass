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
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
	internal class JavaArgumentList : ArgumentList
	{
		internal JavaArgumentList()
		{
		}

		internal JavaArgumentList(int capacity)
			: base(capacity)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The parameter name is already exists.
		/// </exception>
		public override Parameter Add(string declaration)
		{
			var param = JavaParameterDeclaration.Create(declaration);

			if (IsReservedName(param.Name))
				throw new ReservedNameException(param.Name);

			var parameter = new JavaParameter(param.Name, param.Type);
			InnerList.Add(parameter);

			return parameter;
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ReservedNameException">
		/// The parameter name is already exists.
		/// </exception>
		public override Parameter ModifyParameter(Parameter parameter, string declaration)
		{
			var param = JavaParameterDeclaration.Create(declaration);
			int index = InnerList.IndexOf(parameter);

			if (index < 0)
				return parameter;

			if (IsReservedName(param.Name, index))
				throw new ReservedNameException(param.Name);

			var newParameter = new JavaParameter(param.Name, param.Type);
			InnerList[index] = newParameter;
			return newParameter;
		}

		public override ArgumentList Clone()
		{
			JavaArgumentList argumentList = new JavaArgumentList(Capacity);
			foreach (Parameter parameter in InnerList)
			{
				argumentList.InnerList.Add(parameter.Clone());
			}
			return argumentList;
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromString(string declaration)
		{
			InitFromDeclaration(JavaArgumentListDeclaration.Create(declaration));
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromDeclaration(IArgumentListDeclaration<IParameterDeclaration> declaration)
		{
			if (declaration is IJavaArgumentListDeclaration<IJavaParameterDeclaration> javaDeclaration)
			{
				InitFromDeclaration(javaDeclaration);
			}
			else
			{
				throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
			}
		}

		private void InitFromDeclaration(IJavaArgumentListDeclaration<IJavaParameterDeclaration> declaration)
		{
			Clear();

			foreach (var param in declaration)
			{
				InnerList.Add(new JavaParameter(param.Name, param.Type));
			}
		}
	}
}
