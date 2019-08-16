// NClass - Free class diagram editor
// Copyright (C) 2019 Georgi Baychev
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
using NClass.CSharp;
using NClass.Java;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.UseCaseDiagram;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class ClassDiagramCopyPasteTests
    {
        private Language language;
        private ClassDiagram diagramTo;
        private ClassDiagram diagramFrom;

        [SetUp]
        public void Init()
        {
            language = Language.GetLanguage("csharp");
            diagramFrom = new ClassDiagram(language);
            diagramTo = new ClassDiagram(language);
        }

        static readonly Dictionary<string, Action<ClassDiagram, IEntity, IEntity>> DiagramActions = new Dictionary<string, Action<ClassDiagram, IEntity, IEntity>>
        {
            {"Association", (diagram, first, second) => diagram.AddAssociation((TypeBase)first, (TypeBase)second) },
            {"Comment", (diagram, first, second) => diagram.AddCommentRelationship((Comment) first, second) },
            {"Composition", (diagram, first, second) => diagram.AddComposition((TypeBase)first, (TypeBase)second) },
            {"Aggregation", (diagram, first, second) => diagram.AddAggregation((TypeBase)first, (TypeBase)second) },
            {"Generalization", (diagram, first, second) => diagram.AddGeneralization((ClassType)first, (ClassType)second) },
            {"Realization", (diagram, first, second) => diagram.AddRealization((ClassType)first, (InterfaceType)second) },
            {"Dependency", (diagram, first, second) => diagram.AddDependency((ClassType)first, (ClassType)second) },
        };

        private static readonly object[] TestCases =
        {
            new object[] {EntityType.Class, EntityType.Class, "Association" },
            new object[] {EntityType.Comment, EntityType.Class, "Comment" },
            new object[] {EntityType.Class, EntityType.Class, "Composition" },
            new object[] {EntityType.Class, EntityType.Class, "Aggregation" },
            new object[] {EntityType.Class, EntityType.Class, "Generalization" },
            new object[] {EntityType.Class, EntityType.Interface, "Realization" },
            new object[] {EntityType.Class, EntityType.Class, "Dependency" },
        };

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ClassDiagramCanCopyPaste(EntityType firstEntityType, EntityType secondEntityType, string diagramActionName)
        {
            diagramFrom.CreateShapeAt(firstEntityType, new Point(0, 0));
            diagramFrom.CreateShapeAt(secondEntityType, new Point(0, 0));
            DiagramActions[diagramActionName](diagramFrom, diagramFrom.Model.Entities.First(), diagramFrom.Model.Entities.Last());
            diagramFrom.SelectAll();
            diagramFrom.Copy();
            diagramTo.Paste();

            diagramTo.Shapes.Count().ShouldBe(2);
            diagramTo.Model.Entities.Count().ShouldBe(2);
            diagramTo.Connections.Count().ShouldBe(1);
        }

        [Test]
        public void ClassDiagramCanCopyPastePackage()
        {
            diagramFrom.CreateShapeAt(EntityType.Package, new Point(0, 0));
            diagramFrom.CreateShapeAt(EntityType.Class, new Point(1, 1));
            diagramFrom.SelectAll();
            diagramFrom.Copy();
            diagramTo.Paste();
            var package = (Package) diagramFrom.Model.Entities.First();
            var classType = (ClassType) diagramFrom.Model.Entities.Last();
            var copiedPackage = (Package) diagramTo.Model.Entities.Last();
            var copiedClassType = (ClassType) diagramTo.Model.Entities.First();

            package.NestedChilds.Count().ShouldBe(1);
            package.NestedChilds.ShouldContain(classType);
            copiedPackage.NestedChilds.Count().ShouldBe(1);
            copiedPackage.NestedChilds.ShouldContain(copiedClassType);
            diagramTo.Shapes.Count().ShouldBe(2);
            diagramTo.Model.Entities.Count().ShouldBe(2);
        }
    }
}