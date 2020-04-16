using System;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands

{
    public class RenameConnectionCommand
    {
        readonly AbstractConnection connection;
        readonly IDiagram diagram;

        public RenameConnectionCommand(Shape shape, IDiagram diagram)
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