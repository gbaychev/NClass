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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Core.Entities;
using NClass.Translations;

namespace NClass.Dart
{
    public sealed class DartLanguage : Language
    {
        static DartLanguage instance = new DartLanguage();

        #region Regex patterns

        internal const string InitialChar = @"[@\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Pc}\p{Lm}]";
        internal const string DeclarationEnding = @"\s*(;\s*)?$";

        // System.String
        private const string TypeNamePattern = InitialChar + @"(\w|\." + InitialChar + @")*";

        // get
        public const string GetPattern = @"\s*get\s*";

        // get
        public const string SetPattern = @"\s*set\s*";

        // System.String[]
        internal const string BaseTypePattern = TypeNamePattern + @"\??" + GetPattern;

        // <System.String[], System.String[]>
        private const string GenericPattern =
            @"<\s*" + BaseTypePattern + @"(\s*,\s*" + BaseTypePattern + @")*\s*>";
        
        // System.Collections.Generic.List<System.String[], System.String[]>[]
        internal const string GenericTypePattern =
            TypeNamePattern + @"(\s*" + GenericPattern + @")?\??";
        
        // <List<int>[], List<string>>
        private const string GenericPattern2 =
            @"<\s*" + GenericTypePattern + @"(\s*,\s*" + GenericTypePattern + @")*\s*>";
        
        // System.Collections.Generic.List<List<int>[]>[]
        internal const string GenericTypePattern2 =
            TypeNamePattern + @"(\s*" + GenericPattern2 + @")?\??";


        // Name
        internal const string NamePattern = InitialChar + @"\w*";
        
        // Named constructor
        internal const string NamedConstructorPattern = InitialChar + @"\w*[.]?\w*";

        // <T, K>
        private const string BaseGenericPattern =
            @"<\s*" + NamePattern + @"(\s*,\s*" + NamePattern + @")*\s*>";
        
        // Name<T>
        internal const string GenericNamePattern =
            NamePattern + @"(\s*" + BaseGenericPattern + ")?";

        // Interface.Method
        private const string OperationNamePattern =
            "(" + GenericTypePattern + @"(?<namedot>\.))?" + NamePattern;

        // Interface.Method<T>
        internal const string GenericOperationNamePattern =
            OperationNamePattern + @"(\s*" + BaseGenericPattern + ")?";


        // [static | abstract | get | set]
        internal const string OperationModifiersPattern =
            @"((?<modifier>static|abstract|override)\s+)*";

        // [ private]
        internal const string AccessPattern =@"((?<access>private|public)\s+)*";

        // For validating identifier names.
        private const string ClosedNamePattern = @"^\s*(?<name>" + NamePattern + @")\s*$";

        // For validating generic identifier names.
        private const string ClosedGenericNamePattern = @"^\s*(?<name>" + GenericNamePattern + @")\s*$";

        // For validating type names.
        private const string ClosedTypePattern = @"^\s*(?<type>" + GenericTypePattern2 + @")\s*$";

        #endregion

        static Regex nameRegex = new Regex(ClosedNamePattern, RegexOptions.ExplicitCapture);
        static Regex genericNameRegex = new Regex(ClosedGenericNamePattern, RegexOptions.ExplicitCapture);
        static Regex typeRegex = new Regex(ClosedTypePattern, RegexOptions.ExplicitCapture);

        private static readonly string[] reservedNames = {
            "assert", "break", "case", "catch", "class", "const", "continue", "default", "do", "else", "enum", "extends", "false", "final", "finally", "for", "if", "in", "is", "new", "null", "rethrow", "return", "super", "switch", 
            "this", "throw", "true", "try", "var", "while", "with", "abstract", "as", "covariant", "deferred", "export", "extension", "external", "factory", "function", "get", "implements", "import", 
            "interface", "library", "mixin", "operator", "part", "set", "static", "typedef"
    };

        private static readonly string[] typeKeywords = {
            "bool", "double", "int", "num", "Object", "String", "void", "Map", "List, dynamic, null"
        };

        private static readonly Dictionary<AccessModifier, string> validAccessModifiers;
        private static readonly Dictionary<ClassModifier, string> validClassModifiers;
        private static readonly Dictionary<FieldModifier, string> validFieldModifiers;
        private static readonly Dictionary<OperationModifier, string> validOperationModifiers;

        static DartClass objectClass;

        static DartLanguage()
        {
            // objectClass initialization
            string[] objectMethods = {
                "bool operator ==(object other)",
                "int get hashCode",
                "String get runTimeType",
                "dynamic noSuchMethod(Invocation invocation)",
                "String toString()"
            };

            objectClass = new DartClass("Object");
            objectClass.AddConstructor();
            foreach (string methodDeclaration in objectMethods)
                objectClass.AddMethod().InitFromString(methodDeclaration);

            // validAccessModifiers initialization
            validAccessModifiers = new Dictionary<AccessModifier, string>(6);
            validAccessModifiers.Add(AccessModifier.Public, "Public");
            validAccessModifiers.Add(AccessModifier.Private, "Private");
            validAccessModifiers.Add(AccessModifier.Default, "Default");

            // validClassModifiers initialization
            validClassModifiers = new Dictionary<ClassModifier, string>(3);
            validClassModifiers.Add(ClassModifier.Abstract, "Abstract");
            validClassModifiers.Add(ClassModifier.Mixin, "Mixin");

            // validFieldModifiers initialization
            validFieldModifiers = new Dictionary<FieldModifier, string>(5);
            validFieldModifiers.Add(FieldModifier.Static, "Static");
            validFieldModifiers.Add(FieldModifier.Constant, "Const");


            // validOperationModifiers initialization
            validOperationModifiers = new Dictionary<OperationModifier, string>(8);
            validOperationModifiers.Add(OperationModifier.Static, "Static");
            validOperationModifiers.Add(OperationModifier.Abstract, "Abstract");
            validOperationModifiers.Add(OperationModifier.Override, "Override");
            validOperationModifiers.Add(OperationModifier.Factory, "Factory");
        }

        private DartLanguage()
        {
        }

        public static DartLanguage Instance
        {
            get { return instance; }
        }

        internal static DartClass ObjectClass
        {
            get { return objectClass; }
        }

        public override string Name
        {
            get { return "Dart"; }
        }

        public override string AssemblyName
        {
            get { return "Dart"; }
        }

        [XmlIgnore]
        public override Dictionary<AccessModifier, string> ValidAccessModifiers
        {
            get { return validAccessModifiers; }
        }

        [XmlIgnore]
        public override Dictionary<ClassModifier, string> ValidClassModifiers
        {
            get { return validClassModifiers; }
        }

        [XmlIgnore]
        public override Dictionary<FieldModifier, string> ValidFieldModifiers
        {
            get { return validFieldModifiers; }
        }

        [XmlIgnore]
        public override Dictionary<OperationModifier, string> ValidOperationModifiers
        {
            get { return validOperationModifiers; }
        }

        protected override string[] ReservedNames
        {
            get { return reservedNames; }
        }

        protected override string[] TypeKeywords
        {
            get { return typeKeywords; }
        }

        public override string DefaultFileExtension
        {
            get { return ".dart"; }
        }

        public override bool IsValidModifier(AccessModifier modifier)
        {
            if ((modifier == AccessModifier.Default) ||
                (modifier == AccessModifier.Private) ||
                (modifier == AccessModifier.Public))
            {
                return true;
            }

            return false;
        }


        public override bool IsValidModifier(FieldModifier modifier)
        {
            if ((modifier == FieldModifier.Static) ||
                (modifier == FieldModifier.Constant) ||
                (modifier == FieldModifier.None))
            {
                return true;
            }

            return false;
        }

        public override bool IsValidModifier(OperationModifier modifier)
        {
            if ((modifier == OperationModifier.Abstract) ||
                (modifier == OperationModifier.None) ||
                (modifier == OperationModifier.Override) ||
                (modifier == OperationModifier.Static) ||
                (modifier == OperationModifier.Factory))
            {
                return true;
            }

            return false;
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="operation"/> contains invalid modifier combinations.
        /// </exception>
        protected override void ValidateOperation(Operation operation)
        {
            ValidateAccessModifiers(operation);
            ValidateOperationModifiers(operation);
        }

        private static void ValidateOperationModifiers(Operation operation)
        {
            if (operation.IsStatic) {
                if (operation.IsAbstract) {
                    throw new BadSyntaxException(string.Format(
                        Strings.ErrorInvalidModifierCombination, "abstract", "static"));
                }
            }

            if (operation.IsAbstract) {
                ClassType parent = operation.Parent as ClassType;
                if (parent == null)
                    throw new BadSyntaxException(Strings.ErrorInvalidModifier);
                else
                    parent.Modifier = ClassModifier.Abstract;
            }

        }

        private static void ValidateAccessModifiers(Operation operation)
        {
            if (operation.AccessModifier != AccessModifier.Default &&
                operation.Parent is InterfaceType)
            {
                throw new BadSyntaxException(
                    Strings.ErrorInterfaceMemberAccess);
            }
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="field"/> contains invalid modifier combinations.
        /// </exception>
        protected override void ValidateField(Field field)
        {
            if (field.IsConstant) {
                if (field.IsStatic) {
                    throw new BadSyntaxException(string.Format(
                        Strings.ErrorInvalidModifierCombination, "const", "static"));
                }
            }
        }

        /// <exception cref="ArgumentException">
        /// The language does not support explicit interface implementation.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operation"/> is null.-or-
        /// <paramref name="newParent"/> is null.
        /// </exception>
        protected override Operation Implement(Operation operation,
            CompositeType newParent, bool explicitly)
        {
            if (newParent == null)
                throw new ArgumentNullException("newParent");
            if (operation == null)
                throw new ArgumentNullException("operation");

            Operation newOperation = operation.Clone(newParent);

            newOperation.AccessModifier = AccessModifier.Public;
            newOperation.ClearModifiers();
            newOperation.IsStatic = false;

            if (explicitly) {
                newOperation.Name = string.Format("{0}.{1}",
                    ((InterfaceType) operation.Parent).Name, newOperation.Name);
            }

            return newOperation;
        }

        /// <exception cref="ArgumentException">
        /// <paramref name="operation"/> cannot be overridden.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operation"/> is null.
        /// </exception>
        protected override Operation Override(Operation operation, CompositeType newParent)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            if (!operation.IsVirtual && !operation.IsAbstract && !operation.IsOverride ||
                operation.IsSealed)
            {
                throw new ArgumentException(
                    Strings.ErrorCannotOverride, "operation");
            }

            Operation newOperation = operation.Clone(newParent);
            newOperation.IsVirtual = false;
            newOperation.IsAbstract = false;
            newOperation.IsOverride = true;

            return newOperation;
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        public override string GetValidName(string name, bool isGenericName)
        {
            Match match = (isGenericName ? genericNameRegex.Match(name) : nameRegex.Match(name));

            if (match.Success) {
                string validName = match.Groups["name"].Value;
                return base.GetValidName(validName, isGenericName);
            }
            else {
                throw new BadSyntaxException(Strings.ErrorInvalidName);
            }            
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        public override string GetValidTypeName(string name)
        {
            Match match = typeRegex.Match(name);

            if (match.Success) {
                string validName = match.Groups["type"].Value;
                return base.GetValidTypeName(validName);
            }
            else {
                throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
            }
        }

        public override string GetAccessString(AccessModifier access, bool forCode)
        {
            switch (access) {
                case AccessModifier.Default:
                    if (forCode)
                        return "";
                    else
                        return "Default";

                default:
                    if (forCode)
                        return access.ToString().ToLower();
                    else
                        return access.ToString();
            }
        }

        public override string GetFieldModifierString(FieldModifier modifier, bool forCode)
        {
            if (modifier == FieldModifier.None) {
                if (forCode)
                    return "";
                else
                    return "None";
            }

            StringBuilder builder = new StringBuilder(30);
            if ((modifier & FieldModifier.Constant) != 0)
                builder.Append(forCode ? "const " : "const, ");
            if ((modifier & FieldModifier.Static) != 0)
                builder.Append(forCode ? "static " : "static, ");

            if (forCode)
                builder.Remove(builder.Length - 1, 1);
            else
                builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }

        public override string GetOperationModifierString(OperationModifier modifier, bool forCode)
        {
            if (modifier == OperationModifier.None) {
                if (forCode)
                    return "";
                else
                    return "None";
            }

            StringBuilder builder = new StringBuilder(30);
            if ((modifier & OperationModifier.Static) != 0)
                builder.Append(forCode ? "static " : "static, ");
            if ((modifier & OperationModifier.Abstract) != 0)
                builder.Append(forCode ? "abstract " : "abstract, ");
            if ((modifier & OperationModifier.Factory) != 0)
                builder.Append(forCode ? "factory " : "factory, ");
            if ((modifier & OperationModifier.Override) != 0)
                builder.Append(forCode ? "override " : "override, ");

            if (forCode)
                builder.Remove(builder.Length - 1, 1);
            else
                builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }

        public override string GetClassModifierString(ClassModifier modifier, bool forCode)
        {
            if (!forCode)
                return modifier.ToString();
            else if (modifier == ClassModifier.None)
                return "";
            else
                return modifier.ToString().ToLower();
        }

        protected override Package CreatePackage()
        {
            return new DartNamespace();
        }

        protected override ClassType CreateClass()
        {
            return new DartClass();
        }

        protected override StructureType CreateStructure()
        {
            return null;
        }

        protected override InterfaceType CreateInterface()
        {
            return new DartInterface();
        }

        protected override EnumType CreateEnum()
        {
            return new DartEnum();
        }

        protected override DelegateType CreateDelegate()
        {
            return null;
        }

        protected override ArgumentList CreateParameterCollection()
        {
            return new DartArgumentList();
        }

        public override bool SupportsAssemblyImport
        {
            get { return false; }
        }

        public override bool SupportsInterfaces
        {
            get { return true; }
        }

        public override bool SupportsStructures
        {
            get { return false; }
        }

        public override bool SupportsEnums
        {
            get { return true; }
        }

        public override bool SupportsDelegates
        {
            get { return false; }
        }

        public override bool SupportsExplicitImplementation
        {
            get { return true; }
        }

        public override bool ExplicitVirtualMethods
        {
            get { return false; }
        }
    }
}
