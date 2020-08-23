// NClass - Free class diagram editor
// Copyright (C) 2016 - 2017 Georgi Baychev
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

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.Core.Models;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.DiagramEditor.UseCaseDiagram.Connection;
using NClass.DiagramEditor.UseCaseDiagram.ContextMenus;
using NClass.DiagramEditor.UseCaseDiagram.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram
{
    public class UseCaseDiagram : Diagram<UseCaseModel>
    {
        public UseCaseDiagram()
        {
            this.model = new UseCaseModel();
            model.EntityRemoved += OnEntityRemoved;
            model.EntityAdded += OnEntityAdded;
            model.RelationRemoved += OnRelationRemoved;
            model.RelationAdded += OnRelationAdded;
            model.Deserializing += OnDeserializing;
            model.EntityNested += OnEntityNested;
            model.Project = this.Project;
            model.Name = this.name;

            newShapeType = EntityType.UseCase;
            this.diagramDynamicMenu = UseCaseDiagramDynamicMenu.Default;
            this.diagramContextMenu = UseCaseDiagramBlankContextMenu.Default;
            this.DiagramType = DiagramType.UseCaseDiagram;
        }

        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);
        }

        public override void CreateConnection(RelationshipType type)
        {
            connectionCreator = new UseCaseConnectionCreator(this, type);
            base.CreateConnection(type);
        }

        public override void CreateShape(EntityType type, Point? where = null)
        {
            state = State.CreatingShape;
            shapeType = type;
            newShapeType = type;

            switch (type)
            {
                case EntityType.Actor:
                    shapeOutline = ActorShape.GetOutline(Style.CurrentStyle);
                    break;
                case EntityType.UseCase:
                    shapeOutline = UseCaseShape.GetOutline(Style.CurrentStyle);
                    break;
                case EntityType.Comment:
                    shapeOutline = CommentShape.GetOutline(Style.CurrentStyle);
                    break;
                case EntityType.SystemBoundary:
                    shapeOutline = SystemBoundaryShape.GetOutline(Style.CurrentStyle);
                    break;
            }
            shapeOutline.Location = where ?? new Point((int)mouseLocation.X, (int)mouseLocation.Y);
            Redraw();
        }

        public override Shape AddShape(EntityType type)
        {
            switch (type)
            {
                case EntityType.Comment:
                    AddComment();
                    break;

                case EntityType.Actor:
                    AddActor();
                    break;

                case EntityType.UseCase:
                    AddUseCase();
                    break;

                case EntityType.SystemBoundary:
                    AddSystemBoundary();
                    break;

                default:
                    return null;
            }

            RecalculateSize();
            return shapes.FirstValue;
        }

        private Actor AddActor()
        {
            return model.AddActor();
        }

        private UseCase AddUseCase()
        {
            return model.AddUseCase();
        }

        private SystemBoundary AddSystemBoundary()
        {
            return model.AddSystemBoundary();
        }

        private void AddActor(Actor actor)
        {
            AddShape(new ActorShape(actor));
        }

        private void AddUseCase(UseCase useCase)
        {
            AddShape(new UseCaseShape(useCase));
        }

        private void AddSystemBoundary(SystemBoundary systemBoundary)
        {
            AddShape(new SystemBoundaryShape(systemBoundary));
        }


        protected override void OnEntityAdded(object sender, EntityEventArgs e)
        {
            switch (e.Entity.EntityType)
            {
                case EntityType.Comment:
                    AddComment(e.Entity as Comment);
                    break;
                case EntityType.UseCase:
                    AddUseCase(e.Entity as UseCase);
                    break;
                case EntityType.Actor:
                    AddActor(e.Entity as Actor);
                    break;
                case EntityType.SystemBoundary:
                    AddSystemBoundary(e.Entity as SystemBoundary);
                    break;
            }

            RecalculateSize();
        }

        protected override void OnRelationAdded(object sender, RelationshipEventArgs e)
        {
            switch (e.Relationship.RelationshipType)
            {
                case RelationshipType.Extension:
                    AddExtends(e.Relationship as ExtendsRelationship);
                    break;
                case RelationshipType.Inclusion:
                    AddIncludes(e.Relationship as IncludesRelationship);
                    break;
                case RelationshipType.UseCaseAssociation:
                    AddAssociation(e.Relationship as UseCaseAssociation);
                    break;
                case RelationshipType.UseCaseGeneralization:
                    AddGeneralization(e.Relationship as UseCaseGeneralization);
                    break;
                case RelationshipType.Comment:
                    AddCommentRelationship(e.Relationship as CommentRelationship);
                    break;
            }
        }

        public bool InsertActor(Actor actor)
        {
            return model.InsertActor(actor);
        }

        public bool InsertUseCase(UseCase useCase)
        {
            return model.InsertUseCase(useCase);
        }

        public bool InsertSystemBoundary(SystemBoundary systemBoundary)
        {
            return model.InsertSystemBoundary(systemBoundary);
        }

        public override void Deserialize(XmlElement node)
        {
            base.Deserialize(node);

            if (model == null)
            {
                model = new UseCaseModel();
                model.EntityRemoved += OnEntityRemoved;
                model.EntityAdded += OnEntityAdded;
                model.RelationRemoved += OnRelationRemoved;
                model.RelationAdded += OnRelationAdded;
                model.Deserializing += OnDeserializing;
                model.EntityNested += OnEntityNested;
                model.Name = this.name;
            }

            model.Deserialize(node);
            
        }

        public bool InsertIncludes(IncludesRelationship includesRelationship)
        {
            return model.InsertRelationship(includesRelationship);
        }

        public bool InsertExtends(ExtendsRelationship extendsRelationship)
        {
            return model.InsertRelationship(extendsRelationship);
        }

        public bool InsertAssociation(UseCaseAssociation association)
        {
            return model.InsertRelationship(association);
        }

        public bool InsertGeneralization(UseCaseGeneralization generalization)
        {
            return model.InsertRelationship(generalization);
        }

        public IncludesRelationship AddIncludes(UseCase first, UseCase second)
        {
            return model.AddIncludes(first, second);
        }

        private void AddIncludes(IncludesRelationship includesRelationship)
        {
            Shape startShape = GetShape(includesRelationship.First);
            Shape endShape = GetShape(includesRelationship.Second);
            AddConnection(new IncludesConnection(includesRelationship, startShape, endShape));
        }

        public ExtendsRelationship AddExtends(UseCase first, UseCase second)
        {
            return model.AddExtends(first, second);
        }

        private void AddExtends(ExtendsRelationship extendsRelationship)
        {
            Shape startShape = GetShape(extendsRelationship.First);
            Shape endShape = GetShape(extendsRelationship.Second);
            AddConnection(new ExtendsConnection(extendsRelationship, startShape, endShape));
        }

        public UseCaseAssociation AddAssociation(IUseCaseEntity first, IUseCaseEntity second)
        {
            return model.AddAssociation(first, second);
        }

        private void AddAssociation(UseCaseAssociation association)
        {
            var startShape = GetShape(association.First);
            var endShape = GetShape(association.Second);
            AddConnection(new UseCaseAssociationConnection(association, startShape, endShape));
        }

        public UseCaseGeneralization AddGeneralization(IUseCaseEntity first, IUseCaseEntity second)
        {
            return model.AddGeneralization(first, second);
        }

        private void AddGeneralization(UseCaseGeneralization generalization)
        {
            var startShape = GetShape(generalization.First);
            var endShape = GetShape(generalization.Second);
            AddConnection(new UseCaseGeneralizationConnection(generalization, startShape, endShape));
        }

        public override string GetShortDescription()
        {
            return "Use Case Diagram";
        }

        private void OnEntityNested(object sender, EntityEventArgs e)
        {
            var containerEntity = (IEntity)sender;
            var childEntity = e.Entity;
            var containerShape = (ShapeContainer)GetShape(containerEntity);
            var childShape = GetShape(childEntity);
            containerShape.AttachShapes(new List<Shape> { childShape });
        }
    }
}