using System;

namespace NClass.Core.UndoRedo
{
    [Flags]
    public enum UndoRedoAction
    {
        UndoPush = 1,
        UndoPop = 1 << 1,
        RedoPush = 1 << 2,
        RedoPop = 1 << 3
    }
}