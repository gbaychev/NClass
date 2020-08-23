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
    internal sealed class JavaConstructor : Constructor
    {
        /// <exception cref="ArgumentException">
        /// The language of <paramref name="parent"/> does not equal.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent"/> is null.
        /// </exception>
        internal JavaConstructor(CompositeType parent) : base(parent)
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="value"/> does not fit to the syntax.
        /// </exception>
        public sealed override string Name
        {
            get
            {
                return GetNameWithoutGeneric(Parent.Name);
            }
            set
            {
                if (value != null && value != GetNameWithoutGeneric(Parent.Name))
                    throw new BadSyntaxException(Strings.ErrorConstructorName);
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
                if (value)
                    throw new BadSyntaxException(Strings.ErrorCannotSetStatic);
            }
        }

        /// <exception cref="BadSyntaxException">
        /// Cannot set sealed modifier.
        /// </exception>
        public override bool IsSealed
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                    throw new BadSyntaxException(Strings.ErrorCannotSetModifier);
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
            InitFromDeclaration(JavaConstructorDeclaration.Create(declaration));
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="declaration"/> does not fit to the syntax.
        /// </exception>
        public override void InitFromDeclaration(IMethodDeclaration declaration)
        {
            if (declaration is IJavaConstructorDeclaration javaDeclaration) 
            {
                InitFromDeclaration(javaDeclaration);
            }
            else {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }
        }

        public override string GetDeclaration()
        {
            StringBuilder builder = new StringBuilder(50);

            if (AccessModifier != AccessModifier.Default) {
                builder.Append(Language.GetAccessString(AccessModifier, true));
                builder.Append(" ");
            }

            builder.AppendFormat("{0}(", Name);

            for (int i = 0; i < ArgumentList.Count; i++) {
                builder.Append(ArgumentList[i]);
                if (i < ArgumentList.Count - 1)
                    builder.Append(", ");
            }
            builder.Append(")");

            return builder.ToString();
        }

        public override Operation Clone(CompositeType newParent)
        {
            JavaConstructor constructor = new JavaConstructor(newParent);
            constructor.CopyFrom(this);
            return constructor;
        }

        private void InitFromDeclaration(IJavaConstructorDeclaration declaration)
        {
            RaiseChangedEvent = false;

            try {
                ValidName = declaration.Name;
                AccessModifier = declaration.AccessModifier;
                ArgumentList.InitFromDeclaration(declaration.ArgumentList);
            }
            finally {
                RaiseChangedEvent = true;
            }
        }
    }
}
