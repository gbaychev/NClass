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
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Commands;
using NClass.DiagramEditor.Diagrams.Shapes;
using NClass.DiagramEditor.UseCaseDiagram;
using NUnit.Framework;
using Shouldly;

namespace Tests.UndoRedoTests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class RenameShapeTests
    {
        private Language language;
        private ClassDiagram classDiagram;
        private UseCaseDiagram useCaseDiagram;

        [SetUp]
        public void Init()
        {
            language = Language.GetLanguage("csharp");
            classDiagram = new ClassDiagram(language);
            useCaseDiagram = new UseCaseDiagram();
        }

        static readonly Dictionary<string, Func<Shape, string, ICommand>> ClassDiagramCommandFactory = new Dictionary<string, Func<Shape, string, ICommand>>
        {
            {"Class", (shape, newValue) => new ChangePropertyCommand<ClassShape, string>((ClassShape)shape, s => s.ClassType.Name, (s, name) => s.ClassType.Name = name, newValue)},
            {"Package", (shape, newValue) => new ChangePropertyCommand<PackageShape, string>((PackageShape)shape, s => s.Package.Name, (s, name) => s.Package.Name = name, newValue)},
            {"Interface", (shape, newValue) => new ChangePropertyCommand<InterfaceShape, string>((InterfaceShape)shape, s => s.InterfaceType.Name, (s, name) => s.InterfaceType.Name = name, newValue)},
            {"Delegate", (shape, newValue) => new ChangePropertyCommand<DelegateShape, string>((DelegateShape)shape, s => s.DelegateType.Name, (s, name) => s.DelegateType.Name = name, newValue)},
            {"Structure", (shape, newValue) => new ChangePropertyCommand<StructureShape, string>((StructureShape)shape, s => s.StructureType.Name, (s, name) => s.StructureType.Name = name, newValue)},
            {"Enum", (shape, newValue) => new ChangePropertyCommand<EnumShape, string>((EnumShape)shape, s => s.EnumType.Name, (s, name) => s.EnumType.Name = name, newValue)},
        };

        private static readonly object[] ClassDiagramTestCases =
        {
            new object[] {EntityType.Class, "Class" },
            new object[] {EntityType.Package,"Package" },
            new object[] {EntityType.Interface,"Interface" },
            new object[] {EntityType.Delegate,"Delegate" },
            new object[] {EntityType.Structure,"Structure" },
            new object[] {EntityType.Enum,"Enum" },
        };

        [Test]
        [TestCaseSource(nameof(ClassDiagramTestCases))]

        public void RenameClassDiagramShapesUndoRedo(EntityType entityType, string testCaseName)
        {
            classDiagram.CreateShapeAt(entityType, new Point(0,0));
            var shape = classDiagram.Shapes.First();
            var oldName = shape.Entity.Name;
            var newValue = $"{entityType.ToString()}Renamed";
            var renameCommand = ClassDiagramCommandFactory[testCaseName](shape, newValue);
            renameCommand.Execute();

            var newName = classDiagram.Shapes.First().Entity.Name;
            renameCommand.Undo();
            var currentName = classDiagram.Shapes.First().Entity.Name;

            newName.ShouldBe(newValue);
            currentName.ShouldBe(oldName);
        }
    }
}