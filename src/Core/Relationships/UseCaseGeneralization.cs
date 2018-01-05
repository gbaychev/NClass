// NClass - Free class diagram editor
// Copyright (C) 2017 Georgi Baychev
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

namespace NClass.Core
{
    public class UseCaseGeneralization : Relationship
    {
        private IUseCaseEntity first;
        private IUseCaseEntity second;

        public  UseCaseGeneralization(IUseCaseEntity first, IUseCaseEntity second)
        {
            this.first = first ?? throw new ArgumentNullException(nameof(first));
            this.second = second ?? throw new ArgumentNullException(nameof(second));
        }
        
        public sealed override IEntity First
        {
            get => first;
            protected set
            {
                if (value.EntityType != EntityType.Actor &&
                    value.EntityType != EntityType.UseCase)
                {
                    throw new ArgumentException("The entity should be either an actor or a use case");
                }
                first = (IUseCaseEntity)value;
            }
        }

        public sealed override IEntity Second
        {
            get => second;
            protected set
            {
                if (value.EntityType != EntityType.Actor &&
                    value.EntityType != EntityType.UseCase)
                {
                    throw new ArgumentException("The entity should be either an actor or a use case");
                }
                second = (IUseCaseEntity) value;
            }
        }

        public override RelationshipType RelationshipType => RelationshipType.UseCaseGeneralization;
        public override string ToString()
        {
            return $"{first.Name} --- {second.Name}";
        }

        public UseCaseGeneralization Clone(IUseCaseEntity first, IUseCaseEntity second)
        {
            var generalization = new UseCaseGeneralization(first, second);
            generalization.CopyFrom(this);
            return generalization;
        }
    }
}