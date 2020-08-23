using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class AddConnectionCommand : ICommand
    {
        private Relationship _relationship;
        private readonly IDiagram _diagram;
        private readonly Func<Relationship> _createConnectionFactory;

        public AddConnectionCommand(IDiagram diagram, Func<Relationship> createConnectionFactory)
        {
            _diagram = diagram;
            _createConnectionFactory = createConnectionFactory;
        }

        public void Execute()
        {
            _relationship = _createConnectionFactory();
        }

        public void Undo()
        {
            _diagram.RemoveRelationship(_relationship);
            _diagram.Redraw();
        }

        public CommandId CommandId => CommandId.AddConnection;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}