﻿// NClass - Free class diagram editor
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

namespace NClass.Dart
{
    internal sealed class DartEnum : EnumType
    {
        internal DartEnum() : this("NewEnum")
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal DartEnum(string name) : base(name)
        {
        }

        public override AccessModifier AccessModifier
        {
            get
            {
                return base.AccessModifier;
            }
            set
            {
                if (IsTypeNested ||
                    value == AccessModifier.Default ||
                    value == AccessModifier.Public)
                {
                    base.AccessModifier = value;
                }
            }
        }

        public override AccessModifier DefaultAccess
        {
            get { return AccessModifier.Public; }
        }

        public override Language Language
        {
            get { return DartLanguage.Instance; }
        }

        /// <exception cref="ArgumentException">
        /// The <paramref name="value"/> is already a child member of the type.
        /// </exception>
        public override INestable NestingParent
        {
            get
            {
                return base.NestingParent;
            }

            set
            {
                try
                {
                    RaiseChangedEvent = false;

                    base.NestingParent = value;
                    if (NestingParent == null && Access != AccessModifier.Public)
                        AccessModifier = AccessModifier.Internal;
                }
                finally
                {
                    RaiseChangedEvent = true;
                }
            }
        }

        /// <exception cref="BadSyntaxException">
        /// The name does not fit to the syntax.
        /// </exception>
        /// <exception cref="ReservedNameException">
        /// The name is a reserved name.
        /// </exception>
        public override EnumValue AddValue(string declaration)
        {
            EnumValue newValue = new DartEnumValue(declaration);

            AddValue(newValue);
            return newValue;
        }

        /// <exception cref="BadSyntaxException">
        /// The name does not fit to the syntax.
        /// </exception>
        /// <exception cref="ReservedNameException">
        /// The name is a reserved name.
        /// </exception>
        public override EnumValue ModifyValue(EnumValue value, string declaration)
        {
            EnumValue newValue = new DartEnumValue(declaration);

            ChangeValue(value, newValue);
            return value;
        }

        public override string GetDeclaration()
        {
            StringBuilder builder = new StringBuilder();
            var enumName = Name;

            if (AccessModifier == AccessModifier.Private)
            {
                enumName = "_" + enumName;
            }

            builder.AppendFormat("enum {0}", enumName);

            return builder.ToString();
        }

        public override EnumType Clone()
        {
            DartEnum newEnum = new DartEnum();
            newEnum.CopyFrom(this);
            return newEnum;
        }

        public override INestableChild CloneChild()
        {
            return Clone();
        }
    }
}
