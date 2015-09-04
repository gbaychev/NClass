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
using System.Collections.Specialized;
using NClass.Core;
using NClass.Java;

namespace NClass.CodeGenerator
{
	internal sealed class JavaSourceFileGenerator : SourceFileGenerator
	{
		/// <exception cref="NullReferenceException">
		/// <paramref name="type"/> is null.
		/// </exception>
		public JavaSourceFileGenerator(TypeBase type, string rootNamespace)
			: base(type, rootNamespace)
		{
		}

		protected override string Extension
		{
			get { return ".java"; }
		}

		protected override void WriteFileContent()
		{
			WritePackageDeclaration();
			WriteImportList();
			WriteType(Type);
		}

		private void WritePackageDeclaration()
		{
			WriteLine("package " + RootNamespace + ";");
			AddBlankLine();
		}

		private void WriteImportList()
		{
			StringCollection importList = Settings.Default.JavaImportList;
			foreach (string importElement in importList)
				WriteLine("import " + importElement + ";");

			if (importList.Count > 0)
				AddBlankLine();
		}

		private void WriteType(TypeBase type)
		{
			if (type is CompositeType)
				WriteCompositeType((CompositeType) type);
			else if (type is EnumType)
				WriteEnum((EnumType) type);
		}

		private void WriteCompositeType(CompositeType type)
		{
			// Writing type declaration
			WriteLine(type.GetDeclaration() + " {");
			AddBlankLine();
			IndentLevel++;

			if (type is ClassType)
			{
				foreach (TypeBase nestedType in ((ClassType) type).NestedChilds)
				{
					WriteType(nestedType);
					AddBlankLine();
				}
			}

			if (type.FieldCount > 0)
			{
				foreach (Field field in type.Fields)
					WriteField(field);
				AddBlankLine();
			}

			if (type.OperationCount > 0)
			{
				foreach (Method method in type.Operations)
				{
					WriteMethod(method);
					AddBlankLine();
				}
			}

			// Writing closing bracket of the type block
			IndentLevel--;
			WriteLine("}");
		}

		private void WriteEnum(EnumType _enum)
		{
			// Writing type declaration
			WriteLine(_enum.GetDeclaration() + " {");
			AddBlankLine();
			IndentLevel++;

			int valuesRemained = _enum.ValueCount;
			foreach (EnumValue value in _enum.Values)
			{
				if (--valuesRemained > 0)
					WriteLine(value.GetDeclaration() + ",");
				else
					WriteLine(value.GetDeclaration());
			}

			// Writing closing bracket of the type block
			IndentLevel--;
			WriteLine("}");
		}

		private void WriteField(Field field)
		{
			WriteLine(field.GetDeclaration());
		}

		private void WriteMethod(Method method)
		{
			if (method.HasBody)
			{
				WriteLine(method.GetDeclaration() + " {");
				IndentLevel++;
				WriteNotImplementedString();
				IndentLevel--;
				WriteLine("}");
			}
			else
			{
				WriteLine(method.GetDeclaration());
			}
		}

		private void WriteNotImplementedString()
		{
			if (Settings.Default.UseNotImplementedExceptions)
			{
				WriteLine("throw new UnsupportedOperationException(" +
					"\"The method is not implemented yet.\");");
			}
			else
			{
				AddBlankLine(true);
			}
		}
	}
}