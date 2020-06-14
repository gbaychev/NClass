using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands
{
    public class DeleteElementsCommand : ICommand
    {
        private readonly List<Shape> shapes;
        private readonly List<AbstractConnection> connections;
        private readonly IDiagram diagram;

        public DeleteElementsCommand(List<Shape> shapes, 
                                     List<AbstractConnection> connections,
                                     IDiagram diagram)
        {
            this.diagram = diagram;
            this.shapes = shapes;
            this.connections = connections;
        }

        public void Execute()
        {
            diagram.DeselectAll();
            foreach (var shape in shapes)
            {
                diagram.RemoveEntity(shape.Entity);
            }

            foreach (var connection in connections)
            {
                diagram.RemoveRelationship(connection.Relationship);
            }

            diagram.Redraw();
        }

        public void Undo()
        {
            diagram.ReinsertShapes(shapes);
            diagram.ReinsertConnections(connections);
        }

        public CommandId CommandId => CommandId.DeleteElements;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}