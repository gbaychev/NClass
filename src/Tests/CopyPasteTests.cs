using System.Drawing;
using System.Linq;
using System.Threading;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture]
    public class CopyPasteTests
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void CopyPastePackageWorks()
        {
            var diagram = new ClassDiagram(Language.GetLanguage("csharp"));
            var other = new ClassDiagram(Language.GetLanguage("csharp"));

            diagram.CreateShapeAt(EntityType.Package, new Point(0, 0));
            diagram.CreateShapeAt(EntityType.Class, new Point(1, 1));

            diagram.SelectAll();
            diagram.Copy();
            other.Paste();

            other.Model.Entities.Count().ShouldBe(2);
            other.ShapeCount.ShouldBe(2);
        }
    }
}