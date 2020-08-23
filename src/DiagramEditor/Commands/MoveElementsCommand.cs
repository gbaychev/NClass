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
    public class MoveElementsCommand : ICommand
    {
        private readonly List<Shape> shapes;
        private readonly IDiagram diagram;
        private readonly IDictionary<Shape, Point> locations;
        private readonly IDictionary<Shape, Point> oldLocations;

        public MoveElementsCommand(List<Shape> shapes, IDiagram diagram)
        {
            this.shapes = shapes;
            this.diagram = diagram;
            this.locations = shapes.ToDictionary(s => s, s => s.Location);
            this.oldLocations = shapes.ToDictionary(s => s, s => s.OldLocation);
        }

        public void Execute()
        {
            foreach (var shape in shapes)
            {
                shape.Location = locations[shape];
                shape.OldLocation = locations[shape];
            }
            diagram.ReattachShapes(shapes);
        }

        public void Undo()
        {
            foreach (var shape in shapes)
            {
                shape.Location = oldLocations[shape];
                shape.OldLocation = oldLocations[shape];
            }

            diagram.ReattachShapes(shapes);
        }

        public CommandId CommandId => CommandId.MoveElements;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}