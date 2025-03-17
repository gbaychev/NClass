﻿// NClass - Free class diagram editor
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

using System.Collections.Generic;
using System.Linq;

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
            Source = UndoRedoSource.FileNew;
        }

        public void Undo()
        {
            if (UndoStack.Count == 0)
                return;
            var command = UndoStack.Pop();
            command.Undo();
            RedoStack.Push(command);
            var args = new UndoRedoEventArgs(UndoRedoAction.UndoPop | UndoRedoAction.RedoPush, command.DisplayText);
            UndoRedoChanged?.Invoke(this, args);
        }

        public void Redo()
        {
            if (RedoStack.Count == 0)
                return;
            var command = RedoStack.Pop();
            command.Execute();
            UndoStack.Push(command);
            var args = new UndoRedoEventArgs(UndoRedoAction.RedoPop | UndoRedoAction.UndoPush, command.DisplayText);
            UndoRedoChanged?.Invoke(this, args);
        }

        public void TrackCommand(ICommand command)
        {
            UndoStack.Push(command);
            RedoStack.Clear();
            var args = new UndoRedoEventArgs(UndoRedoAction.UndoPush | UndoRedoAction.RedoClear, command.DisplayText);
            UndoRedoChanged?.Invoke(this, args);
        }

        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;

        public void Visualize(IUndoRedoVisualizer undoRedoVisualizer)
        {
            var items = UndoStack.Select(command => new UndoRedoListBoxItem(command.DisplayText, UndoRedoType.Undo))
                                 .Concat(RedoStack.Select(c => new UndoRedoListBoxItem(c.DisplayText, UndoRedoType.Redo))).ToArray();
            undoRedoVisualizer.SetItems(items, Source);
        }

        public UndoRedoSource Source { get; set; }
    }
}