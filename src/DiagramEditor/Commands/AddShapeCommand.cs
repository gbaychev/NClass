using System;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands

{
    public class AddShapeCommand : ICommand
    {
        readonly Shape shape;
        readonly IDiagram diagram;

        public AddShapeCommand(Shape shape, IDiagram diagram)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public CommandId CommandId
        {
            get { throw new NotImplementedException(); }
        }
    }
}