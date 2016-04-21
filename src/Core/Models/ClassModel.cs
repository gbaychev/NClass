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
using System.IO;
using System.Xml;
using NClass.Translations;

namespace NClass.Core.Models
{
    public class ClassModel : Model
    {
        Language language;

        public ClassModel()
        {
            
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="language"/> is null.
        /// </exception>
        public ClassModel(Language language)
        {
            if (language == null)
                throw new ArgumentException("Language cannot be null");

            this.language = language;
        }

        public Language Language
        {
            get { return language; }
        }

        public ClassType AddClass()
        {
            ClassType newClass = Language.CreateClass();
            AddEntity(newClass);
            return newClass;
        }

        /// <exception cref="InvalidOperationException">
        /// The language does not support structures.
        /// </exception>
        public StructureType AddStructure()
        {
            StructureType structure = Language.CreateStructure();
            AddEntity(structure);
            return structure;
        }

        public InterfaceType AddInterface()
        {
            InterfaceType newInterface = Language.CreateInterface();
            AddEntity(newInterface);
            return newInterface;
        }

        public EnumType AddEnum()
        {
            EnumType newEnum = Language.CreateEnum();
            AddEntity(newEnum);
            return newEnum;
        }


        /// <exception cref="InvalidOperationException">
        /// The language does not support delegates.
        /// </exception>
        public DelegateType AddDelegate()
        {
            DelegateType newDelegate = Language.CreateDelegate();
            AddEntity(newDelegate);
            return newDelegate;
        }
        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public AssociationRelationship AddAssociation(TypeBase first, TypeBase second)
        {
            AssociationRelationship association = new AssociationRelationship(first, second);
            AddRelationship(association);
            return association;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public AssociationRelationship AddComposition(TypeBase first, TypeBase second)
        {
            AssociationRelationship composition = new AssociationRelationship(
                first, second, AssociationType.Composition);

            AddRelationship(composition);
            return composition;
        }

        /// <exception cref="RelationshipException">
        /// Cannot create relationship between the two types.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="derivedType"/> or <paramref name="baseType"/> is null.
        /// </exception>
        public GeneralizationRelationship AddGeneralization(CompositeType derivedType,
            CompositeType baseType)
        {
            GeneralizationRelationship generalization =
                new GeneralizationRelationship(derivedType, baseType);

            AddRelationship(generalization);
            return generalization;
        }

        /// <exception cref="RelationshipException">
        /// Cannot create relationship between the two types.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="implementer"/> or <paramref name="baseType"/> is null.
        /// </exception>
        public RealizationRelationship AddRealization(TypeBase implementer,
            InterfaceType baseType)
        {
            RealizationRelationship realization = new RealizationRelationship(
                implementer, baseType);

            AddRelationship(realization);
            return realization;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public DependencyRelationship AddDependency(TypeBase first, TypeBase second)
        {
            DependencyRelationship dependency = new DependencyRelationship(first, second);

            AddRelationship(dependency);
            return dependency;
        }

        /// <exception cref="RelationshipException">
        /// Cannot create relationship between the two types.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parentType"/> or <paramref name="innerType"/> is null.
        /// </exception>
        public NestingRelationship AddNesting(CompositeType parentType, TypeBase innerType)
        {
            NestingRelationship nesting = new NestingRelationship(parentType, innerType);

            AddRelationship(nesting);
            return nesting;
        }
        
        /// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		protected override void LoadRelationships(XmlNode root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            XmlNodeList nodeList = root.SelectNodes(
                "Relationships/Relationship|Relations/Relation"); // old file format

            foreach (XmlElement node in nodeList)
            {
                string type = node.GetAttribute("type");
                string firstString = node.GetAttribute("first");
                string secondString = node.GetAttribute("second");
                int firstIndex, secondIndex;

                if (!int.TryParse(firstString, out firstIndex) ||
                    !int.TryParse(secondString, out secondIndex))
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
                        case "Association":
                            relationship = AddAssociation(first as TypeBase, second as TypeBase);
                            break;

                        case "Generalization":
                            relationship = AddGeneralization(
                                first as CompositeType, second as CompositeType);
                            break;

                        case "Realization":
                            relationship = AddRealization(first as TypeBase, second as InterfaceType);
                            break;

                        case "Dependency":
                            relationship = AddDependency(first as TypeBase, second as TypeBase);
                            break;

                        case "Nesting":
                            relationship = AddNesting(first as CompositeType, second as TypeBase);
                            break;

                        case "Comment":
                        case "CommentRelationship": // Old file format
                            if (first is Comment)
                                relationship = AddCommentRelationship(first as Comment, second);
                            else
                                relationship = AddCommentRelationship(second as Comment, first);
                            break;

                        default:
                            throw new InvalidDataException(
                                Strings.ErrorCorruptSaveFormat);
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

        protected override IEntity GetEntity(string type)
        {
            switch (type)
            {
                case "Class":
                case "CSharpClass":     // Old file format
                case "JavaClass":       // Old file format
                    return AddClass();

                case "Structure":
                case "StructType":      // Old file format
                    return AddStructure();

                case "Interface":
                case "CSharpInterface": // Old file format
                case "JavaInterface":   // Old file format
                    return AddInterface();

                case "Enum":
                case "CSharpEnum":      // Old file format
                case "JavaEnum":        // Old file format
                    return AddEnum();

                case "Delegate":
                case "DelegateType":    // Old file format
                    return AddDelegate();

                case "Comment":
                    return AddComment();

                default:
                    throw new InvalidDataException("Invalid entity type: " + type);
            }
        }

        public override void Serialize(XmlElement node)
        {
            XmlElement languageElement = node.OwnerDocument.CreateElement("Language");
            languageElement.InnerText = Language.AssemblyName;
            node.AppendChild(languageElement);

            base.Serialize(node);
        }
    }
}