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

using NClass.Core;
using NClass.Translations;
using System;
using System.Text;

namespace NClass.Java
{
	internal sealed class JavaMethod : Method
	{
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		internal JavaMethod(CompositeType parent) : this("newMethod", parent)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="name"/> does not fit to the syntax.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The language of <paramref name="parent"/> does not equal.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		internal JavaMethod(string name, CompositeType parent) : base(name, parent)
		{
		}

		protected override string DefaultType
		{
			get { return "void"; }
		}

		public override AccessModifier AccessModifier
		{
			get
			{
				return base.AccessModifier;
			}
			set
			{
				if (value != AccessModifier.Default && value != AccessModifier.Public &&
					Parent is InterfaceType)
				{
					throw new BadSyntaxException(Strings.ErrorInterfaceMemberAccess);
				}

				base.AccessModifier = value;
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set virtual modifier.
		/// </exception>
		public override bool IsVirtual
		{
			get
			{
				return false;
			}
			set
			{
				if (value)
					throw new BadSyntaxException(Strings.ErrorInvalidModifier);
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set override modifier.
		/// </exception>
		public override bool IsOverride
		{
			get
			{
				return false;
			}
			set
			{
				if (value)
					throw new BadSyntaxException(Strings.ErrorInvalidModifier);
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set hider modifier.
		/// </exception>
		public override bool IsHider
		{
			get
			{
				return false;
			}
			set
			{
				if (value)
					throw new BadSyntaxException(Strings.ErrorInvalidModifier);
			}
		}

		public override bool IsOperator
		{
			get { return false; }
		}

		public override Language Language
		{
			get { return JavaLanguage.Instance; }
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromString(string declaration)
		{
			InitFromDeclaration(JavaMethodDeclaration.Create(declaration));
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromDeclaration(IMethodDeclaration declaration)
		{
			if (declaration is IJavaMethodDeclaration javaDeclaration)
			{
				InitFromDeclaration(javaDeclaration);
			}
			else
			{
				throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
			}
		}

		public override string GetDeclaration()
		{
			return GetDeclarationLine(true);
		}

		public string GetDeclarationLine(bool withSemicolon)
		{
			StringBuilder builder = new StringBuilder(100);

			if (AccessModifier != AccessModifier.Default) {
				builder.Append(Language.GetAccessString(AccessModifier, true));
				builder.Append(" ");
			}
			if (IsStatic)
				builder.Append("static ");
			if (IsSealed)
				builder.Append("final ");
			if (IsAbstract)
				builder.Append("abstract ");

			builder.AppendFormat("{0} {1}(", Type, Name);

			for (int i = 0; i < ArgumentList.Count; i++) {
				builder.Append(ArgumentList[i]);
				if (i < ArgumentList.Count - 1)
					builder.Append(", ");
			}
			builder.Append(")");

			if (withSemicolon && !HasBody)
				builder.Append(";");

			return builder.ToString();
		}

		public override Operation Clone(CompositeType newParent)
		{
			JavaMethod method = new JavaMethod(newParent);
			method.CopyFrom(this);
			return method;
		}

		public override string ToString()
		{
			return GetDeclarationLine(false);
		}

		private void InitFromDeclaration(IJavaMethodDeclaration declaration)
		{
			RaiseChangedEvent = false;

			try {
				ClearModifiers();

				if (JavaLanguage.Instance.IsForbiddenName(declaration.Name))
					throw new BadSyntaxException(Strings.ErrorInvalidName);
				if (JavaLanguage.Instance.IsForbiddenTypeName(declaration.Type))
					throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
				
				ValidName = declaration.Name;
				ValidType = declaration.Type;

				ArgumentList.InitFromDeclaration(declaration.ArgumentList);
				AccessModifier = declaration.AccessModifier;
				
				IsStatic = declaration.IsStatic;
				IsAbstract = declaration.IsAbstract;
				IsSealed = declaration.IsSealed;
			}
			finally {
				RaiseChangedEvent = true;
			}
		}
	}
}
