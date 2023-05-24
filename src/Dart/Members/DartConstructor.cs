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

namespace NClass.Dart
{
    internal sealed class DartConstructor : Constructor
    {
        /// <exception cref="ArgumentException">
        /// The language of <paramref name="parent"/> does not equal.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent"/> is null.
        /// </exception>
        internal DartConstructor(CompositeType parent) : base(parent)
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="value"/> does not fit to the syntax.
        /// </exception>
        public override string Name
        {
            get
            {
                return GetNameWithoutGeneric(name != null ? name : Parent.Name);
            }
            set
            {
                if (value == null || string.Equals(value, GetNameWithoutGeneric(name ?? Parent.Name),
                        StringComparison.Ordinal))
                {
                    return;
                }

                throw new BadSyntaxException(Strings.ErrorConstructorName);
            }
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
                try {
                    RaiseChangedEvent = false;

                    if (value != AccessModifier.Default)
                        IsStatic = false;
                    base.AccessModifier = value;
                }
                finally {
                    RaiseChangedEvent = true;
                }
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
                if (value && HasParameter)
                    throw new BadSyntaxException(Strings.ErrorStaticConstructor);

                try {
                    RaiseChangedEvent = false;

                    if (value)
                        AccessModifier = AccessModifier.Default;
                    base.IsStatic = value;
                }
                finally {
                    RaiseChangedEvent = true;
                }
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
            get { return DartLanguage.Instance; }
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="declaration"/> does not fit to the syntax.
        /// </exception>
        public override void InitFromString(string declaration)
        {
            InitFromDeclaration(DartConstructorDeclaration.Create(declaration));
        }
                
        public override void InitFromDeclaration(IMethodDeclaration declaration)
        {
            if (declaration is IDartConstructorDeclaration dartDeclaration)
            {
                InitFromDeclaration(dartDeclaration);
            }
            else
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }
        }

        public override string GetDeclaration()
        {
            StringBuilder builder = new StringBuilder(50);
            var constructorName = Name;

            if (AccessModifier == AccessModifier.Private)
            {
                constructorName = "_" + constructorName;
            }

            if (IsStatic) {
                builder.Append("static ");
            }

            if (IsFactory)
            {
                builder.Append("factory ");
            }

            builder.AppendFormat("{0}(", constructorName);

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
            DartConstructor constructor = new DartConstructor(newParent);
            constructor.CopyFrom(this);
            return constructor;
        }

        private void InitFromDeclaration(IDartConstructorDeclaration declaration)
        {
            RaiseChangedEvent = false;

            try {
                ClearModifiers();
                
                ValidName = declaration.Name;
                AccessModifier = declaration.AccessModifier;
                ArgumentList.InitFromDeclaration(declaration.ArgumentList);
                IsStatic = declaration.IsStatic;
                IsFactory = declaration.IsFactory;
            }
            finally {
                RaiseChangedEvent = true;
            }
        }
    }
}
