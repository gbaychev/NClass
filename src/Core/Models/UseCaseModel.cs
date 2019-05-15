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
using System.IO;
using System.Xml;
using NClass.Translations;

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

                case "SystemBoundary":
                    return AddSystemBoundary();

                default:
                    throw new InvalidDataException("Invalid entity type: " + type);
            }
        }

        protected override void LoadRelationships(XmlNode root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            var nodeList = root.SelectNodes("Relationships/Relationship");
            foreach (XmlElement node in nodeList)
            {
                var type = node.GetAttribute("type");
                var firstString = node.GetAttribute("first");
                var secondString = node.GetAttribute("second");

                if (!int.TryParse(firstString, out var firstIndex) ||
                    !int.TryParse(secondString, out var secondIndex))
                {
                    throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
                }

                if (firstIndex < 0 || firstIndex >= entities.Count ||
                    secondIndex < 0 || secondIndex >= entities.Count)
                {
                    throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
                }

                try
                {
                    IEntity first = entities[firstIndex];
                    IEntity second = entities[secondIndex];
                    Relationship relationship;

                    switch (type)
                    {
                        case "UseCaseAssociation":
                            relationship = AddAssocation(first as IUseCaseEntity, second as IUseCaseEntity);
                            break;

                        case "UseCaseGeneralization":
                            relationship = AddGeneralization(first as IUseCaseEntity, second as IUseCaseEntity);
                            break;

                        case "Extension":
                            relationship = AddExtends(first as UseCase, second as UseCase);
                            break;

                        case "Inclusion":
                            relationship = AddIncludes(first as UseCase, second as UseCase);
                            break;

                        case "Comment":
                            if (first is Comment)
                                relationship = AddCommentRelationship(first as Comment, second);
                            else
                                relationship = AddCommentRelationship(second as Comment, first);
                            break;

                        default:
                            throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
                    }

                    relationship.Deserialize(node);
                }
                catch (ArgumentNullException ex)
                {
                    throw new InvalidDataException("Invalid relationship.", ex);
                }
                catch (RelationshipException ex)
                {
                    throw new InvalidDataException("Invalid relationship.", ex);
                }
            }
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

        public SystemBoundary AddSystemBoundary()
        {
            var systemBoundary = new SystemBoundary();
            AddEntity(systemBoundary);
            return systemBoundary;
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

        public UseCaseGeneralization AddGeneralization(IUseCaseEntity first, IUseCaseEntity second)
        {
            var generalization = new UseCaseGeneralization(first, second);
            AddRelationship(generalization);
            return generalization;
        }

        
    }
}