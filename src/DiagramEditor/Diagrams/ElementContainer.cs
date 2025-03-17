﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Diagrams.Connections;
using NClass.DiagramEditor.Diagrams.Shapes;

namespace NClass.DiagramEditor.Diagrams
{
    internal class ElementContainer : IClipboardItem
    {
        const int BaseOffset = 20;

        List<Shape> shapes = new List<Shape>();
        List<AbstractConnection> connections = new List<AbstractConnection>();
        Dictionary<Shape, Shape> pastedShapes = new Dictionary<Shape, Shape>();
        Dictionary<AbstractConnection, AbstractConnection> pastedConnections = new Dictionary<AbstractConnection, AbstractConnection>();
        int currentOffset = 0;

        public ElementContainer(DiagramType sourceDiagramType, ClipboardCommand clipboardCommand)
        {
            this.SourceDiagramType = sourceDiagramType;
            this.ClipboardCommand = clipboardCommand;
        }

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
            pastedShapes.Add(shape, null);
        }

        public void AddConnection(AbstractConnection connection)
        {
            connections.Add(connection);
        }

        public IEnumerable<Shape> Shapes => shapes;
        public IEnumerable<AbstractConnection> Connections => connections;

        PasteResult IClipboardItem.Paste(IDocument document)
        {
            IDiagram diagram = (IDiagram)document;
            if (diagram == null) return null;
            bool success = false;

            currentOffset += BaseOffset;
            //Size offset = new Size(
            //    (int)((diagram.Offset.X + currentOffset) / diagram.Zoom),
            //    (int)((diagram.Offset.Y + currentOffset) / diagram.Zoom));

            foreach (Shape shape in shapes)
            {
                Shape pasted = shape.Paste(diagram, new Size(currentOffset, currentOffset));
                pastedShapes[shape] = pasted;
                success |= (pasted != null);
            }
            foreach (var shape in shapes)
            {
                if (shape.ParentShape == null)
                {
                    continue;
                }

                if (!pastedShapes.ContainsKey(shape.ParentShape) ||
                    !pastedShapes.ContainsKey(shape))
                {
                    continue;
                }

                var parentShape = (ShapeContainer)pastedShapes[shape.ParentShape];
                var childShape = pastedShapes[shape];
                parentShape.AttachShapes(new List<Shape> { childShape });
            }
            foreach (var connection in connections)
            {
                Shape first = GetShape(connection.Relationship.First);
                Shape second = GetShape(connection.Relationship.Second);

                if (first != null && pastedShapes[first] != null &&
                    second != null && pastedShapes[second] != null)
                {
                    var pasted = connection.Paste(
                        diagram, new Size(currentOffset, currentOffset), pastedShapes[first], pastedShapes[second]);
                    pastedConnections[connection] = pasted;
                    success |= (pasted != null);
                }
            }

            if (success)
            {
                Clipboard.Clear();
            }

            return new PasteResult(pastedConnections, pastedShapes);
        }

        public ClipboardCommand ClipboardCommand { get; }

        public DiagramType SourceDiagramType { get; }

        //TODO: legyenek inkább hivatkozások a shape-ekhez
        public Shape GetShape(IEntity entity)
        {
            foreach (Shape shape in shapes)
            {
                if (shape.Entity == entity)
                    return shape;
            }
            return null;
        }
    }
}
