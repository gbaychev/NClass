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

using NClass.Core;
using NClass.Translations;
using System;
using System.Text;

namespace NClass.Java
{
    internal sealed class JavaField : Field
    {
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent"/> is null.
        /// </exception>
        internal JavaField(CompositeType parent) : this("newField", parent)
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
        internal JavaField(string name, CompositeType parent) : base(name, parent)
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
            set
            {
                if (value == "void")
                    throw new BadSyntaxException(string.Format(Strings.ErrorType, "void"));

                base.Type = value;
            }
        }

        protected override string DefaultType
        {
            get { return "int"; }
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
                    throw new BadSyntaxException(
                        Strings.ErrorInterfaceMemberAccess);
                }

                base.AccessModifier = value;
            }
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
            InitFromDeclaration(JavaFieldDeclaration.Create(declaration));
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="declaration"/> does not fit to the syntax.
        /// </exception>
        public override void InitFromDeclaration(IFieldDeclaration declaration)
        {
            if (declaration is IJavaFieldDeclaration javaDeclaration) 
            {
                InitFromDeclaration(javaDeclaration);
            }
            else {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }
        }

        public override string GetDeclaration()
        {
            return GetDeclarationLine(true);
        }

        public string GetDeclarationLine(bool withSemicolon)
        {
            StringBuilder builder = new StringBuilder(50);

            if (AccessModifier != AccessModifier.Default) {
                builder.Append(Language.GetAccessString(AccessModifier, true));
                builder.Append(" ");
            }

            if (IsStatic)
                builder.Append("static ");
            if (IsReadonly)
                builder.Append("final ");

            builder.AppendFormat("{0} {1}", Type, Name);
            if (HasInitialValue)
                builder.AppendFormat(" = {0}", InitialValue);

            if (withSemicolon)
                builder.Append(";");

            return builder.ToString();
        }

        protected override Field Clone(CompositeType newParent)
        {
            JavaField field = new JavaField(newParent);
            field.CopyFrom(this);
            return field;
        }

        public override string ToString()
        {
            return GetDeclarationLine(false);
        }

        private void InitFromDeclaration(IJavaFieldDeclaration declaration)
        {
            RaiseChangedEvent = false;

            try {
                if (JavaLanguage.Instance.IsForbiddenName(declaration.Name))
                    throw new BadSyntaxException(Strings.ErrorInvalidName);
                if (JavaLanguage.Instance.IsForbiddenTypeName(declaration.Type))
                    throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
                if (declaration.Type == "void")
                    throw new BadSyntaxException(string.Format(Strings.ErrorType, "void"));

                ValidName = declaration.Name;
                ValidType = declaration.Type;
                AccessModifier = declaration.AccessModifier;
                InitialValue = declaration.InitialValue;

                IsStatic = declaration.IsStatic;
                IsReadonly = declaration.IsReadonly;
                IsVolatile = declaration.IsVolatile;
            }
            finally {
                RaiseChangedEvent = true;
            }
        }
    }
}
