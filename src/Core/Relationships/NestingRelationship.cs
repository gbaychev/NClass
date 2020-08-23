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
using NClass.Translations;

namespace NClass.Core
{   
    public sealed class NestingRelationship : Relationship
    {
        INestable first;
        INestableChild second;

        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> is null.-or-
        /// <paramref name="second"/> is null.
        /// </exception>
        internal NestingRelationship(INestable first, INestableChild second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            this.first = first;
            this.second = second;
            Attach();
        }

        //TODO: inkább abstract property-kre hivatkozzon
        public sealed override IEntity First
        {
            get { return first; }
            protected set { first = (INestable)value; }
        }

        public sealed override IEntity Second
        {
            get { return second; }
            protected set { second = (INestableChild)value; }
        }

        public override RelationshipType RelationshipType
        {
            get { return RelationshipType.Nesting; }
        }

        private INestable ParentType
        {
            get { return (INestable)First; }
        }

        private INestableChild InnerType
        {
            get { return (INestableChild)Second; }
        }

        public NestingRelationship Clone(INestable parentType, INestableChild innerType)
        {
            NestingRelationship nesting = new NestingRelationship(parentType, innerType);
            nesting.CopyFrom(this);
            return nesting;
        }

        /// <exception cref="RelationshipException">
        /// Cannot finalize relationship.
        /// </exception>
        protected override void OnAttaching(EventArgs e)
        {
            if (InnerType.NestingParent != null)
                throw new RelationshipException(Strings.ErrorInnerTypeAlreadyNested);

            if (ParentType == InnerType)
                throw new RelationshipException(Strings.ErrorRecursiveNesting);

            if (ParentType.IsNestedAncestor(InnerType))
                throw new RelationshipException(Strings.ErrorCyclicNesting);

            if(ParentType.EntityType != EntityType.Package && InnerType.EntityType == EntityType.Package)
                throw new RelationshipException(Strings.ErrorPackageParentNotPackage);

            InnerType.NestingParent = ParentType;
        }

        protected override void OnDetaching(EventArgs e)
        {
            InnerType.NestingParent = null;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} (+)--> {2}",
                Strings.Nesting, First.Name, Second.Name);
        }
    }
}
