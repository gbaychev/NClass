using System;

namespace NClass.Core.UndoRedo
{
    public class UndoRedoEventArgs : EventArgs
    {
        public UndoRedoEventArgs(UndoRedoAction action, string debugTag)
        {
            Action = action;
            DebugTag = debugTag;
        }
        public UndoRedoAction Action { get; }
        public string DebugTag { get; }
    }
}