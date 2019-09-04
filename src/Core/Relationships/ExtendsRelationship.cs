// NClass - Free class diagram editor
// Copyright (C) 2016 Georgi Baychev
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
    /// <summary>
    /// Represents a use case extends type relationship
    /// </summary>
    public sealed class ExtendsRelationship : UseCaseRelationship
    {
        /// <param name="first">The extending use case</param>
        /// <param name="second">The extended use case</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> is null.-or-
        /// <paramref name="second"/> is null.
        /// </exception>
        public ExtendsRelationship(UseCase first, UseCase second)
            : base(first, second)
        {
            this.label = "<extends>";
        }

        public override bool SupportsLabel => true;

        public override RelationshipType RelationshipType => RelationshipType.Extension;

        public override string ToString()
        {
            var firstName = string.IsNullOrEmpty(first.Name) ? "<use case>" : first.Name;
            var secondName = string.IsNullOrEmpty(second.Name) ? "<use case>" : second.Name;
            return $"{firstName} -<E>-> {secondName}";
        }

        public ExtendsRelationship Clone(UseCase firstUseCase, UseCase secondUseCase)
        {
            var extendsRelationship = new ExtendsRelationship(firstUseCase, secondUseCase);
            extendsRelationship.CopyFrom(this);
            return extendsRelationship;
        }

        protected override void OnAttaching(EventArgs e)
        {
            base.OnAttaching(e);
            if (first.ExtendedUseCase == second ||
                second.ExtendedUseCase == first)
            {
                throw new RelationshipException(Strings.ErrorUseCaseExtendAlreadyExists);
            }

            first.ExtendedUseCase = second;
        }

        protected override void OnDetaching(EventArgs e)
        {
            base.OnDetaching(e);

            first.ExtendedUseCase = null;
        }
    }
}