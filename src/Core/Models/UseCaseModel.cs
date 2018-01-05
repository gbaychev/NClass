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

using System.IO;
using System.Xml;

namespace NClass.Core.Models
{
    public class UseCaseModel : Model
    {
        protected override IEntity GetEntity(string type)
        {
            switch (type)
            {
                case "Actor":
                    return AddActor();

                case "UseCase":
                    return AddUseCase();

                case "Comment":
                    return AddComment();

                default:
                    throw new InvalidDataException("Invalid entity type: " + type);
            }
        }

        protected override void LoadRelationships(XmlNode root)
        {
            // FIXME - removed the not implemented exception
			// in order to test the loading of entities
        }

        public Actor AddActor()
        {
            var actor = new Actor();
            AddEntity(actor);
            return actor;
        }

        public UseCase AddUseCase()
        {
            var useCase = new UseCase();
            AddEntity(useCase);
            return useCase;
        }

        public ExtendsRelationship AddExtends(UseCase first, UseCase second)
        {
            var extendsRelationship = new ExtendsRelationship(first, second);
            AddRelationship(extendsRelationship);
            return extendsRelationship;
        }

        public IncludesRelationship AddIncludes(UseCase first, UseCase second)
        {
            var includesRelationship = new IncludesRelationship(first, second);
            AddRelationship(includesRelationship);
            return includesRelationship;
        }

        public UseCaseAssociation AddAssocation(IUseCaseEntity first, IUseCaseEntity second)
        {
            var assocationRelationship = new UseCaseAssociation(first, second);
            AddRelationship(assocationRelationship);
            return assocationRelationship;
        }

        public void AddGeneralization(IUseCaseEntity first, IUseCaseEntity second)
        {
            var generalization = new UseCaseGeneralization(first, second);
            AddRelationship(generalization);
        }
    }
}