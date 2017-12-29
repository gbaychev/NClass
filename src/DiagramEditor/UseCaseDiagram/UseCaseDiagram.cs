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

using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using NClass.Core;
using NClass.Core.Models;
using NClass.DiagramEditor.ClassDiagram.Connections;
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

        public override void CreateShape(EntityType type)
        {
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
                }
                shapeOutline.Location = new Point((int)mouseLocation.X, (int)mouseLocation.Y);
                Redraw();
            }
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

        private void AddActor(Actor actor)
        {
            AddShape(new ActorShape(actor));
        }

        private void AddUseCase(UseCase useCase)
        {
            AddShape(new UseCaseShape(useCase));
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
                case RelationshipType.UseCaseAssocation:
                    AddAssocation(e.Relationship as UseCaseAssociation);
                    break;
            }
        }
        
        public bool InsertActor(Actor actor)
        {
            if (actor == null || model.Entities.Contains(actor)) return false;

            AddActor(actor);
            return true;
        }

        public bool InsertUseCase(UseCase useCase)
        {
            if (useCase == null || model.Entities.Contains(useCase)) return false;

            AddUseCase(useCase);
            return true;
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
                model.Name = this.name;
            }

            model.Deserialize(node);
        }

        public bool InsertIncludes(IncludesRelationship includesRelationship)
        {
            if (includesRelationship != null && 
                !model.Relationships.Contains(includesRelationship) &&
                model.Entities.Contains(includesRelationship.First) && 
                model.Entities.Contains(includesRelationship.Second))
            {
                AddIncludes(includesRelationship);
                return true;
            }

            return false;
        }

        public bool InsertExtends(ExtendsRelationship extendsRelationship)
        {
            if (extendsRelationship != null && 
                !model.Relationships.Contains(extendsRelationship) &&
                model.Entities.Contains(extendsRelationship.First) && 
                model.Entities.Contains(extendsRelationship.Second))
            {
                AddExtends(extendsRelationship);
                return true;
            }

            return false;
        }

        public bool InsertAssociation(UseCaseAssociation association)
        {
            if (association != null &&
                !model.Relationships.Contains(association) &&
                model.Entities.Contains(association.First) &&
                model.Entities.Contains(association.Second))
            {
                AddAssocation(association);
                return true;
            }

            return false;
        }

        public void AddIncludes(UseCase first, UseCase second)
        {
            model.AddIncludes(first, second);
        }

        private void AddIncludes(IncludesRelationship includesRelationship)
        {
            Shape startShape = GetShape(includesRelationship.First);
            Shape endShape = GetShape(includesRelationship.Second);
            AddConnection(new IncludesConnection(includesRelationship, startShape, endShape));
        }

        public void AddExtends(UseCase first, UseCase second)
        {
            model.AddExtends(first, second);
        }

        private void AddExtends(ExtendsRelationship extendsRelationship)
        {
            Shape startShape = GetShape(extendsRelationship.First);
            Shape endShape = GetShape(extendsRelationship.Second);
            AddConnection(new ExtendsConnection(extendsRelationship, startShape, endShape));
        }

        public void AddAssociation(IUseCaseEntity first, IUseCaseEntity second)
        {
            model.AddAssocation(first, second);
        }

        private void AddAssocation(UseCaseAssociation association)
        {
            var startShape = GetShape(association.First);
            var endShape = GetShape(association.Second);
            AddConnection(new UseCaseAssociationConnection(association, startShape, endShape));
        }
    }
}