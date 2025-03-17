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

using System.Linq;
using System.Threading;
using NClass.CSharp;
using NClass.Java;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class NestableEntityTests
    {
        [Test]
        public void CSharpNamespaceCloneTest()
        {
            var parentPackage = new CSharpNamespace("parent");
            var childPackage = new CSharpNamespace("child");
            var classType1 = new CSharpClass("class1");
            var classType2 = new CSharpClass("class2");

            childPackage.AddNestedChild(classType1);
            childPackage.AddNestedChild(classType2);
            parentPackage.AddNestedChild(childPackage);

            var otherParent = parentPackage.Clone(true);
            var otherChild = (CSharpNamespace)parentPackage.NestedChilds.First();

            otherParent.NestedChilds.Count().ShouldBe(1);
            otherParent.Name.ShouldBe(parentPackage.Name);
            otherChild.NestedChilds.Count().ShouldBe(2);
            otherChild.Name.ShouldBe(childPackage.Name);
            otherChild.NestedChilds.First().Name.ShouldBe(classType1.Name);
            otherChild.NestedChilds.Last().Name.ShouldBe(classType2.Name);
        }

        [Test]
        public void JavaCloneTest()
        {
            var parentPackage = new JavaPackage("parent");
            var childPackage = new JavaPackage("child");
            var classType1 = new JavaClass("class1");
            var classType2 = new JavaClass("class2");

            childPackage.AddNestedChild(classType1);
            childPackage.AddNestedChild(classType2);
            parentPackage.AddNestedChild(childPackage);

            var otherParent = parentPackage.Clone(true);
            var otherChild = (JavaPackage)parentPackage.NestedChilds.First();

            otherParent.NestedChilds.Count().ShouldBe(1);
            otherParent.Name.ShouldBe(parentPackage.Name);
            otherChild.NestedChilds.Count().ShouldBe(2);
            otherChild.Name.ShouldBe(childPackage.Name);
            otherChild.NestedChilds.First().Name.ShouldBe(classType1.Name);
            otherChild.NestedChilds.Last().Name.ShouldBe(classType2.Name);
        }
    }
}