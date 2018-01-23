// NClass - Free class diagram editor
// Copyright (C) 2016 - 2018 Georgi Baychev
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
using System.Linq;
using NClass.Core;
using NUnit.Framework;
using NClass.DiagramEditor.UseCaseDiagram;

namespace Tests
{
    [TestFixture]
    public class UseCaseDiagramTests
    {
        private UseCaseDiagram diagram;

        [SetUp]
        public void Init()
        {
            diagram = new UseCaseDiagram();
        }

        [Test]
        public void ActorCanInheritActor()
        {
            var first = diagram.AddShape(EntityType.Actor);
            var second = diagram.AddShape(EntityType.Actor);

            diagram.AddGeneralization((IUseCaseEntity)first.Entity, (IUseCaseEntity)second.Entity);
            var relationship = diagram.Model.Relationships.First();

            Assert.That(diagram.Model.Entities, Has.Exactly(2).Items);
            Assert.That(diagram.Model.Relationships, Has.Exactly(1).Items);
            Assert.That(relationship.RelationshipType, Is.EqualTo(RelationshipType.UseCaseGeneralization));
            Assert.That(relationship.First, Is.EqualTo(first.Entity));
            Assert.That(relationship.Second, Is.EqualTo(second.Entity));
        }

        [Test]
        public void ActorCannotInheritUseCase()
        {
            var first = diagram.AddShape(EntityType.Actor);
            var second = diagram.AddShape(EntityType.UseCase);

            Assert.Throws<RelationshipException>(() => diagram.AddGeneralization((IUseCaseEntity)first.Entity, (IUseCaseEntity)second.Entity));
        }

        [Test]
        public void UseCaseCanInheritUseCase()
        {
            var first = diagram.AddShape(EntityType.UseCase);
            var second = diagram.AddShape(EntityType.UseCase);

            diagram.AddGeneralization((IUseCaseEntity)first.Entity, (IUseCaseEntity)second.Entity);
            var relationship = diagram.Model.Relationships.First();

            Assert.That(diagram.Model.Entities, Has.Exactly(2).Items);
            Assert.That(diagram.Model.Relationships, Has.Exactly(1).Items);
            Assert.That(relationship.RelationshipType, Is.EqualTo(RelationshipType.UseCaseGeneralization));
            Assert.That(relationship.First, Is.EqualTo(first.Entity));
            Assert.That(relationship.Second, Is.EqualTo(second.Entity));
        }

        [Test]
        public void UseCaseCannotInheritActor()
        {
            var first = diagram.AddShape(EntityType.UseCase);
            var second = diagram.AddShape(EntityType.Actor);

            Assert.Throws<RelationshipException>(() => diagram.AddGeneralization((IUseCaseEntity)first.Entity, (IUseCaseEntity)second.Entity));
        }

        [Test]
        public void CannotDuplicateExtendRelationship()
        {
            var first = diagram.AddShape(EntityType.UseCase);
            var second = diagram.AddShape(EntityType.UseCase);

            diagram.AddExtends((UseCase) first.Entity, (UseCase) second.Entity);
            
            Assert.Throws<RelationshipException>(() => diagram.AddExtends((UseCase)first.Entity, (UseCase)second.Entity));
            Assert.Throws<RelationshipException>(() => diagram.AddExtends((UseCase)second.Entity, (UseCase)first.Entity));
        }

        [Test]
        public void CannotDuplicateIncludeRelationship()
        {
            var first = diagram.AddShape(EntityType.UseCase);
            var second = diagram.AddShape(EntityType.UseCase);

            diagram.AddIncludes((UseCase)first.Entity, (UseCase)second.Entity);

            Assert.Throws<RelationshipException>(() => diagram.AddIncludes((UseCase)first.Entity, (UseCase)second.Entity));
            Assert.Throws<RelationshipException>(() => diagram.AddIncludes((UseCase)second.Entity, (UseCase)first.Entity));
        }
    }
}