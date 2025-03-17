// NClass - Free class diagram editor
// Copyright (C) 2025 Georgi Baychev
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
using System.Linq;
using System.Threading;
using NClass.Core;
using NClass.DiagramEditor.UseCaseDiagram;
using NUnit.Framework;
using Shouldly;

 namespace Tests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class UseCaseDiagramCopyPasteTests
    {
        private UseCaseDiagram diagramFrom;
        private UseCaseDiagram diagramTo;

        [SetUp]
        public void Init()
        {
            diagramFrom = new UseCaseDiagram();
            diagramTo = new UseCaseDiagram();
        }

        static readonly Dictionary<string, Action<UseCaseDiagram, IEntity, IUseCaseEntity>> DiagramActions = new Dictionary<string, Action<UseCaseDiagram, IEntity, IUseCaseEntity>>
        {
            {"Association", (diagram, first, second) => diagram.AddAssociation((IUseCaseEntity)first, second) },
            {"Generalization", (diagram, first, second) => diagram.AddGeneralization((IUseCaseEntity) first, second) },
            {"Extends", (diagram, first, second) => diagram.AddExtends((UseCase)first, (UseCase)second) },
            {"Includes", (diagram, first, second) => diagram.AddIncludes((UseCase)first, (UseCase)second) },
            {"Comment", (diagram, first, second) => diagram.AddCommentRelationship((Comment)first, second) },
        };

        private static readonly object[] TestCases =
        {
            new object[] {EntityType.Actor, EntityType.UseCase, "Association" },
            new object[] {EntityType.Actor, EntityType.Actor, "Generalization" },
            new object[] {EntityType.UseCase, EntityType.UseCase, "Extends" },
            new object[] {EntityType.UseCase, EntityType.UseCase, "Includes" },
            new object[] {EntityType.Comment, EntityType.UseCase, "Comment" }
        };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void UseCaseDiagramCanCopyPaste(EntityType firstEntityType, EntityType secondEntityType, string diagramActionName)
        {
            diagramFrom.CreateShapeAt(firstEntityType, new Point(0, 0));
            diagramFrom.CreateShapeAt(secondEntityType, new Point(0, 0));
            DiagramActions[diagramActionName](diagramFrom, diagramFrom.Model.Entities.First(), (IUseCaseEntity)diagramFrom.Model.Entities.Last());
            diagramFrom.SelectAll();
            diagramFrom.Copy();
            diagramTo.Paste();

            diagramTo.Shapes.Count().ShouldBe(2);
            diagramTo.Model.Entities.Count().ShouldBe(2);
            diagramTo.Connections.Count().ShouldBe(1);
        }
    }
}