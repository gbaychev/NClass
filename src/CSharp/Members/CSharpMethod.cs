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

namespace NClass.CSharp
{
	internal sealed class CSharpMethod : Method
	{
		bool isOperator = false;
		bool isConversionOperator = false;
		bool isExplicitImplementation = false;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="parent"/> is null.
		/// </exception>
		internal CSharpMethod(CompositeType parent) : this("NewMethod", parent)
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
		internal CSharpMethod(string name, CompositeType parent) : base(name, parent)
		{
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="value"/> does not fit to the syntax.
		/// </exception>
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				var declaration = CSharpMethodNameDeclaration.Create(value);
				InitFromNameDeclaration(declaration);
			}
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
			set
			{
				if (IsConversionOperator) {
					if (string.IsNullOrEmpty(value))
						ValidType = value;
					else
						throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
				}
				else {
					if (string.IsNullOrEmpty(value))
						throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
					else
						base.Type = value;
				}
			}
		}

		protected override string DefaultType
		{
			get { return "void"; }
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
				if (value != AccessModifier.Default && IsExplicitImplementation) {
					throw new BadSyntaxException(
						Strings.ErrorExplicitImplementationAccess);
				}
				if (value != AccessModifier.Public && IsOperator) {
					throw new BadSyntaxException(
						Strings.ErrorOperatorMustBePublic);
				}
				if (value != AccessModifier.Default && Parent is InterfaceType) {
					throw new BadSyntaxException(
						Strings.ErrorInterfaceMemberAccess);
				}

				base.AccessModifier = value;
			}
		}

		public override bool IsAccessModifiable
		{
			get
			{
				return (
					base.IsAccessModifiable && !(Parent is InterfaceType) &&
					!IsOperator && !IsExplicitImplementation
				);
			}
		}

		public override bool IsTypeReadonly
		{
			get
			{
				return (base.IsTypeReadonly || IsConversionOperator);
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set static modifier.
		/// </exception>
		public override bool IsStatic
		{
			get
			{
				return base.IsStatic;
			}
			set
			{
				if (!value && IsOperator)
					throw new BadSyntaxException(Strings.ErrorOperatorMustBeStatic);

				base.IsStatic = value;
			}
		}

		/// <exception cref="BadSyntaxException">
		/// Cannot set hider modifier.
		/// </exception>
		public override bool IsHider
		{
			get
			{
				return base.IsHider;
			}
			set
			{
				if (value && IsOperator)
					throw new BadSyntaxException(Strings.ErrorInvalidModifier);

				base.IsHider = value;
			}
		}

		public override bool IsOperator
		{
			get { return isOperator; }
		}

		public bool IsConversionOperator
		{
			get { return isConversionOperator; }
		}

		public bool IsExplicitImplementation
		{
			get
			{
				return isExplicitImplementation;
			}
			private set
			{
				if (isExplicitImplementation != value) {
					try {
						RaiseChangedEvent = false;

						if (value)
							AccessModifier = AccessModifier.Default;
						isExplicitImplementation = value;
						Changed();
					}
					finally {
						RaiseChangedEvent = true;
					}
				}
			}
		}

		public override Language Language
		{
			get { return CSharpLanguage.Instance; }
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromString(string declaration)
		{
			InitFromDeclaration(CSharpMethodDeclaration.Create(declaration));
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromDeclaration(IMethodDeclaration declaration)
		{
			if (declaration is ICSharpMethodDeclaration csharpDeclaration)
			{
				InitFromDeclaration(csharpDeclaration);
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
			if (IsHider)
				builder.Append("new ");
			if (IsStatic)
				builder.Append("static ");
			if (IsVirtual)
				builder.Append("virtual ");
			if (IsAbstract)
				builder.Append("abstract ");
			if (IsSealed)
				builder.Append("sealed ");
			if (IsOverride)
				builder.Append("override ");

			if (string.IsNullOrEmpty(Type))
				builder.AppendFormat("{0}(", Name);
			else
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

		public override string ToString()
		{
			return GetDeclarationLine(false);
		}

		public override Operation Clone(CompositeType newParent)
		{
			CSharpMethod method = new CSharpMethod(newParent);
			method.CopyFrom(this);
			return method;
		}

		private void InitFromDeclaration(ICSharpMethodDeclaration declaration)
		{
			RaiseChangedEvent = false;

			try {
				ClearModifiers();

				if (CSharpLanguage.Instance.IsForbiddenName(declaration.Name))
					throw new BadSyntaxException(Strings.ErrorInvalidName);
				if (CSharpLanguage.Instance.IsForbiddenTypeName(declaration.Type))
					throw new BadSyntaxException(Strings.ErrorInvalidTypeName);

				ValidName = declaration.Name;
				ValidType = declaration.Type;

				isOperator = declaration.IsOperator;
				isConversionOperator = declaration.IsConversionOperator;
				
				if (IsOperator) {
					ClearModifiers();
					IsStatic = true;
					AccessModifier = AccessModifier.Public;
				}
				else {
					AccessModifier = declaration.AccessModifier;
				}

				IsExplicitImplementation = declaration.IsExplicitImplementation;
				ArgumentList.InitFromDeclaration(declaration.ArgumentList);

				if (!declaration.IsSealed && !declaration.IsOverride)
				{
					IsVirtual = declaration.IsVirtual;
				}

				IsStatic = declaration.IsStatic;
				IsAbstract = declaration.IsAbstract;
				IsOverride = declaration.IsOverride;
				IsSealed = declaration.IsSealed;
				IsHider = declaration.IsHider;
			}
			finally {
				RaiseChangedEvent = true;
			}
		}

		private void InitFromNameDeclaration(CSharpMethodNameDeclaration declaration)
		{
			RaiseChangedEvent = false;
			try
			{
				string name = declaration.Name;
				if (Language.IsForbiddenName(name))
					throw new BadSyntaxException(Strings.ErrorForbiddenName);

				ValidName = name;
				isOperator = declaration.IsOperator;
				isConversionOperator = declaration.IsConversionOperator;
				IsExplicitImplementation = declaration.IsExplicitImplementation;

				if (isOperator) {
					ClearModifiers();
					IsStatic = true;
					AccessModifier = AccessModifier.Public;
						
					if (isConversionOperator)
						ValidType = null;
					else if (string.IsNullOrEmpty(Type))
						ValidType = "int";
				}
			}
			finally
			{
				RaiseChangedEvent = true;
			}
		}
	}
}
