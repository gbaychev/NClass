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

using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NClass.Core;
using NClass.Core.Models;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;
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
        }

        public override void KeyDown(KeyEventArgs e)
        {
            throw new System.NotImplementedException();
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
                        break;
                    case EntityType.UseCase:
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
            throw new System.NotImplementedException();
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
    }
}