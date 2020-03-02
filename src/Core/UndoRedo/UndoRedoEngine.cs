// NClass - Free class diagram editor
// Copyright (C) 2020 Georgi Baychev
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

namespace NClass.Core.UndoRedo
{
    public class UndoRedoEngine
    {
        private Stack<Modification> UndoStack { get; }
        private Stack<Modification> RedoStack { get; }

        public event EventHandler UndoRedoChanged;
        
        public UndoRedoEngine()
        {
            UndoStack = new Stack<Modification>(25);
            RedoStack = new Stack<Modification>(25);
        }

        public void Undo()
        {
            if (UndoStack.Count == 0)
                return;
            var modification = UndoStack.Pop();
            modification.UndoAction();
            RedoStack.Push(modification);
            UndoRedoChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Redo()
        {
            if (RedoStack.Count == 0)
                return;
            var modification = RedoStack.Pop();
            modification.RedoAction();
            UndoStack.Push(modification);
            UndoRedoChanged?.Invoke(this, EventArgs.Empty);
        }

        public void TrackModification(Modification modification)
        {
            UndoStack.Push(modification);
            UndoRedoChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;
    }
}