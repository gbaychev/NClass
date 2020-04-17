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

using System.Collections.Generic;

namespace NClass.Core.UndoRedo
{
    public delegate void UndoRedoHandler(object sender, UndoRedoEventArgs args);

    public class UndoRedoEngine
    {
        private Stack<ICommand> UndoStack { get; }
        private Stack<ICommand> RedoStack { get; }

        public event UndoRedoHandler UndoRedoChanged;

        public UndoRedoEngine()
        {
            UndoStack = new Stack<ICommand>(25);
            RedoStack = new Stack<ICommand>(25);
        }

        public void Undo()
        {
            if (UndoStack.Count == 0)
                return;
            var command = UndoStack.Pop();
            command.Undo();
            RedoStack.Push(command);
            var args = new UndoRedoEventArgs(UndoRedoAction.UndoPop | UndoRedoAction.RedoPush, command.ToString());
            UndoRedoChanged?.Invoke(this, args);
        }

        public void Redo()
        {
            if (RedoStack.Count == 0)
                return;
            var command = RedoStack.Pop();
            command.Execute();
            UndoStack.Push(command);
            var args = new UndoRedoEventArgs(UndoRedoAction.RedoPop | UndoRedoAction.UndoPush, command.ToString());
            UndoRedoChanged?.Invoke(this, args);
        }

        public void TrackCommand(ICommand command)
        {
            UndoStack.Push(command);
            var args = new UndoRedoEventArgs(UndoRedoAction.UndoPush, command.ToString());
            UndoRedoChanged?.Invoke(this, args);
        }

        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;
    }
}