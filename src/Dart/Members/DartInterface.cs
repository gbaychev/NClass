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

namespace NClass.Dart
{
    internal sealed class DartInterface : InterfaceType
    {
        internal DartInterface() : this("NewInterface")
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal DartInterface(string name) : base(name)
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

        public override AccessModifier DefaultMemberAccess
        {
            get { return AccessModifier.Public; }
        }

        public override bool SupportsProperties
        {
            get { return true; }
        }

        public override bool SupportsEvents
        {
            get { return false; }
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

        public override Language Language
        {
            get { return DartLanguage.Instance; }
        }

        public override Method AddMethod()
        {
            Method method = new DartMethod(this);

            AddOperation(method);
            return method;
        }

        public override Property AddProperty()
        {
            Property property = new DartProperty(this);

            AddOperation(property);
            return property;
        }

        public override Event AddEvent()
        {
            return null;
        }

        public override string GetDeclaration()
        {
            StringBuilder builder = new StringBuilder(30);
            var interfaceName = Name;

            if (AccessModifier == AccessModifier.Private)
            {
                interfaceName += "_";
            }

            builder.AppendFormat("abstract class {0}", interfaceName);

            if (HasExplicitBase)
            {
                builder.Append(" extends ");
                for (int i = 0; i < BaseList.Count; i++)
                {
                    if (BaseList[i].AccessModifier != AccessModifier.Private) {
                        builder.Append(BaseList[i].Name);
                    }
                    else
                    {
                        builder.Append("_");
                        builder.Append(BaseList[i].Name);
                    }
                    if (i < BaseList.Count - 1)
                        builder.Append(", ");
                }
            }

            return builder.ToString();
        }

        public override InterfaceType Clone()
        {
            DartInterface newInterface = new DartInterface();
            newInterface.CopyFrom(this);
            return newInterface;
        }

        public override INestableChild CloneChild()
        {
            return Clone();
        }
    }
}
