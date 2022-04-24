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
using System.Text;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    internal sealed class CSharpField : Field
    {
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent"/> is null.
        /// </exception>
        internal CSharpField(CompositeType parent) : this("newField", parent)
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
        internal CSharpField(string name, CompositeType parent) : base(name, parent)
        {
            IsConstant = false;
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

        /// <exception cref="BadSyntaxException">
        /// <see cref="IsConst"/> is set to true while 
        /// <see cref="Field.InitialValue"/> is empty.
        /// </exception>
        public override bool IsConstant
        {
            get
            {
                return base.IsConstant;
            }
            set
            {
                if (value && !HasInitialValue) {
                    throw new BadSyntaxException(
                        Strings.ErrorLackOfInitialization);
                }
                base.IsConstant = value;
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
            InitFromDeclaration(CSharpFieldDeclaration.Create(declaration));
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="declaration"/> does not fit to the syntax.
        /// </exception>
        public override void InitFromDeclaration(IFieldDeclaration declaration)
        {
            if (declaration is ICSharpFieldDeclaration sharpDeclaration)
            {
                InitFromDeclaration(sharpDeclaration);
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
            StringBuilder builder = new StringBuilder(50);

            if (AccessModifier != AccessModifier.Default) {
                builder.Append(Language.GetAccessString(AccessModifier, true));
                builder.Append(" ");
            }

            if (IsHider)
                builder.Append("new ");
            if (IsConstant)
                builder.Append("const ");
            if (IsStatic)
                builder.Append("static ");
            if (IsReadonly)
                builder.Append("readonly ");
            if (IsVolatile)
                builder.Append("volatile ");

            builder.AppendFormat("{0} {1}", Type, Name);
            if (HasInitialValue)
                builder.AppendFormat(" = {0}", InitialValue);

            if (withSemicolon)
                builder.Append(";");

            return builder.ToString();
        }

        protected override Field Clone(CompositeType newParent)
        {
            CSharpField field = new CSharpField(newParent);
            field.CopyFrom(this);
            return field;
        }

        public override string ToString()
        {
            return GetDeclarationLine(false);
        }

        private void InitFromDeclaration(ICSharpFieldDeclaration declaration)
        {
            RaiseChangedEvent = false;

            try {
                ClearModifiers();
                
                if (CSharpLanguage.Instance.IsForbiddenName(declaration.Name))
                    throw new BadSyntaxException(Strings.ErrorInvalidName);
                if (CSharpLanguage.Instance.IsForbiddenTypeName(declaration.Type))
                    throw new BadSyntaxException(Strings.ErrorInvalidTypeName);
                if (declaration.Type == "void")
                    throw new BadSyntaxException(string.Format(Strings.ErrorType, "void"));

                ValidName = declaration.Name;
                ValidType = declaration.Type;
                AccessModifier = declaration.AccessModifier;
                InitialValue = (declaration.HasInitialValue) ? declaration.InitialValue : null;

                IsStatic = declaration.IsStatic;
                IsReadonly = declaration.IsReadonly;
                IsConstant = declaration.IsConstant;
                IsHider = declaration.IsHider;
                IsVolatile = declaration.IsVolatile;
            }
            finally {
                RaiseChangedEvent = true;
            }
        }
    }
}
