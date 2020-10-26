using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NClass.Core;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Commands

{
    public class AddShapeCommand : ICommand
    {
        private Shape shape;
        private readonly IDiagram diagram;
        private readonly Point location;
        private readonly EntityType shapeType;

        public AddShapeCommand(EntityType shapeType, IDiagram diagram, Point location)
        {
            this.diagram = diagram;
            this.location = location;
            this.shapeType = shapeType;
        }

        public void Execute()
        {
            if (shape == null)
            {
                diagram.DeselectAll();
                shape = diagram.AddShape(shapeType);
                shape.Location = location;
                shape.OldLocation = location;
                shape.IsSelected = true;
                shape.IsActive = true;

                if (diagram.Shapes.Where(s => s is ShapeContainer).FirstOrDefault(s => s.Contains(shape.Location)) is
                    ShapeContainer container)
                    container.AttachShapes(new List<Shape> {shape});
                if (shape is TypeShape) //TODO: not pretty
                    shape.ShowEditor();
            }
            else
            {
                diagram.ReinsertShape(shape);
            }
        }

        public void Undo()
        {
            diagram.RemoveEntity(this.shape.Entity);
            diagram.Redraw();
        }

        public CommandId CommandId => CommandId.AddShape;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}
