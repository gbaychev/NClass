// // NClass - Free class diagram editor
// // Copyright (C) 2016 Georgi Baychev
// // 
// // This program is free software; you can redistribute it and/or modify it under 
// // the terms of the GNU General Public License as published by the Free Software 
// // Foundation; either version 3 of the License, or (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful, but WITHOUT 
// // ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// // FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with 
// // this program; if not, write to the Free Software Foundation, Inc., 
// // 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

namespace NClass.Core
{
    public abstract class UseCaseRelationship : Relationship
    {
        protected UseCase first;
        protected UseCase second;

        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> is null.-or-
        /// <paramref name="second"/> is null.
        /// </exception>
        public UseCaseRelationship(UseCase first, UseCase second)
        {
            this.First = first ?? throw new ArgumentException(nameof(first));
            this.Second = second ?? throw new ArgumentException(nameof(second));

            Attach();
        }

        public override IEntity First
        {
            get { return first; }
            protected set { first = (UseCase)value; }
        }

        public override IEntity Second
        {
            get { return second; }
            protected set { second = (UseCase)value; }
        }
    }
}