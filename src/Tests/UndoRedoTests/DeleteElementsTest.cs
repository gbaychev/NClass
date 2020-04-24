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

using System.Drawing;
using System.Linq;
using System.Threading;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NUnit.Framework;
using Shouldly;

namespace Tests.UndoRedoTests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class DeleteElementsTest
    {
        private Language language;
        private ClassDiagram classDiagram;

        [SetUp]
        public void Init()
        {
            language = Language.GetLanguage("csharp");
            classDiagram = new ClassDiagram(language);
        }

        [Test]
        public void CanUndoRedoShapes()
        {
            classDiagram.CreateShapeAt(EntityType.Class, new Point(0,0));
            classDiagram.CreateShapeAt(EntityType.Delegate, new Point(200, 200));

            var oldShapesCount = classDiagram.ShapeCount;
            classDiagram.SelectAll();
            classDiagram.DeleteSelectedElements();
            var newShapesCount = classDiagram.ShapeCount;
            classDiagram.Undo();
            var afterUndoShapesCount = classDiagram.ShapeCount;

            oldShapesCount.ShouldBe(2);
            newShapesCount.ShouldBe(0);
            afterUndoShapesCount.ShouldBe(oldShapesCount);
        }

        [Test]
        public void CanUndoRedoShapesAndConnections()
        {
            classDiagram.CreateShapeAt(EntityType.Interface, new Point(0, 0));
            classDiagram.CreateShapeAt(EntityType.Class, new Point(200, 200));
            classDiagram.AddRealization((ClassType)(classDiagram.Shapes.First().Entity), (InterfaceType)(classDiagram.Shapes.Last().Entity));

            var oldShapesCount = classDiagram.ShapeCount;
            var oldConnectionCount = classDiagram.ConnectionCount;
            classDiagram.SelectAll();
            classDiagram.DeleteSelectedElements();
            var newShapesCount = classDiagram.ShapeCount;
            var newConnectionCount = classDiagram.ConnectionCount;
            classDiagram.Undo();
            var afterUndoShapesCount = classDiagram.ShapeCount;
            var afterUndoConnectionCount = classDiagram.ConnectionCount;

            oldShapesCount.ShouldBe(2);
            newShapesCount.ShouldBe(0);
            afterUndoShapesCount.ShouldBe(oldShapesCount);

            oldConnectionCount.ShouldBe(1);
            newConnectionCount.ShouldBe(0);
            afterUndoConnectionCount.ShouldBe(oldConnectionCount);
        }
    }
}