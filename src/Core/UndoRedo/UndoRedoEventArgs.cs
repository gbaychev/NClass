using System;

namespace NClass.Core.UndoRedo
{
    public class UndoRedoEventArgs : EventArgs
    {
        public UndoRedoEventArgs(UndoRedoAction action, string displayText)
        {
            Action = action;
            DisplayText = displayText;
        }
        public UndoRedoAction Action { get; }
        public string DisplayText { get; }
    }
}