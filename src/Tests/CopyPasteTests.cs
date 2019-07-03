using System.Drawing;
using System.Linq;
using System.Threading;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
 using NClass.DiagramEditor.UseCaseDiagram;
using NUnit.Framework;
 using Shouldly;

 namespace Tests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class CopyPasteTests
    {
        [Test]
        public void ClassDiagramCanCopyPaste()
        {
            var language = Language.GetLanguage("csharp");
            var diagramFrom = new ClassDiagram(language);
            var diagramTo = new ClassDiagram(language);

            diagramFrom.CreateShapeAt(EntityType.Class, new Point(0, 0));
            diagramFrom.CreateShapeAt(EntityType.Structure, new Point(0, 0));
            diagramFrom.AddAssociation((TypeBase)diagramFrom.Model.Entities.First(), (TypeBase)diagramFrom.Model.Entities.Last());
            diagramFrom.SelectAll();
            diagramFrom.Copy();
            diagramTo.Paste();

            diagramTo.Shapes.Count().ShouldBe(2);
            diagramTo.Model.Entities.Count().ShouldBe(2);
            diagramTo.Connections.Count().ShouldBe(1);
        }

        [Test]
        public void UseCaseDiagramCanCopyPaste()
        {
            var diagramFrom = new UseCaseDiagram();
            var diagramTo = new UseCaseDiagram();

            diagramFrom.CreateShapeAt(EntityType.Actor, new Point(0, 0));
            diagramFrom.CreateShapeAt(EntityType.UseCase, new Point(0, 0));
            diagramFrom.AddAssociation((IUseCaseEntity)diagramFrom.Model.Entities.First(), (IUseCaseEntity)diagramFrom.Model.Entities.Last());
            diagramFrom.SelectAll();
            diagramFrom.Copy();
            diagramTo.Paste();

            diagramTo.Shapes.Count().ShouldBe(2);
            diagramTo.Model.Entities.Count().ShouldBe(2);
            diagramTo.Connections.Count().ShouldBe(1);
        }
    }
}