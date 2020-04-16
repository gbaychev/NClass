using System;
using System.Collections.Generic;
using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor.Commands

{
    public class PasteCommand
    {
        readonly List<DiagramElement> elements;
        readonly IDiagram diagram;

        public PasteCommand(List<DiagramElement> elements, IDiagram diagram)
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