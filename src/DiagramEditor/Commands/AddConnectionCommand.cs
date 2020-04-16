using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class AddConnectionCommand : ICommand
    {
        readonly AbstractConnection connection;
        readonly IDiagram diagram;

        public AddConnectionCommand(Shape shape, IDiagram diagram)
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