// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev

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
using System.Linq;
using NClass.Core;

namespace NClass.CodeGenerator
{
    internal sealed class DartSourceFileGenerator : SourceFileGenerator
    {
        /// <exception cref="NullReferenceException">
        /// <paramref name="type"/> is null.
        /// </exception>
        public DartSourceFileGenerator(TypeBase type, string rootNamespace)
            : base(type, rootNamespace)
        {
        }

        protected override string Extension
        {
            get { return ".dart"; }
        }

        protected override void WriteFileContent()
        {
            WriteUsings();
            WriteType(Type);
        }

        private void WriteUsings()
        {
            StringCollection importList = Settings.Default.DartImportList;
            foreach (string usingElement in importList)
                WriteLine(usingElement);

            if (importList.Count > 0)
                AddBlankLine();
        }


        private void WriteType(TypeBase type)
        {
            if (type is CompositeType compositeType)
                WriteCompositeType(compositeType);
            else
            {
                if (type is EnumType enumType)
                    WriteEnum(enumType);
            }
        }

        private void WriteCompositeType(CompositeType type)
        {
            var isInterface = false;

            var declaration = type.GetDeclaration();
           

            if (type is ClassType classType)
            {
                WriteLine(declaration);
                WriteLine("{");
                IndentLevel++;

                foreach (var nestableChild in classType.NestedChilds)
                {
                    var nestedType = (TypeBase) nestableChild;
                    WriteType(nestedType);
                    AddBlankLine();
                }
            }

            if (type is InterfaceType)
            {
                WriteLine(declaration);
                WriteLine("{");
                IndentLevel++;
                isInterface = true;
            }

            if (type.SupportsFields)
            {
                foreach (Field field in type.Fields)
                    WriteField(field);
            }

            bool needBlankLine = (type.FieldCount > 0 && type.OperationCount > 0);

            foreach (Operation operation in type.Operations)
            {
                var isAbstract = false;
                if (needBlankLine)
                    AddBlankLine();
                needBlankLine = true;
                if (operation.IsAbstract)
                {
                   isAbstract = true;
                }
                WriteOperation(operation, isInterface, isAbstract);
            }

            // Writing closing bracket of the type block
            IndentLevel--;
            WriteLine("}");
        }

        private void WriteEnum(EnumType @enum)
        {
            var declaration = @enum.GetDeclaration();
            WriteLine(declaration);
            WriteLine("{");
            IndentLevel++;

            int valuesRemained = @enum.ValueCount;
            foreach (EnumValue value in @enum.Values)
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

        private void WriteOperation(Operation operation, bool isInterface, bool isAbstract)
        {
            var declaration = operation.GetDeclaration();
            if (operation is Property property)
            {
                WriteProperty(property);
            }
            else if (operation.HasBody)
            {
                if (operation.IsOverride)
                {
                    WriteLine("@override");
                }
                WriteLine(declaration);
                WriteLine("{");
                IndentLevel++;
                WriteNotImplementedString();
                IndentLevel--;
                WriteLine("}");
            }
            else if (isInterface || isAbstract)
            {
                if (operation.IsOverride)
                {
                    WriteLine("@override");
                }
                WriteLine(declaration + ";");
            }
        }

        private void WriteProperty(Property property)
        {
            var propertyLine = property.GetDeclaration();
            // Split on ';' and write each line separately
            string[] propLines = propertyLine.Split(';');
            propLines = propLines.Take(propLines.Length - 1).ToArray();
            foreach (var propLine in propLines)
            {
                WriteLine(propLine + ';');
            }

        }

        private void WriteNotImplementedString()
        {
            if (Settings.Default.UseNotImplementedExceptions)
            {
                WriteLine("throw new UnimplementedError();");
            }
            else
            {
                AddBlankLine(true);
            }
        }
    }
}
