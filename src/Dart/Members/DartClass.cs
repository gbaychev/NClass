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
using System.Collections.Generic;
using NClass.Core;
using NClass.Core.Entities;
using NClass.Translations;

namespace NClass.Dart
{
    internal sealed class DartClass : ClassType
    {
        internal DartClass() : this("NewClass")
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal DartClass(string name) : base(name)
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The type visibility is not valid in the current context.
        /// </exception>
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
                    value == AccessModifier.Private ||
                    value == AccessModifier.Public)
                {
                    base.AccessModifier = value;
                }
            }
        }

        /// <exception cref="RelationshipException">
        /// The language of <paramref name="value"/> does not equal.-or-
        /// <paramref name="value"/> is static or sealed class.-or-
        /// The <paramref name="value"/> is descendant of the class.
        /// </exception>
        public override ClassType BaseClass
        {
            get
            {
                if (base.BaseClass == null && this != DartLanguage.ObjectClass)
                    return DartLanguage.ObjectClass;
                else
                    return base.BaseClass;
            }
            set
            {
                base.BaseClass = value;
            }
        }

        public override AccessModifier DefaultAccess
        {
            get { return AccessModifier.Public; }
        }

        public override AccessModifier DefaultMemberAccess
        {
            get { return AccessModifier.Private; }
        }

        public override bool SupportsProperties
        {
            get { return true; }
        }

        public override bool SupportsEvents
        {
            get { return false; }
        }

        public override bool SupportsDestructors
        {
            get { return false; }
        }

        public override bool SupportsConstuctors
        {
            get
            {
                // No constructors for Mixins
                if (Modifier == ClassModifier.Mixin)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
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

        /// <exception cref="RelationshipException">
        /// The language of <paramref name="interfaceType"/> does not equal.-or-
        /// <paramref name="interfaceType"/> is earlier implemented interface.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="interfaceType"/> is null.
        /// </exception>
        public override void AddInterface(InterfaceType interfaceType)
        {
            if (!(interfaceType is DartInterface))
                throw new RelationshipException(string.Format(Strings.ErrorInterfaceLanguage, "Dart"));

            base.AddInterface(interfaceType);
        }

        public override Field AddField()
        {
            Field field = new DartField(this);

            AddField(field);
            return field;
        }

        public override Constructor AddConstructor()
        {
            Constructor constructor = new DartConstructor(this);

            AddOperation(constructor);
            return constructor;
        }

        public override Destructor AddDestructor() { return null; }

        public override Method AddMethod()
        {
            Method method = new DartMethod(this);

            method.AccessModifier = AccessModifier.Public;
            method.IsStatic = (Modifier == ClassModifier.Static);
            method.IsAbstract = (Modifier == ClassModifier.Abstract);

            AddOperation(method);
            return method;
        }

        public override Property AddProperty()
        {
            Property property = new DartProperty(this);

            property.AccessModifier = AccessModifier.Public;
            property.IsStatic = (Modifier == ClassModifier.Static);
            property.IsAbstract = (Modifier == ClassModifier.Abstract);

            AddOperation(property);
            return property;
        }

        public override Event AddEvent()
        {
            return null;
        }

        public override string GetDeclaration()
        {
            StringBuilder builder = new StringBuilder();
            var className = Name;

            if (AccessModifier == AccessModifier.Private)
            {
                className = "_" + className;
            }
            if (Modifier != ClassModifier.None)
            {
                if (Modifier == ClassModifier.Abstract)
                {
                    builder.Append(Language.GetClassModifierString(Modifier, true));
                    builder.Append(" ");
                    builder.AppendFormat("class {0}", className);
                }
                else if (Modifier == ClassModifier.Mixin)
                {
                    builder.AppendFormat("mixin {0}", className);
                }
            }
            else
            {
                builder.AppendFormat("class {0}", className);
            }

            // Check for a base class
            if (HasExplicitBase)
            {
                builder.Append(" extends " + BaseClass.Name);
            }
            // Check for interfaces and mixins
            if (InterfaceList.Count > 0)
            {
                // Separate into interfaces(implements) and mixins(with)
                var mixins = new List<ClassType>();
                var interfaces = new List<InterfaceType>();
             
                foreach(var i in InterfaceList)
                {
                    if (i is ClassType)
                    {
                        mixins.Add(i as ClassType);
                    }
                    else
                    {
                        interfaces.Add(i as InterfaceType);
                    }
                }

                // Mixins first
                if (mixins.Count > 0)
                {
                    builder.Append(" with ");

                    for (int j = 0; j < mixins.Count; j++)
                    {
                        builder.Append(mixins[j].Name);
                        if (j < mixins.Count - 1)
                            builder.Append(", ");
                    }
                }

                // Then interfaces
                if (interfaces.Count > 0)
                {
                    builder.Append(" implements ");

                    for (int j = 0; j < interfaces.Count; j++)
                    {
                            builder.Append(interfaces[j].Name);
                            if (j < interfaces.Count - 1)
                                builder.Append(", ");
                    }
                }
            }

            return builder.ToString();
        }

        public override ClassType Clone()
        {
            DartClass newClass = new DartClass();
            newClass.CopyFrom(this);
            return newClass;
        }

        public override INestableChild CloneChild()
        {
            return Clone();
        }
    }
}
