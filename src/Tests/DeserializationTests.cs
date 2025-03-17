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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.UseCaseDiagram;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class DeserializationTests
    {
        [Test]
        public void CanDeserializeOldFormat()
        {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(root, @"DiagramFiles\old_format.ncp");
            var project = Project.Load(path);
            var projectItems = project.Items.Cast<ClassDiagram>().ToArray();
            var diagram1 = projectItems[0];
            var diagram2 = projectItems[1];
            var diagram3 = projectItems[2];

            project.ItemCount.ShouldBe(3);
            project.Name.ShouldBe("FILTERSET");

            diagram1.Name.ShouldBe("Model");
            diagram1.Language.ShouldBe(Language.GetLanguage("csharp"));
            diagram1.Model.Entities.Count().ShouldBe(10);
            diagram1.Model.Relationships.Count().ShouldBe(7);

            diagram2.Name.ShouldBe("ViewModel");
            diagram2.Language.ShouldBe(Language.GetLanguage("csharp"));
            diagram2.Model.Entities.Count().ShouldBe(9);
            diagram2.Model.Relationships.Count().ShouldBe(6);

            diagram3.Name.ShouldBe("View");
            diagram3.Language.ShouldBe(Language.GetLanguage("csharp"));
            diagram3.Model.Entities.Count().ShouldBe(2);
            diagram3.Model.Relationships.Count().ShouldBe(1);
        }

        [Test]
        public void CanDeserializeUseCaseDiagrams()
        {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(root, @"DiagramFiles\use_case.ncp");
            var project = Project.Load(path);
            var projectItems = project.Items.ToArray();
            var diagram1 = (ClassDiagram)projectItems[0];
            var diagram2 = (UseCaseDiagram)projectItems[1];

            project.ItemCount.ShouldBe(2);
            project.Name.ShouldBe("Shapes");

            diagram1.Name.ShouldBe("Shapes");
            diagram1.Language.ShouldBe(Language.GetLanguage("csharp"));
            diagram1.Model.Entities.Count().ShouldBe(6);
            diagram1.Model.Relationships.Count().ShouldBe(3);

            diagram2.Name.ShouldBe("Use Case");
            diagram2.Model.Entities.Count().ShouldBe(8);
            diagram2.Model.Relationships.Count().ShouldBe(8);
        }
    }
}