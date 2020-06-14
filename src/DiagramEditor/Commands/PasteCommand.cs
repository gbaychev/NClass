using NClass.Core.UndoRedo;
using NClass.DiagramEditor.Diagrams;

namespace NClass.DiagramEditor.Commands

{
    public class PasteCommand : ICommand
    {
        private readonly IClipboardItem _elements;
        private readonly IDiagram _diagram;
        private PasteResult _pasteResult;

        public PasteCommand(IDiagram diagram)
        {
            _diagram = diagram;
            _elements = Clipboard.Item;
        }

        public void Execute()
        {
            _diagram.DeselectAll();
            Clipboard.Item = _elements;
            if (_elements.ClipboardCommand == ClipboardCommand.Cut)
            {
                // undo the deletion of elements. Maybe a cleaner solution would be an 
                // different element container for cut and copy, so that the undo is an
                // atomic operation
                _diagram.Redo();
            }
            _pasteResult = Clipboard.Paste(_diagram);
        }

        public void Undo()
        {
            _diagram.DeselectAll();
            foreach (var shape in _pasteResult.PastedShapes.Values)
            {
                _diagram.RemoveEntity(shape.Entity);
            }

            foreach (var connection in _pasteResult.PastedConnections.Values)
            {
                _diagram.RemoveRelationship(connection.Relationship);
            }

            if (_elements.ClipboardCommand == ClipboardCommand.Cut)
            {
                // undo the deletion of elements. Maybe a cleaner solution would be an 
                // different element container for cut and copy, so that the undo is an
                // atomic operation
                _diagram.Undo();
            }

            _diagram.Redraw();
        }

        public CommandId CommandId => CommandId.Paste;
        public string DisplayText => CommandIdToString.GetString(CommandId);
    }
}