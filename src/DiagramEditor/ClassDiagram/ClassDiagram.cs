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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.Core.Models;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram
{
    public class ClassDiagram : Diagram<ClassModel>
    {
        protected ClassDiagram()
        {
            diagramContextMenu = ClassDiagramBlankContextMenu.Default;
            diagramDynamicMenu = ClassDiagramDynamicMenu.Default;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="language"/> is null.
        /// </exception>
        public ClassDiagram(Language language) : this()
        {
            model = new ClassModel(language);
            model.EntityRemoved += OnEntityRemoved;
            model.EntityAdded += OnEntityAdded;
            model.EntityNested += OnEntityNested;
            model.RelationRemoved += OnRelationRemoved;
            model.RelationAdded += OnRelationAdded;
            model.Deserializing += OnDeserializing;
            model.Project = this.Project;
            model.Name = this.name;
            model.Modified += OnModified;

            newShapeType = EntityType.Class;
            DiagramType = DiagramType.ClassDiagram;
        }
        
        public Language Language => model.Language;

        public bool InsertStructure(StructureType structure)
        {
            return model.InsertTypeEntity(structure);
        }

        public bool InsertInterface(InterfaceType newInterface)
        {
            return model.InsertTypeEntity(newInterface);
        }

        public bool InsertEnum(EnumType newEnum)
        {
            return model.InsertTypeEntity(newEnum);
        }

        public bool InsertDelegate(DelegateType newDelegate)
        {
            return model.InsertTypeEntity(newDelegate);
        }

        public bool InsertPackage(Package package)
        {
            return model.InsertPackage(package);
        }

        public bool InsertClass(ClassType newClass)
        {
            return model.InsertTypeEntity(newClass);
        }

        public bool InsertAssociation(AssociationRelationship associaton)
        {
            return model.InsertRelationship(associaton);
        }

        public bool InsertDependency(DependencyRelationship dependency)
        {
            return model.InsertRelationship(dependency);
        }

        public bool InsertGeneralization(GeneralizationRelationship generalization)
        {
            return model.InsertRelationship(generalization);
        }

        public bool InsertNesting(NestingRelationship nesting)
        {
            return model.InsertRelationship(nesting);
        }

        public bool InsertRealization(RealizationRelationship realization)
        {
            return model.InsertRelationship(realization);
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/> or <paramref name="second"/> is null.
        /// </exception>
        public AssociationRelationship AddAssociation(TypeBase first, TypeBase second)
        {
            return model.AddAssociation(first, second);
        }

        public AssociationRelationship AddAssociation(AssociationRelationship association)
        {
            Shape startShape = GetShape(association.First);
            Shape endShape = GetShape(association.Second);
            AddConnection(new Association(association, startShape, endShape));
            return association;
        }

        public AssociationRelationship AddComposition(TypeBase first, TypeBase second)
        {
            return model.AddComposition(first, second);
        }

        public AssociationRelationship AddComposition(AssociationRelationship composition)
        {
            Shape startShape = GetShape(composition.First);
            Shape endShape = GetShape(composition.Second);
            AddConnection(new Association(composition, startShape, endShape));
            return composition;
        }

        public AssociationRelationship AddAggregation(TypeBase first, TypeBase second)
        {
            return model.AddAggregation(first, second);
        }

        public GeneralizationRelationship AddGeneralization(CompositeType derivedType,
            CompositeType baseType)
        {
            return model.AddGeneralization(derivedType, baseType);
        }

        private GeneralizationRelationship AddGeneralization(GeneralizationRelationship generalization)
        {
            Shape startShape = GetShape(generalization.First);
            Shape endShape = GetShape(generalization.Second);
            AddConnection(new Generalization(generalization, startShape, endShape));
            return generalization;
        }

        public RealizationRelationship AddRealization(TypeBase implementer,
            InterfaceType baseType)
        {
            return model.AddRealization(implementer, baseType);
        }

        private RealizationRelationship AddRealization(RealizationRelationship realization)
        {
            Shape startShape = GetShape(realization.First);
            Shape endShape = GetShape(realization.Second);
            AddConnection(new Realization(realization, startShape, endShape));
            return realization;
        }

        public DependencyRelationship AddDependency(TypeBase first, TypeBase second)
        {
            return model.AddDependency(first, second);
        }

        private DependencyRelationship AddDependency(DependencyRelationship dependency)
        {
            Shape startShape = GetShape(dependency.First);
            Shape endShape = GetShape(dependency.Second);
            AddConnection(new Dependency(dependency, startShape, endShape));
            return dependency;
        }

        public NestingRelationship AddNesting(INestable parentType, INestableChild innerType)
        {
            return model.AddNesting(parentType, innerType);
        }

        public NestingRelationship AddNesting(NestingRelationship nesting)
        {
            Shape startShape = GetShape(nesting.First);
            Shape endShape = GetShape(nesting.Second);
            AddConnection(new Nesting(nesting, startShape, endShape));
            return nesting;
        }

        public Package AddPackage()
        {
            return model.AddPackage();
        }

        public ClassType AddClass()
        {
            return model.AddClass();
        }

        private void AddPackage(Package package)
        {
            AddShape(new PackageShape(package));
        }

        private void AddClass(ClassType classType)
        {
            AddShape(new ClassShape(classType));
        }

        public StructureType AddStructure()
        {
            return model.AddStructure();
        }

        private void AddStructure(StructureType structureType)
        {
            AddShape(new StructureShape(structureType));
        }

        public InterfaceType AddInterface()
        {
            return model.AddInterface();
        }

        private void AddInterface(InterfaceType interfaceType)
        {
            AddShape(new InterfaceShape(interfaceType));
        }

        public DelegateType AddDelegate()
        {
            return model.AddDelegate();
        }

        private void AddDelegate(DelegateType delegateType)
        {
            AddShape(new DelegateShape(delegateType));
        }

        public EnumType AddEnum()
        {
            return model.AddEnum();
        }

        private void AddEnum(EnumType enumType)
        {
            AddShape(new EnumShape(enumType));
        }

        public override Shape AddShape(EntityType type)
        {
            switch (type)
            {
                case EntityType.Package:
                    AddPackage();
                    break;

                case EntityType.Class:
                    AddClass();
                    break;

                case EntityType.Comment:
                    AddComment();
                    break;

                case EntityType.Delegate:
                    AddDelegate();
                    break;

                case EntityType.Enum:
                    AddEnum();
                    break;

                case EntityType.Interface:
                    AddInterface();
                    break;

                case EntityType.Structure:
                    AddStructure();
                    break;

                default:
                    return null;
            }

            RecalculateSize();
            return base.AddShape(type);
        }

        protected override void OnEntityAdded(object sender, EntityEventArgs e)
        {
            switch (e.Entity.EntityType)
            {
                case EntityType.Package:
                    AddPackage(e.Entity as Package);
                    break;

                case EntityType.Class:
                    AddClass(e.Entity as ClassType);
                    break;

                case EntityType.Comment:
                    AddComment(e.Entity as Comment);
                    break;

                case EntityType.Delegate:
                    AddDelegate(e.Entity as DelegateType);
                    break;

                case EntityType.Enum:
                    AddEnum(e.Entity as EnumType);
                    break;

                case EntityType.Interface:
                    AddInterface(e.Entity as InterfaceType);
                    break;

                case EntityType.Structure:
                    AddStructure(e.Entity as StructureType);
                    break;
            }

            RecalculateSize();
        }

        protected override void OnRelationAdded(object sender, RelationshipEventArgs e)
        {
            switch (e.Relationship.RelationshipType)
            {
                case RelationshipType.Association:
                    AddAssociation(e.Relationship as AssociationRelationship);
                    break;

                case RelationshipType.Composition:
                    AddComposition(e.Relationship as AssociationRelationship);
                    break;

                case RelationshipType.Aggregation:
                    AddAssociation(e.Relationship as AssociationRelationship);
                    break;

                case RelationshipType.Generalization:
                    AddGeneralization(e.Relationship as GeneralizationRelationship);
                    break;

                case RelationshipType.Realization:
                    AddRealization(e.Relationship as RealizationRelationship);
                    break;

                case RelationshipType.Dependency:
                    AddDependency(e.Relationship as DependencyRelationship);
                    break;

                case RelationshipType.Nesting:
                    AddNesting(e.Relationship as NestingRelationship);
                    break;

                case RelationshipType.Comment:
                    AddCommentRelationship(e.Relationship as CommentRelationship);
                    break;
            }
        }

        private void OnEntityNested(object sender, EntityEventArgs e)
        {
            var containerEntity = (IEntity) sender;
            var childEntity = e.Entity;
            var containerShape = (ShapeContainer)GetShape(containerEntity);
            var childShape = GetShape(childEntity);
            containerShape.AttachShapes(new List<Shape> {childShape});
        }

        public override void KeyDown(KeyEventArgs e)
        {
            //TODO: ActiveElement.KeyDown() - de nem minden esetben (pl. törlésnél nem)
            RedrawSuspended = true;

            if (e.Modifiers == (Keys.Control | Keys.Shift))
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        CreateShape();
                        break;

                    case Keys.C:
                        CreateShape(EntityType.Class);
                        break;

                    case Keys.S:
                        CreateShape(EntityType.Structure);
                        break;

                    case Keys.I:
                        CreateShape(EntityType.Interface);
                        break;

                    case Keys.E:
                        CreateShape(EntityType.Enum);
                        break;

                    case Keys.D:
                        CreateShape(EntityType.Delegate);
                        break;

                    case Keys.N:
                        CreateShape(EntityType.Comment);
                        break;
                }
            }

            base.KeyDown(e);
            RedrawSuspended = false;
        }

        public override void CreateShape(EntityType type, Point? where = null)
        {
            base.CreateShape(type);

            switch (type)
            {
                case EntityType.Class:
                case EntityType.Delegate:
                case EntityType.Enum:
                case EntityType.Interface:
                case EntityType.Structure:
                    shapeOutline = TypeShape.GetOutline(Style.CurrentStyle);
                    break;
                case EntityType.Package:
                    shapeOutline = PackageShape.GetOutline(Style.CurrentStyle);
                    break;
                case EntityType.Comment:
                    shapeOutline = CommentShape.GetOutline(Style.CurrentStyle);
                    break;
            }

            //shapeOutline.Location = new Point((int) mouseLocation.X, (int) mouseLocation.Y);
            shapeOutline.Location = where ?? new Point((int)mouseLocation.X, (int)mouseLocation.Y);
            Redraw();
        }

        public override void CreateConnection(RelationshipType type)
        {
            connectionCreator = new ClassConnectionCreator(this, type);
            base.CreateConnection(type);
        }

        public override void Deserialize(XmlElement node)
        {
            base.Deserialize(node);

            Language language = null;
            XmlElement languageElement = node["Language"];
            try
            {
                language = Language.GetLanguage(languageElement.InnerText);
                if (language == null)
                    throw new InvalidDataException("Invalid project language.");
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Invalid project language.", ex);
            }

            if (model == null)
            {
                model = new ClassModel(language);
                model.EntityRemoved += OnEntityRemoved;
                model.EntityAdded += OnEntityAdded;
                model.EntityNested += OnEntityNested;
                model.RelationRemoved += OnRelationRemoved;
                model.RelationAdded += OnRelationAdded;
                model.Deserializing += OnDeserializing;
                model.Modified += OnModified;
                model.Name = this.name;
            }

            model.Deserialize(node);
        }

        public override string GetShortDescription()
        {
            return Strings.Language + ": " + model.Language.ToString();
        }
    }
}